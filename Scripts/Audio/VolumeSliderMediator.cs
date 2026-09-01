using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace Application.Core.Scripts.Audio
{
    public abstract class VolumeSliderMediator : MonoBehaviour
    {
        [Inject] protected SignalBus SignalBus;
        [SerializeField] protected Slider _slider;

        private const float DefaultVolume = 0.5f;

        protected virtual void Start()
        {
            float savedVolume = PlayerPrefs.GetFloat(GetVolumePrefsKey(), DefaultVolume);
            _slider.value = savedVolume;
            FireVolumeSignal(savedVolume);
        }

        protected virtual void OnEnable() => _slider.onValueChanged.AddListener(HandleSliderChanged);
        protected virtual void OnDisable() => _slider.onValueChanged.RemoveListener(HandleSliderChanged);

        private void HandleSliderChanged(float value)
        {
            PlayerPrefs.SetFloat(GetVolumePrefsKey(), value);
            FireVolumeSignal(value);
        }

        protected abstract string GetVolumePrefsKey();

        protected abstract void FireVolumeSignal(float volume);
    }
}
