using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureSelected : MonoBehaviour, IFigureSelected
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly Point.Factory _pointFactory;
        [Inject] private readonly ConnectionDatabase _connectionDb;

        public void Prepare(FigureData data)
        {
            foreach (var pointData in data.Points)
            {
                var slotData = _connectionDb.GetData(pointData.Position);
                var point = _pointFactory.Create(pointData);
                point.Initialize(pointData, null, slotData);
                point.Transform.SetParent(transform, false);
            }

            _signalBus.Fire(new FigureSelectedSpawnSignal(data));
        }
        
        public class Factory : PlaceholderFactory<FigureData, FigureSelected>
        {
            public override FigureSelected Create(FigureData data)
            {
                var figureSelected = base.Create(data);
                figureSelected.Prepare(data);
                return figureSelected;
            }
        }
    }
}