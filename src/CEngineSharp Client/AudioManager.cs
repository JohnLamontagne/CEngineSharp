using SFML.Audio;
using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Client
{
    public class AudioManager
    {
        private static Dictionary<string, SoundBuffer> _sounds;
        private static Dictionary<string, Music> _music;
        private static Music _currentMusic;

        public static void LoadSounds(string soundFilePath)
        {
            DirectoryInfo dI = new DirectoryInfo(soundFilePath);

            _sounds = new Dictionary<string, SoundBuffer>();

            foreach (var file in dI.GetFiles("*.ogg", SearchOption.AllDirectories))
            {
                _sounds.Add(file.Name.Remove(file.Name.Length - 4, 4), new SoundBuffer(file.FullName));
            }
        }

        public static void PlayMusic(string musicName)
        {
            _currentMusic = _music[musicName];
            _currentMusic.Play();
        }

        public static void StopMusic()
        {
            _currentMusic.Stop();
        }

        public static void PauseMusic()
        {
            _currentMusic.Pause();
        }

        public static void ResumeMusic()
        {
            _currentMusic.Play();
        }

        public static void Play2DSound(string soundName, double listenerX, double listenerY, double soundX, double soundY, int maxSoundDistance)
        {
            double distanceX = Math.Abs(listenerX - soundX);
            double distanceY = Math.Abs(listenerY - soundY);
            double totalDistance = Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceX, 2)));

            if (totalDistance > maxSoundDistance) return;

            int volume = (int)(maxSoundDistance - totalDistance) * (100 / maxSoundDistance);

            Sound sound = new Sound(_sounds[soundName]);
            sound.Volume = volume;
            sound.Play();
        }

        public static void PlaySound(string soundName)
        {
            Sound sound = new Sound(_sounds[soundName]);
            sound.Loop = true;
            sound.Play();
        }

        public static Sound GetSound(string soundName)
        {
            return new Sound(_sounds[soundName]);
        }
    }
}