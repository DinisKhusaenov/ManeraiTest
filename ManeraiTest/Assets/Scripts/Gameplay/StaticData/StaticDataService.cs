using Gameplay.Sound.Config;
using UnityEngine;

namespace Gameplay.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string SoundConfigPath = "Configs/Sound/SoundConfig";
        
        public SoundConfig SoundConfig { get; private set; }

        public StaticDataService()
        {
            LoadSoundConfig();
        }

        private void LoadSoundConfig()
        {
            SoundConfig = Resources.Load<SoundConfig>(SoundConfigPath);
        }
    }
}