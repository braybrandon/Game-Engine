using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace rpg
{
    class Projectile
    {

        public static List<Projectile> projectiles = new List<Projectile>();

        private Vector2 position;
        private Vector2 startPostion;
        private int speed = 1000;
        private int distance = 1000;
        public int radius = 18;
        private Vector2 direction;
        private bool collided = false;
        private bool traveledMaxDistance = false;

        public Projectile(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.startPostion = position;
            this.direction = direction;
        }

        public Vector2 Position { get { return position; } }

        public bool Collided { get { return collided; } set { collided = value; } }
        public bool TraveledMaxDistance { get { return traveledMaxDistance; } }

        private float GetDistanceTraveled()
        {
            return Vector2.Distance(startPostion, position);
        }

        private bool CheckTotalDistance()
        {
            return GetDistanceTraveled() >= distance;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += direction * -speed * dt;
            traveledMaxDistance = CheckTotalDistance();
        }
    }
}
