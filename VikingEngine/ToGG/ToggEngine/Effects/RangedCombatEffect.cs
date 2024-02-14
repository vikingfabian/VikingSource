using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class RangedCombatEffect : AbsUpdateable
    {
        const float MoveSpeed = 0.006f;
        Graphics.Mesh model;
        Vector2 attackDir;
        Vector3 goalPos;

        public RangedCombatEffect(IntVector2 attacker, IntVector2 Defender)
            : base(true)
        {
            model = new Graphics.Mesh(LoadedMesh.plane, 
                toggRef.board.toWorldPos_Center(attacker, 0.2f), new Vector3(0.6f),
                Graphics.TextureEffectType.Flat, SpriteName.cmdArrowAttack, Color.White);
            //model.Y = 0.2f;

             attackDir = (Defender - attacker).Vec;

             goalPos = toggRef.board.toWorldPos_Center(Defender, model.Y);

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
