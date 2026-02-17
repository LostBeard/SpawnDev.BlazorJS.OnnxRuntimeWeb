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
        /// </summary>
        [JsonPropertyName("preferredOutputLocation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PreferredOutputLocation { get; set; }

        /// <summary>
        /// Log severity level for this run.
        /// </summary>
        [JsonPropertyName("logSeverityLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LogSeverityLevel { get; set; }

        /// <summary>
        /// A tag for this run for logging purposes.
        /// </summary>
        [JsonPropertyName("tag")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Tag { get; set; }
    }
}
