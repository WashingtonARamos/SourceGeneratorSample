using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MySourceGenerator.Helpers
{
    public static class FilesOrganizer
    {
        public static Dictionary<string, object> Organize(IEnumerable<AdditionalText> additionalFiles, string assemblyName)
        {
            var sqlFiles = additionalFiles
                .Where(s => s.Path.EndsWith(".sql"))
                .OrderBy(s => s.Path);

            var tree = new Dictionary<string, object>();

            foreach (var file in sqlFiles)
            {
                var pathBaseIndex = file.Path.IndexOf(assemblyName);
                var pathBase = file.Path.Substring(pathBaseIndex).Replace(assemblyName + "\\", "");
                var pathSplit = pathBase.Split('\\');
                
                var currentNode = tree;
                foreach (var part in pathSplit)
                {
                    if (part.EndsWith(".sql"))
                    {
                        currentNode.Add(part, file.GetText());
                        break;
                    }

                    if (currentNode.TryGetValue(part, out var node))
                    {
                        currentNode = node as Dictionary<string, object>;
                        continue;
                    }

                    var nextNode = new Dictionary<string, object>();
                    currentNode.Add(part, nextNode);
                    currentNode = nextNode;
                }
            }

            return tree;
        }
    }
}