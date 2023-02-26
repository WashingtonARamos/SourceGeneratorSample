using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using MySourceGenerator.Helpers;

namespace MySourceGenerator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var builder = new CodeBuilder();
            var additionalFiles = context.AdditionalFiles.Where(s => s.Path.EndsWith(".sql"));
            var files = FilesOrganizer.Organize(additionalFiles, context.Compilation.Assembly.Name);

            builder.DeclareNamespace("SqlGeneratedCode");
            ProcessFile(files, builder);
            builder.CloseScope();

            // the "*.g.cs" naming scheme specifies that a class has been automatically
            // generated, so the programmer knows it shouldn't be modified
            context.AddSource($"SqlGeneratedSources.g.cs", builder.GetText());
        }

        private void ProcessFile(Dictionary<string, object> files, CodeBuilder builder)
        {
            foreach (var o in files)
            {
                if (o.Value is Dictionary<string, object> cur)
                {
                    builder.DeclareClassAndOpenScope(o.Key);
                    ProcessFile(cur, builder);
                    builder.CloseScope();
                    continue;
                }

                builder.DeclareConstString(o.Key.Replace(".sql", ""), o.Value.ToString());
            }
        }
    }
}