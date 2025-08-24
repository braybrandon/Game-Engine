using GameEngine.Common.Components;
using GameEngine.Common.Events;
using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics.Components;
using GameEngine.Common.Physics.Interfaces;
using GameEngine.Graphics.Camera;
using GameEngine.Graphics.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace GameEnginePlayground.Systems
{
    public class MouseEventHandlerSystem: IUpdateSystem
    {
        private IEventManager _eventManager;
        private readonly IWorld _world;
        private readonly IEntity _camera;
        private readonly ITileMap _gameMap;
        private readonly IEntity _playerEntity;
        private readonly IAssetManager _assetManager;
        private readonly IInputManager _inputManager;
        private readonly IQuadTree _quadTree;

        public MouseEventHandlerSystem(IWorld world, IEntity camera, ITileMap gameMap, IEventManager eventManager, IEntity playerEntity, IAssetManager assetManager, IInputManager inputManager, IQuadTree quadTree)
        {
             _world = world;
             _camera = camera;
             _gameMap = gameMap;
            _eventManager = eventManager;
            _eventManager.AddListener<MouseClickEvent>(handleMouseClick);
            _eventManager.AddListener<FireballPressedEvent>(handleFireballEvent);
            _assetManager = assetManager;
            _playerEntity = playerEntity;
            _inputManager = inputManager;
            _quadTree = quadTree;
        }

        private void handleFireballEvent(FireballPressedEvent fireballPressed)
        {
            //Debug.WriteLine(fireballPressed.Timestamp);
            var fireball = _world.CreateEntity();
            var transformComponent = _world.GetComponent<TransformComponent>(_playerEntity.Id);
            var mousePosition = _inputManager.GetMousePosition();
            var cameraComponent = _world.GetComponent<CameraComponent>(_camera.Id);
            var inverseMatrix = Matrix.Invert(cameraComponent.ViewMatrix);
            var worldPosition = Vector2.Transform(mousePosition, inverseMatrix);
            var direction = worldPosition - transformComponent.Position;
            direction.Normalize();
            float projectileSpeed = 200f;
            Texture2D _fireballtexture = _assetManager.LoadTexture("fireball");
            fireball.AddComponent(new TransformComponent { Position = new Vector2(transformComponent.Position.X, transformComponent.Position.Y), Rotation = 0f, Scale = Vector2.One });
            fireball.AddComponent(new DirectionComponent { Value = direction });
            fireball.AddComponent(new SpeedComponent { Value = projectileSpeed });
            fireball.AddComponent(new ProposedPositionComponent());
            fireball.AddComponent(new LifetimeComponent(160));
            var bounds = new Rectangle(_fireballtexture.Width / 2, _fireballtexture.Height / 2, _fireballtexture.Width, _fireballtexture.Height) ;
            fireball.AddComponent(new ColliderComponent { Bounds = bounds, IsTrigger = false, IsStatic = false });

            _quadTree.Insert(fireball, new Rectangle((int)transformComponent.Position.X - bounds.X, (int)transformComponent.Position.Y - bounds.Y, bounds.Width, bounds.Height));

            fireball.AddComponent(new SpriteComponent
            {
                Texture = _fireballtexture,
                SourceRectangle = _fireballtexture.Bounds,
                Color = Color.White,
                Scale = 1f,
                Rotation = 0f,
                Origin = new Vector2(_fireballtexture.Width / 2, _fireballtexture.Height / 2),
                Effects = SpriteEffects.None,
                LayerDepth = 0f
            });
        }

        private void handleMouseClick(MouseClickEvent mouseClickEvent)
        {
            var transformComponent = _world.GetComponent<TransformComponent>(_playerEntity.Id);
            var currentTile = new Vector2((int)transformComponent.Position.X / 16, (int)transformComponent.Position.Y / 16);
            var sourceRect = new Rectangle((int)(currentTile.X - 1) * 16, (int)(currentTile.Y - 1) * 16, 48, 48);
            
            var pos = new Vector2(mouseClickEvent.X, mouseClickEvent.Y);

            var cameraComponent = _world.GetComponent<CameraComponent>(_camera.Id);
            var inverseMatrix = Matrix.Invert(cameraComponent.ViewMatrix);
            var worldPosition = Vector2.Transform(pos, inverseMatrix);
            float distance = Vector2.Distance(transformComponent.Position, worldPosition);
            if (sourceRect.Contains(worldPosition))
            {
              UpdateTile((int)worldPosition.X / 16, (int)worldPosition.Y / 16);
            }
        }

        private void UpdateTile(int tileX, int tileY)
        {
            if (IsSolid(tileX, tileY))
            {
                return;

            }


                var bitmaskToTileId = new Dictionary<int, int>
            {
                // No neighbors
                { 0,  14},
                //Left Neighbor
                { 1,  7},
                //right neightbor
                { 2, 5 },
                //left right neighbors
                { 3,  6},
                // Top Neighbor
                { 4,  24},
                // Left Top Neighbors
                { 5, 20 },
                // Right Top Neighbors
                { 6, 18 },
                //Left Right Top Neighbors
                { 7,  19},
                //bottom Neighbor
                { 8,  8},
                // Left Bottom Neighbor
                { 9, 4 },
                //Right Bottom neighbor
                { 10, 2 },
                // Left Right Bottom Neighbor
                { 11,  3},
                // Top Bottom Neighbor
                { 12, 16 },
                // Top Left bottom neighbor
                { 13, 12 },
                // Top Right bottom neighbor
                { 14,  10},
                //All Neighbors
                {15,  11}
            };
                int bitmask = 0;

                if (IsDirt(tileX - 1, tileY))
                    bitmask |= 1;
                if (IsDirt(tileX + 1, tileY))
                    bitmask |= 2;
                if (IsDirt(tileX, tileY - 1))
                    bitmask |= 4;
                if (IsDirt(tileX, tileY + 1))
                    bitmask |= 8;
                if (bitmaskToTileId.ContainsKey(bitmask))
                {
                    int newTileId = bitmaskToTileId[bitmask];
                    _gameMap.Layers[0].UpdateTileId(tileX, tileY, newTileId);
                }

                UpdateNeighbors(tileX, tileY, bitmaskToTileId);
            
            
        }

        private void UpdateNeighbors(int x, int y, Dictionary<int, int> lookup)
        {
            var neighbors = new (int, int)[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) };
            foreach (var (tileX, tileY) in neighbors)
            {
                if (IsDirt(tileX, tileY)) {
                    // Re-calculate the bitmask for the neighbor.
                    int bitmask = 0;
                    if (IsDirt(tileX - 1, tileY)) bitmask |= 1;
                    if (IsDirt(tileX + 1, tileY)) bitmask |= 2;
                    if (IsDirt(tileX, tileY - 1)) bitmask |= 4;
                    if (IsDirt(tileX, tileY + 1)) bitmask |= 8;

                    if (lookup.ContainsKey(bitmask))
                    {
                        _gameMap.Layers[0].UpdateTileId(tileX, tileY, lookup[bitmask]);
                    }
                }
            }
        }

        private bool IsDirt(int x, int y)
        {
            int id = _gameMap.Layers[0].GetTileId(x, y) - 1;
            var tile = _gameMap.Tilesets[0].Tiles.Find(t => t.Id == id);
            if(tile != default)
            {   
                var prop = tile.Properties?.FirstOrDefault(p => p.Name == "IsDirt");
                if (prop != default)
                return (bool)tile?.Properties[0]?.Value;
            }

            return false;
        }

        private bool IsSolid(int x, int y)
        {
            int id = _gameMap.Layers[0].GetTileId(x, y) - 1;
            var tile = _gameMap.Tilesets[0].Tiles.Find(t => t.Id == id);
            if (tile != default)
            {
                var prop = tile.Properties?.FirstOrDefault(p => p.Name == "IsSolid");
                if (prop != default)
                    return (bool)tile?.Properties[0]?.Value;
            }

            return false;
        }

        public void Update(IWorld world)
        {
        }
    }
}
