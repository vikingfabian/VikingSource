using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.GO
{    
    class Campfire : AbsTileObject
    {
        Graphics.Mesh model;
        CampfireEmitter fireEmitter = null;

        public Campfire(IntVector2 pos)
           : base(pos)
        {
            InteractSettings = new InteractSettings(
                SpriteName.cmdLightAction, "Light the fire",
                InteractType.ActivateItself, 0, true, true);
            InteractSettings.addedDesc = new List<HUD.RichBox.AbsRichBoxMember>{
                new HUD.RichBox.RbText(CampfireProperty.GainRest)
            };

            const float Width = 0.8f;
            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, 
                new Vector3(Width, Width, Width * 1.2f),
                Graphics.TextureEffectType.Flat, SpriteName.hqCampFireOff, Color.White);
            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            model.Position = toggRef.board.toModelCenter(newpos, 0.1f);
        }

        public override bool HasOverridingMoveRestriction(AbsUnit unit,
            out ToggEngine.Map.MovementRestrictionType restrictionType)
        {
            restrictionType = ToggEngine.Map.MovementRestrictionType.Impassable;
            return true;
        }

        public override void AddToUnitCard(UnitCardDisplay card, ref Vector2 position)
        {
            card.startSegment(ref position);
            card.portrait(ref position, SpriteName.hqCampFireOn, "Camp fire", true, 0.6f);
            card.propertyBox(ref position, new CampfireProperty(InteractSettings.activationState));
        }

        public override bool canInteractWithObj(HeroQuest.Unit unit)
        {
            return !InteractSettings.activationState && unit.InInteractRange(position);
        }

        public override void interactEvent(AbsUnit unit)
        {
            if (!InteractSettings.activationState)
            {
                InteractSettings.activationState = true;
                model.SetSpriteName(SpriteName.hqCampFireOn);
                toggRef.board.metaData.campfires.Add(this);

                fireEmitter = new CampfireEmitter(model.Position);

                foreach (var dir in IntVector2.Dir8Array)
                {
                    var u = toggRef.board.getUnit(position + dir);
                    if (u != null && u.hq().IsHero)
                    {
                        u.hq().data.hero.rest(u.hq());
                    }
                }
            }
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
            model.DeleteMe();
            base.DeleteMe();
        }

        public override string ToString()
        {
            return base.ToString() + InteractSettings.InteractStateString();
        }

        override public bool IsTileFillingUnit => true;

        public override TileObjectType Type => TileObjectType.Campfire;
    }

    class CampfireProperty : ToGG.Data.Property.AbsProperty
    {
        public const string GainRest = "All adjacent heroes will gain a Rest Action.";
        bool lit;

        public CampfireProperty(bool lit)
        {
            this.lit = lit;
        }

        public override SpriteName Icon => SpriteName.cmdLightAction;

        public override string Name => lit? "On" : "Off";

        public override string Desc
        {
            get
            {
                string text = "A respawn point for fallen heroes.";

                if (!lit)
                {
                    text = "Stand adjacent to lit the fire. " + GainRest + " Will end the movement." + 
                        Environment.NewLine +
                        "When lit: " + text +
                        Environment.NewLine +
                        HeroData.RestDesc();
                }

                return text;
            }
        }
    }

    class CampfireEmitter : ToggEngine.AbsParticleEmitter
    {
        public CampfireEmitter(Vector3 center)
            : base()
        {
            center.Y -= 0.05f;
            center.Z -= 0.06f;

            this.center = center;
        }

        public override void update()
        {
            Engine.ParticleHandler.AddParticleSphere(ParticleSystemType.TorchFire, center, 0.06f, 5);
            Engine.ParticleHandler.AddParticleSphere(ParticleSystemType.TorchFire, center, 0.10f, 1);
        }
    }
}
