using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using DG.Tweening; // Added for DOTween functionality

namespace Application.Core.Scripts
{
    [UsedImplicitly]
    public class MusicService : IInitializable, System.IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly MusicConfig _musicConfig;

        private AudioSource _musicAudioSource;
        private Dictionary<string, AudioClip> _musicClips;
        private float _defaultVolume = 0.5f; // Added default volume

        public MusicService(SignalBus signalBus, MusicConfig musicConfig)
        {
            _signalBus = signalBus;
            _musicConfig = musicConfig;
        }

        public void Initialize()
        {
            Debug.Log($"Initialize MusicService");
            _signalBus.Subscribe<CoreSignals.PlayMusicSignal>(OnPlayMusicSignal);
            _signalBus.Subscribe<CoreSignals.StopMusicSignal>(OnStopMusicSignal);
            _signalBus.Subscribe<CoreSignals.SetMusicVolumeSignal>(OnSetMusicVolumeSignal);
            _musicAudioSource = Object.Instantiate(new GameObject("MusicAudioSource")).AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(_musicAudioSource.gameObject);
            _musicAudioSource.loop = true; 
            _musicAudioSource.volume = 0f; // Start with 0 volume for fade-in

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
                _musicAudioSource.DOKill(); // Kill any active tweens
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
            Debug.Log($"Play music: {id}");
            if (_musicClips.TryGetValue(id, out var clip))
            {
                var musicClipModel = _musicConfig.MusicClipModels.FirstOrDefault(model => model.Id == id);

                if (musicClipModel != null)
                {
                    if (_musicAudioSource.isPlaying)
                    {
                        _musicAudioSource.DOKill(); // Kill current fade out/in if any
                        _musicAudioSource.DOFade(0f, 0.5f).OnComplete(() => // Fade out current music
                        {
                            _musicAudioSource.clip = clip;
                            _musicAudioSource.loop = musicClipModel.Loop;
                            _musicAudioSource.Play();
                            _musicAudioSource.DOFade(_defaultVolume, 1f); // Fade in new music
                        });
                    }
                    else
                    {
                        _musicAudioSource.clip = clip;
                        _musicAudioSource.loop = musicClipModel.Loop;
                        _musicAudioSource.Play();
                        _musicAudioSource.DOFade(_defaultVolume, 1f); // Fade in new music
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
            _musicAudioSource.DOKill(); // Kill any active tweens
            _musicAudioSource.DOFade(0f, 1f).OnComplete(() => _musicAudioSource.Stop()); // Fade out and then stop
        }

        public void SetVolume(float volume)
        {
            _defaultVolume = volume; // Update default volume
            _musicAudioSource.DOFade(volume, 0.5f); // Smoothly change volume
        }
    }
}