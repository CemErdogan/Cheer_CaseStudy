using Abstractions.FigureSystem;
using UnityEngine;

namespace Game.FigureSystem.Runtime
{
    public static class FigureColorUtil
    {
        public static Color GetColor(ColorType colorType)
        {
            return colorType switch
            {
                ColorType.None => new Color(0.55f, 0.55f, 0.55f),
                ColorType.Red => Color.red,
                ColorType.Green => Color.green,
                ColorType.Blue => Color.blue,
                ColorType.Yellow => Color.yellow,
                _ => Color.white
            };
        }
    }
}