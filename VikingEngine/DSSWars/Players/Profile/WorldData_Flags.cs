using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars
{

    partial class WorldData //FLAGS
    {
        const int FlagsWidthCount = 64;
        PixelTexture flagIconsTexture;

        void initFlagTexture()
        {
            flagIconsTexture = new PixelTexture(new IntVector2(FlagsWidthCount * DssLib.UserHeraldicWidth));
            LoadContent.Textures[(int)LoadedTexture.FlagTexture] = flagIconsTexture;
        }

        public void CreateFlagSprite(int factionIndex, FlagAndColor factionVisual)
        {
            IntVector2 gridPos = new IntVector2(factionIndex % FlagsWidthCount, factionIndex / FlagsWidthCount) * DssLib.UserHeraldicWidth;
            factionVisual.flagDesign.AddToTexture(factionVisual, flagIconsTexture, gridPos);
            flagIconsTexture.updateFlag = true;

            factionVisual.sprite = new Sprite(new Microsoft.Xna.Framework.Rectangle(gridPos.X, gridPos.Y, DssLib.UserHeraldicWidth, DssLib.UserHeraldicWidth), LoadedTexture.FlagTexture);
        }

        public void RefreshFlagsUpdate()
        {
            if (flagIconsTexture.updateFlag)
            {
                flagIconsTexture.updateFlag = false;
                flagIconsTexture.ApplyPixelsToTexture();
            }
        }
    }

}
