namespace SharpExperiments.ML.Classifiers;
using Microsoft.ML;
using Microsoft.ML.Data;

public class ProcessEvent
{
    public float ParentChildDistance; // e.g., how far this process deviates from common parent-child patterns
    public float IsSigned;            // 1 = signed, 0 = unsigned
    public float IntegrityLevel;      // e.g., 0 = low, 1 = medium, 2 = high
    public float ProcessNameLength;   // heuristic feature
    public bool IsSuspicious;         // Label (1 = suspicious, 0 = benign)
}

public class ProcessPrediction
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }
    public float Probability { get; set; }
}

public static class EDRBinaryClassifier
{
    public static void TrainAndRun()
    {
        var mlContext = new MLContext();

        // Mock historical telemetry data (training data)
        var data = new List<ProcessEvent>()
        {
            new ProcessEvent() { ParentChildDistance = 0.2f, IsSigned = 1f, IntegrityLevel = 2f, ProcessNameLength = 7f, IsSuspicious = false },
            new ProcessEvent() { ParentChildDistance = 0.8f, IsSigned = 0f, IntegrityLevel = 0f, ProcessNameLength = 25f, IsSuspicious = true },
            new ProcessEvent() { ParentChildDistance = 0.1f, IsSigned = 1f, IntegrityLevel = 1f, ProcessNameLength = 12f, IsSuspicious = false },
            new ProcessEvent() { ParentChildDistance = 0.9f, IsSigned = 0f, IntegrityLevel = 0f, ProcessNameLength = 30f, IsSuspicious = true },
        };
        var trainData = mlContext.Data.LoadFromEnumerable(data);

        var pipeline = mlContext.Transforms.Concatenate("Features",
            nameof(ProcessEvent.ParentChildDistance),
            nameof(ProcessEvent.IsSigned),
            nameof(ProcessEvent.IntegrityLevel),
            nameof(ProcessEvent.ProcessNameLength)
        )
        .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
            labelColumnName: "IsSuspicious",
            featureColumnName: "Features"));

        var model = pipeline.Fit(trainData);

        var newEvent = new ProcessEvent()
        {
            ParentChildDistance = 0.85f,
            IsSigned = 0f,
            IntegrityLevel = 0f,
            ProcessNameLength = 32f
        };

        var predictor = mlContext.Model.CreatePredictionEngine<ProcessEvent, ProcessPrediction>(model);
        var result = predictor.Predict(newEvent);

        Console.WriteLine($"[ML.NET] Process classified as: {(result.Prediction ? "Suspicious" : "Benign")} | Confidence: {result.Probability:P2}");
    }
}
