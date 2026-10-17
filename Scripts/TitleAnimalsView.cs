using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Application.Core.Scripts
{
    public class TitleAnimalsView : MonoBehaviour
    {
        [Header("Item Settings")]
        [SerializeField] private List<FloatingItem> items = new List<FloatingItem>();

        [Header("Image Pool")]
        [SerializeField] private Sprite[] imagePool;

        [Header("Idle Settings")]
        [SerializeField] private Vector2 idlePositionPunch = new Vector2(10f, 10f);
        [SerializeField] private float idleDuration = 2.5f;
        [SerializeField] private float idleMaxRotation = 3f;

        [Header("Scale Settings")]
        [SerializeField] private float minRandomScale = 0.8f;
        [SerializeField] private float maxRandomScale = 1.2f;

        [Header("Swap Settings")]
        [SerializeField] private float swapCheckInterval = 3f;
        [Range(0f, 1f)] [SerializeField] private float swapProbability = 0.4f;
        [SerializeField] private float transformAnimationDuration = 0.4f;
        [SerializeField] private float punchScaleStrength = 0.25f;

        private void Start()
        {
            InitializeItems();
            StartCoroutine(SwapRoutine());
        }

        private void InitializeItems()
        {
            foreach (var item in items)
            {
                if (item == null || item.rectTransform == null) continue;

                item.originalAnchoredPosition = item.rectTransform.anchoredPosition;
                item.currentScale = item.rectTransform.localScale;
                PlayIdleAnimation(item);
            }
        }

        private void PlayIdleAnimation(FloatingItem item)
        {
            float randomX = Random.Range(-idlePositionPunch.x, idlePositionPunch.x);
            float randomY = Random.Range(-idlePositionPunch.y, idlePositionPunch.y);
            float randomRot = Random.Range(-idleMaxRotation, idleMaxRotation);
            float phaseOffset = Random.Range(0f, idleDuration);

            item.idleTween = DOTween.Sequence()
                .Append(item.rectTransform.DOAnchorPos(item.originalAnchoredPosition + new Vector2(randomX, randomY), idleDuration).SetEase(Ease.InOutSine))
                .Join(item.rectTransform.DORotate(new Vector3(0, 0, randomRot), idleDuration).SetEase(Ease.InOutSine))
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);

            item.idleTween.Goto(phaseOffset, true);
        }

        private IEnumerator SwapRoutine()
        {
            yield return new WaitForSeconds(1f);

            while (true)
            {
                yield return new WaitForSeconds(swapCheckInterval);

                if (items.Count < 2) continue;

                if (Random.value <= swapProbability)
                {
                    int indexA = Random.Range(0, items.Count);
                    int indexB;
                    do
                    {
                        indexB = Random.Range(0, items.Count);
                    } while (indexB == indexA);

                    FloatingItem itemA = items[indexA];
                    FloatingItem itemB = items[indexB];

                    if (itemA.isSwapping || itemB.isSwapping) continue;

                    StartCoroutine(PerformSwap(itemA, itemB));
                }
            }
        }

        private IEnumerator PerformSwap(FloatingItem itemA, FloatingItem itemB)
        {
            itemA.isSwapping = true;
            itemB.isSwapping = true;

            itemA.idleTween?.Kill();
            itemB.idleTween?.Kill();

            Vector2 posA = itemA.rectTransform.anchoredPosition;
            Vector2 posB = itemB.rectTransform.anchoredPosition;

            itemA.originalAnchoredPosition = posB;
            itemB.originalAnchoredPosition = posA;

            float targetScaleAVal = Random.Range(minRandomScale, maxRandomScale);
            float targetScaleBVal = Random.Range(minRandomScale, maxRandomScale);

            Vector3 targetScaleA = new Vector3(targetScaleAVal, targetScaleAVal, 1f);
            Vector3 targetScaleB = new Vector3(targetScaleBVal, targetScaleBVal, 1f);

            itemA.currentScale = targetScaleA;
            itemB.currentScale = targetScaleB;

            float halfDuration = transformAnimationDuration * 0.5f;

            Sequence swapSeqA = DOTween.Sequence();
            Sequence swapSeqB = DOTween.Sequence();

            swapSeqA.Append(itemA.rectTransform.DOScale(Vector3.zero, halfDuration).SetEase(Ease.InBack))
                    .Join(itemA.rectTransform.DOAnchorPos(posB, transformAnimationDuration).SetEase(Ease.InOutQuad));

            swapSeqB.Append(itemB.rectTransform.DOScale(Vector3.zero, halfDuration).SetEase(Ease.InBack))
                    .Join(itemB.rectTransform.DOAnchorPos(posA, transformAnimationDuration).SetEase(Ease.InOutQuad));

            yield return new WaitForSeconds(halfDuration);

            TrySwapImage(itemA);
            TrySwapImage(itemB);

            yield return new WaitForSeconds(halfDuration);

            swapSeqA = DOTween.Sequence();
            swapSeqB = DOTween.Sequence();

            swapSeqA.Append(itemA.rectTransform.DOScale(targetScaleA, halfDuration).SetEase(Ease.OutBack))
                    .Append(itemA.rectTransform.DOPunchScale(targetScaleA * punchScaleStrength, 0.3f, 10, 1f));

            swapSeqB.Append(itemB.rectTransform.DOScale(targetScaleB, halfDuration).SetEase(Ease.OutBack))
                    .Append(itemB.rectTransform.DOPunchScale(targetScaleB * punchScaleStrength, 0.3f, 10, 1f));

            yield return new WaitForSeconds(halfDuration + 0.3f);

            itemA.isSwapping = false;
            itemB.isSwapping = false;

            PlayIdleAnimation(itemA);
            PlayIdleAnimation(itemB);
        }

        private void TrySwapImage(FloatingItem item)
        {
            if (imagePool == null || imagePool.Length == 0 || item.targetImage == null) return;

            Sprite newSprite = imagePool[Random.Range(0, imagePool.Length)];
            if (newSprite != null)
            {
                item.targetImage.sprite = newSprite;
            }
        }

        private void OnDestroy()
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    item.idleTween?.Kill();
                }
            }
        }
    }
}