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
        /// https://cdn.jsdelivr.net/npm/onnxruntime-web@1.24.1/dist/ort.webgpu.bundle.min.mjs
        /// </summary>
        public static string LatestBundledVersionSrc { get; } = "./_content/SpawnDev.BlazorJS.OnnxRuntimeWeb/ort-web-1.24.1.webgpu.bundle.min.mjs";

        /// <summary>
        /// ONNX Runtime Web CDN URL (WebGPU bundle).
        /// </summary>
        public static string LatestCDNVersionSrc { get; } = "https://cdn.jsdelivr.net/npm/onnxruntime-web@1.24.1/dist/ort.webgpu.bundle.min.mjs";

        private static BlazorJSRuntime JS => BlazorJSRuntime.JS;

        /// <inheritdoc/>
        public OnnxRuntime(IJSInProcessObjectReference _ref) : base(_ref) { }

        /// <summary>
        /// Initialize the ONNX Runtime Web module by loading it from CDN.
        /// Returns the module reference which provides access to Tensor, InferenceSession, and env.
        /// </summary>
        /// <param name="srcUrl">Optional CDN URL override. Defaults to the latest WebGPU bundle.</param>
        /// <returns>The loaded OnnxRuntime module.</returns>
        public static async Task<OnnxRuntime> Init(string? srcUrl = null)
        {
            Console.WriteLine(">> OnnxRuntime.Init");
            srcUrl ??= LatestBundledVersionSrc;
            var ort = await JS.Import<OnnxRuntime>(GlobalModuleName, srcUrl);
            if (ort == null)
                throw new Exception("ONNX Runtime Web could not be initialized.");
            Console.WriteLine("<< OnnxRuntime.Init");
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
