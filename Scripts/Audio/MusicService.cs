using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace Application.Core.Scripts.Audio
{
    [UsedImplicitly]
    public class MusicService : IInitializable, System.IDisposable
    {
        private const string MainMenuMusicId = "MaiMenuMusic";
        private const string GameplayMusicId = "GameplayScene";

        private readonly SignalBus _signalBus;
        private readonly MusicConfig _musicConfig;

        private AudioSource _musicAudioSource;
        private Dictionary<string, AudioClip> _musicClips;
        private float _defaultVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        private bool _isFadingOut;
        private bool _hasPendingPlayback;
        private AudioClip _pendingClip;
        private bool _pendingLoop;

        public MusicService(SignalBus signalBus, MusicConfig musicConfig)
        {
            _signalBus = signalBus;
            _musicConfig = musicConfig;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<CoreSignals.PlayMusicSignal>(OnPlayMusicSignal);
            _signalBus.Subscribe<CoreSignals.StopMusicSignal>(OnStopMusicSignal);
            _signalBus.Subscribe<CoreSignals.LoadSceneSignal>(OnLoadSceneSignal);
            _signalBus.Subscribe<CoreSignals.SetMusicVolumeSignal>(OnSetMusicVolumeSignal);
            _musicAudioSource = Object.Instantiate(new GameObject("MusicAudioSource")).AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(_musicAudioSource.gameObject);
            _musicAudioSource.loop = true; 
            _musicAudioSource.volume = 0f;
            _musicClips = _musicConfig.MusicClipModels.ToDictionary(model => model.Id, model => model.AudioClip);

            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayMusicForScene(SceneManager.GetActiveScene());
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<CoreSignals.PlayMusicSignal>(OnPlayMusicSignal);
            _signalBus.TryUnsubscribe<CoreSignals.StopMusicSignal>(OnStopMusicSignal);
            _signalBus.TryUnsubscribe<CoreSignals.LoadSceneSignal>(OnLoadSceneSignal);
            _signalBus.TryUnsubscribe<CoreSignals.SetMusicVolumeSignal>(OnSetMusicVolumeSignal);
            SceneManager.sceneLoaded -= OnSceneLoaded;

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

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayMusicForScene(scene);
        }

        private void PlayMusicForScene(Scene scene)
        {
            switch (scene.name)
            {
                case "MainMenu":
                    Play(MainMenuMusicId);
                    break;
                case "Gameplay":
                    Play(GameplayMusicId);
                    break;
            }
        }

        private void OnStopMusicSignal()
        {
            Stop();
        }

        private void OnLoadSceneSignal(CoreSignals.LoadSceneSignal signal)
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
                    if (_isFadingOut)
                    {
                        _pendingClip = clip;
                        _pendingLoop = musicClipModel.Loop;
                        _hasPendingPlayback = true;
                        return;
                    }

                    if (_musicAudioSource.isPlaying)
                    {
                        _musicAudioSource.DOKill();
                        _musicAudioSource.DOFade(0f, _musicConfig.FadeDuration).OnComplete(() =>
                        {
                            StartPlayback(clip, musicClipModel.Loop);
                        });
                    }
                    else
                    {
                        StartPlayback(clip, musicClipModel.Loop);
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
            _hasPendingPlayback = false;
            _isFadingOut = true;
            _musicAudioSource.DOFade(0f, _musicConfig.FadeDuration).OnComplete(() =>
            {
                _musicAudioSource.Stop();
                _isFadingOut = false;

                if (_hasPendingPlayback)
                {
                    var clip = _pendingClip;
                    var loop = _pendingLoop;
                    _hasPendingPlayback = false;
                    StartPlayback(clip, loop);
                }
            });
        }

        private void StartPlayback(AudioClip clip, bool loop)
        {
            _musicAudioSource.clip = clip;
            _musicAudioSource.loop = loop;
            _musicAudioSource.Play();
            _musicAudioSource.DOFade(_defaultVolume, _musicConfig.FadeDuration);
        }

        public void SetVolume(float volume)
        {
            _defaultVolume = volume;
            _musicAudioSource.volume = volume;
        }
    }
}