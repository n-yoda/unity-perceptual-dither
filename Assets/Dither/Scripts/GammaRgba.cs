using UnityEngine;

namespace Dithering
{
    public struct GammaRgba : IDitherableColor<GammaRgba, Color32, Color16>
    {
        public Color value;

        public GammaRgba(Color value)
        {
            this.value = value;
        }

        public void SetSource(Color32 source)
        {
            this.value = source;
        }

        public Color16 FindNearestDestination()
        {
            return value;
        }

        public GammaRgba Add(GammaRgba value)
        {
            return new GammaRgba(this.value + value.value);
        }

        public GammaRgba Multiply(float value)
        {
            return new GammaRgba(this.value * value);
        }
    }
}
