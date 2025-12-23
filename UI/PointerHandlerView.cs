using System;
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
        
        [SerializeField] protected float _duration = 0.15f;
        [SerializeField] protected float _scale = 1.1f;
        
        private Selectable _selectable;
        private void Start()
        {
            _selectable = GetComponent<Selectable>();
        }
        
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnButtonHover));
            transform.DOScale(Vector3.one * _scale, _duration);
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            transform.DOScale(Vector3.one, _duration);
        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnChoose));
        }
    }
}