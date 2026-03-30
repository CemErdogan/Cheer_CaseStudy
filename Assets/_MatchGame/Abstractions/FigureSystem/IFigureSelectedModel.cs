namespace Abstractions.FigureSystem
{
    public interface IFigureSelectedModel
    {
        FigureData FigureData { get; }
        
        void Prepare(FigureData signalData);
    }
}