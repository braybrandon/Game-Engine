//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System.Globalization;
//using System.Xml.Linq;

//namespace GameEngine.IO.Asset
//{


//        // NOTE: For MonoGame, you would also need these using statements:
//        // using Microsoft.Xna.Framework;
//        // using Microsoft.Xna.Framework.Graphics;

//        // Define classes to represent the Tiled Map structure
//        public class Map
//        {
//            public string Version { get; set; }
//            public string TiledVersion { get; set; }
//            public string Orientation { get; set; }
//            public string RenderOrder { get; set; }
//            public int Width { get; set; }
//            public int Height { get; set; }
//            public int TileWidth { get; set; }
//            public int TileHeight { get; set; }
//            public int Infinite { get; set; }
//            public int NextLayerId { get; set; }
//            public int NextObjectId { get; set; }

//            //public Tileset Tileset { get; set; } // Assuming one tileset for simplicity based on your XML
//            public List<Layer> Layers { get; set; }
//            public List<ObjectGroup> ObjectGroups { get; set; }

//            public Map()
//            {
//                Layers = new List<Layer>();
//                ObjectGroups = new List<ObjectGroup>();
//            }
//        }

//        public class TileDefinition
//        {
//            public int Id { get; set; }
//            public Dictionary<string, string> Properties { get; set; } // Properties for a specific tile ID

//            public TileDefinition()
//            {
//                Properties = new Dictionary<string, string>();
//            }
//        }

//        public class Layer
//        {
//            public int Id { get; set; }
//            public string Name { get; set; }
//            public int Width { get; set; }
//            public int Height { get; set; }
//            public List<int> TileData { get; set; } // For CSV encoded data

//            public Layer()
//            {
//                TileData = new List<int>();
//            }
//        }

//        public class ObjectGroup
//        {
//            public int Id { get; set; }
//            public string Name { get; set; }
//            public List<MapObject> Objects { get; set; }

//            public ObjectGroup()
//            {
//                Objects = new List<MapObject>();
//            }
//        }

//        public class MapObject
//        {
//            public int Id { get; set; }
//            public string Name { get; set; }
//            public double X { get; set; }
//            public double Y { get; set; }
//            // Tiled objects can have width/height, but your example uses <point/> which implies 0 width/height
//            // You might want to add Width and Height properties if your objects have dimensions.
//            // public double Width { get; set; }
//            // public double Height { get; set; }
//        }

//        public class TiledMapParser
//        {
//            /// <summary>
//            /// Parses a Tiled TMX map file from the specified file path into a Map object.
//            /// </summary>
//            /// <param name="filePath">The full path to the TMX file.</param>
//            /// <returns>A Map object representing the parsed TMX data, or null if an error occurs.</returns>
//            public static Map Parse(string filePath)
//            {
//                Map map = new Map();
//                try
//                {
//                    // Load the XML document directly from the file path
//                    XDocument doc = XDocument.Load(filePath);
//                    XElement mapElement = doc.Element("map");

//                    if (mapElement == null)
//                    {
//                        Console.WriteLine("Error: 'map' element not found in the XML.");
//                        return null;
//                    }

//                    // Parse Map attributes
//                    map.Version = mapElement.Attribute("version")?.Value;
//                    map.TiledVersion = mapElement.Attribute("tiledversion")?.Value;
//                    map.Orientation = mapElement.Attribute("orientation")?.Value;
//                    map.RenderOrder = mapElement.Attribute("renderorder")?.Value;
//                    map.Width = int.Parse(mapElement.Attribute("width")?.Value ?? "0");
//                    map.Height = int.Parse(mapElement.Attribute("height")?.Value ?? "0");
//                    map.TileWidth = int.Parse(mapElement.Attribute("tilewidth")?.Value ?? "0");
//                    map.TileHeight = int.Parse(mapElement.Attribute("tileheight")?.Value ?? "0");
//                    map.Infinite = int.Parse(mapElement.Attribute("infinite")?.Value ?? "0");
//                    map.NextLayerId = int.Parse(mapElement.Attribute("nextlayerid")?.Value ?? "0");
//                    map.NextObjectId = int.Parse(mapElement.Attribute("nextobjectid")?.Value ?? "0");

//                    // Parse Tileset
//                    XElement tilesetElement = mapElement.Element("tileset");
//                    if (tilesetElement != null)
//                    {
//                        //map.Tileset = new Tileset
//                        //{
//                        //    FirstGid = int.Parse(tilesetElement.Attribute("firstgid")?.Value ?? "0"),
//                        //    Name = tilesetElement.Attribute("name")?.Value,
//                        //    TileWidth = int.Parse(tilesetElement.Attribute("tilewidth")?.Value ?? "0"),
//                        //    TileHeight = int.Parse(tilesetElement.Attribute("tileheight")?.Value ?? "0"),
//                        //    TileCount = int.Parse(tilesetElement.Attribute("tilecount")?.Value ?? "0"),
//                        //    Columns = int.Parse(tilesetElement.Attribute("columns")?.Value ?? "0")
//                        //};

