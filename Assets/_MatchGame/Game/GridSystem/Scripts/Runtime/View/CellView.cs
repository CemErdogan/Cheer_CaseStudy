using Abstractions.GridSystem;
using UnityEngine;

namespace Game.GridSystem.Runtime
{
    public class CellView : MonoBehaviour, ICellView
    {
        public void Prepare(Vector3 position)
        {
            transform.position = position;
        }
    }
}