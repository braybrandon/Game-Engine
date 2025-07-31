using Common.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Graphics.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameEngine.Graphics.Render
{
    public class SpriteRenderSystem : IDrawSystem
    {

        public void Draw(SpriteBatch spriteBatch, IWorld world)
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
