using System;
using UnityEngine;

namespace NiceRectUI
{
    [Serializable]
    public class FillOption
    {
        public FillType fillType = FillType.SolidColor;
        public Color color = Color.white;
        public GradientType gradientType = GradientType.Linear;
        public Gradient gradient = new Gradient();
        [Range(0, 360)]
        public float gradientRotation = 0f;
        [Range(-1, 1)]
        public float gradientScale = 0f;
    }
}