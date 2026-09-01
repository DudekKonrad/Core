namespace Application.Core.Scripts.Audio
{
    public class MusicOptionMediator : VolumeSliderMediator
    {
        protected override string GetVolumePrefsKey() => "MusicVolume";
        protected override void FireVolumeSignal(float volume) => SignalBus.Fire(new CoreSignals.SetMusicVolumeSignal(volume));
    }
}
