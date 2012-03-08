using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Soul.Manager
{
    class AudioManager
    {
        static Dictionary<string, SoundEffect> effectList = new Dictionary<string, SoundEffect>();
        static Dictionary<string, Song> songList = new Dictionary<string, Song>();
        static ContentManager content;
        static float volume = 1f;
        static Song current;
        Soul game;

        public AudioManager(ContentManager c, Soul game)
        {
            content = c;
            this.game = game;
            initialize();
        }

        private void initialize()
        {
            MediaPlayer.IsRepeating = true; 
            //IniFile config = new IniFile("Content\\Config\\config.ini");
            //config.parse();
            volume = float.Parse(game.config.getValue("Audio", "EffectsVolume")) / 100.0f;
            MediaPlayer.Volume = float.Parse(game.config.getValue("Audio", "MusicVolume")) / 100.0f;
            if (volume > 1.0f)
            {
                volume = 1.0f;
            }
            else if (volume < 0.0f)
            {
                volume = 0.0f;
            }
            if (MediaPlayer.Volume > 1.0f)
            {
                MediaPlayer.Volume = 1.0f;
            }
            else if (MediaPlayer.Volume < 0.0f)
            {
                MediaPlayer.Volume = 0.0f;
            }

            //addSong("iron-man", "backgroundSound0");

            //addEffect("comic010", "fireSound0");
        }

        public void addEffect(string filename, string asset)
        {
            string path = "Audio\\" + filename;
            string test = "Content\\" + path + ".xnb";
            if (System.IO.File.Exists(test) == false)
            {
                return;
            }

            effectList.Add(asset, content.Load<SoundEffect>(path));
        }

        public void addSong(string filename, string asset)
        {
            string path = "Audio\\" + filename;
            string test = "Content\\" + path + ".xnb";
            if (System.IO.File.Exists(test) == false)
            {
                return;
            }
            songList.Add(asset, content.Load<Song>(path));
        }

        public void playSound(string asset)
        {
            SoundEffect sound;
            if (effectList.TryGetValue(asset, out sound))
            {
                sound.Play(volume, 0f, 0f);
            }
        }

        public void playMusic(string asset)
        {
            current = null;

            if (songList.TryGetValue(asset, out current))
            {
                MediaPlayer.Play(current);
            }
        }

        public void stopMusic()
        {
            MediaPlayer.Stop();
        }

        public void setFXVolume(float v)
        {
            volume += v;
            if (volume < 0.0f)
            {
                volume = 0.0f;
            }
            else if (volume > 1.0f)
            {
                volume = 1.0f;
            }
        }

        public float getFXVolume()
        {
            return volume;
        }

        public void setMusicVolume(float v)
        {
            MediaPlayer.Volume += v;
            if (MediaPlayer.Volume < 0.0f)
            {
                MediaPlayer.Volume = 0.0f;
            }
            else if (MediaPlayer.Volume > 1.0f)
            {
                MediaPlayer.Volume = 1.0f;
            }
        }
    }
}
