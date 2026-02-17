using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;

namespace SpawnDev.BlazorJS.OnnxRuntimeWeb
{
    /// <summary>
    /// ONNX Runtime Web module loader and environment configuration.
    /// Loads the onnxruntime-web WebGPU bundle from CDN via dynamic import.
    /// <see href="https://onnxruntime.ai/docs/api/js/"/>
    /// </summary>
    public class OnnxRuntime : JSObject
    {
        /// <summary>
        /// Global module name used for the ORT module reference.
        /// </summary>
        public const string GlobalModuleName = "__ort";

        /// <summary>
        /// ONNX Runtime Web (WebGPU) bundled with this library.<br/>
        /// Downloaded from:<br/>
        /// https://cdn.jsdelivr.net/npm/onnxruntime-web@1.25.0-dev.20260213-bd8f781f2c/dist/ort.webgpu.bundle.min.mjs
        /// </summary>
        public static string LatestBundledVersionSrc { get; } = "./_content/SpawnDev.BlazorJS.OnnxRuntimeWeb/ort-web-1.25.0-dev.webgpu.bundle.min.mjs";

        /// <summary>
        /// ONNX Runtime Web CDN URL (WebGPU bundle).
        /// </summary>
        public static string LatestCDNVersionSrc { get; } = "https://cdn.jsdelivr.net/npm/onnxruntime-web@1.25.0-dev.20260213-bd8f781f2c/dist/ort.webgpu.bundle.min.mjs";

        /// <summary>
        /// Local static assets content path suffix for WASM files bundled with this library.
        /// Combined with the document base URI at runtime to produce the correct absolute URL.
        /// </summary>
        public const string BundledWasmContentPath = "_content/SpawnDev.BlazorJS.OnnxRuntimeWeb/";

        private static BlazorJSRuntime JS => BlazorJSRuntime.JS;

        /// <inheritdoc/>
        public OnnxRuntime(IJSInProcessObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Gets the WASM path prefix by combining the document's base URI with the content path.
        /// This ensures correct resolution regardless of deployment location (e.g., GitHub Pages subdirectory).
        /// </summary>
        private static string GetWasmPrefix()
        {
            var baseUri = JS.Get<string>("document.baseURI");
            if (!baseUri.EndsWith("/")) baseUri += "/";
            return baseUri + BundledWasmContentPath;
        }

        /// <summary>
        /// Initialize the ONNX Runtime Web module.
        /// Returns the module reference which provides access to Tensor, InferenceSession, and env.
        /// </summary>
        /// <param name="srcUrl">Optional URL override for the JS module. Defaults to the bundled version.</param>
        /// <returns>The loaded OnnxRuntime module.</returns>
        public static async Task<OnnxRuntime> Init(string? srcUrl = null)
        {
            srcUrl ??= LatestBundledVersionSrc;
            var ort = await JS.Import<OnnxRuntime>(GlobalModuleName, srcUrl);
            if (ort == null)
                throw new Exception("ONNX Runtime Web could not be initialized.");

            // Configure WASM paths using the base URI to support subdirectory deployments
            using var env = ort.Env;
            using var wasm = env.Wasm;
            wasm.JSRef!.Set("wasmPaths", GetWasmPrefix());

            return ort;
        }

        /// <summary>
        /// Access the ORT environment configuration.
        /// </summary>
        public OrtEnvironment Env => JSRef!.Get<OrtEnvironment>("env");

        /// <summary>
        /// Create an InferenceSession from a model URL or byte array.
        /// </summary>
        /// <param name="modelPath">URL or path to the ONNX model file.</param>
        /// <param name="options">Optional session creation options.</param>
        /// <returns>The created inference session.</returns>
        public Task<OrtInferenceSession> CreateInferenceSessionAsync(string modelPath, SessionCreateOptions? options = null)
            => options == null
                ? JS.CallAsync<OrtInferenceSession>($"{GlobalModuleName}.InferenceSession.create", modelPath)
                : JS.CallAsync<OrtInferenceSession>($"{GlobalModuleName}.InferenceSession.create", modelPath, options);

        /// <summary>
        /// Create an InferenceSession from model bytes.
        /// </summary>
        /// <param name="modelData">The ONNX model as a byte array (ArrayBuffer).</param>
        /// <param name="options">Optional session creation options.</param>
        /// <returns>The created inference session.</returns>
        public Task<OrtInferenceSession> CreateInferenceSessionAsync(ArrayBuffer modelData, SessionCreateOptions? options = null)
            => options == null
                ? JS.CallAsync<OrtInferenceSession>($"{GlobalModuleName}.InferenceSession.create", modelData)
                : JS.CallAsync<OrtInferenceSession>($"{GlobalModuleName}.InferenceSession.create", modelData, options);

        /// <summary>
        /// The ORT Tensor constructor/class. Use this to access static factory methods.
        /// </summary>
        public JSObject TensorClass => JSRef!.Get<JSObject>("Tensor");

        /// <summary>
        /// Create a new ORT Tensor from typed data.
        /// Equivalent to: new ort.Tensor(type, data, dims)
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/Tensor-1.html"/>
        /// </summary>
        /// <param name="type">Data type string (e.g., "float32", "int32", "uint8").</param>
        /// <param name="data">A typed array (Float32Array, Int32Array, etc.) containing the tensor data.</param>
        /// <param name="dims">The dimensions of the tensor.</param>
        /// <returns>A new ORT Tensor.</returns>
        public OrtTensor CreateTensor(string type, JSObject data, int[] dims)
        {
            using var ctor = JSRef!.Get<Function>("Tensor");
            return Reflect.Construct<OrtTensor>(ctor, new object[] { type, data, dims });
        }

        /// <summary>
        /// Create a Tensor from a WebGPU buffer. This is the key method for GPU-resident inference.
        /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/TensorFactory.html#fromGpuBuffer"/>
        /// </summary>
        /// <param name="gpuBuffer">The WebGPU GPUBuffer containing tensor data.</param>
        /// <param name="options">Options specifying dataType and dims.</param>
        /// <returns>An ORT Tensor backed by the GPU buffer.</returns>
        public OrtTensor TensorFromGpuBuffer(GPUBuffer gpuBuffer, TensorFromGpuBufferOptions options)
            => JS.Call<OrtTensor>($"{GlobalModuleName}.Tensor.fromGpuBuffer", gpuBuffer, options);
    }
}
