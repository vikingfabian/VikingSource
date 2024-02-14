using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameObjects.PickUp
{
    
    class VoxelObjPresent : AbsHeroPickUp
    {
        ushort fileIndex;

        public VoxelObjPresent(Vector3 position, Rotation1D dir, int index)
            : base(position)
        {
            this.fileIndex = (ushort)index;
            position += Map.WorldPosition.V2toV3(dir.Direction(5));
            position.Y += 2;

            image.position = position;
            Velocity  = new Velocity( dir, Effects.EffectLib.PickUpStartSideSpeed.GetRandom());

            NetworkShareObject();
        }

        byte sender;
        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write(fileIndex);
        }
        public VoxelObjPresent(System.IO.BinaryReader r, byte sender)
            : base(r)
        {
            this.sender = sender;
            fileIndex = System.IO.BinaryReader.ReadUInt16();
        }

        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.present; }
        }
        
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Red, new Vector3(1.5f));
        protected override Data.TempBlockReplacementSett tempImage
        {
            get { return TempImage; }
        }

        
        public override int UnderType
        {
            get { return (int)PickUpType.VoxelObjPresent; }
        }

        override protected void heroPickUp(Characters.Hero hero)
        {
            if (!localMember)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_AcceptVoxelObjPacket, 
                     Network.SendPacketTo.OneSpecific, sender, Network.PacketReliability.ReliableLasy, Ref.netSession.LocalHostIndex);
                w.Write(fileIndex);
            }
            //hero.PickUpCollect(item);
        }

        const float PickUpLifeTime = 6000;
        override protected float pickUpLifeTime
        {
            get { return PickUpLifeTime; }
        }
        override protected Gadgets.IGadget item
        { get { throw new NotImplementedException(); } }
    }
}
