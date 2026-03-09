using Application.Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Application.Core.Services
{
    public class SceneLoaderService : IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly LoadingScreenView _loadingScreenView;

        public SceneLoaderService(SignalBus signalBus, LoadingScreenView loadingScreenView)
        {
            _signalBus = signalBus;
            _loadingScreenView = loadingScreenView;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<CoreSignals.LoadSceneSignal>(OnLoadScene);
        }

        private async void OnLoadScene(CoreSignals.LoadSceneSignal signal)
        {
            if (signal.ShowLoadingScreen)
            {
                await _loadingScreenView.ShowAsync();
            }

            await SceneManager.LoadSceneAsync(signal.SceneName);

            if (signal.ShowLoadingScreen)
            {
                await _loadingScreenView.HideAsync();
            }
        }
    }
}