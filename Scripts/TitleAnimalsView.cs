using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Application.Core.Scripts
{
    public class TitleAnimalsView : MonoBehaviour
    {
        [SerializeField] private List<FloatingItem> _items = new List<FloatingItem>();
        [SerializeField] private Sprite[] _imagePool;

        [SerializeField] private Vector2 _idlePositionPunch = new Vector2(10f, 10f);
        [SerializeField] private float _idleDuration = 3f;
        [SerializeField] private float _idleMaxRotation = 3f;
        [SerializeField] private Ease _idleEase = Ease.InOutSine;

        private void Start()
        {
            InitializeItems();
        }

        private void InitializeItems()
        {
            List<Sprite> availableSprites = new List<Sprite>();
            if (_imagePool != null && _imagePool.Length > 0)
            {
                availableSprites = new List<Sprite>(_imagePool);
            }

            foreach (var item in _items)
            {
                if (item == null || item.rectTransform == null) continue;

                item.originalAnchoredPosition = item.rectTransform.anchoredPosition;
                item._currentScale = item.rectTransform.localScale;

                if (item.targetImage != null && availableSprites.Count > 0)
                {
                    int randomIndex = Random.Range(0, availableSprites.Count);
                    item.targetImage.sprite = availableSprites[randomIndex];
                    
                    availableSprites.RemoveAt(randomIndex);

                    if (availableSprites.Count == 0 && _imagePool != null && _imagePool.Length > 0)
                    {
                        availableSprites = new List<Sprite>(_imagePool);
                    }
                }

                PlayIdleAnimation(item);
            }
        }

        private void PlayIdleAnimation(FloatingItem item)
        {
            item.idleTween?.Kill();

            float randomX = Random.Range(-_idlePositionPunch.x, _idlePositionPunch.x);
            float randomY = Random.Range(-_idlePositionPunch.y, _idlePositionPunch.y);
            float randomRot = Random.Range(-_idleMaxRotation, _idleMaxRotation);

            var targetPos = item.originalAnchoredPosition + new Vector2(randomX, randomY);
            var targetRot = Quaternion.Euler(0, 0, randomRot);

            item.idleTween = DOTween.Sequence()
                .Append(item.rectTransform.DOAnchorPos(targetPos, _idleDuration).SetEase(_idleEase))
                .Join(item.rectTransform.DORotateQuaternion(targetRot, _idleDuration).SetEase(_idleEase))
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
        }

        private void OnDestroy()
        {
            foreach (var item in _items)
            {
                if (item != null)
                {
                    item.idleTween?.Kill();
                }
            }
        }
    }
}