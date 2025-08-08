using Common.Interfaces;
using GameEngine.ContentPipeline;
using GameEngine.IO.Asset.models;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Collections.Generic;
using System.Linq;

[ContentProcessor(DisplayName = "Tiled Processor")]
public class TiledProcessor : ContentProcessor<TiledMapRaw, TileMap>
{
    public override TileMap Process(TiledMapRaw input, ContentProcessorContext context)
    {
        TileMap tileMap = new TileMap
        {
            Width = input.width,
            Height = input.height,
            TileWidth = input.tilewidth,
            TileHeight = input.tileheight,
        };

        foreach (var ts in input.tilesets)
        {
            var tileset = new Tileset
            {
                Name = ts.name,
                FirstGID = ts.firstgid,
                TileWidth = ts.tilewidth,
                TileHeight = ts.tileheight,
                ImageSource = ts.image,
                ImageWidth = ts.imagewidth,
                ImageHeight = ts.imageheight,
                Columns = ts.columns,
                TileCount = ts.tilecount,
                Margin = ts.margin,
                Spacing = ts.spacing,
                TransparentColor = ts.transparentcolor,  // if present in ts
                Type = ts.type,                          // if present in ts
                Version = ts.version,
                TiledVersion = ts.tiledversion,
                Source = ts.source,
                Grid = ts.grid != null ? new GameEngine.IO.Asset.models.Grid
                {
                    Height = ts.grid.height,
                    Width = ts.grid.width,
                    Orientation = ts.grid.orientation
                } : null,

                Properties = ts.properties?.Select(p => new Property
                {
                    Name = p.name,
                    Type = p.type,
                    Value = p.value
                }).ToList<IProperty>(),

                Tiles = ts.tiles?.Select(tile => new Tile
                {
                    Id = tile.id,
                    Properties = tile.properties?.Select(p => new Property
                    {
                        Name = p.name,
                        Type = p.type,
                        Value = p.value
                    }).ToList<IProperty>(),

                    Animation = tile.animation?.Select(frame => new GameEngine.IO.Asset.models.TileAnimationFrame
                    {
                        Duration = frame.duration,
                        TileId = frame.tileid
                    }).ToList<ITileAnimationFrame>()
                }).ToList<ITile>()
            };

            tileMap.Tilesets.Add(tileset);
        }

        // Convert layers
        foreach (var layer in input.layers)
        {
            if (layer.type == "tilelayer")
            {
                TileLayer tileLayer = new TileLayer
                {
                    Name = layer.name,
                    Height = layer.height,
                    Width = layer.width,
                    Type = layer.type,
                    Tiles = layer.data.ToArray()
                };
                tileMap.Layers.Add(tileLayer);
            }
        }

        return tileMap;
    }
}