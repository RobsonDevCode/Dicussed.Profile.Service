using HotChocolate.Language;
using HotChocolate.Resolvers;

namespace Discussed.Profile.Api.Middleware;

public class FieldSelectionMiddleware
{
    private readonly FieldDelegate _next;

    public FieldSelectionMiddleware(FieldDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(IMiddlewareContext context)
    {
        //extract fields given before executing resolver
        var selection = context.Selection.SyntaxNode.SelectionSet;

        if (selection != null)
        {
            var selectedFields = selection.Selections
                .OfType<FieldNode>()
                .Select(n => n.Name.Value)
                .ToList();

            //store in context data for later access
            context.ContextData["SelectedFields"] = selectedFields;
        }

        await _next(context);
    }
}