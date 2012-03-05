using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Soul
{
    static class Constants
    {
        public const int RESOLUTION_VIRTUAL_WIDTH = 1280;
        public const int RESOLUTION_VIRTUAL_HEIGHT = 720;

        public const int WEAPON_LEVEL_LOWEST = 0;
        public const int WEAPON_LEVEL_HIGHEST = 5;
        public const float WEAPON_DIMENSION = 50.0f;
        public const string WEAPON_POWERUP_FILENAME = "Particles\\weapon_powerup";
        public const float WEAPON_POWERUP_RADIUS = 40.0f;
        
        public const float BULLET_VELOCITY = 15.0f;
        public const float BULLET_RADIUS = 5.0f;

        public const float HEALTH_POWERUP_DIMENSION = 40.0f;
        public const string HEALTH_POWERUP_FILENAME = "Particles\\bullet_glow";
        public const float HEALTH_POWERUP_RADIUS = 40.0f;
        
        public const float PLAYER_ACCELERATION = 1.0f;
        public const float PLAYER_MAX_SPEED = 5.0f;
        public const float PLAYER_LOWEST_SPEED = 1.0f;
        public const int PLAYER_MAX_HEALTH = 100;
        public const int PLAYER_WEAPON_DAMAGE = 10;
        public const float PLAYER_WIDTH = 128.0f;
        public const float PLAYER_HEIGHT = 128.0f;
        public const float PLAYER_RADIUS = 128.0f;
        public const float PLAYER_DEACCELERATION = 0.5f;
        public const int PLAYER_NUMBER_OF_DEATH_GLOWS = 80;
        public const string PLAYER_FILENAME = "SpriteSheets\\Avatar move animation B";
        public const string PLAYER_SHOOT_ANIM = "SpriteSheets\\avatar_shoot_anim_A-1";
        public const string PLAYER_GLOW_FILENAME = "Particles\\Heroine Glow Final_2";
        public const string PLAYER_BULLET_FILENAME = "Particles\\bullet_glow";
        public const long PLAYER_FIRE_RATE = 1200000;
        public const float PLAYER_SHOOT_DIMENSION = 128.0f;

        public const string GLOW_PARTICLE_FILENAME = "Particles\\bullet_glow";
        public const float GLOW_PARTICLE_DIMENSION = 40.0f;
        public const float GLOW_PARTICLE_ACCELERATION = 0.3f;
        public const float GLOW_PARTICLE_MAX_SPEED = 2.0f;

        public const float NIGHTMARE_ACCELERATION = 0.5f;
        public const float NIGHTMARE_MAX_SPEED = 4.0f;
        public const int NIGHTMARE_MAX_HEALTH = 100;
        public const float NIGHTMARE_WIDTH = 128.0f;
        public const float NIGHTMARE_HEIGHT = 128.0f;
        public const float NIGHTMARE_RADIUS = 128.0f;        
        public const int NIGHTMARE_DAMAGE = 10;
        public const string NIGHTMARE_FILENAME = "SpriteSheets\\Nightmare-Move-anim-B";
        
        public const float BLUE_BLOOD_VESSEL_MAX_SPEED = 8.0f;
        public const float BLUE_BLOOD_VESSEL_ACCELERATION = 0.5f;
        public const int BLUE_BLOOD_VESSEL_MAX_HEALTH = 100;
        public const float BLUE_BLOOD_VESSEL_WIDTH = 64.0f;
        public const float BLUE_BLOOD_VESSEL_HEIGHT = 64.0f;
        public const float BLUE_BLOOD_VESSEL_RADIUS = 64.0f;
        public const int BLUE_BLOOD_VESSEL_DAMAGE = 10;
        public const string BLUE_BLOOD_VESSEL_FILENAME = "SpriteSheets\\Bloodvessel1_fixed";

        public const float RED_BLOOD_VESSEL_MAX_SPEED = 8.0f;
        public const float RED_BLOOD_VESSEL_ACCELERATION = 0.5f;
        public const int RED_BLOOD_VESSEL_MAX_HEALTH = 100;
        public const float RED_BLOOD_VESSEL_WIDTH = 64.0f;
        public const float RED_BLOOD_VESSEL_HEIGHT = 64.0f;
        public const float RED_BLOOD_VESSEL_RADIUS = 64.0f;
        public const int RED_BLOOD_VESSEL_DAMAGE = 10;
        public const string RED_BLOOD_VESSEL_FILENAME = "SpriteSheets\\Bloodvessel2";

        public const float PURPLE_BLOOD_VESSEL_MAX_SPEED = 8.0f;
        public const float PURPLE_BLOOD_VESSEL_ACCELERATION = 0.5f;
        public const int PURPLE_BLOOD_VESSEL_MAX_HEALTH = 100;
        public const float PURPLE_BLOOD_VESSEL_WIDTH = 64.0f;
        public const float PURPLE_BLOOD_VESSEL_HEIGHT = 64.0f;
        public const float PURPLE_BLOOD_VESSEL_RADIUS = 64.0f;
        public const int PURPLE_BLOOD_VESSEL_DAMAGE = 10;
        public const string PURPLE_BLOOD_VESSEL_FILENAME = "SpriteSheets\\Bloodvessel3";

        public const int DARK_THOUGHT_WEAPON_DAMAGE = 10;
        public const float DARK_THOUGHT_ATTACK_DELAY = 800.0f;
        public const float DARK_THOUGHT_ATTACK_CONSISTENCY = 300.0f;
        public const float DARK_THOUGHT_ACCELERATION = 0.5f;
        public const float DARK_THOUGHT_MAX_SPEED = 5.0f;
        public const int DARK_THOUGHT_MAX_HEALTH = 100;
        public const float DARK_THOUGHT_WIDTH = 128.0f;
        public const float DARK_THOUGHT_HEIGHT = 128.0f;
        public const string DARK_THOUGHT_FILENAME = "SpriteSheets\\Dark-Thought-Animation-A";
        public const float DARK_THOUGHT_RADIUS = 128.0f;
        public const string DARK_THOUGHT_BULLET_FILENAME = "Particles\\bullet_glow_dark";

        public const int DARK_WHISPER_DAMAGE = 10;
        public const int DARK_WHISPER_SPIKE_DAMAGE = 5;
        public const int DARK_WHISPER_MAX_HEALTH = 100;
        public const float DARK_WHISPER_ACCELERATION = 0.5f;
        public const float DARK_WHISPER_MAX_SPEED = 5.0f;
        public const float DARK_WHISPER_WIDTH = 128.0f;
        public const float DARK_WHISPER_HEIGHT = 128.0f;
        public const float DARK_WHISPER_SPIKE_VELOCITY = 5.0f;
        public const float DARK_WHISPER_RADIUS = 128.0f;
        public const string DARK_WHISPER_FILENAME = "SpriteSheets\\Dark-Whisper-Death-animation";

        public const float LESSER_DEMON_MAX_SPEED = 15.0f;
        public const float LESSER_DEMON_ACCELERATION = 0.5f;
        public const int LESSER_DEMON_MAX_HEALTH = 100;
        public const float LESSER_DEMON_WIDTH = 32.0f;
        public const float LESSER_DEMON_HEIGHT = 32.0f;
        public const string LESSER_DEMON_FILENAME = "SpriteSheets\\LesserDemon1";
        public const string LESSER_DEMON_FILENAME2 = "SpriteSheets\\LesserDemon2";
        public const float LESSER_DEMON_RADIUS = 32.0f;
        public const int INNER_DEMON_LOWEST_SPAWN_RATE = 1000;
        public const int INNER_DEMON_HIGHEST_SPAWN_RATE = 2000;
        public const float INNER_DEMON_MAX_SPEED = 5.0f;
        public const float INNER_DEMON_ACCELERATION = 0.4f;
        public const int INNER_DEMON_MAX_HEALTH = 100;
        public const float INNER_DEMON_WIDTH = 132.0f;
        public const float INNER_DEMON_HEIGHT = 88.0f;
        public const string INNER_DEMON_FILENAME = "SpriteSheets\\InnerDemon1";
        public const string INNER_DEMON_FILENAME2 = "SpriteSheets\\InnerDemon2";
        public const float INNER_DEMON_RADIUS = 120.0f;

        public const string SPLASH_SCREEN_FILENAME = "Backgrounds\\splash_screen_1.1";
        public const string MENU_BG_FILENAME = "Backgrounds\\menubg";
        public const string BLACK = "Particles\\black";
        public const string MENU_HOVER = "GUI\\menu_Marker";

        public const string NORMAL_MAP_FILENAME = "Shaders\\normalmap";
        public const string FLASH_EFFECT_FILENAME = "Shaders\\hit";

        public const string BRAIN_MAP_BG = "BrainMap\\brain_map_base";

        public const string AUDIO_INTRO = "backgroundSound0";
        public const string AUDIO_MENU = "backgroundSound0";
        public const string AUDIO_PLAYER_FIRE = "fireSound0";

        public const string SAVE_DATA_FILENAME = "Content\\Saves\\savedata.dat";

        public const string PILLAR_1 = "Backgrounds\\Pillars\\Pillar1__0001_Normal";
        public const string PILLAR_2 = "Backgrounds\\Pillars\\Pillar2__0001_Normal";
        public const string PILLAR_3 = "Backgrounds\\Pillars\\Pillar3__0001_Normal";
        public const string PILLAR_4 = "Backgrounds\\Pillars\\Pillar4__0001_Normal";
        public const string PILLAR_5 = "Backgrounds\\Pillars\\Pillar5__0001_Normal";
        public const string PILLAR_6 = "Backgrounds\\Pillars\\Pillar6__0001_Normal";
        public const string PILLAR_7 = "Backgrounds\\Pillars\\Pillar7__0001_Normal";
        public const string PILLAR_8 = "Backgrounds\\Pillars\\Pillar8__0001_Normal";
        public const string PILLAR_9 = "Backgrounds\\Pillars\\Pillar9__0001_Normal";
        public const string PILLAR_10 = "Backgrounds\\Pillars\\Pillar10__0001_Normal";
        public const string PILLAR_11 = "Backgrounds\\Pillars\\Pillar11__0001_Normal";
        public const string PILLAR_12 = "Backgrounds\\Pillars\\Pillar12__0001_Normal";
        public const string PILLAR_13 = "Backgrounds\\Pillars\\Pillar13__0001_Normal";
        public const string PILLAR_14 = "Backgrounds\\Pillars\\Pillar14__0001_Normal";
        public const string PILLAR_15 = "Backgrounds\\Pillars\\Pillar15__0001_Normal";

        public const string LEVEL01 = "Content\\Levels\\level01.map";
        public const string LEVEL02 = "Content\\Levels\\level02.map";
        public const string LEVEL03 = "Content\\Levels\\level03.map";
        public const string LEVEL04 = "Content\\Levels\\level04.map";
        public const string LEVEL05 = "Content\\Levels\\level05.map";
        public const string LEVEL06 = "Content\\Levels\\level06.map";

    }
}
