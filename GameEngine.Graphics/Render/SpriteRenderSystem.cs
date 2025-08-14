using Common.Interfaces;
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

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, cameraViewMatrix);

            foreach (var entity in world.GetEntitiesWith<TransformComponent, SpriteComponent>())
            {
                ref var transform = ref entity.GetComponent<TransformComponent>();
                ref var sprite = ref entity.GetComponent<SpriteComponent>();

                Vector2 position = transform.Position;
                Vector2 origin = sprite.Origin;
                Rectangle sourceRect = sprite.SourceRectangle;

                // Draw outline only around the player

                Rectangle outlineRect;
                Vector2 sorting = position;

                if (entity.HasComponent<ColliderComponent>())
                {
                    ref var collider = ref entity.GetComponent<ColliderComponent>();

                    // Local-space collider bounds (relative to sprite origin)
                    Rectangle local = collider.Bounds;

                        //sorting.X = position.X + local.X;
                        //sorting.Y = position.Y + local.Y;


                    // Offset from origin (position is where origin is placed)
                    outlineRect = new Rectangle(
                        (int)(position.X - local.X),
                        (int)(position.Y - local.Y),
                        local.Width,
                        local.Height
                    );
          
                }
                else
                {
                    // Fallback: full sprite frame
                    Vector2 size = new Vector2(sourceRect.Width, sourceRect.Height);
                    Vector2 topLeft = position - origin;
                    outlineRect = new Rectangle(
                        (int)topLeft.X,
                        (int)topLeft.Y,
                        (int)size.X,
                        (int)size.Y
                    );
                }
                sorting.Y = outlineRect.Y + outlineRect.Height;
                float totalWorldHeight = 320f; // Example, this should be the total height of your game world.



                // Normalize the Y position to a value between 0.0f and 1.0f.
                // The depth should be a value from 0 (front) to 1 (back).
                // A higher Y position should have a lower depth value.
                float depth = 1.0f - (sorting.Y / totalWorldHeight);

                // Clamp the value to ensure it's within the valid range [0.0, 1.0].
                depth = MathHelper.Clamp(depth, 0.0f, 1.0f);

                spriteBatch.Draw(
                    sprite.Texture,
                    position,
                    sprite.SourceRectangle,
                    sprite.Color,
                    transform.Rotation,
                    sprite.Origin,
                    transform.Scale,
                    sprite.Effects,
                    depth
                );


                //spriteBatch.Draw(_pixel, sorting - new Vector2(2, 2), null, Color.Red, 0f, Vector2.Zero, scale: 4f, SpriteEffects.None, layerDepth: 0f);
                //DrawRectangleOutline(spriteBatch, outlineRect, Color.Blue, 1);
            }

        spriteBatch.End();
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
        {
            // Top
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            // Bottom
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
            // Left
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            // Right
            spriteBatch.Draw(_pixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
        }
    }
}
