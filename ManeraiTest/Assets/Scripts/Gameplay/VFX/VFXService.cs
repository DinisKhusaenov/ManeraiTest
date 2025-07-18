using Gameplay.Sound.Config;
using Gameplay.StaticData;
using Gameplay.VFX.Config;
using Infrastructure.Services.PoolService;
using Infrastructure.Services.PoolService.Factory;
using UnityEngine;
using Zenject;

namespace Gameplay.VFX
{
    public class VFXService : MonoBehaviour, IVFXService
    {
        private IPoolFactory _poolFactory;
        private VFXConfig _config;
        
        private ObjectPool<VFXItem> _pool;

        [Inject]
        private void Construct(IPoolFactory poolFactory, IStaticDataService staticDataService)
        {
            _poolFactory = poolFactory;
            _pool = new ObjectPool<VFXItem>(_poolFactory);
            _config = staticDataService.VFXConfig;
            
            //для тестового
            _pool.Initialize(_config.PoolCapacity, PoolObjectType.VFX, transform, VFXType.Blood);
        }

        public void Play(VFXType type, Vector3 position, Quaternion rotation)
        {
            VFXItem prefab = _pool.Get(Vector3.zero, transform);
            prefab.transform.position = position;
            prefab.transform.rotation = rotation;
            prefab.OnEnded += ReturnToPool;
            prefab.Play();
        }

        public void Stop(VFXType type)
        {
            foreach (VFXItem item in _pool.Entries)
            {
                if (item.Type == type)
                {
                    item.Stop();
                    item.OnEnded -= ReturnToPool;
                }
            }
        }

        private void ReturnToPool(VFXItem item)
        {
            _pool.Return(item);
        }
    }
}