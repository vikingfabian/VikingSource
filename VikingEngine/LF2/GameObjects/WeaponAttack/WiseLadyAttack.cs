using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    /// <summary>
    /// Will always hit, turning the target into a chicken
    /// </summary>
    class WiseLadyAttack : AbsVoxelObj
    {
        GameObjects.Characters.AbsCharacter target;

        public WiseLadyAttack(Vector3 startPos, GameObjects.Characters.AbsCharacter target)
        {
            this.target = target;
            imageSetup(startPos);
            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            WritePosition(image.position, w);
            target.ObjToNetPacket(w);
        }

        public WiseLadyAttack(System.IO.BinaryReader r)
            :base(r)
        {
            Vector3 startPos = ReadPosition(r);
            target = LfRef.gamestate.GameObjCollection.GetActiveOrClientObjFromIndex(r) as GameObjects.Characters.AbsCharacter;

            imageSetup(startPos);
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (target != null && target.Alive)
            {

                Vector3 diff = PositionDiff3D(target);
                float l = diff.Length();
                if (l > 1)
                {
                    const float Speed = 0.03f;
                    diff.Normalize();
                    image.position += args.time * Speed * diff;
                }
                else
                {
                    image.position = target.Position;
                    //hit the target
                    if (localMember)
                    {
                        target.DeleteMe();
                        new GameObjects.Characters.Critter(Characters.CharacterUtype.CritterWhiteHen, new Map.WorldPosition(target.Position));//target.WorldPosition);
                    }
                    DeleteMe();
                }
            }
            else
            {
                DeleteMe();
            }
        }

        public override void DeleteMe(bool local)
        {
            if (image.InCameraView)
            {
                for (int i = 0; i < 8; ++i)
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.LightSparks,
                        new Graphics.ParticleInitData(image.position, lib.RandomV3(Vector3.Zero, 4)));
                }
            }
            base.DeleteMe(local);
        }

        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(114,209,253), new Vector3(1.3f));
        
        protected void imageSetup(Vector3 startPos)
        {
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.witch_magic, TempImage, 0.8f, 0);
            image.position = startPos;
        }


        public override ObjectType Type
        {
            get { return ObjectType.WeaponAttack; }
        }
        public override int UnderType
        {
            get { return (int)WeaponUtype.WiseLadyAttack; }
        }

        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.MagicLight;
            }
        }
        public override float LightSourceRadius
        {
            get
            {
                return 2.5f;
            }
        }
    }
}
