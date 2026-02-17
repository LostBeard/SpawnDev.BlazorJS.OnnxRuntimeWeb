using Microsoft.JSInterop;

namespace SpawnDev.BlazorJS.OnnxRuntimeWeb
{
    /// <summary>
    /// Strongly-typed wrapper for the feeds object passed to InferenceSession.run().
    /// Maps input names to OrtTensor values.
    /// <see href="https://onnxruntime.ai/docs/api/js/types/InferenceSession.OnnxValueMapType.html"/>
    /// </summary>
    public class OrtFeeds : JSObject
    {
        /// <inheritdoc/>
        public OrtFeeds(IJSInProcessObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Creates a new empty feeds object.
        /// </summary>
        public OrtFeeds() : base(JS.New("Object")) { }

        /// <summary>
        /// Set a named input tensor in the feeds.
        /// </summary>
        /// <param name="inputName">The model input name to set.</param>
        /// <param name="tensor">The input tensor.</param>
        public void Set(string inputName, OrtTensor tensor)
        {
            JSRef!.Set(inputName, tensor);
        }

        /// <summary>
        /// Get a tensor by name from the feeds.
        /// </summary>
        /// <param name="inputName">The model input name.</param>
        /// <returns>The tensor at the given name.</returns>
        public OrtTensor Get(string inputName)
        {
            return JSRef!.Get<OrtTensor>(inputName);
        }
    }
}