//                        // Parse Tileset Properties
//                        XElement tilesetProperties = tilesetElement.Element("properties");
//                        if (tilesetProperties != null)
//                        {
//                            foreach (XElement prop in tilesetProperties.Elements("property"))
//                            {
//                                string propName = prop.Attribute("name")?.Value;
//                                string propValue = prop.Attribute("value")?.Value;
//                                if (propName != null && propValue != null)
//                                {
//                                    map.Tileset.Properties[propName] = propValue;
//                                }
//                            }
//                        }

//                        // Parse Tileset Image
//                        XElement imageElement = tilesetElement.Element("image");
//                        if (imageElement != null)
//                        {
//                            map.Tileset.ImageSource = imageElement.Attribute("source")?.Value;
//                            map.Tileset.ImageWidth = int.Parse(imageElement.Attribute("width")?.Value ?? "0");
//                            map.Tileset.ImageHeight = int.Parse(imageElement.Attribute("height")?.Value ?? "0");
//                        }

//                        // Parse individual Tile Definitions within the Tileset
//                        foreach (XElement tileDefElement in tilesetElement.Elements("tile"))
//                        {
//                            TileDefinition tileDef = new TileDefinition
//                            {
//                                Id = int.Parse(tileDefElement.Attribute("id")?.Value ?? "0")
//                            };

//                            XElement tileProperties = tileDefElement.Element("properties");
//                            if (tileProperties != null)
//                            {
//                                foreach (XElement prop in tileProperties.Elements("property"))
//                                {
//                                    string propName = prop.Attribute("name")?.Value;
//                                    string propValue = prop.Attribute("value")?.Value;
//                                    if (propName != null && propValue != null)
//                                    {
//                                        tileDef.Properties[propName] = propValue;
//                                    }
//                                }
//                            }
//                            map.Tileset.TileDefinitions.Add(tileDef);
//                        }
//                    }

//                    // Parse Layers
//                    foreach (XElement layerElement in mapElement.Elements("layer"))
//                    {
//                        Layer layer = new Layer
//                        {
//                            Id = int.Parse(layerElement.Attribute("id")?.Value ?? "0"),
//                            Name = layerElement.Attribute("name")?.Value,
//                            Width = int.Parse(layerElement.Attribute("width")?.Value ?? "0"),
//                            Height = int.Parse(layerElement.Attribute("height")?.Value ?? "0")
//                        };

//                        XElement dataElement = layerElement.Element("data");
//                        if (dataElement != null && dataElement.Attribute("encoding")?.Value == "csv")
//                        {
//                            string csvData = dataElement.Value.Trim();
//                            layer.TileData = csvData.Split(',')
//                                                    .Select(s => int.Parse(s.Trim(), CultureInfo.InvariantCulture))
//                                                    .ToList();
//                        }
//                        map.Layers.Add(layer);
//                    }

//                    // Parse Object Groups
//                    foreach (XElement objectGroupElement in mapElement.Elements("objectgroup"))
//                    {
//                        ObjectGroup objGroup = new ObjectGroup
//                        {
//                            Id = int.Parse(objectGroupElement.Attribute("id")?.Value ?? "0"),
//                            Name = objectGroupElement.Attribute("name")?.Value
//                        };

//                        foreach (XElement objectElement in objectGroupElement.Elements("object"))
//                        {
//                            MapObject mapObject = new MapObject
//                            {
//                                Id = int.Parse(objectElement.Attribute("id")?.Value ?? "0"),
//                                Name = objectElement.Attribute("name")?.Value,
//                                X = double.Parse(objectElement.Attribute("x")?.Value ?? "0", CultureInfo.InvariantCulture),
//                                Y = double.Parse(objectElement.Attribute("y")?.Value ?? "0", CultureInfo.InvariantCulture)
//                                // If objects can have width/height, parse them here:
//                                // Width = double.Parse(objectElement.Attribute("width")?.Value ?? "0", CultureInfo.InvariantCulture),
//                                // Height = double.Parse(objectElement.Attribute("height")?.Value ?? "0", CultureInfo.InvariantCulture)
//                            };
//                            objGroup.Objects.Add(mapObject);
//                        }
//                        map.ObjectGroups.Add(objGroup);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"An error occurred during parsing: {ex.Message}");
//                    return null;
//                }

