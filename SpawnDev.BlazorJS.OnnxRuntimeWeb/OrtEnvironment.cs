using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;

namespace SpawnDev.BlazorJS.OnnxRuntimeWeb
{
    /// <summary>
    /// Wraps the ONNX Runtime Web environment configuration (ort.env).
    /// Provides access to WASM paths, WebGPU settings, and logging.
    /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/Env.html"/>
    /// </summary>
    public class OrtEnvironment : JSObject
    {
        /// <inheritdoc/>
        public OrtEnvironment(IJSInProcessObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Gets or sets the log level. 
        /// "verbose" | "info" | "warning" | "error" | "fatal"
        /// </summary>
        public string LogLevel
        {
            get => JSRef!.Get<string>("logLevel");
            set => JSRef!.Set("logLevel", value);
        }

        /// <summary>
        /// Gets or sets the log severity level (numeric).
        /// 0 = verbose, 1 = info, 2 = warning, 3 = error, 4 = fatal
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/Env.html#logSeverityLevel"/>
        /// </summary>
        public int LogSeverityLevel
        {
            get => JSRef!.Get<int>("logSeverityLevel");
            set => JSRef!.Set("logSeverityLevel", value);
        }

        /// <summary>
        /// Gets the WASM configuration object to set file paths/overrides.
        /// </summary>
        public JSObject Wasm => JSRef!.Get<JSObject>("wasm");

        /// <summary>
        /// Gets the WebGPU configuration object.
        /// Use this to set the GPUDevice for session creation.
        /// </summary>
        public JSObject WebGpu => JSRef!.Get<JSObject>("webgpu");

        /// <summary>
        /// Set the WebGPU device to use for inference.
        /// This should be the same device used by ILGPU/WebGPU to enable zero-copy buffer sharing.
        /// </summary>
        /// <param name="device">The GPUDevice to use.</param>
        public void SetWebGpuDevice(GPUDevice device)
        {
            JSRef!.Get<JSObject>("webgpu").JSRef!.Set("device", device);
        }

        /// <summary>
        /// Set the preferred output location for WebGPU sessions.
        /// </summary>
        /// <param name="location">"cpu" | "gpu-buffer" | "ml-tensor"</param>
        public void SetPreferredOutputLocation(string location)
        {
            JSRef!.Get<JSObject>("webgpu").JSRef!.Set("preferredOutputLocation", location);
        }
    }
}
