using System.Text.Json.Serialization;

namespace SpawnDev.BlazorJS.OnnxRuntimeWeb
{
    /// <summary>
    /// Options for creating an ONNX Runtime InferenceSession.
    /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html"/>
    /// </summary>
    public class SessionCreateOptions
    {
        /// <summary>
        /// Execution providers to use for inference.
        /// Common values: "webgpu", "wasm", "cpu".
        /// Example: new[] { "webgpu", "wasm" }
        /// </summary>
        [JsonPropertyName("executionProviders")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? ExecutionProviders { get; set; }

        /// <summary>
        /// Graph optimization level.
        /// "disabled" | "basic" | "extended" | "all"
        /// </summary>
        [JsonPropertyName("graphOptimizationLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? GraphOptimizationLevel { get; set; }

        /// <summary>
        /// Preferred output location for GPU-backed sessions.
        /// "cpu" | "gpu-buffer" | "ml-tensor"
        /// </summary>
        [JsonPropertyName("preferredOutputLocation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PreferredOutputLocation { get; set; }

        /// <summary>
        /// Log severity level. 0=verbose, 1=info, 2=warning, 3=error, 4=fatal.
        /// </summary>
        [JsonPropertyName("logSeverityLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LogSeverityLevel { get; set; }
    }
}
