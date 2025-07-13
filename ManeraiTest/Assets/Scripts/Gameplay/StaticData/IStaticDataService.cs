using Gameplay.Sound.Config;

namespace Gameplay.StaticData
{
    public interface IStaticDataService
    {
        SoundConfig SoundConfig { get; }
    }
}