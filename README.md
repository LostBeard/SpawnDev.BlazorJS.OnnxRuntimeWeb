# SpawnDev.BlazorJS.OnnxRuntimeWeb

[![NuGet](https://img.shields.io/nuget/dt/SpawnDev.BlazorJS.OnnxRuntimeWeb.svg?label=SpawnDev.BlazorJS.OnnxRuntimeWeb)](https://www.nuget.org/packages/SpawnDev.BlazorJS.OnnxRuntimeWeb)

**SpawnDev.BlazorJS.OnnxRuntimeWeb** brings GPU-accelerated machine learning inference to Blazor WebAssembly using ONNX Runtime Web with WebGPU support.

This library provides full C# bindings for [ONNX Runtime Web](https://onnxruntime.ai/docs/api/js/), enabling you to run ONNX models directly in the browser with hardware acceleration through WebGPU.

## Features

- 🚀 **GPU-Accelerated Inference** — Leverage WebGPU for high-performance ML inference
- 🔗 **Zero-Copy Buffer Sharing** — Share GPU buffers directly between ONNX Runtime and other WebGPU libraries (e.g., ILGPU)
- 💪 **Strongly-Typed C# API** — Full IntelliSense support with comprehensive XML documentation
- 🎯 **Built on SpawnDev.BlazorJS** — Synchronous JavaScript interop for natural C# usage
- 📦 **Bundled Runtime** — Includes ONNX Runtime Web 1.24.1 with WebGPU bundle
- 🌐 **Multi-Framework Support** — Targets .NET 8, 9, and 10

## Installation

```bash
dotnet add package SpawnDev.BlazorJS.OnnxRuntimeWeb
```

## Getting Started

### 1. Configure Your Blazor WebAssembly App

In your `Program.cs`:

```csharp
using SpawnDev.BlazorJS;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add BlazorJS services
builder.Services.AddBlazorJSRuntime();

// Initialize BlazorJS
await builder.Build().BlazorJSRunAsync();
```

### 2. Initialize ONNX Runtime and Create a Session

```csharp
@inject BlazorJSRuntime JS

@code {
    OnnxRuntime? ort;
    OrtInferenceSession? session;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        // Initialize ONNX Runtime Web module
        ort = await OnnxRuntime.Init();

        // Configure environment (optional)
        using var env = ort.Env;
        env.LogLevel = "warning";

        // Create an inference session from a model URL
        session = await ort.CreateInferenceSessionAsync("models/my-model.onnx", new SessionCreateOptions
        {
            ExecutionProviders = new[] { "webgpu", "wasm" },
            GraphOptimizationLevel = "all",
            LogSeverityLevel = 3
        });

        StateHasChanged();
    }
}
```

You can also create a session from an `ArrayBuffer` (e.g., fetched model bytes):

```csharp
using var response = await JS.Fetch("models/my-model.onnx");
using var arrayBuffer = await response.ArrayBuffer();

session = await ort.CreateInferenceSessionAsync(arrayBuffer, new SessionCreateOptions
{
    LogSeverityLevel = 3
});
```

### 3. Run Inference

```csharp
// Create input tensor
using var inputData = new Float32Array(new float[] { /* your data */ });
using var inputTensor = new OrtTensor("float32", inputData, new long[] { 1, 3, 224, 224 });

// Prepare feeds (maps input names to tensors)
using var feeds = new OrtFeeds();
feeds.Set(session.InputNames[0], inputTensor);

// Run inference
using var result = await session.Run(feeds);

// Get output tensor
using var outputTensor = result.GetTensor(session.OutputNames[0]);
using var outputData = outputTensor.GetData<Float32Array>();

// Process results
Console.WriteLine($"Output dims: [{string.Join(", ", outputTensor.Dims)}]");
Console.WriteLine($"Output type: {outputTensor.Type}");
Console.WriteLine($"Output size: {outputTensor.Size} elements");
```

## WebGPU Buffer Sharing

One of the most powerful features is zero-copy buffer sharing with WebGPU:

```csharp
// Keep tensors on GPU for zero-copy operations
using var result = await session.Run(feeds, new SessionRunOptions
{
    PreferredOutputLocation = "gpu-buffer"
});
using var outputTensor = result.GetTensor(session.OutputNames[0]);

// Check location
Console.WriteLine($"Output location: {outputTensor.Location}"); // "gpu-buffer"

// Access the GPU buffer directly
using var gpuBuffer = outputTensor.GPUBuffer;

// Use with other WebGPU libraries (ILGPU, etc.)
// No data download required!
```

You can also create tensors directly from GPU buffers:

```csharp
// Create a tensor backed by an existing WebGPU buffer
using var tensor = ort.TensorFromGpuBuffer(myGpuBuffer, new TensorFromGpuBufferOptions
{
    DataType = "float32",
    Dims = new long[] { 1, 3, 224, 224 }
});
```

## CDN vs Bundled Runtime

By default, the library uses the bundled ONNX Runtime Web (included in the NuGet package). You can also load from CDN:

```csharp
// Use bundled version (default)
var ort = await OnnxRuntime.Init();

// Or use CDN version
var ort = await OnnxRuntime.Init(OnnxRuntime.LatestCDNVersionSrc);

// Or use a custom URL
var ort = await OnnxRuntime.Init("https://your-cdn.com/ort.webgpu.bundle.min.mjs");
```

## API Overview

### Main Classes

| Class | Description |
|-------|-------------|
| `OnnxRuntime` | Module loader, entry point, and tensor factory |
| `OrtInferenceSession` | Inference session for model execution |
| `OrtTensor` | Multi-dimensional tensor with GPU buffer support |
| `OrtFeeds` | Strongly-typed feeds object (maps input names → tensors) |
| `OrtSessionResult` | Result object (maps output names → tensors) |
| `OrtEnvironment` | Global configuration and environment settings |
| `SessionCreateOptions` | Options for creating inference sessions |
| `SessionRunOptions` | Options for running inference |
| `TensorFromGpuBufferOptions` | Options for creating tensors from GPU buffers |

### Key Methods

```csharp
// ── Initialize ──
OnnxRuntime ort = await OnnxRuntime.Init();

// ── Create Session ──
// From URL:
OrtInferenceSession session = await ort.CreateInferenceSessionAsync(modelUrl, options);
// From ArrayBuffer:
OrtInferenceSession session = await ort.CreateInferenceSessionAsync(arrayBuffer, options);

// ── Create Tensors ──
// Via constructor:
var tensor = new OrtTensor("float32", float32Array, new long[] { 1, 3, 224, 224 });
// Via factory (for GPU buffers):
var tensor = ort.TensorFromGpuBuffer(gpuBuffer, gpuBufferOptions);

// ── Run Inference ──
OrtSessionResult result = await session.Run(feeds);
OrtSessionResult result = await session.Run(feeds, runOptions);

// ── Access Results ──
OrtTensor output = result.GetTensor(session.OutputNames[0]);
Float32Array data = output.GetData<Float32Array>();          // CPU tensor
Float32Array data = await output.GetDataAsync<Float32Array>(); // GPU tensor (downloads)

// ── Access Environment ──
using var env = ort.Env;
env.LogLevel = "verbose";
```

## Requirements

- **Blazor WebAssembly** — This library only works in browser-based WASM projects
- **WebGPU Support** — For GPU acceleration, the browser must support WebGPU
  - Chrome 113+
  - Edge 113+
  - Other browsers: check [WebGPU compatibility](https://caniuse.com/webgpu)
- **SpawnDev.BlazorJS 3.0+** — Core JavaScript interop library

## Browser Compatibility

| Feature | Chrome/Edge | Firefox | Safari |
|---------|-------------|---------|--------|
| WASM Backend | ✅ | ✅ | ✅ |
| WebGPU Backend | ✅ 113+ | 🔄 Experimental | 🔄 In Development |

## Example: Image Classification with SqueezeNet

```csharp
@page "/classify"
@inject BlazorJSRuntime JS

<h3>Image Classification</h3>

@if (session != null)
{
    <label class="btn btn-primary" style="cursor: pointer;">
        Upload Image
        <InputFile OnChange="OnFileSelected" accept="image/*" style="display: none;" />
    </label>

    @if (result != null)
    {
        <p>Prediction: @result</p>
    }
}
else
{
    <p>Loading model...</p>
}

@code {
    OnnxRuntime? ort;
    OrtInferenceSession? session;
    string? result;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        ort = await OnnxRuntime.Init();
        session = await ort.CreateInferenceSessionAsync("models/squeezenet1.0-12.onnx", new SessionCreateOptions
        {
            LogSeverityLevel = 3
        });
        StateHasChanged();
    }

    async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file == null || ort == null || session == null) return;

        // Read image as data URL
        var buffer = new byte[file.Size];
        await file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).ReadExactlyAsync(buffer);
        var dataUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}";

        // Load and resize image using OffscreenCanvas
        using var img = await HTMLImageElement.CreateFromImageAsync(dataUrl);
        using var canvas = new OffscreenCanvas(224, 224);
        using var ctx = canvas.Get2DContext();
        ctx.DrawImage(img, 0, 0, 224, 224);

        // Get pixel data and preprocess for SqueezeNet (NCHW, ImageNet normalization)
        using var imageData = ctx.GetImageData(0, 0, 224, 224);
        using var rgba = imageData.Data;

        float[] mean = { 0.485f, 0.456f, 0.406f };
        float[] std  = { 0.229f, 0.224f, 0.225f };

        var inputValues = new float[3 * 224 * 224];
        for (int y = 0; y < 224; y++)
        {
            for (int x = 0; x < 224; x++)
            {
                int pixelIdx = (y * 224 + x) * 4;
                for (int c = 0; c < 3; c++)
                {
                    float val = rgba[(uint)(pixelIdx + c)] / 255f;
                    inputValues[c * 224 * 224 + y * 224 + x] = (val - mean[c]) / std[c];
                }
            }
        }

        // Create tensor and run inference
        using var floatData = new Float32Array(inputValues);
        using var tensor = new OrtTensor("float32", floatData, new long[] { 1, 3, 224, 224 });

        using var feeds = new OrtFeeds();
        feeds.Set(session.InputNames[0], tensor);

        using var results = await session.Run(feeds);
        using var outputTensor = results.GetTensor(session.OutputNames[0]);
        using var outputData = outputTensor.GetData<Float32Array>();

        // Softmax and find top prediction
        var logits = new float[1000];
        for (int i = 0; i < 1000; i++) logits[i] = outputData[i];

        float maxLogit = logits.Max();
        var exps = logits.Select(l => (float)Math.Exp(l - maxLogit)).ToArray();
        float sumExp = exps.Sum();

        int maxIndex = 0;
        float maxProb = 0;
        for (int i = 0; i < 1000; i++)
        {
            float prob = exps[i] / sumExp;
            if (prob > maxProb) { maxProb = prob; maxIndex = i; }
        }

        result = $"Class {maxIndex} ({maxProb:P2})";
        StateHasChanged();
    }
}
```

## Documentation

- [ONNX Runtime Web Documentation](https://onnxruntime.ai/docs/api/js/)
- [SpawnDev.BlazorJS Documentation](https://github.com/LostBeard/SpawnDev.BlazorJS)
- [WebGPU Specification](https://www.w3.org/TR/webgpu/)

## Related Projects

- [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS) — Core JavaScript interop for Blazor WebAssembly
- [SpawnDev.ILGPU](https://github.com/LostBeard/SpawnDev.ILGPU) — ILGPU backends (WebGPU, WebGL, Wasm) for Blazor

## Contributing

Issues and pull requests are welcome! Please visit the [GitHub repository](https://github.com/LostBeard/SpawnDev.BlazorJS.OnnxRuntimeWeb).

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Credits

- Built on [ONNX Runtime Web](https://onnxruntime.ai/) by Microsoft
- Powered by [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS)
- Created by [LostBeard](https://github.com/LostBeard)
