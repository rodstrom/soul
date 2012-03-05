using System;

namespace Soul
{
#if WINDOWS || XBOX

    enum EntityType
    {
        NONE = 0,
        PLAYER,
        PLAYER_BULLET,
        DARK_THOUGHT,
        DARK_THOUGHT_BULLET,
        NIGHTMARE,
        BLUE_BLOOD_VESSEL,
        RED_BLOOD_VESSEL,
        PURPLE_BLOOD_VESSEL,
        LESSER_DEMON,
        INNER_DEMON,
        DIE_PARTICLE,
        WEAPON_POWERUP,
        HEALTH_POWERUP,
        DARK_WHISPER,
        DARK_WHISPER_SPIKE
    };

    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Soul game = new Soul())
            {
                game.Run();
            }
        }
    }
#endif
}