//                return map;
//            }
//        }

//        /// <summary>
//        /// A class responsible for rendering a Tiled map using MonoGame's SpriteBatch.
//        /// </summary>
//        public class TileMapRenderer
//        {
//            private Map _map;
//            private Texture2D _tilesetTexture;

//            // NOTE: In a MonoGame project, you would pass the loaded Texture2D here.
//            // Example: new TileMapRenderer(parsedMap, Content.Load<Texture2D>("Dungeon_Tiles"));
//            public TileMapRenderer(Map map, Texture2D tilesetTexture)
//            {
//                _map = map ?? throw new ArgumentNullException(nameof(map));
//                _tilesetTexture = tilesetTexture ?? throw new ArgumentNullException(nameof(tilesetTexture));
//            }

//        /// <summary>
//        /// Draws the tile map layers to the screen, applying a camera transformation.
//        /// </summary>
//        /// <param name="spriteBatch">The MonoGame SpriteBatch instance for drawing.</param>
//        /// <param name="cameraTransform">The view matrix from your camera, used to transform the drawing coordinates.</param>
//        // NOTE: You would typically call this method from your MonoGame Game's Draw method.
//        // Example: tileMapRenderer.Draw(spriteBatch, camera.GetViewMatrix());
//        public void Draw(SpriteBatch spriteBatch, Matrix cameraTransform)
//        {
//            if (_map == null || _tilesetTexture == null)
//            {
//                // Handle error or return if map or texture is not set
//                return;
//            }

//            // Begin the SpriteBatch with the camera transformation matrix
//            // This makes all subsequent Draw calls relative to the camera's view.
//            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cameraTransform);

//            // Iterate through each tile layer in the map
//            foreach (var layer in _map.Layers)
//            {
//                // Only draw if the layer has tile data
//                if (layer.TileData == null || layer.TileData.Count == 0)
//                {
//                    continue;
//                }

//                for (int y = 0; y < layer.Height; y++)
//                {
//                    for (int x = 0; x < layer.Width; x++)
//                    {
//                        int tileIndex = y * layer.Width + x;
//                        int gid = layer.TileData[tileIndex];

//                        // GID 0 means no tile (empty space)
//                        if (gid == 0)
//                        {
//                            continue;
//                        }

//                        // Adjust GID to be 0-based index for the tileset
//                        // Tiled GIDs are 1-based, and firstgid indicates the starting GID for this tileset.
//                        // If you have multiple tilesets, you'd need more complex logic here
//                        // to determine which tileset a GID belongs to.
//                        int tileIdInTileset = gid - _map.Tileset.FirstGid;

//                        if (tileIdInTileset < 0 || tileIdInTileset >= _map.Tileset.TileCount)
//                        {
//                            // GID is out of range for this tileset, skip or log error
//                            continue;
//                        }

//                        // Calculate source rectangle on the tileset texture
//                        int tileXInTileset = tileIdInTileset % _map.Tileset.Columns;
//                        int tileYInTileset = tileIdInTileset / _map.Tileset.Columns;

//                        Rectangle sourceRectangle = new Rectangle(
//                            tileXInTileset * _map.Tileset.TileWidth,
//                            tileYInTileset * _map.Tileset.TileHeight,
//                            _map.Tileset.TileWidth,
//                            _map.Tileset.TileHeight
//                        );

//                        // Calculate destination rectangle in world coordinates
//                        // These are the coordinates relative to the map's origin (0,0)
//                        Vector2 worldPosition = new Vector2(
//                            x * _map.TileWidth,
//                            y * _map.TileHeight
//                        );

//                        // Draw the tile
//                        spriteBatch.Draw(_tilesetTexture, worldPosition, sourceRectangle, Color.White);
//                    }
//                }
//            }

//            // End the SpriteBatch after drawing all map elements
//            spriteBatch.End();

//            // You would typically draw objects (e.g., enemies, items) in a separate SpriteBatch.Begin/End block
//            // or within this same block if they also need to be affected by the camera.
//            // If they are drawn with their own textures/positions, they would also need to be drawn
//            // relative to the camera's transform.
//            // Example for drawing objects (assuming you have textures for them):
//            /*
//            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cameraTransform);
//            foreach (var objGroup in _map.ObjectGroups)
//            {
//                foreach (var mapObject in objGroup.Objects)
//                {
//                    // Load object texture (e.g., Texture2D batTexture = Content.Load<Texture2D>("Bat");)
//                    // spriteBatch.Draw(batTexture, new Vector2((float)mapObject.X, (float)mapObject.Y), Color.White);
//                }
//            }
//            spriteBatch.End();
//            */
//        }
//    }

//    }

