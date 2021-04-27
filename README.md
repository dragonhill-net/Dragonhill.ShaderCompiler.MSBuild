# MSBuild SPIR-V Shader Compiler ![Nuget](https://img.shields.io/nuget/v/Dragonhill.ShaderCompiler.MSBuild)

## Installation

A [nuget package](https://www.nuget.org/packages/Dragonhill.ShaderCompiler.MSBuild/) is available. The package name is ```Dragonhill.ShaderCompiler.MSBuild```.

## Requirements

You need to have the [Vulkan SDK](https://www.lunarg.com/vulkan-sdk/) installed and the ```VULKAN_SDK``` environment variable pointing to it.

The ```glslc``` compiler is then used to compile the shaders to the SPIR-V format.

## Usage

Automatically compiles and creates embedded resources for all shader files (```*.vert```, ```*.frag```, ```*.tesc```, ```*.tese```, ```*.geom```, ```*.comp```) in the project.

The name of the generated resources is ```shader:/<relative path of the shader file>.spv```.

For example a file ```Shaders/test.frag``` would be embedded as ```shader:/Shaders/test.frag.spv```.

## Creating a release

### With a release git tag available

```
dotnet msbuild -t:ReleasePackGitTag .\src\Dragonhill.ShaderCompiler.MSBuild\Dragonhill.ShaderCompiler.MSBuild.csproj
```

### Manual version specification

```
dotnet msbuild /p:Version=1.0.0-pre1 -t:pack .\src\Dragonhill.ShaderCompiler.MSBuild\Dragonhill.ShaderCompiler.MSBuild.csproj
```

## Using the Rider Debug configuration

```.run/rider-debug-config.run.xml``` contains a debug configuration for JetBrains Rider.

A custom path variable (```DOTNET_MSBUILD_PATH```) has to be define prior to using it. It must point to your dotnet msbuild.dll (e.g. ```C:\Program Files\dotnet\sdk\5.0.202\MSBuild.dll```).