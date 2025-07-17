using System;
using System.Collections;
using Infrastructure.Services.PoolService;
using UnityEngine;
using UnityEngine.VFX;

namespace Gameplay.VFX
{
    public class VFXItem : MonoBehaviour, IPullObject
    {
        public event Action<VFXItem> OnEnded;
        
        [SerializeField] private VisualEffect _visualEffect;
        
        [field: SerializeField] public VFXType Type { get; private set; }
        
        private Coroutine _finishCoroutine;

        public void Play()
        {
            _visualEffect.Play();
            
            if (_finishCoroutine != null)
                StopCoroutine(_finishCoroutine);
                
            _finishCoroutine = StartCoroutine(FinishPlay());
        }
        
        public void Stop()
        {
            _visualEffect.Stop();
            if (_finishCoroutine != null)
                StopCoroutine(_finishCoroutine);
        }
        
        private IEnumerator FinishPlay()
        {
            while (_visualEffect.aliveParticleCount != 0)
            {
                yield return null;
            }

            OnEnded?.Invoke(this);
        }
        
        public void Reset()
        {
            Stop();
        }
    }
}