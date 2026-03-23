using Application.Core.Scripts;
using Application.Zoo.ProjectContext;
using Application.Zoo.ProjectContext.Enums;

namespace Application.Core
{
    public class CoreSignals
    {
        
        public interface ICoreSignal : ISignal
        {
            
        }
        
        public class PlaySoundSignal : ICoreSignal
        {
            public AudioClipModel.Sounds Sounds { get; }
            public string Id { get; }
            public int Combo { get; }

            public PlaySoundSignal(AudioClipModel.Sounds sounds, string id = "Sfx", int combo = 0)
            {
                Sounds = sounds;
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