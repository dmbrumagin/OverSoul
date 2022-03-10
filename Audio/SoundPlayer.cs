using UnityEngine;
using OSSC;

namespace Sounds
{
    public class SoundPlayer : MonoBehaviour
    {
        //not sure if this limits sounds at all
        public static SoundPlayer soundPlayer;
        private SoundController soundController;
        private ISoundCue music;
        private ISoundCue[] soundChannels = new ISoundCue[6];
        private int channelNumber;

        private void Awake()
        {           
            soundPlayer = this;
            soundController = GetComponent<SoundController>();
            channelNumber = 0;            
        }

        public void Play(string soundName)
        {
            var settings = new PlaySoundSettings();
            settings.Init();
            settings.name = soundName;
            OpenChannel();
            PlaySound(settings);
        }

        public void PlayLooped(string soundName)
        {
            var settings = new PlaySoundSettings();
            settings.Init();
            settings.isLooped = true;
            settings.name = soundName;
            OpenChannel();
            PlaySound(settings);
        }

        public void PlayMusicNow(string soundName)
        {
            if (music != null)
                music.Stop();

            if (soundName.Contains("@"))
            {
                var strings = soundName.Split('@');
                soundName = strings[0];
            }

            var settings = new PlaySoundSettings();
            settings.Init();
            settings.name = soundName;
            settings.fadeInTime = .5f;
            settings.fadeOutTime = .5f;
            settings.categoryName = "Music"; // search only in that category
            settings.parent = transform;
            settings.isLooped = true;
            music = soundController.Play(settings);
        }

        public int OpenChannel()
        {
            for (int i = 0; i < soundChannels.Length - 1; i++)
            {
                if (soundChannels[i] == null)
                {
                    i = channelNumber;
                    return channelNumber;
                }

                else
                {
                    if (!soundChannels[i].IsPlaying)
                    {
                        i = channelNumber;
                        return channelNumber;
                    }
                }
            }

            Debug.LogWarning("No open sound channels");
            channelNumber = -1;
            return channelNumber;
        }
        private void PlaySound(PlaySoundSettings settings)
        {
            if (channelNumber >= 0) soundChannels[channelNumber] = soundController.Play(settings);
            else Debug.LogWarning("Max sounds reached");
        }
    }
}