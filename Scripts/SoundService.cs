using System.Collections.Generic;
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
        [Inject] private SoundConfig _soundConfig;
        
        private AudioSource _audioSourceSfx;
        private readonly Dictionary<string, AudioSource> _audioIdToSources = new Dictionary<string, AudioSource>();
        private readonly HashSet<AudioSource> _audioSources = new HashSet<AudioSource>();

        [Inject]
        public void Construct()
        {
            _signalBus.Subscribe<CoreSignals.PlaySoundSignal>(OnPlaySoundSignal);
            _audioSourceSfx = Object.Instantiate(new GameObject()).AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(_audioSourceSfx.gameObject);
        }
        private void OnPlaySoundSignal(CoreSignals.PlaySoundSignal signal)
        {
            Play(_soundConfig.AudioClipModels.First(_ => _._sounds == signal.Sounds).AudioClips.GetRandomElement());
        }

        public void Dispose()
        {
            if (_audioSourceSfx && _audioSourceSfx.gameObject)
                Object.Destroy(_audioSourceSfx.gameObject);
        }

        public void Play(AudioClip sfxAudioClip, AudioSource targetAudioSource = null, string id = "")
        {
            var audioSource = _audioSourceSfx;

            if (targetAudioSource == null)
            {
                if (!string.IsNullOrEmpty(id) && _audioIdToSources.TryGetValue(id, out audioSource) == false)
                {
                    audioSource = _audioSourceSfx.gameObject.AddComponent<AudioSource>();
                    audioSource.loop = false;
                    _audioIdToSources.Add(id, audioSource);
                }
            }
            else
            {
                audioSource = targetAudioSource;
            }

            audioSource.PlayOneShot(sfxAudioClip);
            _audioSources.Add(audioSource);
        }

        public void Stop(string id = "")
        {
            if (id != null && _audioIdToSources.TryGetValue(id, out var audioSource))
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }
}