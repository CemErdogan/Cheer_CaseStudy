namespace Abstractions.FigureSystem
{
    public interface IFigureFactory
    {
        IFigure Create(FigureData data);
    }
}
