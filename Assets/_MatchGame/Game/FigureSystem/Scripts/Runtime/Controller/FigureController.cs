using System;
using Abstractions.FigureSystem;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureController : IFigureController, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly IFigureModel _model;
        [Inject] private readonly IFigureView _view;
        
        public void Initialize()
        {
            _signalBus.Subscribe<FigureSpawnSignal>(OnSpawn);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<FigureSpawnSignal>(OnSpawn);
        }

        private void OnSpawn(FigureSpawnSignal signal)
        {
            _model.Prepare(signal.Data);
        }
    }
}