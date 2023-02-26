using System;
using System.Text;

namespace MySourceGenerator.Helpers
{
    public class CodeBuilder
    {
        private readonly StringBuilder _builder;
        private int _indentationLevel;

        public CodeBuilder()
        {
            _builder = new StringBuilder();
        }

        public string GetText() => _builder.ToString();

        public void DeclareNamespace(string namespaceName)
        {
            _builder.AppendLine($"namespace {namespaceName}");
            OpenScope();
        }

        public void DeclareClassAndOpenScope(string className)
        {
            AppendIndentation();
            _builder.AppendLine($"public sealed class {className}");
            OpenScope();
        }

        public void DeclareConstString(string stringName, string stringValue)
        {
            AppendIndentation();
            _builder.AppendLine(
                $"public const string {stringName} = \"{stringValue.Replace(Environment.NewLine, " ")}\";");
        }

        public void CloseScope()
        {
            _indentationLevel--;
            AppendIndentation();
            _builder.Append('}');
            _builder.AppendLine();
        }

        private void OpenScope()
        {
            AppendIndentation();
            _builder.Append('{');
            _indentationLevel++;
            _builder.AppendLine();
        }

        private void AppendIndentation()
        {
            for (int i = 0; i < _indentationLevel * 4; i++)
            {
                _builder.Append(' ');
            }
        }
    }
}