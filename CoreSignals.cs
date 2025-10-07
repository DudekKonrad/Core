using Application.Core.Scripts;
namespace Application.Core
{
    public class CoreSignals
    {
        
        public interface ICoreSignal : ISignal
        {
            
        }
        
        public class PlaySoundSignal : ICoreSignal
        {
            private AudioClipModel.Sounds _sounds;
            private string _id;
            public PlaySoundSignal(AudioClipModel.Sounds sounds, string id = "Sfx")
            {
                _sounds = sounds;
                _id = id;
            }

            public AudioClipModel.Sounds Sounds => _sounds;
            public string Id => _id;
        }
        public class StopSoundSignal : ICoreSignal
        {
            private string _id;
            public StopSoundSignal(string id)
            {
                _id = id;
            }
            public string Id => _id;
        }
    }
}