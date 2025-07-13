using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Sound.Config
{
    [CreateAssetMenu(menuName = "Configs/SoundConfig", fileName = "SoundConfig")]
    public class SoundConfig : ScriptableObject
    {
        [SerializeField] private List<SoundData> _soundsData;
        [field: SerializeField] public SoundItem Prefab;
        [field: SerializeField] public int PoolCapacity;

        public SoundData GetDataBy(SoundType type)
        {
            foreach (SoundData soundData in _soundsData)
            {
                if (soundData.SoundType == type)
                    return soundData;
            }

            throw new Exception($"Sound data by type {type} does not exist");
        }
    }
}