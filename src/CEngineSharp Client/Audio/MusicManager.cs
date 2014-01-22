using SFML.Audio;
using System.Collections.Generic;

namespace CEngineSharp_Client.Audio
{
    public sealed class MusicManager
    {
        private Dictionary<string, Music> _music;
        private Music _currentMusic;

        public void LoadMusic(string musicSoundPath)
        {
            _music = new Dictionary<string, Music>();
        }

        public void PlayMusic(string musicName)
        {
            _currentMusic = _music[musicName];
            _currentMusic.Play();
        }

        public void StopMusic()
        {
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
