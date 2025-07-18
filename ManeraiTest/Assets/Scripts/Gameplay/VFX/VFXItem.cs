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
        [SerializeField, Range(0, 20)] private float _vfxLifeTime;
        
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
            yield return new WaitForSeconds(_vfxLifeTime);

            OnEnded?.Invoke(this);
        }
        
        public void Reset()
        {
            Stop();
        }
    }
}