using Application.Core.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using AudioClipModel = Application.Core.Enums.AudioClipModel;

namespace Application.Core.UI
{
    [RequireComponent(typeof(Selectable))]
    [RequireComponent(typeof(Shadow))]
    public class UIPointerHandlerView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Inject] protected SignalBus _signalBus;
        [Inject] protected UIConfig  _uiConfig;

        protected Selectable _selectable;
        protected Shadow _shadow;
        [SerializeField]
        [Tooltip("Visual element animated on pointer interactions. Supports Image, TMP_Text, and other Graphic components.")]
        private Graphic _visual;
        [SerializeField] private bool _animateColor;
        [SerializeField] private Color _hoverColor = Color.white;

        private Vector3 _originalScale;
        private Color _originalColor;
        private Vector2 _originalShadowDistance;

        private Sequence _animationSequence;
        
        protected virtual void Awake()
        {
            _selectable = GetComponent<Selectable>();
            _shadow = GetComponent<Shadow>();

            if (_visual == null)
            {
                Debug.LogError(
                    $"{nameof(UIPointerHandlerView)} requires a visual Graphic reference.",
                    this);
                return;
            }

            _originalScale = _visual.transform.localScale;
            _originalColor = _visual.color;
            _originalShadowDistance = _shadow.effectDistance;
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnButtonHover));
            PlayVisualAnimation(_uiConfig.HoverScale, _uiConfig.HoverEase, _uiConfig.HoverDuration,
                _uiConfig.HoverShadowDistance);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            PlayVisualAnimation(1f, _uiConfig.ExitEase, _uiConfig.HoverDuration, _originalShadowDistance);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnChoose));
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            PlayVisualAnimation(_uiConfig.PressedScale, Ease.OutQuad, _uiConfig.PressedDuration, Vector2.zero);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            bool isPointerOver = eventData.pointerEnter == gameObject;
            PlayVisualAnimation(
                isPointerOver ? _uiConfig.HoverScale : 1f,
                isPointerOver ? _uiConfig.HoverEase : _uiConfig.ExitEase,
                _uiConfig.HoverDuration,
                isPointerOver ? _uiConfig.HoverShadowDistance : _originalShadowDistance);
        }

        private void PlayVisualAnimation(float scale, Ease ease, float duration, Vector2 shadowDistance)
        {
            if (_visual == null) return;

            _animationSequence?.Kill();
            _animationSequence = DOTween.Sequence()
                .Join(_visual.transform.DOScale(_originalScale * scale, duration).SetEase(ease))
                .Join(DOTween.To(
                    () => _shadow.effectDistance,
                    value => _shadow.effectDistance = value,
                    shadowDistance,
                    _uiConfig.ShadowDuration).SetEase(Ease.OutQuad));

            if (_animateColor)
            {
                Color targetColor = scale > 1f ? _hoverColor : _originalColor;
                _animationSequence.Join(_visual.DOColor(targetColor, duration).SetEase(Ease.OutQuad));
            }
        }

        protected virtual void OnDisable()
        {
            _animationSequence?.Kill();

            if (_visual == null) return;

            _visual.transform.localScale = _originalScale;
            _shadow.effectDistance = _originalShadowDistance;

            if (_animateColor)
            {
                _visual.color = _originalColor;
            }
        }
    }
}