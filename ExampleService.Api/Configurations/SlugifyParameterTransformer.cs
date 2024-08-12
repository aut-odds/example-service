using System.Text.RegularExpressions;

namespace ExampleService.Api.Configurations;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value) => value == null ? null : Slugify(value.ToString()!);

    private string Slugify(string input)
    {
        return Regex.Replace(input, "([a-z])([A-Z])", "$1-$2", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100)).ToLowerInvariant();
    }
}
