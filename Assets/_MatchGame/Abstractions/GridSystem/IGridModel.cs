namespace Abstractions.GridSystem
{
    public interface IGridModel
    {
        GridData GridData { get; }
        
        void Prepare(GridData signalData);
    }
}