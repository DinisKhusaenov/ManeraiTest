using Gameplay.Damage;
using Gameplay.Sound.Config;
using Gameplay.VFX.Config;

namespace Gameplay.StaticData
{
    public interface IStaticDataService
    {
        SoundConfig SoundConfig { get; }
        DamageConfig DamageConfig { get; }
        VFXConfig VFXConfig { get; }
    }
}