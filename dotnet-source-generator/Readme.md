# Dotnet Source Generator Sample

This sample demonstrates two incremental source generators:
- Generator that creates a static class for all app settings keys for easy reference in code.
- Generator that creates a static class with constants corresponding to all .txt-documents and their content.

## Incremental generators vs regular generators
Incremental generators utilizes caching between each step of the execution pipeline, which should improve source generator performance.

## Notes
To register a project as a source generator, it is important that the project reference specifies the correct `OutputItemType`:
```xml
<ProjectReference Include="..\SourceGenerator\SourceGenerator.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false"  />
```

## Further reading
https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md