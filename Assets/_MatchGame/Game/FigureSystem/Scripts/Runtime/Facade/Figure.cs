using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class Figure : MonoBehaviour, IFigure
    {
        
        public class Factory : PlaceholderFactory<Figure> { }
    }
}