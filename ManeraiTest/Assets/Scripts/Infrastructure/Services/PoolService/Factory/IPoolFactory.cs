using Gameplay.VFX;
using UnityEngine;

namespace Infrastructure.Services.PoolService.Factory
{
    public interface IPoolFactory
    {
        TComponent CreateSound<TComponent>(Vector3 position, Transform parent = null) 
            where TComponent : MonoBehaviour;
        
        TComponent CreateVFX<TComponent>(Vector3 position, VFXType type, Transform parent = null) 
            where TComponent : MonoBehaviour;
    }
}