using Application.Zoo.ProjectContext.Enums;
using Application.Zoo.ProjectContext.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Application.Core.UI
{
    public class DifficultyToggleMediator : UIPointerHandlerView
    {
        [SerializeField] private Difficulty _difficulty;
        [Inject] private SessionModel _sessionModel;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!_selectable.interactable) return;

            base.OnPointerClick(eventData);
            _sessionModel.SelectedDifficulty.Value = _difficulty;
        }
    }
}