using UnityEngine;
using UnityEngine.UI;
namespace Application.Core.UI
{
    [RequireComponent(typeof(Toggle))]
    public abstract class ToggleMediator : UIPointerHandlerView
    {
        protected Toggle Toggle;

        private void Start()
        {
            Toggle = GetComponent<Toggle>();
            Toggle.onValueChanged.AddListener(OnToggleClicked);
        }
        protected abstract void OnToggleClicked(bool value);
    }
}