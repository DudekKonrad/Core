using Application.Core.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
namespace Application.Core.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonMediator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject] private SignalBus _signalBus;
        
        [SerializeField] protected float _duration = 0.25f;
        [SerializeField] protected float _scale = 1.1f;
        
        protected Button Button;

        private void Start()
        {
            Button = GetComponent<Button>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            _signalBus.Fire(new CoreSignals.PlayUISoundSignal(AudioClipModel.UISounds.OnHover));
            gameObject.transform.DOScale(Vector3.one * _scale, _duration);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.transform.DOScale(Vector3.one, _duration);
        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            _signalBus.Fire(new CoreSignals.PlayUISoundSignal(AudioClipModel.UISounds.OnChoose));
        }
    }
}