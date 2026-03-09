using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Application.Core.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreenView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.5f;

        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            DontDestroyOnLoad(gameObject);
        }

        public async UniTask ShowAsync()
        {
            _canvasGroup.blocksRaycasts = true;
            await _canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        }

        public async UniTask HideAsync()
        {
            await _canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.InQuad).AsyncWaitForCompletion();
            _canvasGroup.blocksRaycasts = false;
        }
    }
}