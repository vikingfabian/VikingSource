using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Conscript
{
    static class GodConscript
    {
        public static void ToHud(RichBoxContent content, Action<int> addSoldier)
        {
            var weapons_groups = ConscriptDataLib.AllConstriptWeapons();
            foreach (var group in weapons_groups)
            {
                content.newLine();
                foreach (var item in group)
                {
                    IconName.Item(item, out var icon, out var name);
                    content.Add(new ArtToggle(item == BattleLabStorage.Singleton.setup.conscript.weapon, new List<AbsRichBoxMember> { new RbImage(icon) },
                        new RbAction1Arg<ItemResourceType>(selectWeapon, item, RbSoundType.Option), new RbTooltip_Text(name)));
                }
            }

            content.newParagraph();

            var shields = BattleLabStorage.Singleton.setup.conscript.AvailableShields();
            foreach (var item in shields)
            {
                IconName.Item(item, out var icon, out var name);
                content.Add(new ArtToggle(item == BattleLabStorage.Singleton.setup.conscript.shield, new List<AbsRichBoxMember> { new RbImage(icon) },
                    new RbAction1Arg<ItemResourceType>(selectShield, item, RbSoundType.Option), new RbTooltip_Text(name)));
            }

            content.newParagraph();

            foreach (var item in ConscriptDataLib.ArmorOptions)
            {
                IconName.Item(item, out var icon, out var name);
                content.Add(new ArtToggle(item == BattleLabStorage.Singleton.setup.conscript.armorLevel, new List<AbsRichBoxMember> { new RbImage(icon) },
                    new RbAction1Arg<ItemResourceType>(selectArmor, item, RbSoundType.Option), new RbTooltip_Text(name)));
            }
            content.newParagraph();

            var animals = ConscriptDataLib.AnimalTypes;
            foreach (var item in animals)
            {
                IconName.Item(item, out var icon, out var name);
                content.Add(new ArtToggle(item == BattleLabStorage.Singleton.setup.conscript.animal, new List<AbsRichBoxMember> { new RbImage(icon) },
                    new RbAction1Arg<ItemResourceType>(selectAnimal, item, RbSoundType.Option), new RbTooltip_Text(name)));
            }

            content.newParagraph();

            var animalArmors = BattleLabStorage.Singleton.setup.conscript.AvailableAnimalArmor();
            if (animalArmors != null)
            {
                foreach (var item in animalArmors)
                {
                    IconName.Item(item, out var icon, out var name);
                    content.Add(new ArtToggle(item == BattleLabStorage.Singleton.setup.conscript.mountArmor, new List<AbsRichBoxMember> { new RbImage(icon) },
                        new RbAction1Arg<ItemResourceType>(selectAnimalArmor, item, RbSoundType.Option), new RbTooltip_Text(name)));
                }

                content.newParagraph();
            }

            var wagons = BattleLabStorage.Singleton.setup.conscript.AvailableWagons();
            if (wagons != null)
            {
                foreach (var item in wagons)
                {
                    IconName.Item(item, out var icon, out var name);
                    content.Add(new ArtToggle(item == BattleLabStorage.Singleton.setup.conscript.vehicle, new List<AbsRichBoxMember> { new RbImage(icon) },
                        new RbAction1Arg<ItemResourceType>(selectVehicle, item, RbSoundType.Option), new RbTooltip_Text(name)));
                }

                content.newParagraph();
            }

            content.Add(new ArtButton( RbButtonStyle.GodPower,new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.Hud_AddX, 1), HudLib.GodPower_Color) }, new RbAction1Arg<int>(addSoldier, 1), null, true));
            {
                const int AddCount = 5;
                content.Add(new ArtButton(  RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.Hud_XTimes, AddCount), HudLib.GodPower_Color) }, new RbAction1Arg<int>(addSoldier, AddCount), null, true));
            }
            {
                const int AddCount = 20;
                content.Add(new ArtButton( RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText(string.Format(DssRef.lang.Hud_XTimes, AddCount), HudLib.GodPower_Color) }, new RbAction1Arg<int>(addSoldier, AddCount), null, true));
            }

            void selectWeapon(ItemResourceType item)
            {
                BattleLabStorage.Singleton.setup.conscript.weapon = item;
            }
            void selectShield(ItemResourceType item)
            {
                BattleLabStorage.Singleton.setup.conscript.shield = item;
            }
            void selectArmor(ItemResourceType item)
            {
                BattleLabStorage.Singleton.setup.conscript.armorLevel = item;
            }
            void selectAnimal(ItemResourceType item)
            {
                BattleLabStorage.Singleton.setup.conscript.animal = item;
            }
            void selectAnimalArmor(ItemResourceType item)
            {
                BattleLabStorage.Singleton.setup.conscript.mountArmor = item;
            }
            void selectVehicle(ItemResourceType item)
            {
                BattleLabStorage.Singleton.setup.conscript.vehicle = item;
            }
        }
    }
}
