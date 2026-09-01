using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.Core.Scripts
{
    public class MusicOptionMediator : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [SerializeField] private Slider _slider;

        private const string MusicVolumeKey = "MusicVolume";

        private void Start()
        {
            float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
            _slider.value = savedVolume;
            _signalBus.Fire(new CoreSignals.SetMusicVolumeSignal(savedVolume));
        }
        
        private void OnEnable() => _slider.onValueChanged.AddListener(HandleSlider);
        private void OnDisable() => _slider.onValueChanged.RemoveListener(HandleSlider);

        private void HandleSlider(float value)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
            _signalBus.Fire(new CoreSignals.SetMusicVolumeSignal(value));
        }
    }
}