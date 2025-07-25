using GameEngine.Core.Components;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Physics
{
    public class MovementSystem : EngineSystem
    {
        public MovementSystem(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in ComponentManager.GetEntitiesWith<PositionComponent, VelocityComponent>())
            {
                // Get copies of the components
                var position = ComponentManager.GetComponent<PositionComponent>(entity);
                var velocity = ComponentManager.GetComponent<VelocityComponent>(entity);

                position.Position += velocity.Value * dt;

                // Add the modified position component back to update it
                ComponentManager.AddComponent(entity, position);
            }
        }
    }
}
