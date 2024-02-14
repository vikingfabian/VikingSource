using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.ToggEngine.GO
{
    class Lever : AbsTileObject
    {
        const float Scale = 0.8f;
        const float StickAngle = MathExt.TauOver8 * 0.8f;
        Graphics.Mesh baseModel, cog, stick, statusFlag;
        Vector3 statusFlagRelPos;

        public Lever(IntVector2 pos, object args)
           : base(pos)
        {
            InteractSettings = new InteractSettings(SpriteName.cmdPullLever, "Pull lever",
                 InteractType.SendActivation, 0, true, true);
            if (args != null)
            {
                InteractSettings.interactId = (int)args;
            }
            
            baseModel = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, 
                new Vector3(1f, 1f, 0.5f) * Scale,
                Graphics.TextureEffectType.Flat, SpriteName.hqLeverBase, Color.White);

            stick = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero,
                new Vector3(Scale),
                Graphics.TextureEffectType.Flat, SpriteName.hqLeverStick, Color.White);
            stick.Rotation = toggLib.PlaneTowardsCamWithRotation(-StickAngle);//.PlaneTowardsCam;

            cog = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(Scale * 0.5f),
                Graphics.TextureEffectType.FlatNoOpacity, SpriteName.hqLeverCog, Color.White);
            cog.Rotation = toggLib.PlaneTowardsCam;

            statusFlag = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(Scale * 0.16f),
                Graphics.TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles, Color.DarkRed);
            statusFlag.Rotation = toggLib.PlaneTowardsCam;
            statusFlagRelPos = new Vector3(Scale * 0.16f, 0.1f * statusFlag.ScaleZ, Scale * -0.13f);

            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            baseModel.Position = toggRef.board.toModelCenter(newpos, 0.04f * Scale);
            stick.position = baseModel.Position;
            stick.position.Y -= Scale * 0.03f;
            stick.position.Z += Scale * 0.11f;

            statusFlag.Position = baseModel.Position + statusFlagRelPos;

            cog.Position = baseModel.Position;
            cog.X += Scale * 0.06f;
            cog.Y -= Scale * 0.03f;
            cog.Z += Scale * -0.02f;
        }
        
        public override bool HasOverridingMoveRestriction(AbsUnit unit,
            out ToGG.ToggEngine.Map.MovementRestrictionType restrictionType)
        {
            restrictionType = ToGG.ToggEngine.Map.MovementRestrictionType.Impassable;
            return true;
        }

        public override void AddToUnitCard(UnitCardDisplay card, ref Vector2 position)
        {
            card.startSegment(ref position);
            card.portrait(ref position, SpriteName.cmdPullLever, "Lever", true, 0.6f);
            if (!InteractSettings.activationState)
            {
                card.propertyBox(ref position, new ButtonOffProperty());
            }
        }

        public override bool canInteractWithObj(HeroQuest.Unit unit)
        {
            return !InteractSettings.activationState && unit.InInteractRange(position);
        }

        //public override void interactToolTip(out SpriteName icon, out string text)
        //{
        //    icon = SpriteName.cmdPullLever;
        //    text = "Pull lever";
        //}

        public override void interactEvent(AbsUnit unit)
        {
            InteractSettings.activationState = true;
            stick.Rotation = toggLib.PlaneTowardsCamWithRotation(StickAngle);

            statusFlagRelPos.X *= -1f;
            statusFlag.Color = Color.DarkGreen;
            statusFlag.Position = baseModel.Position + statusFlagRelPos;
        }

        public override void Write(System.IO.BinaryWriter w)
        {
            base.Write(w);
            InteractSettings.WriteInteractData(w);
        }
        public override void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            base.Read(r, version);
            InteractSettings.ReadInteractData(r, version);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            baseModel.DeleteMe();
            stick.DeleteMe();
        }

        public override string ToString()
        {
            return base.ToString() + InteractSettings.InteractStateString();
        }

        //public override InteractType interactType => InteractType.SendActivation;

        override public bool IsTileFillingUnit => true;

        public override TileObjectType Type => TileObjectType.Lever;
    }

    class ButtonOffProperty : ToGG.Data.Property.AbsProperty
    {
        public ButtonOffProperty()
        {
        }

        public override string Name => "Off";

        public override string Desc => "Stand adjacent to interact with it. Will end the movement.";
    }
}
