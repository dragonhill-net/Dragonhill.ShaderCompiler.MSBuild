using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Dragonhill.ShaderCompiler.MSBuild
{
    public class GenerateResourceEntriesTask : Task
    {
        [Required]
        public ITaskItem[] InputFiles { get; set; }
		
        [Required]
        public ITaskItem[] OutputFiles { get; set; }

        [Output]
        public ITaskItem[] ResourceFiles { get; set; }

        public override bool Execute()
        {
            ResourceFiles = new ITaskItem[InputFiles.Length];

            for (var i = 0; i < InputFiles.Length; i++)
            {
                ResourceFiles[i] = new TaskItem(OutputFiles[i].ItemSpec, new Dictionary<string, string>
                    {
                        {"LogicalName", $"shader:/{InputFiles[i].ItemSpec.Replace(Path.DirectorySeparatorChar, '/')}.spv"}
                    });
            }

            return true;
        }
    }
}