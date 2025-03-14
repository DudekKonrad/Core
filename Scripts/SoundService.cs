using System.Linq;
using Application.Utils.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Application.Core.Scripts
{
    [UsedImplicitly]
    public class SoundService
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private readonly UISoundConfig _uiSoundConfig;
        
        private AudioSource _audioSourceSfx;

        [Inject]
        public void Construct()
        {
            _signalBus.Subscribe<CoreSignals.PlayUISoundSignal>(OnplayUISoundSignal);
            
            _audioSourceSfx = Object.Instantiate(new GameObject()).AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(_audioSourceSfx.gameObject);
        }
        private void OnplayUISoundSignal(CoreSignals.PlayUISoundSignal signal)
        {
            Play(signal.UISounds);
        }

        public void Play(AudioClipModel.UISounds uiSounds)
        { 
            _audioSourceSfx.PlayOneShot(_uiSoundConfig.AudioClipModels.First(_ => _._uiSounds == uiSounds).AudioClips.GetRandomElement());
        }

        public void Stop()
        {
            _audioSourceSfx.Stop();
        }
    }
}