using Gameplay.Damage;
using Gameplay.Sound.Config;
using Gameplay.VFX.Config;
using UnityEngine;

namespace Gameplay.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string SoundConfigPath = "Configs/Sounds/SoundConfig";
        private const string DamageConfigPath = "Configs/Damage/DamageConfig";
        private const string VFXConfigPath = "Configs/VFX/VFXConfig";
        
        public SoundConfig SoundConfig { get; private set; }
        public DamageConfig DamageConfig { get; private set; }
        public VFXConfig VFXConfig { get; private set; }

        public StaticDataService()
        {
            LoadSoundConfig();
            LoadDamageConfig();
            LoadVFXConfig();
        }

        private void LoadDamageConfig()
        {
            DamageConfig = Resources.Load<DamageConfig>(DamageConfigPath);
        }

        private void LoadSoundConfig()
        {
            SoundConfig = Resources.Load<SoundConfig>(SoundConfigPath);
        }
        
        private void LoadVFXConfig()
        {
            VFXConfig = Resources.Load<VFXConfig>(VFXConfigPath);
        }
    }
}