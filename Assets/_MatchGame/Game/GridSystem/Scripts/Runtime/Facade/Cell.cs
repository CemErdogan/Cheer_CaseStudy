using Abstractions.GridSystem;
using UnityEngine;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class Cell : MonoBehaviour, ICell
    {
        public class Factory : PlaceholderFactory<Cell>
        {
            public override Cell Create()
            {
                return base.Create();
            }
        }
    }
}