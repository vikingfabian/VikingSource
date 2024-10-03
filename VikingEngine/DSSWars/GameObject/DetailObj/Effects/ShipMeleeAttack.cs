using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{
    class ShipMeleeAttack:AbsInGameUpdateable
    {
        Graphics.AbsVoxelObj model;
        AbsSoldierUnit ship;
        Vector3 posDiff;
        public ShipMeleeAttack(AbsSoldierUnit ship, Rotation1D dir)
            :base(true)
        {
            this.ship = ship;

            int frame = Ref.rnd.Int(2);
            Vector3 offset = new Vector3(-0.076f, 0.14f, 0.07f);
            offset.Y -= 0.03f;
            offset.X -= 0.08f;
            
            if (dir.radians < MathExt.TauOver2)
            {
                offset.X = -offset.X;
                frame += 2;
            }

            posDiff = ship.SoldierProfile().modelScale * offset;

            model = DssRef.models.ModelInstance(LootFest.VoxelModelName.wars_shipmelee, DssConst.Men_StandardModelScale * 2f, false);
            model.Frame = frame;
            model.AddToRender(DrawGame.UnitDetailLayer);
        }

        public override void Time_Update(float time_ms)
        {
            WP.Rotation1DToQuaterion(model, ship.rotation.Radians);
            model.position = model.Rotation.TranslateAlongAxis(
                posDiff, ship.position);

            if (!ship.inAttackAnimation() || ship.isDeleted)
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
