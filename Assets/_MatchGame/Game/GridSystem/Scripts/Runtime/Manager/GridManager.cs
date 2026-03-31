using Abstractions.GridSystem;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class GridManager : IGridManager
    {
        [Inject] private readonly Grid.Factory _gridFactory;
        [Inject] private readonly Cell.Factory _cellFactory;
    }
}