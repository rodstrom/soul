using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Soul
{
    class LesserDemon : Entity
    {
        private bool playerHit = false;
        private Vector2 hitPosition = Vector2.Zero;
        private HitFX hitFx = null;

         public LesserDemon(SpriteBatch spriteBatch, Soul game, string alias, Vector2 target)
            : base(spriteBatch, game, Constants.LESSER_DEMON_FILENAME, new Vector2(Constants.LESSER_DEMON_WIDTH, Constants.LESSER_DEMON_HEIGHT), alias, EntityType.LESSER_DEMON)
        {
            //maxVelocity = new Vector2 (Constants.LESSER_DEMON_MAX_SPEED, Constants.LESSER_DEMON_MAX_SPEED);
            acceleration = new Vector2 (Constants.LESSER_DEMON_ACCELERATION, Constants.LESSER_DEMON_ACCELERATION);
            //this.health = Constants.LESSER_DEMON_MAX_HEALTH;
            hitFx = new HitFX(game);
            //this.hitRadius = Constants.LESSER_DEMON_RADIUS;
        }

         public override void Draw()
         {
             if (hitFx.IsHit == true)
             {
                 Rectangle rect = new Rectangle(animation.CurrentFrame * (int)dimension.X, (int)animationState * (int)dimension.Y, (int)dimension.X, (int)dimension.Y);
                 sprite.Draw(position, rect, Color.White, rotation, offset, scale, SpriteEffects.None, layer, hitFx.Effect);
             }
             else
             {
                 base.Draw();
             }
         }

         public override void Update(GameTime gameTime)
         {
             hitFx.Update();
             //base.Update(gameTime);
             Move(gameTime);
             if (position.X > screenBoundaries.Width)
             {
                 killMe = false;
             }
         }

         public void Move(GameTime gameTime)
         {
             if (playerHit == false)
             {
                 Vector2 newVelocity = target - position;
                 newVelocity.Normalize();
                 velocity.Y = newVelocity.Y;

                 velocity = velocity + newVelocity * acceleration;
                 if (position.X >= target.X)
                 {
                     velocity.X += 1.0f;
                 }

                 if (velocity.Y > 0.0f)
                 {
                     velocity.Y += 2.0f;
                 }
                 else if (velocity.Y < 0.0f)
                 {
                     velocity.Y -= 2.0f;
                 }

                 if (velocity.Length() > maxVelocity.Length())
                 {
                     velocity.Normalize();
                     velocity *= maxVelocity;
                 }

                 position += velocity;
             }
             else if (playerHit == true)
             {                 
                 position = target + hitPosition;
             }
         }

         public override void onCollision(Entity entity)
         {

             if (entity.Type == EntityType.PLAYER_BULLET || entity.Type == EntityType.DARK_WHISPER || entity.Type == EntityType.DARK_WHISPER_SPIKE)
                 {
                     health -= entity.getDamage();
                     hitFx.Hit();
                     if (health <= 0)
                     {
                         killMe = true;
                     }
                 }
                 else if (entity.Type == EntityType.PLAYER && playerHit == false)
                 {
                     playerHit = true;
                     hitPosition = new Vector2(position.X, position.Y);
                     hitPosition -= entity.position;
                     ghost = true;
                 }
         }

         public override void takeDamage(int value)
         {
             health -= value;
             hitFx.Hit();
             if (health <= 0)
             {
                 killMe = true;
             }
         }

         public override int getDamage()
         {
             return 0;
         }

    }
}
