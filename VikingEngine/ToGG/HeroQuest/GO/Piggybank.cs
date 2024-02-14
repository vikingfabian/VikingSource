using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.ToGG.HeroQuest.GO
{
    class Piggybank : AbsTileObject
    {
        Graphics.Mesh model;
        Vector3 posOffset;
        public const int Cost = 20;

        public Piggybank(IntVector2 pos)
           : base(pos)
        {
            InteractSettings = new InteractSettings(
                 SpriteName.cmdCoin, "Put " + Cost.ToString() + " coins in the pig",
                 InteractType.ActivateItself, 0, false, false);
            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero,
               new Vector3(0.6f), Graphics.TextureEffectType.Flat,
              SpriteName.pigP1WingUp, Color.White);
            posOffset = new Vector3(-0.1f, model.Scale.Y * 0.3f, -0.1f);

            model.Rotation = toggLib.PlaneTowardsCam;
            newPosition(pos);
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            model.Position = toggRef.board.toWorldPos_Center(newpos, 0f) + posOffset;
        }

        public override bool HasOverridingMoveRestriction(AbsUnit unit,
            out  MovementRestrictionType restrictionType)
        {
            restrictionType = MovementRestrictionType.Impassable;
            return true;
        }

        public override void AddToUnitCard(UnitCardDisplay card, ref Vector2 position)
        {
            base.AddToUnitCard(card, ref position);

            card.portrait(ref position, SpriteName.pigP1WingUp, "Piggy bank", true, 1.2f);
            card.propertyBox(ref position, new PiggybankProperty());
        }

        public override bool canInteractWithObj(HeroQuest.Unit unit)
        {
            return true;//unit.PlayerHQ.Backpack.canPurchase(Cost);
        }
        
        public override void interactEvent(AbsUnit unit)
        {
            if (unit.hq().PlayerHQ.Backpack().purchase(Cost))
            {
                DeleteMe();
                TileObjLib.CreateObject(TileObjectType.ItemCollection, position,
                    new Gadgets.TileItemCollData(LootLevel.Level2), false);
                Gadgets.TileItemCollection.NetWrite(position);
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }

        public override TileObjectType Type => TileObjectType.Piggybank;

        override public bool IsTileFillingUnit => true;
    }

    class PiggybankProperty : ToGG.Data.Property.AbsProperty
    {
        public override string Name => "Bank " + Piggybank.Cost.ToString() + " coins";

        public override string Desc => "Put " + Piggybank.Cost.ToString() + " coins in the piggy to recieve a reward (Placeholder for a salesman)";
    }
}
