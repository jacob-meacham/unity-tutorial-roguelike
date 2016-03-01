using System;
using System.Linq;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class _
{
    public static IEnumerable<TResult> Zip<TA, TB, TResult>(
        this IEnumerable<TA> a, IEnumerable<TB> b, Func<TA, TB, TResult> func)
    {
        if (a == null)
        {
            throw new ArgumentNullException("a");
        }

        if (b == null)
        {
            throw new ArgumentNullException("b");
        }

        using (var iteratorA = a.GetEnumerator())
        using (var iteratorB = b.GetEnumerator())
        {
            while (iteratorA.MoveNext() && iteratorB.MoveNext())
            {
                yield return func(iteratorA.Current, iteratorB.Current);
            }
        }
    }

    public static TA RandomElement<TA>(
        this IList<TA> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
