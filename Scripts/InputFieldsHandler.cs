using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Application.Core.Scripts
{
    public class InputFieldsHandler : MonoBehaviour
    {
        [SerializeField] private List<TMP_InputField> _inputFieldsSequence = new List<TMP_InputField>();

        public void OnTab(InputValue value)
        {
            if (!value.isPressed || _inputFieldsSequence.Count == 0) return;

            NavigateForward();
        }

        public void NavigateForward()
        {
            int nextIndex = CalculateNextIndex();
            SelectField(_inputFieldsSequence[nextIndex]);
        }

        public void SelectField(TMP_InputField field)
        {
            if (field == null || !field.gameObject.activeInHierarchy || !field.interactable) return;

            field.Select();
            field.ActivateInputField();
        }

        private int CalculateNextIndex()
        {
            int currentIndex = GetCurrentFocusedIndex();

            if (currentIndex == -1)
            {
                return 0;
            }

            return (currentIndex + 1) % _inputFieldsSequence.Count;
        }

        private int GetCurrentFocusedIndex()
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null || eventSystem.currentSelectedGameObject == null)
            {
                return -1;
            }

            GameObject selectedObject = eventSystem.currentSelectedGameObject;

            for (int i = 0; i < _inputFieldsSequence.Count; i++)
            {
                if (_inputFieldsSequence[i] != null && _inputFieldsSequence[i].gameObject == selectedObject)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}