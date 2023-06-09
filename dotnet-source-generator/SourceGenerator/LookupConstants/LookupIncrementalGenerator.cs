using Microsoft.CodeAnalysis;

namespace SourceGenerator.LookupConstants;

[Generator]
public class LookupIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var textFiles = context.AdditionalTextsProvider.Where(static file => file.Path.EndsWith(".txt"));
        var namesAndContents = textFiles.Select((text, ct) =>
            (name: Path.GetFileNameWithoutExtension(text.Path), text: text.GetText(ct)!.ToString())).Collect();
        
        context.RegisterSourceOutput(namesAndContents, (spc, nameAndContent) =>
        {
            var constants = string.Join("\n", nameAndContent.Select(t => $"""public const string {t.name} =  "{t.text}";"""));
            spc.AddSource($"ConstStrings.g.cs", $$"""
public static partial class ConstStrings 
{
    {{constants}}
}
""");
        });
    }

}