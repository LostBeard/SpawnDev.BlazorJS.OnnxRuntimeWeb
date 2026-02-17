using System.Text.Json.Serialization;

namespace SpawnDev.BlazorJS.OnnxRuntimeWeb
{
    /// <summary>
    /// Options for creating a tensor from a WebGPU GPU buffer.
    /// <see href="https://onnxruntime.ai/docs/api/js/interfaces/TensorFromGpuBufferOptions.html"/>
    /// </summary>
    public class TensorFromGpuBufferOptions
    {
        /// <summary>
        /// The data type of the tensor. If omitted, defaults to "float32".
        /// </summary>
        [JsonPropertyName("dataType")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DataType { get; set; } = "float32";

        /// <summary>
        /// The dimensions of the tensor. Required.
        /// </summary>
        [JsonPropertyName("dims")]
        public long[] Dims { get; set; } = new long[] { 1 };
    }
}
