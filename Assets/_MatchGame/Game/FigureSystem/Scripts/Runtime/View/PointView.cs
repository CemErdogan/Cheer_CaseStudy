using Abstractions.FigureSystem;
using UnityEngine;

namespace Game.FigureSystem.Runtime
{
    public class PointView : MonoBehaviour, IPointView
    {
        [SerializeField] private MeshRenderer meshRenderer;

        private MaterialPropertyBlock _propertyBlock;
        private static readonly int ColorID = Shader.PropertyToID("_Color");

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void Prepare(ColorType colorType, Vector3 localPosition)
        {
            transform.localPosition = localPosition;
            
            meshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(ColorID, FigureColorUtil.GetColor(colorType));
            meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}