using System;
using SFML.Graphics;

namespace Shellblade.Graphics.UI
{
    public class OutlinedBox : Box
    {
        public Color Color
        {
            get => rectangle.OutlineColor;
            set => rectangle.OutlineColor = value;
        }

        public float OutlineThickness
        {
            get => rectangle.OutlineThickness;
            set => rectangle.OutlineThickness = value;
        }

        public Color FillColor
        {
            get => rectangle.FillColor;
            set => rectangle.FillColor = value;
        }

        public OutlinedBox() : base()
        {
            rectangle.OutlineColor = Color.White;
            rectangle.OutlineThickness = 1f;
        }
    }
}
