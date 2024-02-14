﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2
{
    interface ICompassIconLocation
    {
        Vector2 CompassIconLocation { get; }
        SpriteName CompassIcon { get; }
        bool CompassIconVisible { get; }
    }

    abstract class AbsCompassIcon
    {
        const float FarScale = 0.5f;
        protected const float FullScale = 0.9f;//0.8f;

        public bool removeFlag;
        protected Image image;
        public Vector2 Position { get { return image.Position; } }
        protected float adjustedScale = 1;

        virtual public void Update(Players.Player player, float compassRadius, Rotation1D compassRotation, Vector2 compassPos)
        {
            const float CloseRadius = Map.WorldPosition.ChunkWidth * 1.6f;
            const float FarRadius = Map.WorldPosition.ChunkWidth * 10;
            
            const float FarScaleOff = 1 - FarScale;

            Vector2 diff = posDiff(player);
            float length = diff.Length();
            image.Visible = visible && visualRange.IsWithinRange(length);

            if (image.Visible)
            {

                float percentFromCenter = 1;
                float scale = compassRadius * FullScale;
                if (length <= CloseRadius)
                {
                    percentFromCenter = lib.SetMaxFloatVal(length / CloseRadius, 1);
                }
                else
                {
                    float offCompass = Bound.Min(1 - (length - CloseRadius) / FarRadius, 0);
                    scale *= FarScale + FarScaleOff * offCompass;
                }

                Rotation1D rot = Rotation1D.FromDirection(diff);
                rot.Add(compassRotation);
                if (useRotation)
                {
                    image.Rotation = rotation.Add(compassRotation).Radians;
                }
                image.Position = compassPos + rot.Direction(percentFromCenter * compassRadius);
                image.Size = new Vector2(scale * adjustedScale);
            }
        }
        protected void createImage(SpriteName icon, int iconIndex)
        {
            image = new Image(icon, Vector2.Zero, Vector2.One, ImageLayers.Background3, true);
            image.PaintLayer += PublicConstants.LayerMinDiff * iconIndex;
        }
        public void DeleteMe()
        {
            image.DeleteMe();
        }
        abstract protected bool useRotation { get; }
        virtual protected Rotation1D rotation { get { throw new NotImplementedException(); } }
        abstract protected Vector2 posDiff(Players.Player player);
        abstract protected IntervalF visualRange { get; }
        virtual protected bool visible { get { return true; } }
    }

    class CompassIconCraftsMan : AbsCompassIcon
    {
        public const float MaxDistance = 140;
        GameObjects.NPC.AbsNPC craftsMan;

        public CompassIconCraftsMan(GameObjects.NPC.AbsNPC craftsMan, int iconIndex)
            : base()
        {
            switch (craftsMan.CompassIcon)
            {
                case GameObjects.NPC.Worker.WeaponSmithIcon:
                    adjustedScale = 0.6f;
                    break;
                case GameObjects.NPC.Worker.BlackSmithIcon:
                    adjustedScale = 0.8f;
                    break;
                case GameObjects.NPC.Worker.VolcanSmithIcon:
                    adjustedScale = 0.7f;
                    break;
                case GameObjects.NPC.Worker.PriestIcon:
                    adjustedScale = 0.8f;
                    break;
                case GameObjects.NPC.Worker.WiseLadyIcon:
                    adjustedScale = 0.6f;
                    break;
                case GameObjects.NPC.Healer.HealerCompassIcon:
                    adjustedScale = 0.55f;
                    break;
                case GameObjects.NPC.Worker.CookIcon:
                    adjustedScale = 0.7f;
                    break;
                case GameObjects.NPC.Worker.BankerIcon:
                    adjustedScale = 0.6f;
                    break;
                case GameObjects.NPC.Builder.BuilderCompassIcon:
                    adjustedScale = 0.7f;
                    break;
                case SpriteName.IconEggNest:
                    adjustedScale = 0.6f;
                    break;
                case SpriteName.IconEggNestDestroyed: goto case SpriteName.IconEggNest;

            }
            this.craftsMan = craftsMan;
            createImage(craftsMan.CompassIcon, iconIndex);
        }

        

        protected override bool useRotation
        {
            get { return false; }
        }
        protected override Vector2 posDiff(Players.Player player)
        {
            return player.hero.PositionDiff(craftsMan);
        }
        static readonly IntervalF VisualRange = new IntervalF(0, 92); 
        protected override IntervalF visualRange
        {
            get { return VisualRange; }
        }
    }
    class CompassIconGamer : AbsCompassIcon
    {
        Players.AbsPlayer gamer;
        
        public CompassIconGamer(Players.AbsPlayer gamer, int iconIndex)
            : base()
        {
            adjustedScale = 0.6f;

            this.gamer = gamer;
            image = new GamerPictureInstance(gamer.gamerPicture, Vector2.Zero, Vector2.One, 
                ImageLayers.Background3);
            image.PaintLayer += PublicConstants.LayerMinDiff * iconIndex;
           
        }

        protected override bool useRotation
        {
            get { return true; }
        }
        protected override Vector2 posDiff(Players.Player player)
        {
            return player.hero.PositionDiff(gamer.hero);
        }
        protected override Rotation1D rotation
        {
            get { return gamer.hero.Rotation; }
        }
        static readonly IntervalF VisualRange = new IntervalF(0, float.MaxValue);
        protected override IntervalF visualRange
        {
            get { return VisualRange; }
        }
    }

    class CompassIconMapLocation : AbsCompassIcon
    {
        Vector2 position;
        bool alwaysVisible;
        public CompassIconMapLocation(IMiniMapLocation l, int iconIndex)
            : base()
        {
         //   Debug.DebugLib.Print(Debug.PrintCathegoryType.Output, "++new CompassIconMapLocation");
            alwaysVisible = l.MiniMapIcon == SpriteName.IconEggNest || l.MiniMapIcon == SpriteName.IconEndBossTomb;
            createImage(l.MiniMapIcon, iconIndex);
            position = l.MapLocationChunk.Vec * Map.WorldPosition.ChunkWidth;
        }

        protected override bool useRotation
        {
            get { return false; }
        }
        protected override Vector2 posDiff(Players.Player player)
        {
            return position - player.hero.PlanePos;
        }

        const float MaxViewRange = Map.WorldPosition.ChunkWidth * 30;
        static readonly IntervalF FullVisualRange = new IntervalF(0, MaxViewRange);
        static readonly IntervalF LimitedVisualRange = new IntervalF(CompassIconCraftsMan.MaxDistance, MaxViewRange);
        protected override IntervalF visualRange
        {
            get {
                if (alwaysVisible)
                    return FullVisualRange;
                else
                    return LimitedVisualRange;
            }
        }
    }

    class CompassGoalLocation : AbsCompassIcon
    {
        ICompassIconLocation location;
        public CompassGoalLocation(ICompassIconLocation location, int iconIndex)
            : base()
        {
            createImage(location.CompassIcon, iconIndex);
            this.location = location;
        }

        protected override bool useRotation
        {
            get { return false; }
        }
        protected override Vector2 posDiff(Players.Player player)
        {
            return location.CompassIconLocation - player.hero.PlanePos;
        }
        static readonly IntervalF VisualRange = new IntervalF(0, float.MaxValue);
        protected override IntervalF visualRange
        {
            get { return VisualRange; }
        }

        protected override bool visible
        {
            get
            {
                return location.CompassIconVisible;
            }
        }
    }
}
