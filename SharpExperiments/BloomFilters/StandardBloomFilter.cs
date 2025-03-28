namespace SharpExperiments.BloomFilters;
using System;
using SharpExperiments.Hashing;

/// <summary>
/// Non-ThreadSafe Standard Bloom Filter (SBF). Demonstrates the usage of with Murmur3
/// and practical visualizations for the functionality of bloom filters.
/// </summary>
/// <typeparam name="T"></typeparam>
public class StandardBloomFilter<T>
{
    /// <summary>
    /// **Maximum Bloom Filter size** (1GB).
    /// Limits the **maximum number of bits** allocated to prevent excessive memory usage.
    /// </summary>
    private const long MAX_BIT_ARRAY_SIZE = 8_589_934_592; // 1 GB in bits

    /// <summary>
    /// **Total number of bits (m)** in the Bloom Filter.
    /// Computed based on the expected number of elements and target false positive rate.
    /// </summary>
    private readonly long _size;

    /// <summary>
    /// **Optimal number of hash functions (k)**.
    /// Ensures the best balance between **false positive rate** and performance.
    /// </summary>
    private readonly int _hashCount;

    /// <summary>
    /// **Byte array representation** of the Bloom Filter's **bit storage**.
    /// Instead of using a `BitArray`, we use a **byte array** for direct bitwise manipulation,
    /// reducing memory overhead.
    /// </summary>
    private readonly byte[]? _bitArray;

    /// <summary>
    /// **Size of `_bitArray` in bytes**.
    /// Calculated as `(bitSize + 7) / 8` to ensure proper byte alignment.
    /// </summary>
    private readonly long _byteSize;

    /// <summary>
    /// **Expected Number of elements inserted** into the Bloom Filter.
    /// This count is used for tracking insert operations but does not affect query performance.
    /// </summary>
    private long _expectedElements = 0;

    /// <summary>
    /// **Number of elements inserted** into the Bloom Filter.
    /// This count is used for tracking insert operations but does not affect query performance.
    /// </summary>
    private long _insertedElements = 0;

    /// <summary>
    /// **Stores the Bloom Filter configuration details** as a formatted string.
    /// This is used for **debugging, logging, and performance monitoring**.
    /// </summary>
    private readonly string? _bloomFilterConfiguration;

    /// <summary>
    /// **Stores the acceptable Bloom Filter false positive rate** as a double.
    /// </summary>
    private double _fpRate = 0.0;

