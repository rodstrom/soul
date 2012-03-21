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

        public const int BOSS_WIDTH = 150;
        public const int BOSS_HEIGHT = 600;
        public const int BOSS_DEATH_WIDTH = 640;
        public const int BOSS_DEATH_HEIGHT = 700;
        public const int BOSS_MAX_HEALTH = 200;
        public const int BOSS_DAMAGE = 20;
        public const string BOSS_IDLE_FILENAME = "SpriteSheets\\Boss";
        public const string BOSS_SHOOT_FILENAME = "SpriteSheets\\Boss_Shoot";
        public const string BOSS_SPAWN_FILENAME = "SpriteSheets\\Boss_Spawn";
        public const string BOSS_DEATH_FILENAME = "SpriteSheets\\Boss_Death";
        public const string BOSS_BULLET_FILENAME = "Particles\\bullet_glow_dark";

        public const int WEAPON_LEVEL_LOWEST = 0;
        public const int WEAPON_LEVEL_HIGHEST = 5;

        public const float WEAPON_POWERUP_SPREAD_DIMENSION = 64.0f;
        public const float WEAPON_POWERUP_SPREAD_RADIUS = 35.0f;
        public const string WEAPON_POWERUP_SPREAD_FILENAME = "SpriteSheets\\Powerup_Spread";

        public const float WEAPON_POWERUP_RAPID_DIMENSION = 64.0f;
        public const float WEAPON_POWERUP_RAPID_RADIUS = 20.0f;
        public const string WEAPON_POWERUP_RAPID_FILENAME = "SpriteSheets\\Powerup_Rapid";
        
        public const float BULLET_VELOCITY = 15.0f;
        public const float BULLET_RADIUS = 5.0f;

        public const float HEALTH_POWERUP_DIMENSION = 64.0f;
        public const string HEALTH_POWERUP_FILENAME = "SpriteSheets\\Powerup_Health";
        public const float HEALTH_POWERUP_RADIUS = 20.0f;
        
        public const float PLAYER_ACCELERATION = 1.0f;
        public const float PLAYER_MAX_SPEED = 5.0f;
        public const float PLAYER_LOWEST_SPEED = 1.0f;
        public const int PLAYER_MAX_HEALTH = 100;
        public const int PLAYER_WEAPON_DAMAGE = 10;
        public const float PLAYER_WIDTH = 128.0f;
        public const float PLAYER_HEIGHT = 128.0f;
        public const float PLAYER_RADIUS = 128.0f;
        public const float PLAYER_DEACCELERATION = 1.0f;
        public const int PLAYER_NUMBER_OF_DEATH_GLOWS = 10;
        public const string PLAYER_FILENAME = "SpriteSheets\\Avata animation Sprite_Idle B";
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
        public const string NIGHTMARE_FILENAME = "SpriteSheets\\Nightmare-Animation_A";
        
        public const float BLUE_BLOOD_VESSEL_MAX_SPEED = 8.0f;
        public const float BLUE_BLOOD_VESSEL_ACCELERATION = 0.5f;
        public const int BLUE_BLOOD_VESSEL_MAX_HEALTH = 100;
        public const float BLUE_BLOOD_VESSEL_WIDTH = 64.0f;
        public const float BLUE_BLOOD_VESSEL_HEIGHT = 64.0f;
        public const float BLUE_BLOOD_VESSEL_RADIUS = 64.0f;
        public const int BLUE_BLOOD_VESSEL_DAMAGE = 10;
        public const string BLUE_BLOOD_VESSEL_FILENAME = "SpriteSheets\\Blood-Vessel-Death-Anim-Blue";

        public const float RED_BLOOD_VESSEL_MAX_SPEED = 8.0f;
        public const float RED_BLOOD_VESSEL_ACCELERATION = 0.5f;
        public const int RED_BLOOD_VESSEL_MAX_HEALTH = 100;
        public const float RED_BLOOD_VESSEL_WIDTH = 64.0f;
        public const float RED_BLOOD_VESSEL_HEIGHT = 64.0f;
        public const float RED_BLOOD_VESSEL_RADIUS = 64.0f;
        public const int RED_BLOOD_VESSEL_DAMAGE = 10;
        public const string RED_BLOOD_VESSEL_FILENAME = "SpriteSheets\\BloodVesselDeathAnimRed";

        public const float PURPLE_BLOOD_VESSEL_MAX_SPEED = 8.0f;
        public const float PURPLE_BLOOD_VESSEL_ACCELERATION = 0.5f;
        public const int PURPLE_BLOOD_VESSEL_MAX_HEALTH = 100;
        public const float PURPLE_BLOOD_VESSEL_WIDTH = 64.0f;
        public const float PURPLE_BLOOD_VESSEL_HEIGHT = 64.0f;
        public const float PURPLE_BLOOD_VESSEL_RADIUS = 64.0f;
        public const int PURPLE_BLOOD_VESSEL_DAMAGE = 10;
        public const string PURPLE_BLOOD_VESSEL_FILENAME = "SpriteSheets\\BloodVesselDeathAnimPurple";

        public const int DARK_THOUGHT_WEAPON_DAMAGE = 10;
        public const float DARK_THOUGHT_ATTACK_DELAY = 800.0f;
        public const float DARK_THOUGHT_ATTACK_CONSISTENCY = 300.0f;
        public const float DARK_THOUGHT_ACCELERATION = 0.5f;
        public const float DARK_THOUGHT_MAX_SPEED = 5.0f;
        public const int DARK_THOUGHT_MAX_HEALTH = 100;
        public const float DARK_THOUGHT_WIDTH = 128.0f;
        public const float DARK_THOUGHT_HEIGHT = 128.0f;
        public const string DARK_THOUGHT_FILENAME = "SpriteSheets\\Dark-Thought-Animation";
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
        public const float INNER_DEMON_WIDTH = 128.0f;
        public const float INNER_DEMON_HEIGHT = 128.0f;
        public const string INNER_DEMON_FILENAME = "SpriteSheets\\InnerDemonDeathAnim";
        public const string INNER_DEMON_FILENAME2 = "SpriteSheets\\InnerDemon2";
        public const float INNER_DEMON_RADIUS = 120.0f;

        public const string SPLASH_SCREEN_FILENAME = "Backgrounds\\splash_screen_1.1";
        public const string MENU_BG_FILENAME = "Backgrounds\\menubg";
        public const string BLACK = "Particles\\black";
        public const string MENU_HOVER = "GUI\\menu_Marker";

        public const string NORMAL_MAP_FILENAME = "Shaders\\normalmap";
        public const string FLASH_EFFECT_FILENAME = "Shaders\\hit";
        public const string FLASH_EFFECT_GREEN_FILENAME = "Shaders\\hitGreen";
        public const string FLASH_EFFECT_RED_FILENAME = "Shaders\\hitRed";

        public const string BRAIN_MAP_BG = "BrainMap\\brain_map_base";
        public const string BRAIN_MAP_STATUS = "BrainMap\\status";
        public const string BRAIN_MAP_INFECTED = "BrainMap\\infected";
        public const string BRAIN_MAP_CLEANSED = "BrainMap\\cleansed";

        public const string AUDIO_INTRO = "backgroundSound0";
        public const string AUDIO_MENU = "backgroundSound0";
        public const string AUDIO_PLAYER_FIRE = "fireSound0";

        public const string SAVE_DATA_FILENAME = "Content\\Saves\\savedata.dat";

        public const string PILLAR_1 = "Backgrounds\\Pillars\\Pillar01";
        public const string PILLAR_2 = "Backgrounds\\Pillars\\Pillar02";
        public const string PILLAR_3 = "Backgrounds\\Pillars\\Pillar03";
        public const string PILLAR_4 = "Backgrounds\\Pillars\\Pillar04";
        public const string PILLAR_5 = "Backgrounds\\Pillars\\Pillar05";
        public const string PILLAR_6 = "Backgrounds\\Pillars\\Pillar06";
        public const string PILLAR_7 = "Backgrounds\\Pillars\\Pillar07";
        public const string PILLAR_8 = "Backgrounds\\Pillars\\Pillar08";
        public const string PILLAR_9 = "Backgrounds\\Pillars\\Pillar09";
        public const string PILLAR_10 = "Backgrounds\\Pillars\\Pillar10";
        public const string PILLAR_11 = "Backgrounds\\Pillars\\Pillar11";
        public const string PILLAR_12 = "Backgrounds\\Pillars\\Pillar12";
        public const string PILLAR_13 = "Backgrounds\\Pillars\\Pillar13";
        public const string PILLAR_14 = "Backgrounds\\Pillars\\Pillar14";
        public const string PILLAR_15 = "Backgrounds\\Pillars\\Pillar15";

        public const string BACKGROUND_FOG = "Backgrounds\\background__0002s_0001_Layer-8";
        public const string BACKGROUND_FOG_NORMAL = "Backgrounds\\background__0002s_0001_Layer-8_fixed_depth";

        public const string LEVEL01 = "Content\\Levels\\level01.map";
        public const string LEVEL02 = "Content\\Levels\\level02.map";
        public const string LEVEL03 = "Content\\Levels\\level03.map";
        public const string LEVEL04 = "Content\\Levels\\level04.map";
        public const string LEVEL05 = "Content\\Levels\\level05.map";
        public const string LEVEL06 = "Content\\Levels\\level06.map";

        public const string GUI_SLIDER = "GUI\\options_Slider";
        public const string GUI_SLIDER_MARKER = "GUI\\options_SliderMarker";
        public const string GUI_EFFECTS_VOLUME = "GUI\\options_EffectsVolume";
        public const string GUI_MUSIC_VOLUME = "GUI\\options_MusicVolume";
        public const string GUI_FULLSCREEN = "GUI\\options_Fullscreen";
        public const string GUI_WINDOWED = "GUI\\options_Windowed";
        public const string GUI_ARROW_LEFT = "GUI\\arrow_Left";
        public const string GUI_ARROW_RIGHT = "GUI\\arrow_Right";
        public const string GUI_BACK = "GUI\\menu_Back";
        public const string GUI_OPTIONS = "GUI\\menu_Options";
        public const string GUI_LOGO = "GUI\\logo_SOUL";
        public const string GUI_MARKER = "GUI\\menu_Marker";
        public const string GUI_CONTROLS = "GUI\\menu_Controls";
        public const string GUI_GRAPHICS = "GUI\\gui_graphics";
        public const string GUI_SOUND = "GUI\\gui_sound";
        public const string GUI_START = "GUI\\menu_start";
        public const string GUI_CREDITS = "GUI\\menu_credits";
        public const string GUI_QUIT = "GUI\\menu_Quit";
        public const string GUI_DYNAMIC_LIGHTING = "GUI\\gui_dynamicLighting";
        public const string GUI_HIGH = "GUI\\gui_high";
        public const string GUI_MEDIUM = "GUI\\gui_medium";
        public const string GUI_LOW = "GUI\\gui_low";
        public const string GUI_ON = "GUI\\gui_on";
        public const string GUI_OFF = "GUI\\gui_off";
        public const string GUI_SPECULAR = "GUI\\gui_specular";
        public const string GUI_SCREEN_MODE = "GUI\\gui_screenMode";
        public const string GUI_RESOLUTION = "GUI\\gui_resolution";
        public const string GUI_SHOOT = "GUI\\controls_Shoot";
        public const string GUI_UP = "GUI\\controls_up";
        public const string GUI_DOWN = "GUI\\controls_down";
        public const string GUI_LEFT = "GUI\\controls_Left";
        public const string GUI_RIGHT = "GUI\\controls_Right";
        public const string GUI_DEVELOPERS_CREDITS = "GUI\\text_Developers";
        public const string GUI_YES = "GUI\\gui_yes";
        public const string GUI_NO = "GUI\\gui_no";
        public const string GUI_CLEANSE = "GUI\\gui_cleanse";
        public const string GUI_CONTINUE = "GUI\\Continue";
        public const string GUI_GAME_PLAY = "GUI\\Game_play";
        public const string GUI_TUTORIAL = "GUI\\Tutorial";

        public const string GUI_FONT = "GUI\\Extrafine";

        public const string MENU_COMBINED_BG_COLORMAP = "Backgrounds\\background__0002s_0008_Layer-1_combined";
        public const string MENU_COMBINED_BG_NORMALMAP = "Backgrounds\\background__0002s_0008_Layer-1_combined_depth";
        public const string MENU_FRONT_BG_COLORMAP = "Backgrounds\\bg_colorMap";
        public const string MENU_FRONT_BG_NORMALMAP = "Backgrounds\\bg_normalMap";
        public const string MENU_SPIKES_BG_COLORMAP = "Backgrounds\\bg2_color_map";

        public const string TUTORIAL_BUTTON_FRAME = "TutorialGFX\\buttonFrame";
        public const string TUTORIAL_ARROW = "TutorialGFX\\Arrow";
        public const string TUTORIAL_BUTTON_FRAME_LARGE = "TutorialGFX\\bigButtonFrame";
        public const string TUTORIAL_BUTTON_FRAME_XLLARGE = "TutorialGFX\\biggerButtonFrame";
        public const string TUTORIAL_BUTTON_ENTER = "TutorialGFX\\Enter";
        public const string TUTORIAL_BUTTON_FRAME_ENTER = "TutorialGFX\\enterFrame";
        public const string TUTORIAL_BUTTON_SHIFT = "TutorialGFX\\Shift";
    }
}
