using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class Point : MonoBehaviour, IPoint
    {
        [Inject] private readonly SignalBus _signalBus;
        
        public void Prepare(PointData data)
        {
            _signalBus.Fire(new PointSpawnSignal(data));
        }

        public class Factory : PlaceholderFactory<PointData, Point>
        {
            public override Point Create(PointData data)
            {
                var point = base.Create(data);
                point.Prepare(data);
                return point;
            }
        }
    }
}