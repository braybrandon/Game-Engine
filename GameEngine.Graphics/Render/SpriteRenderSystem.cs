using Common.Interfaces;
using Common.Physics.Components;
using GameEngine.Core.Components;
using GameEngine.Graphics.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameEngine.Graphics.Render
{
    public class SpriteRenderSystem(Texture2D pixel) : IDrawSystem
    {
        private readonly Texture2D _pixel = pixel;
        public void Draw(SpriteBatch spriteBatch, IWorld world)
        {

            Matrix cameraViewMatrix = Matrix.Identity;
            var cameraEntities = world.GetEntitiesWith<CameraComponent, TransformComponent>();
            foreach (var entity in cameraEntities)
            {
                ref CameraComponent camera = ref entity.GetComponent<CameraComponent>();
                cameraViewMatrix = camera.ViewMatrix;
                break;
            }

            var drawableEntities = world.GetEntitiesWith<TransformComponent, SpriteComponent>()
                                       .OrderBy(entity => entity.GetComponent<TransformComponent>().Position.Y +
                                                         (entity.GetComponent<SpriteComponent>().SourceRectangle.Height * entity.GetComponent<TransformComponent>().Scale.Y) -
                                                         (entity.GetComponent<SpriteComponent>().Origin.Y * entity.GetComponent<TransformComponent>().Scale.Y));

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cameraViewMatrix);

            foreach (var entity in drawableEntities)
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
