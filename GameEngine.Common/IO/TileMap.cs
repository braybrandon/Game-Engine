// GameEngine.Common/TileMap.cs
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameEngine.Common;

public sealed class Int2DArrayConverter : JsonConverter<int[,]>
{
    public override int[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // read jagged -> pack into [,]
        var rows = JsonSerializer.Deserialize<List<List<int>>>(ref reader, options)
                   ?? throw new JsonException("Expected 2D array (array of arrays).");

        int h = rows.Count;
        int w = h == 0 ? 0 : rows.Max(r => r.Count);
        var data = new int[h, w];

        for (int y = 0; y < h; y++)
        {
            var row = rows[y];
            for (int x = 0; x < w; x++)
                data[y, x] = x < row.Count ? row[x] : -1; // pad
        }
        return data;
    }

    public override void Write(Utf8JsonWriter writer, int[,] value, JsonSerializerOptions options)
    {
        int h = value.GetLength(0);
        int w = value.GetLength(1);

        writer.WriteStartArray();
        for (int y = 0; y < h; y++)
        {
            writer.WriteStartArray();
            for (int x = 0; x < w; x++)
                writer.WriteNumberValue(value[y, x]);
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }
}

public class TileMap
{
    public int Width { get; set; }
    public int Height { get; set; }

    // Converter handles 2D array
    [JsonConverter(typeof(Int2DArrayConverter))]
    public int[,] Tiles { get; set; } = new int[0, 0];

    // ---- parameterless ctor for deserialization ----
    public TileMap() { }

    // ---- convenience ctor for runtime/editor usage ----
    public TileMap(int width, int height, int fill = -1)
    {
        Width = width;
        Height = height;
        Tiles = new int[height, width];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                Tiles[y, x] = fill;
    }

    private static readonly JsonSerializerOptions _opts = new()
    {
        WriteIndented = true
        // no need to add the converter here because we used the [JsonConverter] attribute
    };

    public void Save(string filePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        var json = JsonSerializer.Serialize(this, _opts);
        File.WriteAllText(filePath, json);
    }

    public static TileMap Load(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<TileMap>(json, _opts)!;
    }
}