    /// <summary>
    /// Initializes a **Standard Bloom Filter** with optimized parameters for **memory efficiency**
    /// and a target **false positive rate**.
    ///
    /// <para>Key Features:</para>
    /// - Computes the **optimal bit array size (m)** for storing `expectedElements`
    ///   while maintaining the specified **false positive rate (p)**.
    /// - Computes the **optimal number of hash functions (k)** to minimize false positives.
    /// - Uses a **byte array representation** (`_bitArray`) instead of a `BitArray`,
    ///   improving memory alignment and **reducing overhead**.
    ///
    /// <para>Use Cases:</para>
    /// - **Reducing unnecessary disk lookups** in large-scale systems.
    /// - **Efficient membership testing** in networking and caching.
    /// - **Space-efficient set representation** in database systems.
    ///
    /// <para>References:</para>
    /// - [Bloom Filters - Optimal Parameters](https://en.wikipedia.org/wiki/Bloom_filter#Optimal_number_of_hash_functions)
    /// - [Bloom Filters: Space-Efficient](https://micahkepe.com/blog/bloom-filters/)
    ///
    /// </summary>
    /// <param name="expectedElements">
    /// The number of **unique elements** expected to be inserted into the Bloom Filter.
    /// </param>
    /// <param name="falsePositiveRate">
    /// The **target false positive rate** (default: **1%**).
    /// Lower values require a **larger bit array size** to maintain accuracy.
    /// </param>
    public StandardBloomFilter(long expectedElements, double falsePositiveRate = 0.01)
    {
        // Compute the optimal bit size (m) based on expected elements and target FP rate.
        long calculatedSize = CalculateBitSize(expectedElements, falsePositiveRate) + 16;

        // Check if the calculated size exceeds the limit
        if (calculatedSize > MAX_BIT_ARRAY_SIZE)
        {
            string msize = $"{MAX_BIT_ARRAY_SIZE / (8.0 * 1024 * 1024):F2}";
            Console.WriteLine($"Warning: Bloom filter size exceeds maximum limit of {msize} MB.");
            return;
        }
        // Use the calculated size if within limit
        _size = calculatedSize;

        // Compute the optimal number of hash functions (k)
        _hashCount = CalculateHashCount(_size, expectedElements);

        // Convert bit size to byte size with proper alignment
        _byteSize = (_size + 7) / 8;

        // Initialize the bit array as a byte array
        _bitArray = new byte[_byteSize];

        // Store the initial expected size
        _expectedElements = expectedElements;

        // Store initial FP rate
        _fpRate = falsePositiveRate;

        // Store configuration details for debugging/logging
        _bloomFilterConfiguration = $@"
        ================================================
              SBF - Standard BloomFilter w/ Murmur3

        Config
        
            Bit Array Size  (m): {_size} bits ({_byteSize} bytes)
            Elements        (n): {_expectedElements}
            Hash Functions  (k): {_hashCount}
            FP Rate         (p): {falsePositiveRate} or {falsePositiveRate  * 100:F1}%
        ================================================";
    }
    /// <summary>
    /// Inserts an item into the Bloom Filter by setting the **corresponding bits**
    /// in the bit array based on multiple **hash values**.
    ///
    /// <para>Key Features:</para>
    /// - Computes **multiple hash values** for the given item.
    /// - Maps each hash value to a **specific bit index** in the bit array.
    /// - Sets the corresponding bits in the **Bloom Filter's bit array**.
    /// - Does **not support deletions**, since clearing a bit could remove multiple elements.
    ///
    /// <para>Use Cases:</para>
    /// - **Tracking seen items** in large datasets.
    /// - **Efficient duplicate detection** in streaming data.
    /// - **Reducing unnecessary database queries** in caching systems.
    ///
    /// <para>References:</para>
    /// - [Bloom Filters in Databases](https://en.wikipedia.org/wiki/Bloom_filter#Database_applications)
    /// - [Bloom Filters: Space-Efficient](https://micahkepe.com/blog/bloom-filters/)
    ///
    /// </summary>
    /// <param name="item">The item to insert into the Bloom Filter.</param>
    public void Add(T item)
    {
        Span<long> insertHashes = stackalloc long[_hashCount];
        bool isNewElement = false;

        // Generate hashes safely
        Murmur3.CreateHashes(item, insertHashes);

        foreach (var hash in insertHashes)
        {
            // Use long to prevent overflow in bitIndex calculation
            long bitIndex = hash % _size;

            // Validate bitIndex within bounds
            if (bitIndex < 0 || bitIndex >= _size)
            {
                throw new IndexOutOfRangeException($"Invalid bit index: {bitIndex}. Possible hash error or overflow.");
            }

            // Use long to calculate byteIndex safely
            long byteIndex = bitIndex >> 3;  // Same as bitIndex / 8

            // Validate byteIndex before accessing the array
            if (byteIndex >= _bitArray.Length || byteIndex < 0)
            {
                throw new IndexOutOfRangeException($"Byte index {byteIndex} is out of bounds for array size {_bitArray.Length}.");
            }

            // Calculate bit mask for the bit position within the byte
            int bitMask = 1 << (int)(bitIndex & 7);  // Only this part stays as int (0-7 is safe)

            // Check if the bit is already set
            if ((_bitArray[byteIndex] & bitMask) == 0)
            {
                // Set the bit
                _bitArray[byteIndex] |= (byte)bitMask;
                isNewElement = true;
            }
        }

        if (isNewElement)
        {
            _insertedElements++;
        }
    }

    /// <summary>
    /// Gets the chosen number of element during initialization.
    /// </summary>
    /// <returns></returns>
    public long GetAllocatedFilterSize()
    {
        return _expectedElements;
    }

    /// <summary>
    /// Gets the current number of elements inserted.
    /// </summary>
    /// <returns></returns>
    public long GetCurrentFilterSize()
    {
        return _insertedElements;
    }

