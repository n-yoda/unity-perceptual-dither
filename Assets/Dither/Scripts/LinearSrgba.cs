using UnityEngine;

namespace Dithering
{
    public struct LinearSrgba : IDitherableColor<LinearSrgba, Color32, Color16>
    {
        public Color value;

        LinearSrgba(Color linear)
        {
            this.value = linear;
        }

        public void SetSource(Color32 source)
        {
            this.value = ((Color)source).linear;
        }

        public Color16 FindNearestDestination()
        {
            var mid = (Color16)value.gamma;
            var curr = mid;
            var error = SqrError(mid);

            curr.r = mid.r - 1;
            var nR = SqrError(curr);
            curr.r = mid.r + 1;
            var pR = SqrError(curr);
            mid.r = SelectMinError(mid.r, mid.r - 1, mid.r + 1, error, nR, pR);
            curr.r = mid.r;
            error = SqrError(mid);

            curr.g = mid.g - 1;
            var nG = SqrError(curr);
            curr.g = mid.g + 1;
            var pG = SqrError(curr);
            mid.g = SelectMinError(mid.g, mid.g - 1, mid.g + 1, error, nG, pG);
            curr.g = mid.g;
            error = SqrError(mid);

            curr.b = mid.b - 1;
            var nB = SqrError(curr);
            curr.b = mid.b + 1;
            var pB = SqrError(curr);
            mid.b = SelectMinError(mid.b, mid.b - 1, mid.b + 1, error, nB, pB);

            return mid;
        }

        int SelectMinError(int a, int b, int c, float ae, float be, float ce) {
            if (ae <= be && ae <= ce) {
                return a;
            } else if (be <= ae && be <= ce) {
                return b;
            } else {
                return c;
            }
        }

        float SqrError(Color16 c)
        {
            var linear = ((Color)c).linear;
            return Vector4.SqrMagnitude(value - linear);
        }

        public LinearSrgba Add(LinearSrgba value)
        {
            return new LinearSrgba(this.value + value.value);
        }

        public LinearSrgba Multiply(float value)
        {
            return new LinearSrgba(this.value * value);
        }
    }
}
