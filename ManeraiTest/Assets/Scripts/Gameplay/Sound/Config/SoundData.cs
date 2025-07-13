using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Gameplay.Sound.Config
{
    [Serializable]
    public class SoundData
    {
        public SoundType SoundType;
        public AudioClip AudioClip;
        public AudioMixerGroup MixerGroup;
        public bool isLoop;
    }
}