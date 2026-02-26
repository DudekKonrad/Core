using UnityEngine;
using UnityEngine.UI;

namespace Application.Core.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonMediator : UIPointerHandlerView
    {
        protected Button Button;

        private void Awake()
        {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(OnButtonClicked);
        }
        protected abstract void OnButtonClicked();
    }
}