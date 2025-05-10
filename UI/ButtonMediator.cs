using Application.Core.Scripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
namespace Application.Core.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonMediator : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        [Inject] private SignalBus _signalBus;

        [SerializeField] protected float _duration = 0.15f;
        [SerializeField] protected float _scale = 1.1f;

        protected Button Button;
        
        private void Start()
        {
            Button = GetComponent<Button>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnButtonHover));
            gameObject.transform.DOScale(Vector3.one * _scale, _duration);
        }
        public void OnDeselect(BaseEventData eventData)
        {
            gameObject.transform.DOScale(Vector3.one, _duration);
        }
        public void OnSubmit(BaseEventData eventData)
        {
            _signalBus.Fire(new CoreSignals.PlaySoundSignal(AudioClipModel.Sounds.OnChoose));
        }
    }
}