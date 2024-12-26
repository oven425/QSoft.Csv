using Microsoft.CodeAnalysis;
using QSoft.CsvSG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QSoft.Csv
{
    [Generator]
    public class AttributeGenerator : IIncrementalGenerator
    {
        
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            Debugger.Launch();
            var namespacestr = "QSoft.CsvSG";
            var ccc = context.SyntaxProvider;
            var aa = context.CompilationProvider.Select((compilation, token) =>
            {
                var namespacesymbol = compilation.GetEntryPoint(token)?.ContainingNamespace;
                if (namespacesymbol is not null && !namespacesymbol.IsGlobalNamespace)
                {
                    namespacestr = namespacesymbol.ToDisplayString();
                }


                var typeNameList = new List<string>();

                var referencedAssemblySymbols = compilation.SourceModule.ReferencedAssemblySymbols;

                foreach (IAssemblySymbol? referencedAssemblySymbol in referencedAssemblySymbols)
                {
                    var allTypeSymbol = referencedAssemblySymbol.GlobalNamespace.GetAllTypeSymbol();

                    foreach (var typeSymbol in allTypeSymbol)
                    {
                        if (typeSymbol.TypeKind == TypeKind.Class)
                        {
                            //System.Diagnostics.Trace.WriteLine(typeSymbol.ContainingAssembly.Locations.FirstOrDefault()?.SourceTree.FilePath);
                            var classname = typeSymbol.ToDisplayString();
                            if (classname.Contains("QSoft"))
                            {

                            }
                            System.Diagnostics.Trace.WriteLine($"{classname}");

                            //var attrs = typeSymbol.GetAttributes();
                            //var attr = attrs.FirstOrDefault(x => x.AttributeClass.OriginalDefinition.ToDisplayString() == "Attribute");
                            //if (attr != null)
                            //{
                            //    typeNameList.Add(typeSymbol.ToDisplayString());
                            //}

                        }
                    }
                }
                    return "";
            });
            context.RegisterSourceOutput(aa, (productionContext, list) =>
            {

                var code = $@"";



                productionContext.AddSource("AddTransientAttributeSG.g.cs", code);
            });
        }
    }
}
