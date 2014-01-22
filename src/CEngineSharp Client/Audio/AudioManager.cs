using SFML.Audio;
using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Client.Audio
{
    public class AudioManager
    {
        #region Singleton

        private static AudioManager _audioManager;

        public static AudioManager Instance { get { return _audioManager ?? (_audioManager = new AudioManager()); } }

        #endregion

        public MusicManager MusicManager { get; set; }

        public SfxManager SfxManager { get; set; }

        public AudioManager()
        {
            SfxManager = new SfxManager();
            MusicManager = new MusicManager();

        }
    }
}