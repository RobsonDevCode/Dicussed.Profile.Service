using HotChocolate.Language;
using HotChocolate.Resolvers;

namespace Discussed.Profile.Api.Extensions.ResolverFields;

public static class ResolverExtensions
{
    public static IReadOnlyCollection<string> GetItemsFields(this IResolverContext resolverContext)
    {
        return resolverContext.Selection.SyntaxNode.SelectionSet?.Selections
            .OfType<FieldNode>()
            .Where(f => f.Name.Value == "items")
            .SelectMany(f => f.SelectionSet?.Selections.OfType<FieldNode>() ?? [])
            .Select(f => f.Name.Value)
            .ToList() ?? [];
    }

    public static IReadOnlyList<string>? GetSelectedFields(this IResolverContext resolverContext)
    {
        return resolverContext.Selection.SyntaxNode.SelectionSet?.Selections
            .OfType<FieldNode>()
            .SelectMany(f => f.SelectionSet?.Selections.OfType<FieldNode>() ?? [])
            .Select(f => f.Name.Value)
            .ToList();
    }
}