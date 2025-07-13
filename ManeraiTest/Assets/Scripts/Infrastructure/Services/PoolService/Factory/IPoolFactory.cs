using UnityEngine;

namespace Infrastructure.Services.PoolService.Factory
{
    public interface IPoolFactory
    {
        TComponent Create<TComponent>(PoolObjectType type, Vector3 position, Transform parent = null) 
            where TComponent : MonoBehaviour;
    }
}