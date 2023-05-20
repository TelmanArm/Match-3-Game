using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio.Configs
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/Audio")]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] private AudioType audioType;
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioMixerGroup mixerGroup;
        [SerializeField] private bool loop;
        [Range(0.1f, 1f)]
        [SerializeField] private float volume;
        public AudioType AudioType => audioType;
        public AudioClip Clip => clip;
        public AudioMixerGroup AudioMixerGroup => mixerGroup;
        public bool Loop => loop;
        public float Volume => volume;
    }
}