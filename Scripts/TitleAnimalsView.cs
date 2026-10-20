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
        [SerializeField] private float _minIdleDuration = 2f;
        [SerializeField] private float _maxIdleDuration = 4f;
        [SerializeField] private float _idleMaxRotation = 3f;
        [SerializeField] private Ease _idleEase = Ease.InOutSine;

        private void Start()
        {
            InitializeItems();
        }

        private void InitializeItems()
        {
            var availableSprites = new List<Sprite>();
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
            float randomDuration = Random.Range(_minIdleDuration, _maxIdleDuration);

            Vector2 punchOffset = new Vector2(randomX, randomY);

            Vector2 targetPos1 = item.originalAnchoredPosition + punchOffset;
            Vector2 targetPos2 = item.originalAnchoredPosition - punchOffset;

            Quaternion targetRot1 = Quaternion.Euler(0, 0, randomRot);
            Quaternion targetRot2 = Quaternion.Euler(0, 0, -randomRot);

            item.idleTween = DOTween.Sequence()
                .Append(item.rectTransform.DOAnchorPos(targetPos1, randomDuration).SetEase(_idleEase))
                .Join(item.rectTransform.DORotateQuaternion(targetRot1, randomDuration).SetEase(_idleEase))
                .Append(item.rectTransform.DOAnchorPos(targetPos2, randomDuration).SetEase(_idleEase))
                .Join(item.rectTransform.DORotateQuaternion(targetRot2, randomDuration).SetEase(_idleEase))
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);

            item.idleTween.Goto(Random.Range(0f, randomDuration * 2f), true);
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