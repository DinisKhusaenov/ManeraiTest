using System;
using UnityEngine;

namespace Gameplay.Sound.Config
{
    [Serializable]
    public class SoundData
    {
        public SoundType SoundType;
        public AudioClip AudioClip;
        public bool isLoop;
    }
}