using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Display
{
    class LevelProgressLabel : AbsInteractDisplay
    {
        //Graphics.Image bg, completeIc;
        //Graphics.TextG name;
        Graphics.TextG text;

        Graphics.AbsVoxelObj lockModel;

        public LevelProgressLabel(Players.Player p,  VikingEngine.LootFest.GO.EnvironmentObj.Teleport teleport)//Map.WorldLevelEnum level, Vector3 teleportPos)
        {
            //bg = new Graphics.Image(SpriteName.WhiteArea, p.pData.view.safeScreenArea.CenterTop, 
            //    new Vector2(12, 2.0f) * Engine.Screen.IconSize, 
            //    ImageLayers.Top6);
            //bg.CenterRelative = new Vector2(0.5f, 0);
            //bg.Opacity = 0.9f;

            //completeIc = new Graphics.Image(SpriteName.NO_IMAGE, new Vector2(bg.RealLeft + Engine.Screen.IconSize, bg.RealCenter.Y),
            //    new Vector2(Engine.Screen.IconSize), ImageLayers.Top5, true);

            //name = new Graphics.TextG(LoadedFont.PhoneText, completeIc.RightTop, new Vector2(Engine.Screen.TextSize),
            //    Graphics.Align.CenterHeight, null, Color.Black, ImageLayers.Top5);
            text = new Graphics.TextG(LoadedFont.Regular, p.SafeScreenArea.PercentToPosition(new Vector2(0.5f, 0.3f)),
                new Vector2(Engine.Screen.TextSize * 2f), Graphics.Align.CenterAll, "", Color.Yellow, ImageLayers.Foreground5);
            

            refresh(p, teleport);
        }

        override public void refresh(Players.Player p, object sender)
        {
            base.refresh(p, sender);

            VikingEngine.LootFest.GO.EnvironmentObj.Teleport teleport = sender as VikingEngine.LootFest.GO.EnvironmentObj.Teleport;
            //var level = teleport.toLocation;
            Vector3 teleportPos = teleport.CollisionAndDefaultBound.MainBound.center;

            //var levelComp = p.getLevelCompleteStatus(level);
            //completeIc.SetSpriteName(VikingEngine.LootFest.BlockMap.LevelsManager.LevelDoorIcon(level, levelComp.completed));

            //if (level == BlockMap.LevelEnum.EndBoss)
            //{
            //    name.TextString = "Final Boss";
            //}
            //else if (level == BlockMap.LevelEnum.Challenge_Zombies)
            //{
            //    name.TextString = "Challenge Level";
            //}
            //else
            //{
            //    name.TextString = "Level " + TextLib.IndexToString(LfLib.EnemyLevelIndex(level));
            //}
            //if (PlatformSettings.DevBuild)
            //{
            //    name.TextString += " (" + level.ToString() + ")";
            //}


            //name.Align = Graphics.Align.CenterHeight;

            //time.Seconds = MinDisplayTimeSec;

            //bg.Visible = levelComp.unlocked;
            //name.Visible = levelComp.unlocked;
            //completeIc.Visible = levelComp.unlocked;

            bool locked = p.Storage.progress.canTravelTo(teleport.toLocation) == false;

            if (locked)
            {
                text.TextString = p.Storage.progress.StoredBabyLocations.TrueCount().ToString() + "/" + VikingEngine.LootFest.Players.PlayerProgress.BossCastleUnlockBabyCount.ToString() + " babies";
            }
            text.Visible = locked;

            viewLockModel(locked, teleport);
        }

        void viewLockModel(bool visible, VikingEngine.LootFest.GO.EnvironmentObj.Teleport teleport)
        {
            if (lockModel == null)
            {
                if (visible)
                {
                    lockModel = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.area_lock, 2f, 0f, true);
                    Map.WorldPosition.Rotation1DToQuaterion(lockModel, teleport.Rotation.Radians + MathHelper.Pi);
                }
            }
            else
            {
                var teleportPos = teleport.Position + VectorExt.V2toV3XZ(teleport.Rotation.Direction(3f), 1f);

                lockModel.position = teleportPos;
                lockModel.Visible = visible;
            }
        }

        override public void DeleteMe()
        {
            //bg.DeleteMe(); completeIc.DeleteMe();
            //name.DeleteMe();

            text.DeleteMe();

            if (lockModel != null)
            {
                lockModel.DeleteMe();
            }
        }
    }
}
