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

        [Header("Pointer animation")]
        [SerializeField] private float _hoverScale = 1.06f;
        [SerializeField] private float _hoverDuration = 0.18f;
        [SerializeField] private Ease _hoverEase = Ease.OutBack;
        [SerializeField] private Ease _exitEase = Ease.OutQuad;
        [SerializeField] private float _pressedScale = 0.96f;
        [SerializeField] private float _pressedDuration = 0.08f;
        [SerializeField] private Vector2 _hoverShadowDistance = new Vector2(2f, -2f);
        [SerializeField] private float _shadowDuration = 0.14f;
        
        public float Duration => _uiElementScaleDuration;
        public float Scale => _uiElementScaleValue;
        public Ease Ease => _uiElementScaleEase;
        public float ShakeStrength => _uiElementShakeStrength;
        public int Vibrato => _uiElementShakeVibrato;
        public int Randomness => _uiElementShakeRandomness;
        
        public float HoverScale => _hoverScale;
        public float HoverDuration => _hoverDuration;
        public Ease HoverEase => _hoverEase;
        public Ease ExitEase => _exitEase;
        public float PressedScale => _pressedScale;
        public float PressedDuration => _pressedDuration;
        public Vector2 HoverShadowDistance => _hoverShadowDistance;
        public float ShadowDuration => _shadowDuration;
    }
}