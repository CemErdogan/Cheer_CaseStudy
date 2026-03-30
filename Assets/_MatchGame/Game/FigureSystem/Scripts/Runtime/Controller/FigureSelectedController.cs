using System;
using Abstractions.FigureSystem;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureSelectedController : IFigureSelectedController, IInitializable, IDisposable
    {
        [Inject] private readonly IFigureSelectedModel _model;
        [Inject] private readonly IFigureSelectedView _view;
        [Inject] private readonly SignalBus _signalBus;
        
        public void Initialize()
        {
            _signalBus.Subscribe<FigureSelectedSpawnSignal>(OnSpawn);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<FigureSelectedSpawnSignal>(OnSpawn);
        }
        
        private void OnSpawn(FigureSelectedSpawnSignal signal)
        {
            _model.Prepare(signal.Data);
        }
    }
}