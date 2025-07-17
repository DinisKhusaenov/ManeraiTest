using Gameplay.StaticData;
using Gameplay.VFX;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.PoolService.Factory
{
    public class PoolFactory : IPoolFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticDataService;

        public PoolFactory(IInstantiator instantiator, IStaticDataService staticDataService)
        {
            _instantiator = instantiator;
            _staticDataService = staticDataService;
        }
        
        public TComponent CreateSound<TComponent>( 
            Vector3 position, 
            Transform parent = null) where TComponent : MonoBehaviour
        {
            GameObject prefab = _staticDataService.SoundConfig.Prefab.gameObject;
            
            return _instantiator.InstantiatePrefab(
                prefab, 
                position, 
                Quaternion.identity, 
                parent).GetComponent<TComponent>();
        }

        public TComponent CreateVFX<TComponent>(Vector3 position, VFXType type, Transform parent = null) where TComponent : MonoBehaviour
        {
            GameObject prefab = _staticDataService.VFXConfig.GetPrefabByType(type).gameObject;
            
            return _instantiator.InstantiatePrefab(
                prefab, 
                position, 
                Quaternion.identity, 
                parent).GetComponent<TComponent>();
        }
    }
}