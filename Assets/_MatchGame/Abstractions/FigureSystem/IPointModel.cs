namespace Abstractions.FigureSystem
{
    public interface IPointModel
    {
        PointData PointData { get; }
        
        void Prepare(PointData signalData);
    }
}