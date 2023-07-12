using CSharpComposer.Generator.Models;
using System.Xml.Serialization;

namespace CSharpComposer.Generator.Registries;

internal class CSharpRegistry
{
    private Tree? _tree;

    public Tree Tree => _tree ?? throw new InvalidOperationException("Registry has not been initialized.");

    public async Task FetchRegistrysync()
    {
        var registryUrl = "https://raw.githubusercontent.com/dotnet/roslyn/Visual-Studio-2022-Version-17.5/src/Compilers/CSharp/Portable/Syntax/Syntax.xml";
        using var httpClient = new HttpClient();

        Console.WriteLine("Fetching XML...");

        var registryXml = await httpClient.GetStringAsync(registryUrl);

        var serializer = new XmlSerializer(typeof(Tree));

        using var reader = new StringReader(registryXml);

        _tree = (Tree)serializer.Deserialize(reader)!;
    }
}
