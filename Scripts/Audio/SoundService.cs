using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Application.Core.Scripts.Audio
{
    [UsedImplicitly]
    public class SoundService : IInitializable, System.IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly SoundConfig _soundConfig;

        private AudioSource _audioSourceSfx;
        private float _volume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        private readonly Dictionary<string, AudioSource> _audioIdToSources = new Dictionary<string, AudioSource>();
        private readonly HashSet<AudioSource> _audioSources = new HashSet<AudioSource>();

        public SoundService(SignalBus signalBus, SoundConfig soundConfig)
        {
            _signalBus = signalBus;
            _soundConfig = soundConfig;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<CoreSignals.PlaySoundSignal>(OnPlaySoundSignal);
            _signalBus.Subscribe<CoreSignals.SetSoundVolumeSignal>(OnSetSoundVolumeSignal);
            _audioSourceSfx = Object.Instantiate(new GameObject("SoundAudioSource")).AddComponent<AudioSource>();
            _audioSourceSfx.volume = _volume;
            Object.DontDestroyOnLoad(_audioSourceSfx.gameObject);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<CoreSignals.PlaySoundSignal>(OnPlaySoundSignal);
            _signalBus.TryUnsubscribe<CoreSignals.SetSoundVolumeSignal>(OnSetSoundVolumeSignal);
            if (_audioSourceSfx && _audioSourceSfx.gameObject)
            {
                Object.Destroy(_audioSourceSfx.gameObject);
            }
        }

        private void OnPlaySoundSignal(CoreSignals.PlaySoundSignal signal)
        {
            Play(_soundConfig.Sounds[signal.Sound], null, signal.Id, signal.Combo);
        }

        private void OnSetSoundVolumeSignal(CoreSignals.SetSoundVolumeSignal signal)
        {
            SetVolume(signal.Volume);
        }

        public void Play(AudioClip sfxAudioClip, AudioSource targetAudioSource = null, string id = "", int combo = 0)
        {
            var audioSource = _audioSourceSfx;

            if (targetAudioSource == null)
            {
                if (!string.IsNullOrEmpty(id) && _audioIdToSources.TryGetValue(id, out audioSource) == false)
                {
                    audioSource = _audioSourceSfx.gameObject.AddComponent<AudioSource>();
                    audioSource.loop = false;
                    audioSource.volume = _volume;
                    _audioIdToSources.Add(id, audioSource);
                }
            }
            else
            {
                audioSource = targetAudioSource;
            }

            audioSource.volume = _volume;
            audioSource.pitch = 1 + combo * 0.1f;
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

        public void SetVolume(float volume)
        {
            _volume = volume;
            _audioSourceSfx.volume = volume;
            foreach (var source in _audioSources)
            {
                source.volume = volume;
            }
        }
    }
}