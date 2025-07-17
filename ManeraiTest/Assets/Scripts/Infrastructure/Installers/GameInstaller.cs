using Gameplay.Sound;
using Gameplay.StaticData;
using Gameplay.VFX;
using Infrastructure.Input;
using Infrastructure.Services.LogService;
using Infrastructure.Services.PoolService.Factory;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SoundService _soundService;
        [SerializeField] private VFXService _vfxService;
        
        public override void InstallBindings()
        {
            BindFactory();
            BindGameplayServices();
            BindServices();
            BindInput();
        }

        private void BindInput()
        {
            Container.BindInterfacesTo<StandaloneInput>().AsSingle();
        }

        private void BindFactory()
        {
            Container.Bind<IPoolFactory>().To<PoolFactory>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<LogService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundService>().FromComponentInNewPrefab(_soundService).AsSingle();
            Container.BindInterfacesAndSelfTo<VFXService>().FromComponentInNewPrefab(_vfxService).AsSingle();
        }
        
        private void BindGameplayServices()
        {
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
        }
    }
}
