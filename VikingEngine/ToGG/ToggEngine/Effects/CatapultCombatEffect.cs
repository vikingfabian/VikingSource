using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Effects
{
    class CatapultCombatEffect : AbsUpdateable
    {
        const float MoveSpeed = 0.006f;
        Graphics.Mesh model;
        Vector2 attackDir;
        Vector3 goalPos;

        public CatapultCombatEffect(AbsUnit attacker, IntVector2 targetPos)
            : base(true)
        {
            model = new Graphics.Mesh(LoadedMesh.plane, attacker.soldierModel.Position, new Vector3(1.2f),
                Graphics.TextureEffectType.Flat, SpriteName.cmdArrowAttack, Color.White);
            model.Y = 0.2f;

            attackDir = (targetPos - attacker.squarePos).Vec;

            goalPos = toggLib.ToV3(targetPos, model.Y);

            model.Rotation = toggLib.Rotation1DToQuaterion(lib.V2ToAngle(attackDir));
            attackDir.Normalize();
        }

        public override void Time_Update(float time)
        {
            model.X += MoveSpeed * time * attackDir.X;
            model.Z += MoveSpeed * time * attackDir.Y;

            if ((model.Position - goalPos).Length() <= 0.4f)
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
