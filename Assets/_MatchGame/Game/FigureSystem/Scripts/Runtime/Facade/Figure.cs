using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class Figure : MonoBehaviour, IFigure
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly Point.Factory _pointFactory;
        
        public void Prepare(FigureData data)
        {
            _signalBus.Fire(new FigureSelectedSpawnSignal(data));
        }
        
        public class Factory : PlaceholderFactory<FigureData, Figure>
        {
            public override Figure Create(FigureData data)
            {
                var figure = base.Create(data);
                figure.Prepare(data);
                return figure;
            }
        }
    }
}