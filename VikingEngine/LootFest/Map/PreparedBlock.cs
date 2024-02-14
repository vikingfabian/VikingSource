using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LootFest.Map
{
    interface IPreparedBlock
    {
        bool HaveSurface { get; }
        bool ShadowSide { get; }
        void AddShadowValue(float value);
        
    }
    struct EmptyBlock : IPreparedBlock
    {
        public bool HaveSurface { get { return false; } }
        public bool ShadowSide { get { return false; } }
        public void AddShadowValue(float value)
        { }
    }
   
    
}
