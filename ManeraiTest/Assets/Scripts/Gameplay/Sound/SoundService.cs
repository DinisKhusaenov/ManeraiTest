using Gameplay.Sound.Config;
using Gameplay.StaticData;
using Infrastructure.Services.PoolService;
using Infrastructure.Services.PoolService.Factory;
using UnityEngine;
using Zenject;

namespace Gameplay.Sound
{
    public class SoundService : MonoBehaviour, ISoundService
    {
        private IPoolFactory _poolFactory;
        private SoundConfig _config;
        
        private ObjectPool<SoundItem> _pool;

        [Inject]
        private void Construct(IPoolFactory poolFactory, IStaticDataService staticDataService)
        {
            _poolFactory = poolFactory;
            _pool = new ObjectPool<SoundItem>(_poolFactory);
            _config = staticDataService.SoundConfig;
            _pool.Initialize(_config.PoolCapacity, PoolObjectType.Sound, transform);
        }

        public void Play(SoundType type)
        {
            var prefab = _pool.Get(Vector3.zero, transform);
            prefab.Play(_config.GetDataBy(type));
            prefab.OnEnded += ReturnToPool;
        }

        public void Stop(SoundType type)
        {
            foreach (SoundItem item in _pool.Entries)
            {
                if (item.SoundData.SoundType == type)
                    item.Stop();
            }
        }

        private void ReturnToPool(SoundItem item)
        {
            _pool.Return(item);
        }
    }
}