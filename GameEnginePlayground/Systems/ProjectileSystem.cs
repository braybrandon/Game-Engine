using GameEngine.Common.Events;
using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Components;
using GameEngine.Common.Physics;
using GameEngine.Common.Physics.Components;
using GameEngine.Graphics.Camera;
using GameEngine.IO.Audio.Event;
using GameEnginePlayground.Factories;
using GameEnginePlayground.Factories.DataObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace GameEnginePlayground.Systems
{
    /// <summary>
    /// Event system that handles projectile events, creates projectile entities, and triggers sound effects.
    /// Uses input manager, asset manager, and projectile factory to configure and spawn projectiles in the game world.
    /// </summary>
    public class ProjectileSystem : IEventSystem
    {
        private IEventManager _eventManager;
        private IInputManager _inputManager;
        private IAssetManager _assetManager;
        private IFactory<IEntity, ProjectileData> _projectileFactory;
        private GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Initializes a new instance of the ProjectileSystem with required dependencies and registers the event listener.
        /// </summary>
        /// <param name="eventManager">The event manager for event handling.</param>
        /// <param name="inputManager">The input manager for input queries.</param>
        /// <param name="assetManager">The asset manager for loading assets.</param>
        /// <param name="projectileFactory">The factory for creating projectile entities.</param>
        public ProjectileSystem(IEventManager eventManager, IInputManager inputManager, IAssetManager assetManager, IFactory<IEntity, ProjectileData> projectileFactory, GraphicsDevice graphicsDevice)
        {
            _eventManager = eventManager;
            _inputManager = inputManager;
            _assetManager = assetManager;
            _projectileFactory = projectileFactory;
            _graphicsDevice = graphicsDevice;
            _eventManager.AddListener<ProjectileEvent>(OnProjectile);
        }

        /// <summary>
        /// Handles ProjectileEvent by creating a projectile entity and triggering its sound effect.
        /// Calculates direction and world position based on mouse input and camera transform.
        /// </summary>
        /// <param name="projectileEvent">The projectile event containing entity and world information.</param>
        private void OnProjectile(ProjectileEvent projectileEvent)
        {
            var transformComponent = projectileEvent.World.GetComponent<TransformComponent>(projectileEvent.EntityId);
            var isPlayer = projectileEvent.World.HasComponent<PlayerInputComponent>(projectileEvent.EntityId);
            var tag = isPlayer ? Filters.PlayerProjectile : Filters.EnemyProjectile;
            if(isPlayer) 
            {
                var mousePosition = _inputManager.GetMousePosition();
                var camera = projectileEvent.World.GetEntitiesWith<CameraComponent>().FirstOrDefault();
                var cameraComponent = projectileEvent.World.GetComponent<CameraComponent>(camera.Id);
                var inverseMatrix = Matrix.Invert(cameraComponent.ViewMatrix);
                var worldPosition = Vector2.Transform(mousePosition, inverseMatrix);
                var direction = worldPosition - transformComponent.Position;
                direction.Normalize(); 
                projectileEvent.Direction = direction;
            }
            float projectileSpeed = 150f;
            Texture2D _fireballtexture = BuildFilledCircle(_graphicsDevice, 8);
            var bounds = new Rectangle(_fireballtexture.Width / 2, _fireballtexture.Height / 2, _fireballtexture.Width, _fireballtexture.Height);
            ProjectileData data = new ProjectileData
            {
                Tag = tag,
                SoundFile = "fireball.shoot",
                SoundVolume = 0.5f,
                SoundPan = 0f,
                SoundPitch = 0f,
                Texture = _fireballtexture,
                Lifetime = 200,
                IsTrigger = false,
                IsStatic = false,
                SpriteColor = Color.White,
                SpriteScale = 1f,
                SpriteRotation = 0f,
                ProjectileSpeed = projectileSpeed,
                SpriteLayerDepth = 0.5f,
                SpriteEffects = SpriteEffects.None,
                InitialPosition = transformComponent.Position,
                InitialRotation = 0f,
                InitialScale = new Vector2(1f, 1f),
                Direction = projectileEvent.Direction,
                ColliderBounds = bounds,
                SpriteSourceRectangle = new Rectangle(0, 0, _fireballtexture.Width, _fireballtexture.Height),
                SpriteOrigin = new Vector2(_fireballtexture.Width / 2, _fireballtexture.Height / 2)
            };
            _eventManager.Publish(new PlaySoundFxEvent("fireball.shoot", transformComponent.Position, 1f));
            var projectileEntity = _projectileFactory.Create(data);
        }

        // Build once (e.g., LoadContent). A crisp white disc you can tint/scale.
        static Texture2D BuildFilledCircle(GraphicsDevice gd, int diameter)
        {
            var tex = new Texture2D(gd, diameter, diameter);
            var data = new Color[diameter * diameter];

            int r = diameter / 2;
            int r2 = r * r;
            int i = 0;

            for (int y = 0; y < diameter; y++)
            {
                int dy = y - r;
                for (int x = 0; x < diameter; x++)
                {
                    int dx = x - r;
                    // inside circle -> opaque white, else transparent
                    data[i++] = (dx * dx + dy * dy) <= r2 ? Color.Blue : Color.Transparent;
                }
            }
            tex.SetData(data); // load pixel data into the texture
            return tex;
        }
    }
}
