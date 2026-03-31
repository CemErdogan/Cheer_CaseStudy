using Abstractions.GridSystem;
using UnityEngine;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class Cell : MonoBehaviour, ICell
    {
        [Inject] private readonly SignalBus _signalBus;
        
        public void Prepare(CellData data)
        {
            _signalBus.Fire(new CellSpawnSignal(data));
        }
        
        public class Factory : PlaceholderFactory<CellData, Cell>
        {
            public override Cell Create(CellData data)
            {
                var cell = base.Create(data);
                cell.Prepare(data);
                return cell;
            }
        }
    }
}