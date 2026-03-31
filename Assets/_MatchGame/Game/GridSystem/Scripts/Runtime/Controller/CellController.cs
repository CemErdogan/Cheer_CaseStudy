using System;
using Abstractions.GridSystem;
using Common.Util;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class CellController : ICellController, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly ICellModel _model;
        [Inject] private readonly ICellView _view;
        
        public void Initialize()
        {
            _signalBus.Subscribe<CellSpawnSignal>(OnSpawn);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<CellSpawnSignal>(OnSpawn);
        }

        private void OnSpawn(CellSpawnSignal signal)
        {
            _model.Prepare(signal.Data);
            _view.Prepare(_model.Data.Coord.ToPosition());
        }
    }
}