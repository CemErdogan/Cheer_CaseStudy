using Abstractions.FigureSystem;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureManager : IFigureManager, IInitializable
    {
        private readonly Figure.Factory _figureFactory;

        public FigureManager(Figure.Factory figureFactory)
        {
            _figureFactory = figureFactory;
        }

        public void Initialize()
        {

        }
    }
}