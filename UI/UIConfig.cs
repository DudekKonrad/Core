using DG.Tweening;
using UnityEngine;
namespace Application.Core.UI
{
    [CreateAssetMenu(menuName = "Core/Create UIConfig", fileName = "UIConfig", order = 0)]
    public class UIConfig : ScriptableObject
    {
        [Header("Scale properties")]
        [SerializeField] private float _uiElementScaleDuration = 0.15f;
        [SerializeField] private float _uiElementScaleValue = 1.1f;
        [SerializeField] private Ease _uiElementScaleEase = Ease.Linear;
        [Header("Shake properties")]
        [SerializeField] private float _uiElementShakeDuration = 1f;
        [SerializeField] private float _uiElementShakeStrength = 1f;
        [SerializeField] private int _uiElementShakeVibrato = 10;
        [SerializeField] private int _uiElementShakeRandomness = 5;
        
        public float Duration => _uiElementScaleDuration;
        public float Scale => _uiElementScaleValue;
        public Ease Ease => _uiElementScaleEase;
        public float ShakeStrength => _uiElementShakeStrength;
        public int Vibrato => _uiElementShakeVibrato;
        public int Randomness => _uiElementShakeRandomness;
    
    
    
    }
}