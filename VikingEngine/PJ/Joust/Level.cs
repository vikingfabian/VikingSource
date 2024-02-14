using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.PJ.Joust.DropItem;

namespace VikingEngine.PJ.Joust
{
    class Level
    {
        //public float PlayerRoofY;
        //public float PlayerGroundY;
        public float roofY;
        public float groundY;
        public float leftX;
        public float rightX;
        DropLane[] dropLanes;
        public List<AbsLevelObject> LevelObjects = new List<AbsLevelObject>(16);

        public VectorRect playArea;
        public float autoJumpY;

        public Level()
        {
            JoustRef.level = this;

            roofY = (int)(Engine.Screen.Height * 0.12f);
            groundY = (int)(Engine.Screen.Height * 0.85f);
            leftX = Engine.Screen.Width * 0.1f;
            rightX = Engine.Screen.Width * 0.9f;

            playArea = VectorRect.FromEdges(leftX, roofY, rightX, groundY);
            autoJumpY = playArea.PercentToPosition(new Vector2(0.7f)).Y;

            float playerRad = Gamer.ImageScale * PjLib.AnimalCharacterSzToBoundSz * 0.95f;
            //PlayerRoofY = roofY + playerRad;
            //PlayerGroundY = groundY - playerRad;

            var bgTex = Engine.LoadContent.Texture(LoadedTexture.BirdJoustBG);
            Vector2 bgSz = Vector2.Zero;

            bgSz.Y = (int)(groundY - roofY + 2 + 2);
            bgSz.X = bgSz.Y / bgTex.Height * bgTex.Width;

            if (bgSz.X < Engine.Screen.Width)
            {
                bgSz.X = Engine.Screen.Width + 2f;
                bgSz.Y = bgSz.X / bgTex.Width * bgTex.Height;
            }

            Graphics.ImageAdvanced bg = new Graphics.ImageAdvanced(SpriteName.WhiteArea, 
                new Vector2(Engine.Screen.CenterScreen.X - bgSz.X * 0.5f, roofY - 1), 
                bgSz, ImageLayers.Bottom9, false);
            bg.Texture = bgTex;
            bg.SetFullTextureSource();

            //Roof
            Graphics.Image roof = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(Engine.Screen.Width, roofY), ImageLayers.Bottom6);
            roof.Color = Color.Black;

            const float RoofLineH = 4;
            Graphics.Image roofLine = new Graphics.Image(SpriteName.WhiteArea, new Vector2(0, roofY - 4 - RoofLineH), new Vector2(Engine.Screen.Width, RoofLineH), ImageLayers.Bottom5);
            roofLine.Color = Color.DarkRed;

            Vector2 groundPos = new Vector2(-10, groundY);
            Vector2 groundSize = Vector2.Zero;
            groundSize.Y = (int)((Engine.Screen.Height - groundY) * 1.2f);
            groundSize.X = (int)(groundSize.Y / 3 * 4);

            while (groundPos.X < Engine.Screen.Width)
            {
                Graphics.Image ground = new Graphics.Image(SpriteName.birdGroundTex, groundPos, groundSize, ImageLayers.Bottom6);
                groundPos.X += groundSize.X;
            }
            //Sides
            const float SideLineWidth = 4f;
            Graphics.Image leftSide = new Graphics.Image(SpriteName.WhiteArea, new Vector2(leftX - SideLineWidth, 0), 
                new Vector2(SideLineWidth, Engine.Screen.Height), ImageLayers.Bottom7);

            Graphics.Image rightSide = new Graphics.Image(SpriteName.WhiteArea, new Vector2(rightX, 0), 
                new Vector2(SideLineWidth, Engine.Screen.Height), ImageLayers.Bottom7);

            Graphics.Image leftSideSoft = new Graphics.Image(SpriteName.WhiteArea, new Vector2(leftX - SideLineWidth, 0),
                new Vector2(SideLineWidth, Engine.Screen.Height), ImageLayers.Bottom7);
            leftSideSoft.Xpos -= 3;
            leftSideSoft.Opacity = 0.3f;

            Graphics.Image rightSideSoft = new Graphics.Image(SpriteName.WhiteArea, new Vector2(rightX, 0),
                new Vector2(SideLineWidth, Engine.Screen.Height), ImageLayers.Bottom7);
            rightSideSoft.Xpos += 3;
            rightSideSoft.Opacity = 0.3f;

            //Drop lanes

            int laneCount = Ref.rnd.Int(3, 6);

            dropLanes = new DropLane[laneCount];
            for (int laneIx = 0; laneIx < laneCount; ++laneIx)
            {
                dropLanes[laneIx] = new DropLane(laneIx, laneCount, this);
            }           
        }

        public void Update()
        {
            foreach (DropLane d in dropLanes)
            {
                d.Update();
            }

            for (int i = LevelObjects.Count - 1; i >= 0; --i)
            {
                if (LevelObjects[i].Update())
                {
                    LevelObjects.RemoveAt(i);
                }
            }
        }        
    }

    enum JoustObjectType
    {
        Spikes,
        PushedSpikes,
        ChangeDir,
        SpeedBoost,
        Coin,
        RandomItemBox,
        LazerBullet,
        SpikeShieldBall,
        NUM
    }
        
}
