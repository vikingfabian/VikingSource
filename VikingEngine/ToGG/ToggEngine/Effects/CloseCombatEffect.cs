using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class CloseCombatEffect :AbsUpdateable
    {
        const float LifeTime = 360;
        const float FadeSpeed = 0.2f / LifeTime;
        const float MoveSpeed = 0.004f;
        const float GrowSpeed = 0.06f;

        Graphics.Mesh model;
        Time lifeTime = new Time(LifeTime);
        Vector2 attackDir;

        public CloseCombatEffect(IntVector2 attacker, IntVector2 Defender)
            : base(true)
        {
            model = new Graphics.Mesh(LoadedMesh.plane, 
                toggRef.board.toWorldPos_Center(attacker, 0.2f), new Vector3(0.2f),
                Graphics.TextureEffectType.Flat, SpriteName.cmdCCAttack, Color.White);
            //model.Y = 0.2f;

             attackDir = (Defender - attacker).Vec;
            model.Rotation = toggLib.Rotation1DToQuaterion(lib.V2ToAngle(attackDir));
            attackDir.Normalize();
        }

        public override void Time_Update(float time)
        {
            if (lifeTime.CountDown())
            {
                DeleteMe();
                return;
            }

            model.Opacity -= FadeSpeed * time;
            model.Scale += new Vector3(GrowSpeed);

            model.X += MoveSpeed * time * attackDir.X;
            model.Z += MoveSpeed * time * attackDir.Y;

        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}
