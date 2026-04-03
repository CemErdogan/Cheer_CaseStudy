using System;
using Abstractions.FigureSystem;
using DG.Tweening;
using UnityEngine;

namespace Game.FigureSystem.Runtime
{
    public class PointView : MonoBehaviour, IPointView
    {
        [SerializeField] private MeshRenderer meshRenderer;

        private MaterialPropertyBlock _propertyBlock;
        private static readonly int ColorID = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void Prepare(SlotPosition slot, ColorType colorType, ConnectionData slotData, bool isConnected)
        {
            if (isConnected)
            {
                transform.localPosition = slotData.LocalPosition + slotData.PositionOffset;
                transform.localScale    = slotData.ScaleAdjustment;
            }
            else
            {
                transform.localPosition = slotData.LocalPosition;
                transform.localScale    = slotData.LocalScale;
            }

            SetColor(colorType);
        }

        public void SetConnectedVisual(ConnectionData data)
        {
            var targetPos   = data.LocalPosition + data.PositionOffset;
            var targetScale = data.ScaleAdjustment;

            transform.DOLocalMove(targetPos, 0.2f);
            transform.DOScale(targetScale, 0.2f);
        }

        public void AnimateDestroy(Vector3 center, float duration, Action onComplete)
        {
            var halfDuration = duration * 0.5f;
            var sequence = DOTween.Sequence();

            sequence.Append(transform.DOScale(transform.localScale * 1.1f, halfDuration));
            sequence.Join(transform.DOMove(center, halfDuration));
            sequence.Append(transform.DOScale(Vector3.zero, halfDuration).SetEase(Ease.InBack));
            sequence.OnComplete(() => onComplete?.Invoke());
        }

        private void SetColor(ColorType colorType)
        {
            meshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(ColorID, FigureColorUtil.GetColor(colorType));
            meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
