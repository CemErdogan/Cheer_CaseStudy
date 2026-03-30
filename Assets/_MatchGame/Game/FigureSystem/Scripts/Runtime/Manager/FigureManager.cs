using System;
using Abstractions.FigureSystem;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureManager : IFigureManager, IInitializable, IDisposable
    {
        [Inject] private readonly Figure.Factory _figureFactory;

        public void Initialize()
        {

        }

        public void Dispose()
        {
            
        }
    }
}