    /// <summary>
    /// Gets the computed number of independent hash functions during initialization.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHashCount()
    {
        return _hashCount;
    }

    /// <summary>
    /// Gets the false positive rate chosen during initialization.
    /// </summary>
    /// <returns></returns>
    public double GetFalsePositiveRate()
    {
        return _fpRate;
    }

    // Returns the size of the bit array in bits
    public long GetBitArraySize()
    {
        return _size;
    }

    // Returns the raw byte array storing Bloom filter bits
    public byte[] GetRawBitArray()
    {
        return _bitArray;
    }

    /// <summary>
    /// Checks whether the specified item **might be present** in the Bloom Filter.
    /// Since Bloom Filters allow false positives, a **true** result means the item
    /// **could be present**, but a **false** result guarantees that the item is **not present**.
    ///
    /// <para>Key Features:</para>
    /// - Computes multiple **hash values** for the item.
    /// - Maps each hash to a **specific bit index** in the bit array.
    /// - If **any of the mapped bits are not set**, the item **is definitely not present**.
    /// - If **all bits are set**, the item **might be present** (false positives possible).
    ///
    /// <para>Use Cases:</para>
    /// - **Avoiding unnecessary database lookups** in caching systems.
    /// - **Fast membership tests** in distributed systems and networking applications.
    /// - **Preventing redundant computations** in large-scale data processing.
    ///
    /// <para>References:</para>
    /// - [Bloom Filters - False Positives](https://en.wikipedia.org/wiki/Bloom_filter#False_positive_rate)
    /// - [Bloom Filters: Space-Efficient](https://micahkepe.com/blog/bloom-filters/)
    ///
    /// </summary>
    /// <param name="item">The item to check for presence in the Bloom Filter.</param>
    /// <returns>
    /// **True** if the item **might be present** (false positives possible),
    /// **False** if the item **is definitely not present**.
    /// </returns>
    public bool MightContain(T item)
    {
        Span<long> lookupHashes = stackalloc long[_hashCount];
        Murmur3.CreateHashes(item, lookupHashes);

        foreach (var hash in lookupHashes)
        {
            // Use long for safe bit index calculation
            long bitIndex = hash % _size;

            // Validate bitIndex to prevent overflow
            if (bitIndex < 0 || bitIndex >= _size)
            {
                throw new IndexOutOfRangeException($"Invalid bit index: {bitIndex}. Possible hash error or overflow.");
            }

            // Calculate byte index using long
            long byteIndex = bitIndex >> 3;  // bitIndex / 8

            // Check byteIndex bounds before accessing _bitArray
            if (byteIndex >= _bitArray.Length || byteIndex < 0)
            {
                throw new IndexOutOfRangeException($"Byte index {byteIndex} is out of bounds for array size {_bitArray.Length}.");
            }

            // Bit mask stays as int (0-7 range is safe for int)
            int bitMask = 1 << (int)(bitIndex & 7);

            // Check if the bit is set
            bool isSet = (_bitArray[byteIndex] & bitMask) != 0;

            // Return false immediately if any bit is not set
            if (!isSet)
            {
                return false; // If any bit is missing, the item is not in the filter.
            }
        }

        // All bits are set, the item *might* be present
        return true;
    }

    /// <summary>
    /// Show the configuration of the BloomFilter
    /// </summary>
    public void ShowConfiguration()
    {
        Console.WriteLine(_bloomFilterConfiguration);
    }

