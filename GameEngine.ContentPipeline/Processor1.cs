using GameEngine.Common.Interfaces;
using GameEngine.ContentPipeline;
using GameEngine.IO.Asset.models;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Extended.Collisions.Layers;
using System;
using System.Linq;
using System.Text.Json;

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
                    Objects = tile.objectgroup?.objects?.Select(obj => new TileObject
                    {
                        Gid = obj.gid,
                        Height = obj.height,
                        Id = obj.id,
                        Name = obj.name,
                        Point = obj.point,
                        Rotation = obj.rotation,
                        Type = obj.type,
                        Visible = obj.visible,
                        Width = obj.width,
                        X = obj.x,
                        Y = obj.y
                    }).ToList<ITileObject>(),
                    Properties = tile.properties?.Select(p => new Property
                    {
                        Name = p.name,
                        Type = p.type,
                        Value = ParseTiledValue(p.type, p.value)
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
            else if (layer.type == "objectgroup")
            {
                TileLayer tileLayer = new TileLayer
                {
                    Name = layer.name,
                    Type = layer.type,
                    Draworder = layer.draworder,
                    Id = layer.id,
                    Objects = layer.objects.Select(obj => new TileObject
                    {
                        Gid = obj.gid,
                        Height = obj.height,
                        Id = obj.id,
                        Name = obj.name,
                        Point = obj.point,
                        Rotation = obj.rotation,
                        Type = obj.type,
                        Visible = obj.visible,
                        Width = obj.width,
                        X = obj.x,
                        Y = obj.y
                    }).ToList<ITileObject>(),
                    Opacity = layer.opacity,
                    Visible = layer.visible,
                    X = layer.x,
                    Y = layer.y
                };
                if(layer.name == "PlayerSpawn")
                    tileMap.PlayerLayer = tileLayer;
                else if(layer.name == "Grass")
                    tileMap.ObjectTileLayer = tileLayer;
                else if(layer.name == "Trees")
                    tileMap.TreeLayer = tileLayer;
                }
        }

            return tileMap;
 }

    private object ParseTiledValue(string type, JsonElement value)
    {
        return type switch
        {
            "bool" => value.GetBoolean(),
            "int" => value.GetInt32(),
            "float" => value.GetSingle(),
            "string" => value.GetString(),
            _ => throw new NotSupportedException($"Unsupported property type '{type}'")
        };
    }
}