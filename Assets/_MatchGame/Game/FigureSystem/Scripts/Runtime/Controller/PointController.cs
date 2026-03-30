using System;
using Abstractions.FigureSystem;
using Common.Util;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class PointController : IPointController, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus; 
        [Inject] private readonly IPointModel _model;
        [Inject] private readonly IPointView _view;
        
        public void Initialize()
        {
            _signalBus.Subscribe<PointSpawnSignal>(OnPointSpawn);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<PointSpawnSignal>(OnPointSpawn);
        }
        
        private void OnPointSpawn(PointSpawnSignal signal)
        {
            _model.Prepare(signal.Data);
            _view.Prepare(signal.Data.ColorType, signal.Data.Coord.ToPosition());
        }
    }
}