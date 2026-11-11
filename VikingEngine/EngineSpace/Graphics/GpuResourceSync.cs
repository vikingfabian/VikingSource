using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    public static class GpuResourceSync
    {
        // >= native freeFrames (swapchainCount + 1). IndexBuffers are never frame-stamped in MGG_Vulkan.cpp.
        private const int DisposeDelayFrames = 4;
        private static readonly object gpuLock = new object();
        private static readonly ConcurrentQueue<(GraphicsResource res, long frame)> pending =
            new ConcurrentQueue<(GraphicsResource res, long frame)>();
        private static readonly ConditionalWeakTable<GraphicsResource, FinalizeSentinel> tracked =
            new ConditionalWeakTable<GraphicsResource, FinalizeSentinel>();
        private static long drawFrame;

        public static bool Enabled
        {
            get { return PlatformSettings.ForceForegroundDestructors && PlatformSettings.LoadResourcesInBackground; }
        }

        private sealed class FinalizeSentinel
        {
            private GraphicsResource res;

            public FinalizeSentinel(GraphicsResource r)
            {
                res = r;
            }

            ~FinalizeSentinel()
            {
                var r = Interlocked.Exchange(ref res, null);
                if (r != null && !r.IsDisposed)
                {
                    pending.Enqueue((r, Interlocked.Read(ref drawFrame)));
                }
            }
        }

        public readonly struct Scope : IDisposable
        {
            private readonly bool taken;

            public Scope(bool taken)
            {
                this.taken = taken;
            }

            public void Dispose()
            {
                if (taken)
                {
                    Monitor.Exit(gpuLock);
                }
            }
        }

        public static Scope Acquire()
        {
            if (!Enabled)
            {
                return new Scope(false);
            }
            bool taken = false;
            Monitor.Enter(gpuLock, ref taken);
            return new Scope(taken);
        }

        public static bool EnterFrame()
        {
            if (!Enabled)
            {
                return false;
            }
            bool taken = false;
            Monitor.Enter(gpuLock, ref taken);
            long frame = Interlocked.Increment(ref drawFrame);
            while (pending.TryPeek(out var item) && frame - item.frame >= DisposeDelayFrames)
            {
                if (pending.TryDequeue(out item) && item.res != null && !item.res.IsDisposed)
                {
                    item.res.Dispose();
                }
            }
            return taken;
        }

        public static void ExitFrame(bool taken)
        {
            if (taken)
            {
                Monitor.Exit(gpuLock);
            }
        }

        public static void SuppressFinalizer(GraphicsResource res)
        {
            if (Enabled && res != null)
            {
                GC.SuppressFinalize(res);
            }
        }

        public static T Track<T>(T res) where T : GraphicsResource
        {
            if (Enabled && res != null)
            {
                GC.SuppressFinalize(res);
                tracked.GetValue(res, static r => new FinalizeSentinel(r));
            }
            return res;
        }

        public static void Release(GraphicsResource res)
        {
            if (res == null)
            {
                return;
            }
            if (!Enabled)
            {
                if (!res.IsDisposed)
                {
                    res.Dispose();
                }
                return;
            }
            GC.SuppressFinalize(res);
            if (!res.IsDisposed)
            {
                pending.Enqueue((res, Interlocked.Read(ref drawFrame)));
            }
        }
    }
}
