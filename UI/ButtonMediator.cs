using Application.Core.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Application.Core.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonMediator : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject] private SignalBus _signalBus;

        [SerializeField] protected float _duration = 0.15f;
        [SerializeField] protected float _scale = 1.1f;

        protected Button Button;

        private void Awake()
        {
            Button = GetComponent<Button>();
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!Button.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnButtonHover));
            transform.DOScale(Vector3.one * _scale, _duration);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (!Button.interactable) return;

            transform.DOScale(Vector3.one, _duration);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!Button.interactable) return;

            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnChoose));
        }
    }
}