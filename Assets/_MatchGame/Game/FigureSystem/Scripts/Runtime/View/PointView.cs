using Abstractions.FigureSystem;
using UnityEngine;

namespace Game.FigureSystem.Runtime
{
    public class PointView : MonoBehaviour, IPointView
    {
        [SerializeField] private MeshRenderer meshRenderer;

        private static readonly MaterialPropertyBlock PropertyBlock = new ();
        private static readonly int ColorID = Shader.PropertyToID("_Color");

        public void Prepare(ColorType colorType, Vector3 localPosition)
        {
            transform.localPosition = localPosition;
            
            meshRenderer.GetPropertyBlock(PropertyBlock);
            PropertyBlock.SetColor(ColorID, FigureColorUtil.GetColor(colorType));
            meshRenderer.SetPropertyBlock(PropertyBlock);
        }
    }
}