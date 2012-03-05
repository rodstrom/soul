using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Soul.Manager;

namespace Soul
{
    class CreditsState : State
    {
        Sprite background;
        Sprite logo;
        Sprite credits;
        Sprite back;
        Sprite hover;
        Sprite text;

        public CreditsState(SpriteBatch spriteBatch, Soul game, AudioManager audioManager, InputManager controls, string id) : base(spriteBatch, game, audioManager, controls, id) {}

        public override void initialize(string data)
        {
            background = new Sprite(spriteBatch, game, "Backgrounds\\menubg");
            logo = new Sprite(spriteBatch, game, "GUI\\logo_SOUL");
            credits = new Sprite(spriteBatch, game, "GUI\\text_Developers");
            back = new Sprite(spriteBatch, game, "GUI\\menu_Back");
            hover = new Sprite(spriteBatch, game, "GUI\\menu_Marker");
            text = new Sprite(spriteBatch, game, "GUI\\menu_Credits");
        }

        public override void shutdown()
        {

            changeState = false;
        }

        public override string getNextState()
        {
            return "MenuState";
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(0, null, null, null, null, null, Resolution.getTransformationMatrix());
            background.Draw(Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            logo.Draw(new Vector2(50f, 0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            credits.Draw(new Vector2(Constants.RESOLUTION_VIRTUAL_WIDTH / 2 - 180f, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            hover.Draw(new Vector2(90f, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 190), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            back.Draw(new Vector2(120f, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 + 190), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            text.Draw(new Vector2(120f, Constants.RESOLUTION_VIRTUAL_HEIGHT / 2 - 50f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        public override bool Update(GameTime gameTime)
        {
            if (controls.Pause || controls.ShootingOnce)
            {
                changeState = true;
            }
            return changeState;
        }

        public override string StateData()
        {
            return "";
        }
    }
}
