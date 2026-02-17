using System.Text.Json.Serialization;

namespace SpawnDev.BlazorJS.OnnxRuntimeWeb
{
    /// <summary>
    /// Options for running inference.
    /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.RunOptions.html"/>
    /// </summary>
    public class SessionRunOptions
    {
        /// <summary>
        /// Preferred output location for tensor data.
        /// Set to "gpu-buffer" to keep output tensors on the GPU.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.RunOptions.html#preferredOutputLocation"/>
        /// </summary>
        [JsonPropertyName("preferredOutputLocation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PreferredOutputLocation { get; set; }

        /// <summary>
        /// Log severity level for this run. 0=verbose, 1=info, 2=warning, 3=error, 4=fatal.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.RunOptions.html#logSeverityLevel"/>
        /// </summary>
        [JsonPropertyName("logSeverityLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LogSeverityLevel { get; set; }

        /// <summary>
        /// Log verbosity level for this run.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.RunOptions.html#logVerbosityLevel"/>
        /// </summary>
        [JsonPropertyName("logVerbosityLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LogVerbosityLevel { get; set; }

        /// <summary>
        /// A tag for this run for logging purposes.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.RunOptions.html#tag"/>
        /// </summary>
        [JsonPropertyName("tag")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Tag { get; set; }

        /// <summary>
        /// Terminate all incomplete OrtRun calls as soon as possible if true.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.RunOptions.html#terminate"/>
        /// </summary>
        [JsonPropertyName("terminate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Terminate { get; set; }
    }
}
