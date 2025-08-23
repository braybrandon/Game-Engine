using Common.Interfaces;
using Common.Physics.Components;
using GameEngine.Graphics.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics.Render
{
    public class LightFxRenderSystem(GraphicsDevice graphicsDevice, IEntity playerEntity, IEntity cameraEntity, RenderTarget2D lightTarget, RenderTarget2D mainTarget, Texture2D lightTexture, Effect lightEffect) : IDrawSystem
    {
        private readonly IEntity _playerEntity = playerEntity;
        private readonly IEntity _cameraEntity = cameraEntity;
        private readonly GraphicsDevice _graphicsDevice = graphicsDevice;
        private readonly RenderTarget2D _lightTarget = lightTarget;
        private readonly RenderTarget2D _mainTarget = mainTarget;
        private Texture2D _lightTexture = lightTexture;
        private Effect _lightEffect = lightEffect;

        public void Draw(SpriteBatch _spriteBatch, IWorld world)
        {
            var camera = world.GetComponent<CameraComponent>(_cameraEntity.Id);
            var viewMatrix = camera.ViewMatrix;
            _graphicsDevice.SetRenderTarget(_lightTarget);
            _graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(blendState: BlendState.Additive, transformMatrix: viewMatrix);
            Vector2 playerPos = world.GetComponent<TransformComponent>(_playerEntity.Id).Position;
            _spriteBatch.Draw(_lightTexture, playerPos, null, Color.White, 0f, new Vector2(_lightTexture.Width / 2), 2f, SpriteEffects.None, 0f);
            _spriteBatch.End();

            _graphicsDevice.SetRenderTarget(null);
            _graphicsDevice.Clear(Color.Black);
            _lightEffect.Parameters["lightMask"].SetValue(_lightTarget);

            _spriteBatch.Begin(effect: _lightEffect, blendState: BlendState.Opaque);
            _spriteBatch.Draw(_mainTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }
    }
}
