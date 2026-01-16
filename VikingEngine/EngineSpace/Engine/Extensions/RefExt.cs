using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    static class RefExt
    {
        public static bool EqTarget<T>(WeakReference<T> a, WeakReference<T> b) where T : class
        {
            return a.TryGetTarget(out var targetA) && b.TryGetTarget(out var targetB) && targetA == targetB && targetA != null;
        }

        public static bool EqTarget<T>(T obj, WeakReference<T> b) where T : class
        {
            return b.TryGetTarget(out var targetB) && obj == targetB && obj != null;
        }

        public static bool EqTarget_safe<T>(T obj, WeakReference<T> b) where T : class
        {
            return b != null && b.TryGetTarget(out var targetB) && obj == targetB && obj != null;
        }

        public static T Target<T>(WeakReference<T> wref) where T : class
        {
            return wref.TryGetTarget(out T target) ? target : null;
        }
        public static T Target_safe<T>(WeakReference<T> wref) where T : class
        {
            if (wref == null) return null;

            return wref.TryGetTarget(out T target) ? target : null;
        }

        public static bool EqTarget<T>(WeakReference<T> a, WeakReference<T> b, out T targetA) where T : class
        {
            return a.TryGetTarget(out targetA) && b.TryGetTarget(out var targetB) && targetA == targetB && targetA != null;


            ////if (a == null || b == null)
            ////    return false;

            //if (!a.TryGetTarget(out var targetA))
            //    return false;

            //if (!b.TryGetTarget(out var targetB))
            //    return false;

            //return ReferenceEquals(targetA, targetB);
        }
    }
}
