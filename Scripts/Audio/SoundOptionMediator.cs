using UnityEngine;
using UnityEngine.UI;

namespace Application.Core.Scripts.Audio
{
    public class SoundOptionMediator : VolumeSliderMediator
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Sprite _soundOnSprite;
        [SerializeField] private Sprite _soundMutedSprite;

        protected override void Start()
        {
            base.Start();
            UpdateIcon(_slider.value);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _slider.onValueChanged.AddListener(UpdateIcon);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _slider.onValueChanged.RemoveListener(UpdateIcon);
        }

        protected override string GetVolumePrefsKey() => "SoundVolume";

        protected override void FireVolumeSignal(float volume) => SignalBus.Fire(new CoreSignals.SetSoundVolumeSignal(volume));

        private void UpdateIcon(float volume)
        {
            if (_iconImage == null) return;

            bool isMuted = Mathf.Approximately(volume, 0f);
            _iconImage.sprite = isMuted ? _soundMutedSprite : _soundOnSprite;
        }
    }
}