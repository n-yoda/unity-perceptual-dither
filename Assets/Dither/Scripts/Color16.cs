using UnityEngine;

namespace Dithering
{
    public struct Color16 : IConvertible<Color32>, IConvertible<Color>
    {
        public byte rg, ba;

        public Color16(byte rg, byte ba)
        {
            this.rg = rg;
            this.ba = ba;
        }

        public Color16(int r, int g, int b, int a)
        {
            this.rg = (byte)((r << 4) | g);
            this.ba = (byte)((b << 4) | a);
        }

        public int r
        {
            get
            {
                return rg >> 4;
            }
            set
            {
                rg = (byte)(value << 4 | (rg & 0xf));
            }
        }

        public int g
        {
            get
            {
                return rg & 0xf;
            }
            set
            {
                rg = (byte)(value | (rg & 0xf0));
            }
        }

        public int b
        {
            get
            {
                return ba >> 4;
            }
            set
            {
                ba = (byte)(value << 4 | (ba & 0xf));
            }
        }

        public int a
        {
            get
            {
                return ba & 0xf;
            }
            set
            {
                ba = (byte)(value | (ba & 0xf0));
            }
        }

        public static implicit operator Color32(Color16 x)
        {
            return new Color32((byte)(x.r * 17), (byte)(x.g * 17), (byte)(x.b * 17), (byte)(x.a * 17));
        }

        public static implicit operator Color16(Color32 x)
        {
            return new Color16(x.r >> 4, x.g >> 4, x.b >> 4, x.a >> 4);
        }

        public static implicit operator Color(Color16 x)
        {
            return (Color32)x;
        }

        public static implicit operator Color16(Color x)
        {
            return (Color32)x;
        }

        Color32 IConvertible<Color32>.Convert()
        {
            return this;
        }

        Color IConvertible<Color>.Convert()
        {
            return this;
        }
    }
}
