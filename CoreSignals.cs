using Application.Core.Scripts;
using AudioClipModel = Application.Core.Enums.AudioClipModel;

namespace Application.Core
{
    public class CoreSignals
    {
        
        public interface ICoreSignal : ISignal
        {
            
        }
        
        public class PlaySoundSignal : ICoreSignal
        {
            public AudioClipModel.Sounds Sound { get; }
            public string Id { get; }
            public int Combo { get; }

            public PlaySoundSignal(AudioClipModel.Sounds sound, string id = "Sfx", int combo = 0)
            {
                Sound = sound;
                Id = id;
                Combo = combo;
            }
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

        public class PlayMusicSignal : ICoreSignal
        {
            public string Id { get; }

            public PlayMusicSignal(string id)
            {
                Id = id;
            }
        }

        public class StopMusicSignal : ICoreSignal
        {
            public StopMusicSignal() { }
        }

        public class SetMusicVolumeSignal : ICoreSignal
        {
            public float Volume { get; }

            public SetMusicVolumeSignal(float volume)
            {
                Volume = volume;
            }
        }

        public class SetSoundVolumeSignal : ICoreSignal
        {
            public float Volume { get; }

            public SetSoundVolumeSignal(float volume)
            {
                Volume = volume;
            }
        }

        public class LoadSceneSignal : ICoreSignal
        {
            public string SceneName { get; }
            public bool ShowLoadingScreen { get; }

            public LoadSceneSignal(string sceneName, bool showLoadingScreen = true)
            {
                SceneName = sceneName;
                ShowLoadingScreen = showLoadingScreen;
            }
        }
    }
}