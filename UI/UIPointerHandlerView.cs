using Application.Core.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

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
        private Vector2 _originalShadowDistance;

        private Tween _hoverTween;
        private Tween _clickTween;
        private Tween _shadowTween;
        
        protected virtual void Awake()
        {
            _selectable = GetComponent<Selectable>();
            _shadow = GetComponent<Shadow>();
            _originalShadowDistance = _shadow.effectDistance;
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnButtonHover));
            
            _hoverTween?.Kill();
            _hoverTween = transform.DOScale(Vector3.one * _uiConfig.Scale, _uiConfig.Duration).SetEase(_uiConfig.Ease);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;
            
            _hoverTween?.Kill();
            _hoverTween = transform.DOScale(Vector3.one, _uiConfig.Duration).SetEase(_uiConfig.Ease);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnChoose));
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _clickTween?.Kill();
            _shadowTween?.Kill();

            _clickTween = transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.OutQuad);
            _shadowTween = DOTween.To(() => _shadow.effectDistance, x => _shadow.effectDistance = x, Vector2.zero, _uiConfig.ButtonDuration)
                .SetEase(Ease.OutQuad);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _clickTween?.Kill();
            _shadowTween?.Kill();

            float targetScale = eventData.pointerEnter == gameObject ? _uiConfig.Scale : 1f;
            _clickTween = transform.DOScale(Vector3.one * targetScale, _uiConfig.ButtonDuration).SetEase(Ease.OutBack);
            _shadowTween = DOTween.To(() => _shadow.effectDistance, x => _shadow.effectDistance = x, _originalShadowDistance, _uiConfig.ButtonDuration)
                .SetEase(Ease.OutBack);
        }
    }
}