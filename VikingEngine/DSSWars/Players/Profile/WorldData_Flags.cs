using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public Sprite blackFlagSprite;

        void initFlagTexture()
        {
            flagIconsTexture = new PixelTexture(new IntVector2(FlagsWidthCount * DssLib.UserHeraldicWidth));
            LoadContent.Textures[(int)LoadedTexture.FlagTexture] = flagIconsTexture;

            //Create a black sprite at bottom right
            IntVector2 topLeft = flagIconsTexture.Size() - new IntVector2(DssLib.UserHeraldicWidth);
            IntVector2 toPos = IntVector2.Zero;
            for (int y = 0; y < DssLib.UserHeraldicWidth; y++)
            {
                toPos.Y = topLeft.Y + y;
                for (int x = 0; x < DssLib.UserHeraldicWidth; x++)
                {
                    toPos.X = topLeft.X + x;
                    flagIconsTexture.SetPixel(toPos, Color.Black);
                }
            }
            blackFlagSprite = new Sprite(new Microsoft.Xna.Framework.Rectangle(topLeft.X, topLeft.Y, DssLib.UserHeraldicWidth, DssLib.UserHeraldicWidth), LoadedTexture.FlagTexture);
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
