# SpawnDev.BlazorJS.OnnxRuntimeWeb

[![NuGet](https://img.shields.io/nuget/dt/SpawnDev.BlazorJS.OnnxRuntimeWeb.svg?label=SpawnDev.BlazorJS.OnnxRuntimeWeb)](https://www.nuget.org/packages/SpawnDev.BlazorJS.OnnxRuntimeWeb)

**SpawnDev.BlazorJS.OnnxRuntimeWeb** brings GPU-accelerated machine learning inference to Blazor WebAssembly using ONNX Runtime Web with WebGPU support.

This library provides full C# bindings for [ONNX Runtime Web](https://onnxruntime.ai/docs/api/js/), enabling you to run ONNX models directly in the browser with hardware acceleration through WebGPU.

## Features

- 🚀 **GPU-Accelerated Inference** - Leverage WebGPU for high-performance ML inference
- 🔗 **Zero-Copy Buffer Sharing** - Share GPU buffers directly between ONNX Runtime and other WebGPU libraries (e.g., ILGPU)
- 💪 **Strongly-Typed C# API** - Full IntelliSense support with comprehensive XML documentation
- 🎯 **Built on SpawnDev.BlazorJS** - Synchronous JavaScript interop for natural C# usage
- 📦 **Bundled Runtime** - Includes ONNX Runtime Web 1.24.1 with WebGPU bundle
- 🌐 **Multi-Framework Support** - Targets .NET 8, 9, and 10

## Installation

```bash
dotnet add package SpawnDev.BlazorJS.OnnxRuntimeWeb
```

## Getting Started

### 1. Configure Your Blazor WebAssembly App

In your `Program.cs`:

```csharp
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.OnnxRuntimeWeb;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add BlazorJS services
builder.Services.AddBlazorJSRuntime();

// Initialize BlazorJS
await builder.Build().BlazorJSRunAsync();
```

### 2. Initialize ONNX Runtime

```csharp
@inject BlazorJSRuntime JS

@code {
    OnnxRuntime? ort;
    OrtInferenceSession? session;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Initialize ONNX Runtime Web module
            ort = await OnnxRuntime.Init();

            // Configure environment (optional)
            var env = ort.GetEnvironment();
            env.LogLevel = "warning";

            // Create an inference session
            session = await ort.CreateSessionAsync("path/to/model.onnx", new SessionCreateOptions 
            {
                ExecutionProviders = new[] { "webgpu", "wasm" },
                GraphOptimizationLevel = "all"
            });

            StateHasChanged();
        }
    }
}
```

### 3. Run Inference

```csharp
// Create input tensor
using var inputData = JS.NewFloat32Array(new float[] { /* your data */ });
using var inputTensor = ort.CreateTensor("float32", inputData, new long[] { 1, 3, 224, 224 });

// Prepare feeds (input dictionary)
using var feeds = JS.NewObject();
feeds.Set("input", inputTensor);

// Run inference
using var results = await session.RunAsync(feeds);

// Get output tensor
var outputTensor = results.Get<OrtTensor>("output");
var outputData = await outputTensor.GetDataAsync<Float32Array>();

// Process results
Console.WriteLine($"Output dims: [{string.Join(", ", outputTensor.Dims)}]");
```

## WebGPU Buffer Sharing

One of the most powerful features is zero-copy buffer sharing with WebGPU:

```csharp
// Keep tensors on GPU for zero-copy operations
var runOptions = new SessionRunOptions 
{ 
    PreferredOutputLocation = "gpu-buffer" 
};

using var results = await session.RunAsync(feeds, runOptions);
var outputTensor = results.Get<OrtTensor>("output");

// Check location
Console.WriteLine($"Output location: {outputTensor.Location}"); // "gpu-buffer"

// Access the GPU buffer directly
using var gpuBuffer = outputTensor.GPUBuffer;

// Use with other WebGPU libraries (ILGPU, etc.)
// No data download required!
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

- **`OnnxRuntime`** - Module loader and entry point
- **`OrtInferenceSession`** - Inference session for model execution
- **`OrtTensor`** - Multi-dimensional tensor with GPU buffer support
- **`OrtEnvironment`** - Global configuration and environment settings
- **`SessionCreateOptions`** - Options for creating inference sessions
- **`SessionRunOptions`** - Options for running inference

### Key Methods

```csharp
// Initialize ONNX Runtime
OnnxRuntime ort = await OnnxRuntime.Init();

// Create session from URL or ArrayBuffer
OrtInferenceSession session = await ort.CreateSessionAsync(modelPath, options);

// Create tensors
OrtTensor tensor = ort.CreateTensor(type, data, dims);

// Run inference
OrtSessionResult results = await session.RunAsync(feeds);
OrtSessionResult results = await session.RunAsync(feeds, runOptions);

// Access environment
OrtEnvironment env = ort.GetEnvironment();
env.LogLevel = "verbose";
```

## Requirements

- **Blazor WebAssembly** - This library only works in browser-based WASM projects
- **WebGPU Support** - For GPU acceleration, the browser must support WebGPU
  - Chrome 113+
  - Edge 113+
  - Other browsers: check [WebGPU compatibility](https://caniuse.com/webgpu)
- **SpawnDev.BlazorJS 3.0+** - Core JavaScript interop library

## Browser Compatibility

| Feature | Chrome/Edge | Firefox | Safari |
|---------|-------------|---------|--------|
| WASM Backend | ✅ | ✅ | ✅ |
| WebGPU Backend | ✅ 113+ | 🔄 Experimental | 🔄 In Development |

## Example: Image Classification

```csharp
@page "/classify"
@inject BlazorJSRuntime JS

<h3>Image Classification</h3>

@if (session != null)
{
    <input type="file" @ref="fileInput" accept="image/*" />
    <button @onclick="Classify">Classify Image</button>

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
    ElementReference fileInput;
    OnnxRuntime? ort;
    OrtInferenceSession? session;
    string? result;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ort = await OnnxRuntime.Init();
            session = await ort.CreateSessionAsync("models/mobilenet.onnx", new SessionCreateOptions
            {
                ExecutionProviders = new[] { "webgpu", "wasm" }
            });
            StateHasChanged();
        }
    }

    async Task Classify()
    {
        // Preprocess image and create input tensor
        // (preprocessing code omitted for brevity)

        using var inputTensor = ort!.CreateTensor("float32", preprocessedData, new long[] { 1, 3, 224, 224 });
        using var feeds = JS.NewObject();
        feeds.Set(session!.InputNames[0], inputTensor);

        // Run inference
        using var results = await session.RunAsync(feeds);
        using var outputTensor = results.Get<OrtTensor>(session.OutputNames[0]);
        using var outputData = await outputTensor.GetDataAsync<Float32Array>();

        // Find top prediction
        var scores = outputData.ToArray();
        var maxIndex = Array.IndexOf(scores, scores.Max());
        result = $"Class {maxIndex} ({scores[maxIndex]:P2})";

        StateHasChanged();
    }
}
```

## Documentation

- [ONNX Runtime Web Documentation](https://onnxruntime.ai/docs/api/js/)
- [SpawnDev.BlazorJS Documentation](https://github.com/LostBeard/SpawnDev.BlazorJS)
- [WebGPU Specification](https://www.w3.org/TR/webgpu/)

## Related Projects

- [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS) - Core JavaScript interop for Blazor WebAssembly
- [SpawnDev.BlazorJS.WebGPU](https://github.com/LostBeard/SpawnDev.BlazorJS.WebGPU) - WebGPU bindings for Blazor

## Contributing

Issues and pull requests are welcome! Please visit the [GitHub repository](https://github.com/LostBeard/SpawnDev.BlazorJS.OnnxRuntimeWeb).

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Credits

- Built on [ONNX Runtime Web](https://onnxruntime.ai/) by Microsoft
- Powered by [SpawnDev.BlazorJS](https://github.com/LostBeard/SpawnDev.BlazorJS)
- Created by [LostBeard](https://github.com/LostBeard)
