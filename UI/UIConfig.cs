using DG.Tweening;
using UnityEngine;
namespace Application.Core.UI
{
    [CreateAssetMenu(menuName = "Core/Create UIConfig", fileName = "UIConfig", order = 0)]
    public class UIConfig : ScriptableObject
    {
        [SerializeField] private float _duration = 0.15f;
        [SerializeField] private float _scale = 1.1f;
        [SerializeField] private Ease _ease = Ease.Linear;
        
        public float Duration => _duration;
        public float Scale => _scale;
        public Ease Ease => _ease;
    }
}