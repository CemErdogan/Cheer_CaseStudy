using Abstractions.FigureSystem;

namespace Game.FigureSystem.Runtime
{
    public class FigureSelectedModel : IFigureSelectedModel
    {
        public FigureData FigureData { get; set; }
        
        public void Prepare(FigureData signalData)
        {
            FigureData = signalData;
        }
    }
}