using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    /// <summary>
    /// A cirkular explosion shoots out and kills enemies, when the boss dies
    /// </summary>
    class BossDeathExplosion : GO.AbsNoImageObj
    {
        Vector3 center;
        Graphics.Mesh[] cubes;
        float radius = 0;
        new float angleDiff;
        float startAngle;

        public BossDeathExplosion(GO.GoArgs args)
            :base(args)
        {
            this.center = args.startPos;
            int cubeCount;
            switch (Ref.gamesett.DetailLevel)
            {
                default://case 0:
                    cubeCount = 8;
                    break;
                case 1:
                    cubeCount = 16;
                    break;
                case 2:
                    cubeCount = 32;
                    break;
            }

            cubes = new Graphics.Mesh[cubeCount];


            //var texEffect = new Graphics.TextureEffect( Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea);
            angleDiff = MathHelper.TwoPi / cubes.Length;
            startAngle = Ref.rnd.Float(angleDiff);
            
            for (int i = 0; i < cubes.Length; ++i)
            {
                cubes[i] = new Graphics.Mesh(LoadedMesh.cube_repeating, center,new Vector3(0.2f),
                    Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea, Color.White);
                cubes[i].Rotation.RotateWorldX(startAngle + angleDiff * i);
            }

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void Time_Update(GO.UpdateArgs args)
        {
            base.Time_Update(args);
            if (radius < 24)
            {
                radius += 31f * Ref.DeltaTimeSec;

                Rotation1D dir = new Rotation1D(startAngle);
                for (int i = 0; i < cubes.Length; ++i)
                {
                    cubes[i].Position = center + VectorExt.V2toV3XZ(dir.Direction(radius));
                    if (Ref.TimePassed16ms)
                    {
                        Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.BulletTrace, cubes[i].Position);
                    }
                    dir.Add(angleDiff);
                }
            }
            else
            {
                DeleteMe();
            }
        }

        List<GO.AbsUpdateObj> affectedGOs = new List<GO.AbsUpdateObj>(16);
        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            //base.AsynchGOUpdate(args);
            args.localMembersCounter.Reset();
            while (args.localMembersCounter.Next())
            {
                if (VectorExt.V3XZtoV2(args.localMembersCounter.GetSelection.Position - center).Length() <= radius)
                {
                    if (!affectedGOs.Contains(args.localMembersCounter.GetSelection))
                    {
                        affectedGOs.Add(args.localMembersCounter.GetSelection);
                        if (args.localMembersCounter.GetSelection is GO.Characters.AbsEnemy &&
                            args.localMembersCounter.GetSelection.Alive)
                        {
                            new Process.UnthreadedDamage(
                                new GO.WeaponAttack.DamageData(3,
                                GO.WeaponAttack.WeaponUserType.Friendly, NetworkId.Empty, 
                                GO.Magic.MagicElement.NoMagic, GO.WeaponAttack.SpecialDamage.IgnoreShield, false),
                                args.localMembersCounter.GetSelection);
                        }
                        args.localMembersCounter.GetSelection.Force(center, 1f);
                    }
                }
            }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            for (int i = 0; i < cubes.Length; ++i)
            {
                cubes[i].DeleteMe();
            }
        }

        public override GO.GameObjectType Type
        {
            get { return GO.GameObjectType.BossDeathExplosion; }
        }

        public override GO.NetworkShare NetworkShareSettings
        {
            get
            {
                return GO.NetworkShare.OnlyCreation;
            }
        }
    }
}
