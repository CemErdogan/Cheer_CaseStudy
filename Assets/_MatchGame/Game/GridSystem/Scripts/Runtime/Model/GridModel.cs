using Abstractions.GridSystem;

namespace Game.GridSystem.Runtime
{
    public class GridModel : IGridModel
    {
        public GridData GridData { get; set; }
        
        public void Prepare(GridData signalData)
        {
            GridData = signalData;
        }
    }
}