using GameEngine.Core.Components;
using GameEngine.Core.Systems;
using GameEngine.Rendering.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameEngine.Rendering
{
    public class SpriteRenderSystem : EngineSystem
    {
        public SpriteRenderSystem(Game game) : base(game) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Matrix cameraViewMatrix = Matrix.Identity;

            var cameraEntities = ComponentManager.GetEntitiesWith<CameraComponent, TransformComponent>();
            foreach(var entity in cameraEntities)
            {
                CameraComponent camera = ComponentManager.GetComponent<CameraComponent>(entity);
                cameraViewMatrix = camera.ViewMatrix;
                break;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cameraViewMatrix);

            foreach (var entity in ComponentManager.GetEntitiesWith<TransformComponent, SpriteComponent>())
            {
                var transform = ComponentManager.GetComponent<TransformComponent>(entity);
                var sprite = ComponentManager.GetComponent<SpriteComponent>(entity);

                spriteBatch.Draw(
                    sprite.Texture,
                    transform.Position,
                    sprite.SourceRectangle,
                    sprite.Color,
                    transform.Rotation,
                    sprite.Origin,
                    transform.Scale,
                    sprite.Effects,
                    sprite.LayerDepth
                );
            }

            spriteBatch.End();
        }
    }
}
