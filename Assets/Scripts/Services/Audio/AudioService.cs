using System.Collections.Generic;
using System.Linq;
using Services.Audio.Configs;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField] private AudioConfig[] audioConfigs;
        private List<AudioSource> sources;
        private void Awake()
        {
            sources = transform.GetComponents<AudioSource>().ToList();
        }

        public void Play(AudioType audioType)
        {
            AudioConfig soundConfig = audioConfigs.FirstOrDefault(audio => audio.AudioType == audioType);
            if (soundConfig == null)
            {
                Debug.LogError($"{audioType.ToString() } is null please check Play");
                return;
            }
            AudioSource source;
            if (IsPlaying(soundConfig, out source))
            {
                if(source.loop) source.Stop();
            }
            AudioSource audioSource = GetAudioSource();
            audioSource.clip = soundConfig.Clip;
            audioSource.outputAudioMixerGroup = soundConfig.AudioMixerGroup;
            audioSource.loop = soundConfig.Loop;
            audioSource.volume = soundConfig.Volume;
            audioSource.Play();
        }
        
        private bool IsPlaying(AudioConfig config, out AudioSource audioSource)
        {
            audioSource = null;

            if (sources == null || sources.Count == 0) return false;

            audioSource = sources.FirstOrDefault(s => s.clip == config.Clip);

            if (audioSource == null) return false;

            return audioSource.isPlaying;
        }
        private AudioSource GetAudioSource()
        {
            AudioSource audioSource = sources.FirstOrDefault(sources => !sources.isPlaying);
            if (audioSource == null)
            {
                audioSource = transform.gameObject.AddComponent<AudioSource>();
                sources.Add(audioSource);
            }
            return audioSource;
        }
    }
}