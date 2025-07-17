using Gameplay.Damage;
using Gameplay.Sound;
using Gameplay.Sound.Config;
using Gameplay.VFX;
using Infrastructure.Input;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class HeadHitHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _headLayer;
        [SerializeField] private float _maxDistance = 100f;

        private Camera _cam;
        private ISoundService _soundService;
        private IInputService _inputService;
        private IVFXService _vfxService;

        [Inject]
        private void Construct(ISoundService soundService, IInputService inputService, IVFXService vfxService)
        {
            _vfxService = vfxService;
            _inputService = inputService;
            _soundService = soundService;
            
            _cam = Camera.main;
            _inputService.OnHitClicked += Hit;
        }

        private void OnDestroy()
        {
            _inputService.OnHitClicked -= Hit;
        }

        private void Hit(Vector3 position)
        {
            Ray ray = _cam.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out var hit, _maxDistance, _headLayer))
            {
                var painter = hit.collider.GetComponent<DamagePainter>();
                if (painter != null)
                {
                    painter.PaintAt(hit.textureCoord);
                    _vfxService.Play(VFXType.Blood, hit.point, hit.transform.rotation);
                    _soundService.Play(SoundType.Hit);
                }
            }
        }
    }
}