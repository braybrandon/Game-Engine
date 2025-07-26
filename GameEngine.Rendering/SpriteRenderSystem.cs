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

        public override void Draw(SpriteBatch spriteBatch, World world)
        {
            Matrix cameraViewMatrix = Matrix.Identity;

            var cameraEntities = world.GetEntitiesWith<CameraComponent, TransformComponent>();
            foreach(var entity in cameraEntities)
            {
                ref CameraComponent camera = ref entity.GetComponent<CameraComponent>();
                cameraViewMatrix = camera.ViewMatrix;
                break;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cameraViewMatrix);

            foreach (var entity in world.GetEntitiesWith<TransformComponent, SpriteComponent>())
            {
                ref var transform = ref entity.GetComponent<TransformComponent>();
                ref var sprite = ref entity.GetComponent<SpriteComponent>();

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
