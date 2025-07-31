using GameEngine.ContentPipeline;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Text.Json;
using System.IO;

[ContentImporter(".json", DisplayName = "Tiled JSON Importer", DefaultProcessor = "TiledProcessor")]
public class TiledImporter : ContentImporter<TiledMapRaw>
{
    public override TiledMapRaw Import(string filename, ContentImporterContext context)
    {
        string json = File.ReadAllText(filename);
        // For basic deserialization, this is all you need
        var map = JsonSerializer.Deserialize<TiledMapRaw>(json);
        return map;
    }

}
