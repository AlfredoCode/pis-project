using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace PRegSys.SourceGenerators;

[Generator]
public class DtoExtensionMethodGenerator : IIncrementalGenerator
{
    public const string AttributeName = "GenerateDtoExtensions";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        const string attributeClassName = $"{AttributeName}Attribute";

        context.RegisterPostInitializationOutput(ctx => {
            ctx.AddSource("GenerateDtoExtensionsAttribute.g.cs", $$"""
                using System;
                using System.Diagnostics.CodeAnalysis;

                using PRegSys.API.DTO;
                using PRegSys.DAL.Entities;

                namespace PRegSys;

                [AttributeUsage(AttributeTargets.Class)]
                sealed class {{attributeClassName}}<TEntity, TReadDto, TWriteDto> : Attribute
                    where TEntity : IEntity
                    where TReadDto : IReadDto<TReadDto, TEntity>
                    where TWriteDto : IWriteDto<TWriteDto, TEntity>;

                [AttributeUsage(AttributeTargets.Class)]
                sealed class {{attributeClassName}}<TEntity, TReadDto> : Attribute
                    where TEntity : IEntity
                    where TReadDto : IReadDto<TReadDto, TEntity>;
                """);
        });

        var rwPipeline = getPipelineForAttribute(context, $"PRegSys.{attributeClassName}`3");
        var roPipeline = getPipelineForAttribute(context, $"PRegSys.{attributeClassName}`2");

        context.RegisterSourceOutput(rwPipeline, GenerateSourceCode);
        context.RegisterSourceOutput(roPipeline, GenerateSourceCode);

        static IncrementalValuesProvider<Model?> getPipelineForAttribute(
            IncrementalGeneratorInitializationContext context, string attributeMetadataName)
            => context.SyntaxProvider.ForAttributeWithMetadataName(attributeMetadataName,
                static (node, _) => node is ClassDeclarationSyntax,
                static (context, _) => {
                    var extensionsClass = context.TargetSymbol;
                    var symbolDisplayFormat = SymbolDisplayFormat.FullyQualifiedFormat
                        .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);

                    if (context.Attributes[0].AttributeClass!.TypeArguments
                            is not [{ } entityType, { } readDtoType, .. { Length: <= 1 } rest])
                        return null;

                    var writeDtoType = rest.SingleOrDefault();

                    return new Model(
                        Namespace: extensionsClass.ContainingNamespace?.ToDisplayString(symbolDisplayFormat),
                        ClassName: extensionsClass.Name,
                        EntityTypeName: entityType.ToDisplayString(symbolDisplayFormat),
                        ReadDtoTypeName: readDtoType.ToDisplayString(symbolDisplayFormat),
                        WriteDtoTypeName: writeDtoType?.ToDisplayString(symbolDisplayFormat)
                    );
                });
    }

    static void GenerateSourceCode(SourceProductionContext context, Model? model)
    {
        if (model is null)
            return;

        string namespaceDeclaration = model.Namespace is null ? "" : $$"""
            namespace {{model.Namespace}};
            """;

        string readModelExtensions = $$"""
            public static {{model.ReadDtoTypeName}} ToDto(this {{model.EntityTypeName}} entity)
                => {{model.ReadDtoTypeName}}.FromEntity(entity);
                
            public static IEnumerable<{{model.ReadDtoTypeName}}> ToDto(this IEnumerable<{{model.EntityTypeName}}> entities)
                => entities.Select(ToDto);
        """.Trim();

        string writeModelExtensions = "";

        var sourceText = SourceText.From($$"""
            #nullable enable

            using System.Linq;

            {{namespaceDeclaration}}

            static partial class {{model.ClassName}}
            {
                {{readModelExtensions}}
                    
                {{writeModelExtensions}}
            }
            """, Encoding.UTF8);

        context.AddSource($"{model.ClassName}_{model.EntityTypeName}.g.cs", sourceText);
    }

    private record Model(string? Namespace, string ClassName,
        string EntityTypeName, string ReadDtoTypeName, string? WriteDtoTypeName);
}
