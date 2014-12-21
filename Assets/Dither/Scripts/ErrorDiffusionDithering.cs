using UnityEngine;
using System.Collections;

namespace Dithering
{
    public struct ErrorDiffusion
    {
        public ErrorDiffusion(int x, int y, float weight)
        {
            this.x = x;
            this.y = y;
            this.weight = weight;
        }

        public int x, y;
        public float weight;

        public static readonly ErrorDiffusion[] FloydSteinberg = new ErrorDiffusion[]
        {
            new ErrorDiffusion(1, 0, 7 / 16f),
            new ErrorDiffusion(-1, 1, 3 / 16f),
            new ErrorDiffusion(0, 1, 5 / 16f),
            new ErrorDiffusion(1, 1, 1 / 16f),
        };
    }

    public static class ErrorDiffusionDithering
    {
        public static void Dither<TError, TSrc, TDst>(TSrc[] source, int width, ErrorDiffusion[] diffuse)
        where TError : struct, IDitherableColor<TError, TSrc, TDst>
        where TDst : IConvertible<TSrc>
        {
            var height = source.Length / width;
            var error = new TError[source.Length];
            var at = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var target = new TError();
                    target.SetSource(source[at]);
                    target = target.Add(error[at]);
                    source[at] = target.FindNearestDestination().Convert();
                    var cvt = new TError();
                    cvt.SetSource(source[at]);
                    var err = target.Add(cvt.Multiply(-1));
                    for (int i = 0; i < diffuse.Length; i++)
                    {
                        var u = diffuse[i].x + x;
                        var v = diffuse[i].y + y;
                        if (u >= 0 && v >= 0 && u < width && v < height)
                        {
                            var at2 = u + v * width;
                            error[at2] = error[at2].Add(err.Multiply(diffuse[i].weight));
                        }
                    }
                    at++;
                }
            }
        }
    }
}
