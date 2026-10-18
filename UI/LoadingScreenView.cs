using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Core.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreenView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _transitionImage;
        [SerializeField] private float _fadeDuration = 0.5f;

        private static readonly int ProgressProperty = Shader.PropertyToID("_Progress");
        private Material _material;

        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            if (_transitionImage != null)
            {
                _material = _transitionImage.material;
            }

            SetProgress(0f);
            
            _canvasGroup.blocksRaycasts = false;
            DontDestroyOnLoad(gameObject);
        }

        public async UniTask ShowAsync()
        {
            _canvasGroup.blocksRaycasts = true;

            if (_material != null)
            {
                await DOTween.To(() => _material.GetFloat(ProgressProperty), 
                                 x => _material.SetFloat(ProgressProperty, x), 
                                 1f, _fadeDuration)
                             .SetEase(Ease.OutQuad)
                             .AsyncWaitForCompletion();
            }
            else
            {
                await _canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
            }
        }

        public async UniTask HideAsync()
        {
            if (_material != null)
            {
                await DOTween.To(() => _material.GetFloat(ProgressProperty), 
                                 x => _material.SetFloat(ProgressProperty, x), 
                                 0f, _fadeDuration)
                             .SetEase(Ease.InQuad)
                             .AsyncWaitForCompletion();
            }
            else
            {
                await _canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.InQuad).AsyncWaitForCompletion();
            }

            _canvasGroup.blocksRaycasts = false;
        }

        private void SetProgress(float value)
        {
            if (_material != null)
            {
                _material.SetFloat(ProgressProperty, value);
            }
            _canvasGroup.alpha = 1f;
        }
    }
}