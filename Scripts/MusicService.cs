using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using DG.Tweening;

namespace Application.Core.Scripts
{
    [UsedImplicitly]
    public class MusicService : IInitializable, System.IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly MusicConfig _musicConfig;

        private AudioSource _musicAudioSource;
        private Dictionary<string, AudioClip> _musicClips;
        private float _defaultVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

        public MusicService(SignalBus signalBus, MusicConfig musicConfig)
        {
            _signalBus = signalBus;
            _musicConfig = musicConfig;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<CoreSignals.PlayMusicSignal>(OnPlayMusicSignal);
            _signalBus.Subscribe<CoreSignals.StopMusicSignal>(OnStopMusicSignal);
            _signalBus.Subscribe<CoreSignals.SetMusicVolumeSignal>(OnSetMusicVolumeSignal);
            _musicAudioSource = Object.Instantiate(new GameObject("MusicAudioSource")).AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(_musicAudioSource.gameObject);
            _musicAudioSource.loop = true; 
            _musicAudioSource.volume = 0f;

            _musicClips = _musicConfig.MusicClipModels.ToDictionary(model => model.Id, model => model.AudioClip);
            Play("MainMenuMusic");
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<CoreSignals.PlayMusicSignal>(OnPlayMusicSignal);
            _signalBus.TryUnsubscribe<CoreSignals.StopMusicSignal>(OnStopMusicSignal);
            _signalBus.TryUnsubscribe<CoreSignals.SetMusicVolumeSignal>(OnSetMusicVolumeSignal);

            if (_musicAudioSource && _musicAudioSource.gameObject)
            {
                _musicAudioSource.DOKill();
                Object.Destroy(_musicAudioSource.gameObject);
            }
        }

        private void OnPlayMusicSignal(CoreSignals.PlayMusicSignal signal)
        {
            Play(signal.Id);
        }

        private void OnStopMusicSignal()
        {
            Stop();
        }

        private void OnSetMusicVolumeSignal(CoreSignals.SetMusicVolumeSignal signal)
        {
            SetVolume(signal.Volume);
        }

        private void Play(string id)
        {
            if (_musicClips.TryGetValue(id, out var clip))
            {
                var musicClipModel = _musicConfig.MusicClipModels.FirstOrDefault(model => model.Id == id);

                if (musicClipModel != null)
                {
                    if (_musicAudioSource.isPlaying)
                    {
                        _musicAudioSource.DOKill();
                        _musicAudioSource.DOFade(0f, 0.5f).OnComplete(() =>
                        {
                            _musicAudioSource.clip = clip;
                            _musicAudioSource.loop = musicClipModel.Loop;
                            _musicAudioSource.Play();
                            _musicAudioSource.DOFade(_defaultVolume, 1f);
                        });
                    }
                    else
                    {
                        _musicAudioSource.clip = clip;
                        _musicAudioSource.loop = musicClipModel.Loop;
                        _musicAudioSource.Play();
                        _musicAudioSource.DOFade(_defaultVolume, 1f);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Music clip with ID '{id}' not found.");
            }
        }

        public void Stop()
        {
            _musicAudioSource.DOKill();
            _musicAudioSource.DOFade(0f, 1f).OnComplete(() => _musicAudioSource.Stop()); // Fade out and then stop
        }

        public void SetVolume(float volume)
        {
            _defaultVolume = volume;
            _musicAudioSource.volume = volume;
        }
    }
}