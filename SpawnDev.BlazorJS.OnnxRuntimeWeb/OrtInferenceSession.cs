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
        /// </summary>
        /// <param name="feeds">A dictionary or JSObject mapping input names to OrtTensors.</param>
        /// <returns>A JSObject mapping output names to OrtTensors.</returns>
        public Task<OrtSessionResult> RunAsync(JSObject feeds)
            => JSRef!.CallAsync<OrtSessionResult>("run", feeds);

        /// <summary>
        /// Run the inference session with feeds and run options.
        /// This overload allows specifying preferredOutputLocation for GPU-resident output.
        /// </summary>
        /// <param name="feeds">A dictionary or JSObject mapping input names to OrtTensors.</param>
        /// <param name="options">Run options including preferredOutputLocation.</param>
        /// <returns>A JSObject mapping output names to OrtTensors.</returns>
        public Task<OrtSessionResult> RunAsync(JSObject feeds, SessionRunOptions options)
            => JSRef!.CallAsync<OrtSessionResult>("run", feeds, options);

        /// <summary>
        /// Release the inference session and its resources.
        /// </summary>
        public Task ReleaseAsync() => JSRef!.CallVoidAsync("release");
    }

    /// <summary>
    /// Result of an inference session run. 
    /// Provides access to output tensors by name.
    /// </summary>
    public class OrtSessionResult : JSObject
    {
        /// <inheritdoc/>
        public OrtSessionResult(IJSInProcessObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Get an output tensor by name.
        /// </summary>
        /// <param name="outputName">The name of the output tensor.</param>
        /// <returns>The output tensor.</returns>
        public OrtTensor GetTensor(string outputName)
            => JSRef!.Get<OrtTensor>(outputName);

        /// <summary>
        /// Try to get an output tensor by name. Returns null if not found.
        /// </summary>
        public OrtTensor? TryGetTensor(string outputName)
        {
            try
            {
                if (JSRef!.IsUndefined(outputName)) return null;
                return JSRef!.Get<OrtTensor>(outputName);
            }
            catch { return null; }
        }
    }
}
