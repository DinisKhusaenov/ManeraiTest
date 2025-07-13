using System;
using Gameplay.StaticData;
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
        
        public TComponent Create<TComponent>(
            PoolObjectType type, 
            Vector3 position, 
            Transform parent = null) where TComponent : MonoBehaviour
        {
            GameObject prefab = GetPrefabBy(type);
            
            return _instantiator.InstantiatePrefab(
                prefab, 
                position, 
                Quaternion.identity, 
                parent).GetComponent<TComponent>();
        }

        private GameObject GetPrefabBy(PoolObjectType type)
        {
            switch (type)
            {
                case PoolObjectType.Sound:
                    return _staticDataService.SoundConfig.Prefab.gameObject;
            }

            throw new Exception($"PoolObject by type {type} does not exist");
        }
    }
}