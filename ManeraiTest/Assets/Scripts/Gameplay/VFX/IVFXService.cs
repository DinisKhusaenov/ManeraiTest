using UnityEngine;

namespace Gameplay.VFX
{
    public interface IVFXService
    {
        void Play(VFXType type, Vector3 position, Quaternion rotation);
        void Stop(VFXType type);
    }
}