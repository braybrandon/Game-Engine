using GameEngine.Core.Components;
using GameEngine.Core.Systems;
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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var entity in ComponentManager.GetEntitiesWith<PositionComponent, SpriteComponent>())
            {
                var position = ComponentManager.GetComponent<PositionComponent>(entity);
                var sprite = ComponentManager.GetComponent<SpriteComponent>(entity);

                spriteBatch.Draw(
                    sprite.Texture,
                    position.Position,
                    sprite.SourceRectangle,
                    sprite.Color,
                    sprite.Rotation,
                    sprite.Origin,
                    sprite.Scale,
                    sprite.Effects,
                    sprite.LayerDepth
                );
            }

            spriteBatch.End();
        }
    }
}
