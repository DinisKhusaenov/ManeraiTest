using System;
using System.Collections;
using Gameplay.Sound.Config;
using Infrastructure.Services.PoolService;
using UnityEngine;

namespace Gameplay.Sound
{
    public class SoundItem : MonoBehaviour, IPullObject
    {
        public event Action<SoundItem> OnEnded;
        
        [SerializeField] private AudioSource _audioSource;

        private Coroutine _finishCoroutine;
        
        public SoundData SoundData { get; private set; }

        public void Play(SoundData soundData)
        {
            SoundData = soundData;

            _audioSource.clip = SoundData.AudioClip;
            _audioSource.outputAudioMixerGroup = SoundData.MixerGroup;
            _audioSource.loop = SoundData.isLoop;

            _audioSource.Play();

            if (!SoundData.isLoop)
            {
                if (_finishCoroutine != null)
                    StopCoroutine(_finishCoroutine);
                
                _finishCoroutine = StartCoroutine(FinishPlay());
            }
        }

        public void Stop()
        {
            _audioSource.Stop();
            if (_finishCoroutine != null)
                StopCoroutine(_finishCoroutine);
            OnEnded?.Invoke(this);
        }

        private IEnumerator FinishPlay()
        {
            float remainingTime = _audioSource.clip.length - _audioSource.time;
            yield return new WaitForSeconds(remainingTime);

            while (_audioSource.isPlaying)
            {
                yield return null;
            }

            OnEnded?.Invoke(this);
        }

        public void Reset()
        {
            SoundData = null;
            _audioSource.clip = null;
            _audioSource.outputAudioMixerGroup = null;
            _audioSource.loop = false;
        }
    }
}