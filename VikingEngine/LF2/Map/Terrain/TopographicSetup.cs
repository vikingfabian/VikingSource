using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.Map.Terrain
{
    class TopographicSetup
    {
        //Each world have a couple of templated
        //height templates
        //detail height
        //Terrain objects template
        //Villages/towns setup

        //const int NumHeightTemplates = 16;
        HeightTemplate[] heightTemplates;
       // byte keyAdd;

        public TopographicSetup()
        {
            heightTemplates = new HeightTemplate[Terrain.HandmadeTemplates.Height1Templates.Count];
            for (int i = 0; i < Terrain.HandmadeTemplates.Height1Templates.Count; i++)
            {
                heightTemplates[i] = new HeightTemplate(Terrain.HandmadeTemplates.Height1Templates[i]);
            }
        }
        public HeightTemplate ChunkTemplate()
        {
            //detta ska ske en gång innan varje screen öppnas, kommer flyttas
            return heightTemplates[Data.RandomSeed.Instance.Next(Terrain.HandmadeTemplates.Height1Templates.Count)];
        }
       
    }
}
