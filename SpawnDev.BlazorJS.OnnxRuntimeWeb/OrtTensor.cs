using Microsoft.JSInterop;
using SpawnDev.BlazorJS.JSObjects;

namespace SpawnDev.BlazorJS.OnnxRuntimeWeb
{
    /// <summary>
    /// Wraps the ONNX Runtime Web Tensor object.
    /// Represents multi-dimensional arrays for model inference input/output.
    /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/Tensor-1.html"/>
    /// </summary>
    public class OrtTensor : JSObject
    {
        /// <inheritdoc/>
        public OrtTensor(IJSInProcessObjectReference _ref) : base(_ref) { }
        /// <summary>
        /// Initializes a new instance of the OrtTensor class using the specified data type, data values, and tensor
        /// shape.
        /// </summary>
        /// <remarks>Ensure that the data array length matches the total number of elements implied by the
        /// shape. Mismatched data and shape may result in runtime errors when using the tensor in ONNX
        /// operations.</remarks>
        /// <param name="dataType">The data type of the tensor elements, specified as a string (for example, "float" or "int").</param>
        /// <param name="data">A typed array containing the values to populate the tensor.</param>
        /// <param name="shape">An array of long integers that defines the dimensions of the tensor. The product of the dimensions must
        /// match the length of the data array.</param>
        public OrtTensor(string dataType, TypedArray data, long[] shape) : base(JS.New($"{OnnxRuntime.GlobalModuleName}.Tensor", dataType, data, shape)) { }
        /// <summary>
        /// The dimensions of the tensor.
        /// </summary>
        public long[] Dims => JSRef!.Get<long[]>("dims");

        /// <summary>
        /// The data type of the tensor (e.g., "float32", "int32", "uint8").
        /// </summary>
        public string Type => JSRef!.Get<string>("type");

        /// <summary>
        /// The number of elements in the tensor.
        /// </summary>
        public long Size => JSRef!.Get<long>("size");

        /// <summary>
        /// The data location of the tensor.
        /// Possible values: "none", "cpu", "cpu-pinned", "texture", "gpu-buffer", "ml-tensor".
        /// </summary>
        public string Location => JSRef!.Get<string>("location");

        /// <summary>
        /// Get the WebGPU buffer that holds the tensor data.
        /// Throws if the data is not on GPU as a WebGPU buffer.
        /// </summary>
        public GPUBuffer GPUBuffer => JSRef!.Get<GPUBuffer>("gpuBuffer");

        /// <summary>
        /// Get the tensor data. Throws if tensor is on GPU (use GetDataAsync for GPU tensors).
        /// </summary>
        /// <typeparam name="TData">The typed array type (Float32Array, Int32Array, etc.)</typeparam>
        public TData GetData<TData>() => JSRef!.Get<TData>("data");

        /// <summary>
        /// Get the tensor data asynchronously. Downloads from GPU if needed.
        /// </summary>
        /// <typeparam name="TData">The typed array type.</typeparam>
        /// <param name="releaseData">If true, release the data on GPU after download.</param>
        public Task<TData> GetDataAsync<TData>(bool releaseData = false)
            => JSRef!.CallAsync<TData>("getData", releaseData);

        /// <summary>
        /// Dispose the tensor data. For GPU tensors, this releases the GPU buffer.
        /// </summary>
        public void DisposeData() => JSRef!.CallVoid("dispose");

        /// <summary>
        /// Create a new tensor with the same data buffer and specified dims.
        /// </summary>
        /// <param name="dims">New dimensions. Size should match the old one.</param>
        public OrtTensor Reshape(long[] dims) => JSRef!.Call<OrtTensor>("reshape", dims);
    }
}
