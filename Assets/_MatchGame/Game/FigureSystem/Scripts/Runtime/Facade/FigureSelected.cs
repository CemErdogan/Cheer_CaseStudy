using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureSelected : MonoBehaviour, IFigureSelected
    {
        [Inject] private readonly SignalBus _signalBus;
        
        public void Prepare(FigureData data)
        {
            _signalBus.Fire(new FigureSelectedSpawnSignal(data));
        }
        
        public class Factory : PlaceholderFactory<FigureData, FigureSelected> { }
    }
}