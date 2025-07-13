using Gameplay.Sound;
using Gameplay.StaticData;
using Infrastructure.Services.LogService;
using Infrastructure.Services.PoolService.Factory;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SoundService _soundService;
        
        public override void InstallBindings()
        {
            BindFactory();
            BindServices();
            BindGameplayServices();
        }
        
        private void BindFactory()
        {
            Container.Bind<IPoolFactory>().To<PoolFactory>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<LogService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundService>().FromComponentInNewPrefab(_soundService).AsSingle();
        }
        
        private void BindGameplayServices()
        {
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
        }
    }
}
