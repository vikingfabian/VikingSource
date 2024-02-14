//using VikingEngine.Input;
//using VikingEngine.LootFest.Players;
//using VikingEngine.SteamWrapping;
//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.Graphics
//{
//    abstract class InputMapImage : Image
//    {
//        /* Const */
//        protected const float MILLISECONDS_PER_PIC = 3000;

//        /* Properties */
//        public UpdateType UpdateType { get { return UpdateType.Full; } }
//        public bool SavingThread { get { return false; } }

//        /* Fields */
//        protected PlayerInputMap mapSource;
//        //bool rotatePics;

//        //List<SpriteName> SpriteNames;
//        //List<SpriteName> steamSpriteNames;
//        int steamControllerIconCount;
//        bool currentlyShowNonSteam;
//        float countDown_ms;
//        int picIndex;

//        /* Constructors */
//        public InputMapImage(PlayerInputMap mapSource, Vector2 pos, Vector2 sz, ImageLayers layer, bool centerMidpoint, bool addToRender)// bool rotatePics)
//            : base(SpriteName.NO_IMAGE, pos, sz, layer, centerMidpoint, addToRender)
//        {
//            this.mapSource = mapSource;
//            //this.rotatePics = rotatePics;
//            //SpriteNames = GetNonSteamIcons();
//            //steamSpriteNames = new List<SpriteName>();
//            steamControllerIconCount = 0;
//            currentlyShowNonSteam = true;
//            picIndex = 0;
//            //Ref.update.AddToOrRemoveFromUpdate(this, true);

//            countDown_ms = 0;
//            //Time_Update(0);
//        }

//        public InputMapImage(PlayerInputMap mapSource, bool addToRender)//, bool rotatePics)
//            : this(mapSource, Vector2.Zero, Vector2.Zero, ImageLayers.NUM, false, addToRender)
//        { }

//        /* Family methods */
//        protected abstract List<SpriteName> GetNonSteamIcons();
//        protected abstract int UpdateSteamIcons(List<SpriteName> steamTiles);

//        //public override void DeleteMe()
//        //{
//        //    base.DeleteMe();
//        //    Ref.update.AddToOrRemoveFromUpdate(this, false);
//        //}

//        /* Novelty methods */
//        //public void Time_Update(float time_ms)
//        public override void settingsChangedRefresh()
//        {
//            SpriteNames = GetNonSteamIcons();
//            //if (SpriteNames != newSpriteNames)
//            //{
//            //    SpriteNames = newSpriteNames;
//            //}
//            steamControllerIconCount = UpdateSteamIcons(steamSpriteNames);

//            //if (rotatePics)
//            //{
//            //    countDown_ms -= time_ms;
//            //    if (countDown_ms < 0)
//            //    {
//            //        countDown_ms = MILLISECONDS_PER_PIC;
//            //        UpdatePic();
//            //    }
//            //}
//            //else
//            //{
//            UpdatePic();
//            //}
//        }

//        void UpdatePic()
//        {
//            //if (rotatePics)
//            //{
//            //    ++picIndex;
//            //    if (currentlyShowNonSteam)
//            //    {
//            //        if (picIndex < SpriteNames.Count)
//            //        {
//            //            // just show pic
//            //        }
//            //        else
//            //        {
//            //            if (mapSource.UsingSteamController && steamControllerIconCount > 0)
//            //            {
//            //                currentlyShowNonSteam = false;
//            //                picIndex = 0;
//            //            }
//            //            else if (SpriteNames.Count > 0)
//            //            {
//            //                picIndex = 0;
//            //            }
//            //            else
//            //            {
//            //                SetSpriteName(SpriteName.NO_IMAGE);
//            //                return;
//            //            }
//            //        }

//            //        SetSpriteName(SpriteNames[picIndex]);
//            //    }
//            //    else
//            //    {
//            //        if (picIndex < steamControllerIconCount)
//            //        {
//            //            // just show pic
//            //        }
//            //        else
//            //        {
//            //            if (SpriteNames.Count > 0)
//            //            {
//            //                currentlyShowNonSteam = true;
//            //                picIndex = 0;
//            //            }
//            //            else if (steamControllerIconCount > 0)
//            //            {
//            //                picIndex = 0;
//            //            }
//            //            else
//            //            {
//            //                SetSpriteName(SpriteName.NO_IMAGE);
//            //                return;
//            //            }
//            //        }

//            //        SetSpriteName(steamSpriteNames[picIndex]);
//            //    }
//            //}
//            //else
//            //{
//            if (SpriteNames != null && SpriteNames.Count > 0)
//            {
//                SetSpriteName(SpriteNames[0]);
//            }
//            else if (steamControllerIconCount > 0)
//            {
//                SetSpriteName(steamSpriteNames[0]);
//            }
//            else
//            {
//                SetSpriteName(SpriteName.NO_IMAGE);
//            }
//            //}
//        }
//    }

//    class BtnMapImage : Image
//    {
//        /* Fields */
//        ButtonActionType btn;

//        /* Constructors */
//        public BtnMapImage(ButtonActionType btn, Vector2 pos, Vector2 sz, ImageLayers layer, bool centerMidpoint, bool addToRender)//, bool rotatePics)
//            : base(mapSource, pos, sz, layer, centerMidpoint, addToRender)
//        {
//            this.btn = btn;
//            settingsChangedRefresh();
//        }

//        public BtnMapImage(PlayerInputMap mapSource, ButtonActionType btn, bool addToRender)//, bool rotatePics)
//            : base(mapSource, addToRender)
//        {
//            this.btn = btn;
//            settingsChangedRefresh();
//        }

//        /* Family methods */
//        protected override List<SpriteName> GetNonSteamIcons()
//        {
//            return mapSource.GetNonSteamIcons(btn);
//        }
//        protected override int UpdateSteamIcons(List<SpriteName> steamSpriteNames)
//        {
//            return mapSource.GetSteamIcons(btn, steamSpriteNames);
//        }
//    }

//    class DirMapImage : InputMapImage
//    {
//        /* Fields */
//        DirActionType dir;

//        /* Constructors */
//        public DirMapImage(PlayerInputMap mapSource, DirActionType dir, Vector2 pos, Vector2 sz, ImageLayers layer, bool centerMidpoint, bool addToRender)//, bool rotatePics)
//            : base(mapSource, pos, sz, layer, centerMidpoint, addToRender)//, rotatePics)
//        {
//            this.dir = dir;
//            settingsChangedRefresh();
//        }

//        public DirMapImage(PlayerInputMap mapSource, DirActionType dir, bool addToRender)//, bool rotatePics)
//            : base(mapSource, addToRender)//, rotatePics)
//        {
//            this.dir = dir;
//        }

//        /* Family methods */
//        protected override List<SpriteName> GetNonSteamIcons()
//        {
//            return mapSource.GetNonSteamIcons(dir);
//        }
//        protected override int UpdateSteamIcons(List<SpriteName> steamSpriteNames)
//        {
//            return mapSource.GetSteamIcons(dir, steamSpriteNames);
//        }
//    }
//}
