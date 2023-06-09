using System.Collections.Immutable;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;

namespace SourceGenerator.AppSettingsGenerator;

[Generator]
public class AppSettingsIncrementalGenerator  : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var textFiles = context.AdditionalTextsProvider.Where(static file => file.Path.Contains("appsettings.json"));
        var contents = textFiles.Select(Parse).Where(static m => m is not null).Collect();
        context.RegisterSourceOutput(contents,  (productionContext, array) => Execute(productionContext, array!));
    }

    static JsonDocument? Parse(AdditionalText text, CancellationToken ct)
    {
        var t = text.GetText(ct);
        if (t == null)
        {
            return null;
        }
        try
        {
            return JsonDocument.Parse(t.ToString());
        }
        catch 
        {
            return null;
        }
    }

    static void Execute(SourceProductionContext context, ImmutableArray<JsonDocument> nodes)
    {
        var appSettings = nodes.FirstOrDefault();
        
        if (appSettings?.RootElement.ValueKind != JsonValueKind.Object)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("APP100", "Appsettings", "Expected root element to be object, got {0}", "", DiagnosticSeverity.Warning, true),
                Location.None, appSettings?.RootElement.ValueKind.ToString() ?? "NULL"));
            return;
        }

        var sb = new StringBuilder("""
namespace GeneratedAppSettings { 
    public static class AppSettings {

""");

        foreach (var property in appSettings.RootElement.EnumerateObject())
        {
            switch (property.Value.ValueKind)
            {
                case JsonValueKind.Object:
                    // TODO Implement
                    break;
                default:
                    sb.AppendLine($"""public const string {property.Name} = "{property.Name}";""");
                    break;
            }
        }

        sb.AppendLine("""
    }
}
""");
        context.AddSource("AppSettings.g.cs", sb.ToString());
        
    }
}