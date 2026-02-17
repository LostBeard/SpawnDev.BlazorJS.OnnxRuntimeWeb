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
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#executionProviders"/>
        /// </summary>
        [JsonPropertyName("executionProviders")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? ExecutionProviders { get; set; }

        /// <summary>
        /// Graph optimization level.
        /// "disabled" | "basic" | "extended" | "all"
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#graphOptimizationLevel"/>
        /// </summary>
        [JsonPropertyName("graphOptimizationLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? GraphOptimizationLevel { get; set; }

        /// <summary>
        /// Preferred output location for GPU-backed sessions.
        /// "cpu" | "gpu-buffer" | "ml-tensor"
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#preferredOutputLocation"/>
        /// </summary>
        [JsonPropertyName("preferredOutputLocation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PreferredOutputLocation { get; set; }

        /// <summary>
        /// Log severity level. 0=verbose, 1=info, 2=warning, 3=error, 4=fatal.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#logSeverityLevel"/>
        /// </summary>
        [JsonPropertyName("logSeverityLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LogSeverityLevel { get; set; }

        /// <summary>
        /// Log verbosity level.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#logVerbosityLevel"/>
        /// </summary>
        [JsonPropertyName("logVerbosityLevel")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LogVerbosityLevel { get; set; }

        /// <summary>
        /// Log ID.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#logId"/>
        /// </summary>
        [JsonPropertyName("logId")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LogId { get; set; }

        /// <summary>
        /// Whether to enable CPU memory arena.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#enableCpuMemArena"/>
        /// </summary>
        [JsonPropertyName("enableCpuMemArena")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableCpuMemArena { get; set; }

        /// <summary>
        /// Whether to enable memory pattern.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#enableMemPattern"/>
        /// </summary>
        [JsonPropertyName("enableMemPattern")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableMemPattern { get; set; }

        /// <summary>
        /// Execution mode. "sequential" | "parallel".
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#executionMode"/>
        /// </summary>
        [JsonPropertyName("executionMode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExecutionMode { get; set; }

        /// <summary>
        /// Whether to enable profiling.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#enableProfiling"/>
        /// </summary>
        [JsonPropertyName("enableProfiling")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableProfiling { get; set; }

        /// <summary>
        /// Whether to enable graph capture (WebGPU EP only).
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#enableGraphCapture"/>
        /// </summary>
        [JsonPropertyName("enableGraphCapture")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? EnableGraphCapture { get; set; }

        /// <summary>
        /// Profile file prefix.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#profileFilePrefix"/>
        /// </summary>
        [JsonPropertyName("profileFilePrefix")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProfileFilePrefix { get; set; }

        /// <summary>
        /// Optimized model file path.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#optimizedModelFilePath"/>
        /// </summary>
        [JsonPropertyName("optimizedModelFilePath")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? OptimizedModelFilePath { get; set; }

        /// <summary>
        /// Free dimension overrides. Keys are dimension names, values are the override sizes.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession.SessionOptions.html#freeDimensionOverrides"/>
        /// </summary>
        [JsonPropertyName("freeDimensionOverrides")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, int>? FreeDimensionOverrides { get; set; }
    }
}
