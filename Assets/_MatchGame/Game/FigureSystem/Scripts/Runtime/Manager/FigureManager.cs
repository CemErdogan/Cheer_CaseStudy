using Abstractions.FigureSystem;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureManager : IFigureManager
    {
        [Inject] private readonly Figure.Factory _figureFactory;
    }
}