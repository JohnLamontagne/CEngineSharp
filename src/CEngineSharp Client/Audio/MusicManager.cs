using SFML.Audio;
using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Client.Audio
{
    public sealed class MusicManager
    {
        private Dictionary<string, Music> _music;
        private Music _currentMusic;

        public void LoadMusic(string musicSoundPath)
        {
            var dI = new DirectoryInfo(musicSoundPath);

            if (_music == null)
                _music = new Dictionary<string, Music>();
            else
                _music.Clear();

            Console.WriteLine(@"Loading music.");

            foreach (var file in dI.GetFiles("*.ogg", SearchOption.AllDirectories))
            {
                _music.Add(file.Name.Remove(file.Name.Length - 4, 4), new Music(file.FullName));
            }

            Console.WriteLine(@"Loaded {0} music file(s).", _music.Count);
        }

        public void PlayMusic(string musicName)
        {
            _currentMusic = _music[musicName];
            _currentMusic.Play();
        }

        public void StopMusic()
        {
            if (_currentMusic != null)
                _currentMusic.Stop();
        }

        public void PauseMusic()
        {
            _currentMusic.Pause();
        }

        public void ResumeMusic()
        {
            _currentMusic.Play();
        }
    }
}