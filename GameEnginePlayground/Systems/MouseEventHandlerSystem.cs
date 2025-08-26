using GameEngine.Common.Events;
using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics.Components;
using GameEngine.Graphics.Camera;
using GameEngine.IO.Audio.Event;
using GameEngine.Common.IO.Interface;
using GameEnginePlayground.Services;
using Microsoft.Xna.Framework;

namespace GameEnginePlayground.Systems
{
    /// <summary>
    /// Event system that handles mouse click events for tile-based interactions, such as digging and autotiling.
    /// Converts screen coordinates to world coordinates, checks dig area, and triggers sound and terrain updates.
    /// </summary>
    public sealed class MouseEventHandlerSystem : IEventSystem
    {
        private const int TileSize = 16;

        private readonly IEventManager _events;
        private readonly IWorld _world;
        private readonly IEntity _camera;
        private readonly IEntity _player;
        private readonly AutotileService _autotile;
        private readonly TilePropertyCache _props;

        /// <summary>
        /// Initializes a new instance of the MouseEventHandlerSystem, sets up autotiling and event listeners.
        /// </summary>
        /// <param name="world">The world containing entities and components.</param>
        /// <param name="camera">The camera entity for coordinate transformation.</param>
        /// <param name="player">The player entity for dig area calculation.</param>
        /// <param name="map">The tile map for autotiling and property lookup.</param>
        /// <param name="events">The event manager for event handling.</param>
        public MouseEventHandlerSystem(
            IWorld world, IEntity camera, IEntity player, ITileMap map, IEventManager events)
        {
            _world = world;
            _camera = camera;
            _player = player;
            _events = events;

            _props = new TilePropertyCache(map);
            _autotile = new AutotileService(map, _props, layerIndex: 0);

            _events.AddListener<MouseClickEvent>(HandleMouseClick);
        }

        /// <summary>
        /// Handles mouse click events, converts screen coordinates to world coordinates, checks dig area, and triggers sound and autotiling.
        /// </summary>
        /// <param name="ev">The mouse click event containing screen coordinates.</param>
        private void HandleMouseClick(MouseClickEvent ev)
        {
            ref var cam = ref _world.GetComponent<CameraComponent>(_camera.Id);
            ref var playerXf = ref _world.GetComponent<TransformComponent>(_player.Id);

            var world = Vector2.Transform(new Vector2(ev.X, ev.Y), Matrix.Invert(cam.ViewMatrix));

            // Limit to a 3x3 tile box around the player
            var playerTile = new Point((int)(playerXf.Position.X / TileSize), (int)(playerXf.Position.Y / TileSize));
            var digBox = new Rectangle((playerTile.X - 1) * TileSize, (playerTile.Y - 1) * TileSize, TileSize * 3, TileSize * 3);

            var wp = new Point((int)world.X, (int)world.Y);
            if (!digBox.Contains(wp))
                return;

            var tx = wp.X / TileSize;
            var ty = wp.Y / TileSize;

            if (_props.IsSolidId(tx) || _props.IsSolidId(ty)) { }

            // Play SFX and dig
            _events.Publish(new PlaySoundFxEvent("shovel.dig", playerXf.Position, 1f));
            _autotile.DigAt(tx, ty);
        }
    }
}
