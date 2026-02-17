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
        public string[]? ExecutionProviders { get; set; }

        /// <summary>
        /// Graph optimization level.
        /// "disabled" | "basic" | "extended" | "all"
        /// </summary>
        public string? GraphOptimizationLevel { get; set; }

        /// <summary>
        /// Preferred output location for GPU-backed sessions.
        /// "cpu" | "gpu-buffer" | "ml-tensor"
        /// </summary>
        public string? PreferredOutputLocation { get; set; }

        /// <summary>
        /// Log severity level. 0=verbose, 1=info, 2=warning, 3=error, 4=fatal.
        /// </summary>
        public int? LogSeverityLevel { get; set; }
    }

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
        public string? PreferredOutputLocation { get; set; }

        /// <summary>
        /// Log severity level for this run.
        /// </summary>
        public int? LogSeverityLevel { get; set; }

        /// <summary>
        /// A tag for this run for logging purposes.
        /// </summary>
        public string? Tag { get; set; }
    }
}
