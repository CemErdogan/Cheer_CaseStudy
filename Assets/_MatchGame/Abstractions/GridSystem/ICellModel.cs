namespace Abstractions.GridSystem
{
    public interface ICellModel
    {
        CellData Data { get; }
        
        void Prepare(CellData signalData);
    }
}