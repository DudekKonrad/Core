using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Core.Scripts
{
    public class ProcessingOverlayView : MonoBehaviour
    {
        public enum TransitionType
        {
            FadeOnly,
            SlideOnly,
            FadeAndSlide
        }

        public enum SlideDirection
        {
            Left,
            Right,
            Top,
            Bottom
        }

        [Header("Components")]
        [SerializeField] private Image _processingBackground;
        [SerializeField] private Image _processingGearIcon;

        [Header("Background Settings")]
        [SerializeField] private float _targetAlpha = 0.65f;
        [SerializeField] private float _startAlpha = 0f;

        [Header("Enter Settings")]
        [SerializeField] private TransitionType _enterType = TransitionType.FadeOnly;
        [SerializeField] private SlideDirection _enterSlideFrom = SlideDirection.Left;
        [SerializeField] private float _enterDuration = 0.3f;
        [SerializeField] private Ease _enterEase = Ease.OutQuad;

        [Header("Exit Settings")]
        [SerializeField] private TransitionType _exitType = TransitionType.FadeOnly;
        [SerializeField] private SlideDirection _exitSlideTo = SlideDirection.Right;
        [SerializeField] private float _exitDuration = 0.25f;
        [SerializeField] private Ease _exitEase = Ease.InQuad;

        [Header("Slide Offset Distance")]
        [SerializeField] private float _slideOffset = 1000f;

        [Header("Gear Icon Animation")]
        [SerializeField] private float _processingGearScaleDuration = 0.25f;
        [SerializeField] private float _processingGearRotationDuration = 0.45f;

        private RectTransform _bgRectTransform;
        private Vector2 _defaultBgPosition;

        private Tween _processingBackgroundTween;
        private Tween _processingGearScaleTween;
        private Tween _processingGearRotationTween;

        private void Awake()
        {
            if (_processingBackground != null)
            {
                _bgRectTransform = _processingBackground.GetComponent<RectTransform>();
                if (_bgRectTransform != null)
                {
                    _defaultBgPosition = _bgRectTransform.anchoredPosition;
                }
            }
        }

        public void ShowProcessingPanel()
        {
            KillProcessingTweens();

            _processingBackground.gameObject.SetActive(true);
            _processingGearIcon.gameObject.SetActive(true);
            _processingBackground.raycastTarget = true;
            _processingGearIcon.raycastTarget = false;

            // Reset Gear
            _processingGearIcon.transform.localScale = Vector3.zero;
            _processingGearIcon.transform.localRotation = Quaternion.identity;

            // Setup Background initial state for Enter
            SetupEnterBackgroundState();

            // Create Sequence for Background Enter Animation
            Sequence bgSequence = DOTween.Sequence();

            if (_enterType == TransitionType.FadeOnly || _enterType == TransitionType.FadeAndSlide)
            {
                bgSequence.Join(_processingBackground.DOFade(_targetAlpha, _enterDuration).SetEase(_enterEase));
            }

            if (_enterType == TransitionType.SlideOnly || _enterType == TransitionType.FadeAndSlide)
            {
                bgSequence.Join(_bgRectTransform.DOAnchorPos(_defaultBgPosition, _enterDuration).SetEase(_enterEase));
            }

            _processingBackgroundTween = bgSequence;

            // Gear Tweens
            _processingGearScaleTween = _processingGearIcon.transform
                .DOScale(Vector3.one, _processingGearScaleDuration)
                .SetEase(Ease.OutBack);

            _processingGearRotationTween = _processingGearIcon.transform
                .DORotate(new Vector3(0f, 0f, -360f), _processingGearRotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        public void HideProcessingPanel()
        {
            if (_processingBackground == null || _processingGearIcon == null)
            {
                return;
            }

            _processingBackground.raycastTarget = false;

            _processingBackgroundTween?.Kill();
            _processingGearScaleTween?.Kill();

            Sequence bgSequence = DOTween.Sequence();

            if (_exitType == TransitionType.FadeOnly || _exitType == TransitionType.FadeAndSlide)
            {
                bgSequence.Join(_processingBackground.DOFade(_startAlpha, _exitDuration).SetEase(_exitEase));
            }

            if (_exitType == TransitionType.SlideOnly || _exitType == TransitionType.FadeAndSlide)
            {
                Vector2 exitTargetPos = GetSlideOffsetVector(_exitSlideTo, _defaultBgPosition);
                bgSequence.Join(_bgRectTransform.DOAnchorPos(exitTargetPos, _exitDuration).SetEase(_exitEase));
            }

            bgSequence.OnComplete(() =>
            {
                _processingBackground.gameObject.SetActive(false);
                ResetBackgroundPosition();
            });

            _processingBackgroundTween = bgSequence;

            _processingGearScaleTween = _processingGearIcon.transform
                .DOScale(Vector3.zero, _processingGearScaleDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _processingGearRotationTween?.Kill();
                    _processingGearIcon.gameObject.SetActive(false);
                });
        }

        public void HideProcessingPanelInstant()
        {
            if (_processingBackground == null || _processingGearIcon == null)
            {
                return;
            }

            KillProcessingTweens();

            SetImageAlpha(_processingBackground, _startAlpha);
            ResetBackgroundPosition();

            _processingBackground.raycastTarget = false;
            _processingBackground.gameObject.SetActive(false);

            _processingGearIcon.transform.localScale = Vector3.zero;
            _processingGearIcon.transform.localRotation = Quaternion.identity;
            _processingGearIcon.raycastTarget = false;
            _processingGearIcon.gameObject.SetActive(false);
        }

        public void KillProcessingTweens()
        {
            _processingBackgroundTween?.Kill();
            _processingGearScaleTween?.Kill();
            _processingGearRotationTween?.Kill();
        }

        private void SetupEnterBackgroundState()
        {
            if (_enterType == TransitionType.FadeOnly || _enterType == TransitionType.FadeAndSlide)
            {
                SetImageAlpha(_processingBackground, _startAlpha);
            }
            else
            {
                SetImageAlpha(_processingBackground, _targetAlpha);
            }

            if (_enterType == TransitionType.SlideOnly || _enterType == TransitionType.FadeAndSlide)
            {
                _bgRectTransform.anchoredPosition = GetSlideOffsetVector(_enterSlideFrom, _defaultBgPosition);
            }
            else
            {
                ResetBackgroundPosition();
            }
        }

        private Vector2 GetSlideOffsetVector(SlideDirection direction, Vector2 basePosition)
        {
            return direction switch
            {
                SlideDirection.Left => basePosition + new Vector2(-_slideOffset, 0f),
                SlideDirection.Right => basePosition + new Vector2(_slideOffset, 0f),
                SlideDirection.Top => basePosition + new Vector2(0f, _slideOffset),
                SlideDirection.Bottom => basePosition + new Vector2(0f, -_slideOffset),
                _ => basePosition
            };
        }

        private void ResetBackgroundPosition()
        {
            if (_bgRectTransform != null)
            {
                _bgRectTransform.anchoredPosition = _defaultBgPosition;
            }
        }

        private void SetImageAlpha(Image image, float alpha)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}