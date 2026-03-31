using Abstractions.GridSystem;

namespace Game.GridSystem.Runtime
{
    public class CellModel : ICellModel
    {
        public CellData Data { get; set; }

        public void Prepare(CellData signalData)
        {
            Data = signalData;
        }
    }
}