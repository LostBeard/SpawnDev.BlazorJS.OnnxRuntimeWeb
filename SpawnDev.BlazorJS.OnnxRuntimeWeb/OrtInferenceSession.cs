using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;

namespace SpawnDev.BlazorJS.OnnxRuntimeWeb
{
    /// <summary>
    /// Wraps the ONNX Runtime Web InferenceSession.
    /// Provides methods to create sessions and run inference with GPU buffer support.
    /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession-1.html"/>
    /// </summary>
    public class OrtInferenceSession : JSObject
    {
        /// <inheritdoc/>
        public OrtInferenceSession(IJSInProcessObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// The input names of the model.
        /// </summary>
        public string[] InputNames => JSRef!.Get<string[]>("inputNames");

        /// <summary>
        /// The output names of the model.
        /// </summary>
        public string[] OutputNames => JSRef!.Get<string[]>("outputNames");

        /// <summary>
        /// Run the inference session with the given feeds.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession-1.html#run"/>
        /// </summary>
        /// <param name="feeds">A dictionary or JSObject mapping input names to OrtTensors.</param>
        /// <returns>A JSObject mapping output names to OrtTensors.</returns>
        public Task<OrtSessionResult> Run(JSObject feeds)
            => JSRef!.CallAsync<OrtSessionResult>("run", feeds);

        /// <summary>
        /// Run the inference session with feeds and run options.
        /// This overload allows specifying preferredOutputLocation for GPU-resident output.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession-1.html#run"/>
        /// </summary>
        /// <param name="feeds">A dictionary or JSObject mapping input names to OrtTensors.</param>
        /// <param name="options">Run options including preferredOutputLocation.</param>
        /// <returns>A JSObject mapping output names to OrtTensors.</returns>
        public Task<OrtSessionResult> Run(JSObject feeds, SessionRunOptions options)
            => JSRef!.CallAsync<OrtSessionResult>("run", feeds, options);

        /// <summary>
        /// Release the inference session and its resources.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/InferenceSession-1.html#release"/>
        /// </summary>
        public Task Release() => JSRef!.CallVoidAsync("release");
    }
}
