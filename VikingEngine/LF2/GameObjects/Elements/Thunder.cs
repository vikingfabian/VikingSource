using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2;

namespace VikingEngine.LF2.GameObjects.Elements
{
    class Thunder : AbsVoxelObj
    {
        ThunderState state = ThunderState.Waiting;
        int stateTime = 8;
        static readonly WeaponAttack.DamageData Damage = new WeaponAttack.DamageData(LootfestLib.ThunderStrikeDamage, 
            WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Lightning, 
            WeaponAttack.SpecialDamage.NONE, false);
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(169,218,255), new Vector3(0.5f, 6, 0.5f));

        public Thunder(Vector3 pos)
            :base()
        {
            const float DamageRadius = 6;
            CollisionBound = new LF2.ObjSingleBound(
               new BoundData2(new Physics.StaticBoxBound(
                    new VectorVolume(pos, new Vector3(DamageRadius, Map.WorldPosition.ChunkHeight * PublicConstants.Twice, DamageRadius))), Vector3.Zero));

            WorldPosition = new Map.WorldPosition(pos);
            pos.Y = LfRef.chunks.GetHighestYpos(WorldPosition) + 1;

            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.thunder, TempImage, 0, 0);
            image.position = pos;
            image.Visible = false;
            image.scale = Vector3.One * 1;
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            if (state == ThunderState.Damage)
            {
                characterCollCheck(args.allMembersCounter);
                //state++;
                stateTime = 0;
            }
        }
        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            if (character.WeaponTargetType != WeaponAttack.WeaponUserType.NON)
            {
                
                 //spread the lightning effect
               // new Magic.Lightning(character, null);
                new Process.UnthreadedDamage(Damage, character);
            }
            return false;
        }
        
        public override void Time_Update(UpdateArgs args)
        {
#if !CMODE
            //base.Time_Update(args);
            stateTime--;
            if (stateTime <= 0)
            {
                state++;
                switch (state)
                {
                    case ThunderState.Damage:
                        stateTime = int.MaxValue;
                        break;
                    case ThunderState.Visual:
                        Director.LightsAndShadows.Instance.AddLight(this, true);
                        image.Visible = true;
                        stateTime = 4;
                        //sparks
                        const float SideWaysSpeed = 4;
                        Vector3 position = image.position;
                        position.Y += 1;
                        Vector3 upspeed = Vector3.Up * 6;
                        for (int i = 0; i < 8; i++)
                        {
                            Vector3 speed = upspeed;
                            speed.X += Ref.rnd.Plus_MinusF(SideWaysSpeed);
                            speed.Z += Ref.rnd.Plus_MinusF(SideWaysSpeed);
                            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.LightSparks, new Graphics.ParticleInitData(
                                lib.RandomV3(image.position, 1), speed));
                        }
                        break;
                    case ThunderState.Remove:
                        DeleteMe();
                        break;
                }
            }
#endif
        }

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.NON;
            }
        }
        public override ObjectType Type
        {
            get { return ObjectType.Element; }
        }
        public override int UnderType
        {
            get { return (int)ElementType.Thunder; }
        }

        public override Graphics.LightSourcePrio LightSourcePrio
        {
            get
            {
                return Graphics.LightSourcePrio.VeryLow;
            }
        }
        public override float LightSourceRadius
        {
            get
            {
                return 20;//state == ThunderState.Visual? 20 : 0;
            }
        }
        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return state >= ThunderState.Visual? 
                    Graphics.LightParticleType.Lightning :
                     Graphics.LightParticleType.NUM_NON;
            }
        }
        protected override RecieveDamageType recieveDamageType
        {
            get { return RecieveDamageType.NoRecieving; }
        }

        enum ThunderState
        {
            Waiting,
            Damage,
            Visual,
            Remove,
        }
    }
}
