using System;
using Abstractions.GridSystem;
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
            
        }

        public void Dispose()
        {
            
        }
    }
}