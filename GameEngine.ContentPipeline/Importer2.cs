using GameEngine.ContentPipeline;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace GameEngine.ContentPipeline
{
    [ContentImporter(".json", DisplayName = "JSON Key Value Pair Importer", DefaultProcessor = "KeybindProcessor")]
    public class JsonImporter : ContentImporter<KeybindRaw>
    {
        public override KeybindRaw Import(string filename, ContentImporterContext context)
        {
            string json = File.ReadAllText(filename);
            // For basic deserialization, this is all you need
            var kvp = JsonSerializer.Deserialize<KeybindRaw>(json);
            return kvp;
        }

    }

}
