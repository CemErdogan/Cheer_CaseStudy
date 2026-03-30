using Abstractions.FigureSystem;

namespace Game.FigureSystem.Runtime
{
    public class PointModel : IPointModel
    {
        public PointData PointData { get; set; }
        
        public void Prepare(PointData signalData)
        {
            PointData = signalData;
        }
    }
}