using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Conscript
{
    struct BarracksStatus
    {
        public const int MaxQue = 5;

        public ConscriptActiveStatus active;
        public ConscriptProfile profile;

        public ConscriptProfile inProgress;
        public TimeInGameCountdown countdown;
        public BarracksType type;
        public int menCollected;
        public int equipmentCollected;

        public int idAndPosition;
        public int que;

        public BarracksStatus(BarracksType type)
            : this()
        {
            this.type = type;

            switch (type)
            {
                case BarracksType.Soldier:
                    profile.weapon = ItemResourceType.SharpStick;
                    break;
                case BarracksType.Archer:
                    profile.weapon = ItemResourceType.SlingShot;
                    break;
                case BarracksType.Warmashine:
                    profile.weapon = ItemResourceType.Ballista;
                    break;
                case BarracksType.Knight:
                    profile.weapon = ItemResourceType.Warhammer;
                    break;
                case BarracksType.Gun:
                    profile.weapon = ItemResourceType.HandCannon;
                    break;
            }
        }

        public void halt(City city)
        {
            que = 0;

            returnItems(city);

        }

        public void returnItems(City city)
        {
            if (active == ConscriptActiveStatus.CollectingEquipment ||
                    active == ConscriptActiveStatus.CollectingMen)
            {
                //return items
                ItemResourceType weaponItem = inProgress.weapon;
                ItemResourceType armorItem = inProgress.armorLevel;

                city.AddGroupedResource(weaponItem, equipmentCollected);

                if (inProgress.armorLevel != ItemResourceType.NONE)
                {
                    city.AddGroupedResource(armorItem, equipmentCollected);
                }

                city.workForce.amount += menCollected;

                active = ConscriptActiveStatus.Idle;

                //city.conscriptBuildings[selectedConscript] = status;
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)active);
            profile.writeGameState(w);
            if (active != ConscriptActiveStatus.Idle)
            {
                inProgress.writeGameState(w);
            }
            switch (active)
            {
                case ConscriptActiveStatus.CollectingEquipment:
                    w.Write((byte)equipmentCollected);
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    w.Write((byte)menCollected);
                    break;

                case ConscriptActiveStatus.Training:
                    countdown.writeGameState(w);
                    break;
            }
            w.Write((byte)type);
            w.Write(idAndPosition);
            w.Write((byte)que);
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            active = (ConscriptActiveStatus)r.ReadByte();
            profile.readGameState(r);
            if (active != ConscriptActiveStatus.Idle)
            {
                inProgress.readGameState(r);
            }
            switch (active)
            {
                case ConscriptActiveStatus.CollectingEquipment:
                    equipmentCollected = r.ReadByte();
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    equipmentCollected = DssConst.SoldierGroup_DefaultCount;
                    menCollected = r.ReadByte();
                    break;

                case ConscriptActiveStatus.Training:
                    equipmentCollected = DssConst.SoldierGroup_DefaultCount;
                    menCollected = DssConst.SoldierGroup_DefaultCount;
                    countdown.readGameState(r);
                    break;
            }
            if (subVersion >= 13 && subVersion < 40)
            {
                bool nobelmen = r.ReadBoolean();
            }

            if (subVersion >= 40)
            {
                type = (BarracksType)r.ReadByte();
            }
            idAndPosition = r.ReadInt32();
            que = r.ReadByte();
        }
        public bool CountDownQue()
        {
            if (que > 0)
            {
                if (que <= MaxQue)
                {
                    --que;
                }

                return true;
            }

            return false;
        }


        public TimeLength TimeLength()
        {
            return new TimeLength(ConscriptProfile.TrainingTime(inProgress.training, type));
        }

        public string activeStringOf(ConscriptActiveStatus status)
        {
            string result = null;


            switch (status)
            {
                case ConscriptActiveStatus.Idle:
                    result = DssRef.lang.Hud_Idle;
                    break;

                case ConscriptActiveStatus.CollectingEquipment:
                    {
                        var progress = string.Format(DssRef.lang.Language_CollectProgress, equipmentCollected, DssConst.SoldierGroup_DefaultCount);
                        result = string.Format(DssRef.lang.Conscription_Status_CollectingEquipment, progress);
                    }
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    {
                        var progress = string.Format(DssRef.lang.Language_CollectProgress, menCollected, DssConst.SoldierGroup_DefaultCount);
                        result = string.Format(DssRef.lang.Conscription_Status_CollectingMen, progress);
                    }
                    break;
            }

            return result;
        }

        public string shortActiveString()
        {
            string result = null;
            if (active == ConscriptActiveStatus.Training)
            {
                result = string.Format(DssRef.lang.Conscription_Status_Training, countdown.RemainingLength().ShortString());
            }
            else
            {
                result = activeStringOf(active) + ", " + string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_Queue, que <= MaxQue ? que.ToString() : DssRef.lang.Hud_NoLimit);
            }

            return result;
        }

        public string longTimeProgress()
        {
            string remaining;
            if (active == ConscriptActiveStatus.Training)
            {
                remaining = countdown.RemainingLength().LongString();
            }
            else
            {
                remaining = TimeLength().LongString();
            }
            return string.Format(DssRef.lang.Conscription_Status_Training, remaining);
        }

        public void tooltip(LocalPlayer player, City city, RichBoxContent content)
        {

            ItemResourceType weaponItem = profile.weapon;
            bool hasWeapons = city.GetGroupedResource(weaponItem).amount >= DssConst.SoldierGroup_DefaultCount;

            bool hasArmor = true;
            ItemResourceType armorItem = profile.armorLevel;
            if (profile.armorLevel != ItemResourceType.NONE)
            {
                //armorItem = ConscriptProfile.ArmorItem(profile.armorLevel);
                hasArmor = city.GetGroupedResource(armorItem).amount >= DssConst.SoldierGroup_DefaultCount;
            }

            bool hasMen = city.workForce.amount >= DssConst.SoldierGroup_DefaultCount;

            bool available = hasWeapons && hasArmor && hasMen;

            content.Add(new RichBoxImage(available ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));
            content.space(0.5f);
            content.Add(new RichBoxImage(
                            new SoldierConscriptProfile() { conscript = profile }.Icon()
                            ));
            //ItemResourceType weaponitem = ConscriptProfile.WeaponItem(profile.weapon);
            content.Add(new RichBoxImage(ResourceLib.Icon(weaponItem)));

            if (profile.armorLevel != ItemResourceType.NONE)
            {
                //ItemResourceType armoritem = ConscriptProfile.ArmorItem(profile.armorLevel);
                content.Add(new RichBoxImage(ResourceLib.Icon(armorItem)));
            }
            content.Add(new RichBoxImage((SpriteName)((int)SpriteName.WarsUnitLevelMinimal + (int)profile.training)));

            content.newLine();
            content.Add(new RichBoxImage(player.input.Stop.Icon));
            content.space(0.5f);
            content.Add(new RichBoxText(shortActiveString()));

            content.newLine();
            content.Add(new RichBoxImage(player.input.Copy.Icon));
            content.space(0.5f);
            content.Add(new RichBoxText(DssRef.lang.Hud_CopySetup));
            content.space(2);
            content.Add(new RichBoxImage(player.input.Paste.Icon));
            content.space(0.5f);
            content.Add(new RichBoxText(DssRef.lang.Hud_Paste));
        }
    }
}
