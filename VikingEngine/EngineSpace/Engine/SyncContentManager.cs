using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using VikingEngine.Graphics;

namespace VikingEngine.Engine
{
    class SyncContentManager : ContentManager
    {
        public SyncContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }

        public override T Load<T>(string assetName)
        {
            if (typeof(T) == typeof(SoundEffect))
            {
                return base.Load<T>(assetName);
            }
            using (GpuResourceSync.Acquire())
            {
                return base.Load<T>(assetName);
            }
        }
    }
}
