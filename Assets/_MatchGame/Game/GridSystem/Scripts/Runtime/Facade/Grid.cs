using Abstractions.GridSystem;
using UnityEngine;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class Grid : MonoBehaviour, IGrid
    {
        public class Factory : PlaceholderFactory<GridData, Grid> { }
    }
}