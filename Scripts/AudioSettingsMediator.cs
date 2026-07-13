using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.Core.Scripts
{
    [UsedImplicitly]
    public class AudioSettingsMediator : MonoBehaviour, IInitializable
    {
        [Inject] private readonly SignalBus _signalBus;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _soundVolumeSlider;

        public void Initialize()
        {
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);

            _musicVolumeSlider.value = 0.5f; 
            _soundVolumeSlider.value = 0.5f;

            _signalBus.Fire(new CoreSignals.SetMusicVolumeSignal(_musicVolumeSlider.value));
            _signalBus.Fire(new CoreSignals.SetSoundVolumeSignal(_soundVolumeSlider.value));
            _signalBus.Fire(new CoreSignals.PlayMusicSignal("MainMenuMusic"));
        }

        private void OnMusicVolumeChanged(float value)
        {
            _signalBus.Fire(new CoreSignals.SetMusicVolumeSignal(value));
        }

        private void OnSoundVolumeChanged(float value)
        {
            _signalBus.Fire(new CoreSignals.SetSoundVolumeSignal(value));
        }

        public void Dispose()
        {
            _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            _soundVolumeSlider.onValueChanged.RemoveListener(OnSoundVolumeChanged);
        }
    }
}