using Application.Core.Scripts;
using Application.GameCore;
namespace Application.Core
{
    public class CoreSignals
    {
        
        public interface ICoreSignal : ISignal
        {
            
        }
        
        public class PlayUISoundSignal : ICoreSignal
        {
            private AudioClipModel.UISounds _uiSounds;
            public PlayUISoundSignal(AudioClipModel.UISounds uiSounds)
            {
                _uiSounds = uiSounds;
            }

            public AudioClipModel.UISounds UISounds => _uiSounds;
        }
    }
}