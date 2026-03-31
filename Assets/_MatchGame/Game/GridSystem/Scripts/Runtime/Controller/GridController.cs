using System;
using Abstractions.GridSystem;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class GridController : IGridController, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly IGridModel _model;
        [Inject] private readonly IGridView _view;
        
        public void Initialize()
        {
            _signalBus.Subscribe<GridSpawnSignal>(OnSpawn);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GridSpawnSignal>(OnSpawn);
        }

        private void OnSpawn(GridSpawnSignal signal)
        {
            _model.Prepare(signal.Data);
        }
    }
}