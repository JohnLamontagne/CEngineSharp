using SFML.Audio;
using System;
using System.Collections.Generic;
using System.IO;

namespace CEngineSharp_Client.Audio
{
    public sealed class SfxManager
    {
        private Dictionary<string, SoundBuffer> _sounds;

        public void LoadSounds(string soundFilePath)
        {
            var dI = new DirectoryInfo(soundFilePath);

            if (_sounds == null)
                _sounds = new Dictionary<string, SoundBuffer>();
            else
                _sounds.Clear();

            Console.WriteLine(@"Loading sound effects.");

            foreach (var file in dI.GetFiles("*.ogg", SearchOption.AllDirectories))
            {
                _sounds.Add(file.Name.Remove(file.Name.Length - 4, 4), new SoundBuffer(file.FullName));
            }

            Console.WriteLine(@"Loaded {0} sound effects.", _sounds.Count);
        }

        public void PlaySound(string soundName, double listenerX, double listenerY, double soundX, double soundY, int maxSoundDistance)
        {
            var distanceX = Math.Abs(listenerX - soundX);
            var distanceY = Math.Abs(listenerY - soundY);
            var totalDistance = Math.Sqrt((Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2)));

            if (totalDistance > maxSoundDistance) return;

            var volume = (int)(maxSoundDistance - totalDistance) * (100 / maxSoundDistance);

            var sound = new Sound(_sounds[soundName])
            {
                Volume = volume
            };
            sound.Play();
        }

        public void PlaySound(string soundName)
        {
            var sound = new Sound(_sounds[soundName])
            {
                Loop = true
            };

            sound.Play();
        }

        public Sound GetSound(string soundName)
        {
            return new Sound(_sounds[soundName]);
        }

    }
}