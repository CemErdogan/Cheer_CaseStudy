using System;

namespace Game.FigureSystem.Runtime
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class EditableFigure : Attribute
    {
        public EditableFigure(){}
    }
}