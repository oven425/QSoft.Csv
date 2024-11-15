using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace QSoft.CsvSG
{
    public static class INamespaceSymbolExtension
    {
        public static IEnumerable<INamedTypeSymbol> GetAllTypeSymbol(this INamespaceSymbol namespaceSymbol)
        {
            var typeMemberList = namespaceSymbol.GetTypeMembers();

            foreach (var typeSymbol in typeMemberList)
            {
                yield return typeSymbol;
            }

            foreach (var namespaceMember in namespaceSymbol.GetNamespaceMembers())
            {
                foreach (var typeSymbol in GetAllTypeSymbol(namespaceMember))
                {
                    yield return typeSymbol;
                }
            }
        }
    }

}
