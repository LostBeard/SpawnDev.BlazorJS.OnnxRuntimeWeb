using Microsoft.JSInterop;

namespace SpawnDev.BlazorJS.OnnxRuntimeWeb
{
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
