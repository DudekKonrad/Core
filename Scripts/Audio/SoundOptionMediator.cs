namespace Application.Core.Scripts.Audio
{
    public class SoundOptionMediator : VolumeSliderMediator
    {
        protected override string GetVolumePrefsKey() => "SoundVolume";
        protected override void FireVolumeSignal(float volume) => SignalBus.Fire(new CoreSignals.SetSoundVolumeSignal(volume));
    }
}
