namespace SharpExperiments
{
    using SharpExperiments.DirectoryInfo;
    class Program
    {
        static void Main(string[] args)
        {
            var result = DirectoryHelper.GetDirectoryFilesByFilterWhereToArray();
            Console.WriteLine(result);
        }
    }
}
