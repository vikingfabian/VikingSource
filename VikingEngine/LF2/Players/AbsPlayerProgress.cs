using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//

namespace VikingEngine.LF2.Players
{
    abstract class AbsPlayerProgress
    {
        public Map.MiniMapGoal mapFlag = new Map.MiniMapGoal();
        protected GameObjects.Gadgets.WeaponGadget.Stick defaultWeapon;
        protected GameObjects.Gadgets.WeaponGadget.HandWeapon buildHammer;
        protected EquipSetup equipSetup;
        protected GameObjects.Gadgets.Shield shield = null;

        public AbsPlayerProgress()
        {
            defaultWeapon = new GameObjects.Gadgets.WeaponGadget.Stick();
            buildHammer = new GameObjects.Gadgets.WeaponGadget.HandWeapon(GameObjects.Gadgets.WeaponGadget.StandardHandWeaponType.BuildHammer);
            equipSetup = new EquipSetup(0, defaultWeapon, null);// { new EquipSetup(0, defaultWeapon), new EquipSetup(1, defaultWeapon), new EquipSetup(2, defaultWeapon) };
            
        }

        public void NetworkWriteEquipSetup(System.IO.BinaryWriter w)
        {
            equipSetup.NetworkSend(w);

            if (shield == null)
            {
                w.Write(byte.MaxValue);
            }
            else
            {
                w.Write((byte)shield.Type);
            }
            //w.Write(shield != null);
            //if (shield != null)
            //    shield.WriteStream(w);
        }

        public void NetworkReadEquipSetup(System.IO.BinaryReader r)
        {
            equipSetup.NetworkRecieve(r);

            byte shieldType = r.ReadByte();
            if (shieldType == byte.MaxValue) 
                shield = null;
            else 
                shield = new GameObjects.Gadgets.Shield((GameObjects.Gadgets.ShieldType)shieldType);
            //bool hasShield = r.ReadBoolean();
            //if (hasShield)
            //{
            //    shield = new GameObjects.Gadgets.Shield(r, byte.MaxValue);
            //}
        }

        public void NetworkWriteMapFlag()
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_MapFlag, Network.PacketReliability.Reliable);
            mapFlag.WriteStream(w);
        }
        public void NetworkReadMapFlag(System.IO.BinaryReader r)
        {
            mapFlag.ReadStream(r);
        }

        public GameObjects.Gadgets.Shield Shield()
        {
            return shield;
        }
    }
    class ClientPlayerProgress : AbsPlayerProgress
    {
        public ClientPlayerProgress(System.IO.BinaryReader r)
            : base()
        {
            NetworkReadEquipSetup(r);
        }

        public void StartAttack(System.IO.BinaryReader r, GameObjects.Characters.Hero hero)
        {
            int fireButtonIx = r.ReadByte();
            equipSetup.ClientUse(fireButtonIx, hero);
        }
    }
}
