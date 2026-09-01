using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.Core.Scripts
{
    public class SoundOptionMediator : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [SerializeField] private Slider _slider;

        private const string SoundVolumeKey = "SoundVolume";

        private void Start()
        {
            float savedVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 0.5f);
            _slider.value = savedVolume;
            _signalBus.Fire(new CoreSignals.SetSoundVolumeSignal(savedVolume));
        }
        
        private void OnEnable() => _slider.onValueChanged.AddListener(HandleSlider);
        private void OnDisable() => _slider.onValueChanged.RemoveListener(HandleSlider);

        private void HandleSlider(float value)
        {
            PlayerPrefs.SetFloat(SoundVolumeKey, value);
            _signalBus.Fire(new CoreSignals.SetSoundVolumeSignal(value));
        }
    }
}