    /// <summary>
    /// Displays the Bloom Filter as a **pure 8x8 grid of bits**.
    /// - **Zeros (`0`) are dark gray**.
    /// - **Ones (`1`) are bold bright green**.
    /// - **8 bytes per row for structured readability**.
    /// </summary>
    public void ShowArrayGrid()
    {
        const int BYTES_PER_ROW = 8; // 8 bytes per row
        const string SPACE = " ";
        string header = $"\n\n============[ Bloom Filter Grid View | ({_bitArray.Length:N0} Bytes Array) | Elements ({_insertedElements:N0} / {_expectedElements:N0}) ]============\n\n";
        string footer = $"\n\n{new string('=', header.Length)}";

        Console.WriteLine(header);

        for (int byteIndex = 0; byteIndex < _byteSize; byteIndex++)
        {
            byte currentByte = _bitArray[byteIndex];

            for (int bit = 7; bit >= 0; bit--)
            {
                bool isSet = (currentByte & (1 << bit)) != 0;
                Console.Write(isSet ? "\u001b[1;32m1\u001b[0m" : "\u001b[90m0\u001b[0m");
            }

            Console.Write(SPACE);

            // Newline every `BYTES_PER_ROW` bytes
            if ((byteIndex + 1) % BYTES_PER_ROW == 0)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine(footer);
    }

    /// <summary>
    /// Computes the **optimal number of bits (m)** needed for a **Bloom Filter**
    /// to store `n` elements while maintaining a given **false positive rate (p)**.
    ///
    /// <para>**Key Concept:**</para>
    /// - The size of a **Bloom Filter bit array** is determined using the formula:
    ///   \[
    ///   m = \frac{-n \ln(p)}{(\ln(2))^2}
    ///   \]
    /// - This ensures that the filter has **enough space** to keep false positives
    ///   **at or below the desired rate**.
    ///
    /// <para>**Use Cases:**</para>
    /// - **Bloom Filters for network caching** (e.g., DNS caching, CDN lookups).
    /// - **Database query optimization** (e.g., avoiding unnecessary disk reads).
    ///
    /// <para>**References:**</para>
    /// - [Bloom Filters in Practice](https://en.wikipedia.org/wiki/Bloom_filter#Optimal_number_of_hash_functions)
    /// - [Bloom Filter Theory by Mitzenmacher](https://www.eecs.harvard.edu/~michaelm/postscripts/im2005b.pdf)
    ///
    /// </summary>
    /// <param name="n">The expected number of elements to store in the Bloom Filter.</param>
    /// <param name="p">The desired false positive rate (between 0 and 1).</param>
    /// <returns>The computed optimal bit array size (m), rounded up.</returns>
    private long CalculateBitSize(long n, double p)
    {
        // Prevent invalid inputs (e.g., p should be between 0 and 1)
        if (n <= 0 || p <= 0 || p >= 1)
        {
            throw new ArgumentException("Invalid parameters: Ensure n > 0 and 0 < p < 1.");
        }

        // **Optimal Bloom Filter Size Calculation**
        // Formula: m = (-n * ln(p)) / (ln(2)^2)
        return (long)Math.Ceiling(-n * Math.Log(p) / Math.Pow(Math.Log(2), 2));
    }

    /// <summary>
    /// Computes the **optimal number of hash functions (k)** for a Bloom Filter.
    /// This ensures that **false positives are minimized** while keeping the 
    /// computational cost low.
    ///
    /// <para>**Key Concept:**</para>
    /// - The **optimal number of hash functions (k)** is given by:
    ///   \[
    ///   k = \frac{m}{n} \ln(2)
    ///   \]
    /// - This formula minimizes the false positive rate **without excessive hashing**.
    ///
    /// <para>**Use Cases:**</para>
    /// - **Ensuring efficient space utilization** in Bloom Filters.
    /// - **Reducing computational overhead** (avoiding excessive hashing).
    ///
    /// <para>**References:**</para>
    /// - [Mitzenmacher & Upfal - Bloom Filter Theory](https://www.eecs.harvard.edu/~michaelm/postscripts/im2005b.pdf)
    /// - [Optimal Hash Function Count in Bloom Filters](https://en.wikipedia.org/wiki/Bloom_filter#Optimal_number_of_hash_functions)
    ///
    /// </summary>
    /// <param name="m">The number of bits in the Bloom Filter.</param>
    /// <param name="n">The expected number of elements to store.</param>
    /// <returns>The optimal number of hash functions (k), rounded up.</returns>
    private int CalculateHashCount(long m, long n)
    {
        // Prevent division by zero or invalid values
        if (m <= 0 || n <= 0)
        {
            throw new ArgumentException("Invalid parameters: Ensure m > 0 and n > 0.");
        }

        // **Optimal Number of Hash Functions Calculation**
        // Formula: k = (m / n) * ln(2)
        return (int)Math.Ceiling(m / (double)n * Math.Log(2));
    }
}