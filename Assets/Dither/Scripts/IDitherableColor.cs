using UnityEngine;

namespace Dithering
{
    public interface IDitherableColor<T, TSrc, TDst>
    where T : struct, IDitherableColor<T, TSrc, TDst>
    where TDst : IConvertible<TSrc>
    {
        void SetSource(TSrc source);

        TDst FindNearestDestination();

        T Add(T value);

        T Multiply(float value);
    }
}
