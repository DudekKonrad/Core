using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Core.Scripts
{
    [System.Serializable]
    public class FloatingItem : MonoBehaviour
    {
        public RectTransform rectTransform;
        public Image targetImage;

        [HideInInspector] public Vector2 originalAnchoredPosition;
        [HideInInspector] public Tween idleTween;
        [HideInInspector] public bool isSwapping = false;
        [HideInInspector] public Vector3 currentScale = Vector3.one;
    }
}