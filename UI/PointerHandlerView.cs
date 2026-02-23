using Application.Core.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
namespace Application.Core.UI
{
    [RequireComponent(typeof(Selectable))]
    public class PointerHandlerView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject] protected SignalBus _signalBus;
        [Inject] protected UIConfig  _uiConfig;
        
        private Selectable _selectable;
        private Tween _hoverTween;
        
        private void Start()
        {
            _selectable = GetComponent<Selectable>();
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnButtonHover));
            transform.DOScale(Vector3.one * _uiConfig.Scale, _uiConfig.Duration).SetEase(_uiConfig.Ease);
            _hoverTween?.Kill();
            
            var seq = DOTween.Sequence();
            seq.Append(transform.DOScale(Vector3.one * 1.1f, 0.2f));
            seq.Join(transform.DOShakeRotation(0.2f, 15f, 10, 90f));
            
            _hoverTween = seq;
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            transform.DOScale(Vector3.one, _uiConfig.Duration).SetEase(_uiConfig.Ease);
            transform.DORotate(Vector3.one, _uiConfig.Duration).SetEase(_uiConfig.Ease);
        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnChoose));
        }
    }
}