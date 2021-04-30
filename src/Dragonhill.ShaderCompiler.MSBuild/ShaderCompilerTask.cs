using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Dragonhill.ShaderCompiler.MSBuild
{
    public class ShaderCompilerTask : Task
    {
        [Required]
        public ITaskItem[] InputFiles { get; set; }
        
        [Required]
        public ITaskItem[] OutputFiles { get; set; }
        
        public override bool Execute()
        {
            var vulkanSdkPath = Environment.GetEnvironmentVariable("VULKAN_SDK");

            if (string.IsNullOrWhiteSpace(vulkanSdkPath))
            {
                Log.LogError("Could not find the vulkan SDK environment variable (VULKAN_SDK)");
                return false;
            }

            if (!Directory.Exists(vulkanSdkPath))
            {
                Log.LogError("The vulkan SDK directory does not exist");
                return false;
            }

            var binPath = Path.Combine(vulkanSdkPath, Environment.Is64BitOperatingSystem ? "bin" : "bin32");
            if (!Directory.Exists(binPath))
            {
                Log.LogError("The vulkan SDK bin directory does not exist");
                return false;
            }

            var compilerCandidates = Directory.GetFiles(binPath, "glslc*");
            if (compilerCandidates.Length == 0)
            {
                Log.LogError($"glslc compiler was not found in '{binPath}'");
                return false;
            }

            if (compilerCandidates.Length > 1)
            {
                Log.LogError($"More than one glslc file was found in '{binPath}'");
                return false;
            }

            var glslcPath = compilerCandidates[0];
            var cwd = Directory.GetCurrentDirectory();

            for (var i = 0; i < InputFiles.Length; i++)
            {
                var inputFile = InputFiles[i].ItemSpec;
                var outputFile = OutputFiles[i].ItemSpec;

                var inputFileAbsolute = Path.Combine(cwd, inputFile);
                var outputFileAbsolute = Path.Combine(cwd, outputFile);

                var outputDir = Path.GetDirectoryName(outputFile);

                Directory.CreateDirectory(outputDir);

                var processStartInfo = new ProcessStartInfo(glslcPath)
                    {
                        Arguments = $"\"{inputFileAbsolute}\" -o \"{outputFileAbsolute}\"",
                        UseShellExecute = false,
                        RedirectStandardError = true
                    };

                using (var process = Process.Start(processStartInfo))
                {
                    if (process == null)
                    {
                        Log.LogError($"Couldn't start glslc for '{inputFile}'");
                        return false;
                    }
                    
                    process.WaitForExit();
                    
                    var errorMessage = process.StandardError.ReadToEnd();

                    if (process.ExitCode != 0)
                    {
                        if (string.IsNullOrWhiteSpace(errorMessage))
                        {
                            Log.LogError($"Error running glslc for '{inputFile}'");
                            return false;
                        }
                        
                        Log.LogError(errorMessage);
                        return false;
                    }

                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        Log.LogWarning(errorMessage);
                    }
                }
            }

            return true;
        }
    }
}