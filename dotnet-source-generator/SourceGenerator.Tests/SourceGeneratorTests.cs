using System.ComponentModel;
using GeneratedAppSettings;

namespace SourceGenerator.Tests;

public class SourceGeneratorTests
{
    [Fact, DisplayName("Key in AppSettings becomes constant in static class")]
    public void AppSettingsGenerator_KeyInAppsettingsBecomesConstantInStaticClass()
    {
        Assert.Equal("TestValue", AppSettings.TestValue);
    }

    [Fact, DisplayName("Content of text file becomes constant string in static class")]
    public void LookupGenerator_TextFileContentBecomesConstantStringInStaticClass()
    {
        Assert.Equal("Some text", ConstStrings.Constant1);
    }
}