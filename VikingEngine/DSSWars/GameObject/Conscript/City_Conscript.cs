using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{

    /*
     * conscription är bunden till barracks
     * varje barrack tränar 1 grupp åt gången, välj 0-5 eller unlimited i kö, välj träning profil och slutmål
     * varje stad väljer conscription (och till vilken stad)
     */

    //partial class City
    //{
    //    void conscriptTab(LocalPlayer player, RichBoxContent content)
    //    { 
    //        new ConscriptMenu().ToHud(player, content);
    //    }
    //}

    class ConscriptMenu
    {
        LocalPlayer player;
        ConsriptProfile currentProfile = new ConsriptProfile();

        public void ToHud(LocalPlayer player, RichBoxContent content)
        {
            this.player = player;

            content.h1(DssRef.todoLang.Hud_Conscription);

            content.newParagraph();

            HudLib.Label(content, "Weapon");
            content.newLine();
            for (MainWeapon weapon = 0; weapon < MainWeapon.NUM; weapon++)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember>{
                   new RichBoxText( LangLib.Weapon(weapon))
                }, new RbAction1Arg<MainWeapon> (weaponClick, weapon));
                button.setGroupSelectionColor(HudLib.RbSettings, weapon == currentProfile.weapon);
                content.Add(button);
                content.space();
            }

            content.newParagraph();

            HudLib.Label(content, "Armor");
            content.newLine();
            for (ArmorLevel armorLvl = 0; armorLvl < ArmorLevel.NUM; armorLvl++)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember>{
                   new RichBoxText( LangLib.Armor(armorLvl))
                }, new RbAction1Arg<ArmorLevel>(armorClick, armorLvl));
                button.setGroupSelectionColor(HudLib.RbSettings, armorLvl == currentProfile.armorLevel);
                content.Add(button);
                content.space();
            }

            content.newParagraph();

            HudLib.Label(content, "Training");
            content.newLine();
            for (Training training = 0; training < Training.NUM; training++)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember>{
                   new RichBoxText( LangLib.Training(training))
                }, new RbAction1Arg<Training>(trainingClick, training));
                button.setGroupSelectionColor(HudLib.RbSettings, training == currentProfile.training);
                content.Add(button);
                content.space();
            }

            content.newParagraph();

            HudLib.Label(content, "Que");
            content.space();
            HudLib.InfoButton(content, new RbAction(queInfo));
            content.newLine();
            for (int length = 0; length <= ConsriptProfile.MaxQue; length++)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember>{
                   new RichBoxText( length.ToString())
                }, new RbAction1Arg<int>(queClick, length));
                button.setGroupSelectionColor(HudLib.RbSettings, length == currentProfile.que);
                content.Add(button);
                content.space();
            }
            {
                var button = new RichboxButton(new List<AbsRichBoxMember>{
                   new RichBoxText( "No limit")
                }, new RbAction1Arg<int>(queClick, 1000));
                button.setGroupSelectionColor(HudLib.RbSettings, currentProfile.que > ConsriptProfile.MaxQue);
                content.Add(button);
            }

        }
        void weaponClick(MainWeapon weapon)
        {
            currentProfile.weapon = weapon;
        }
        void armorClick(ArmorLevel armor)
        {
            currentProfile.armorLevel = armor;
        }
        void trainingClick(Training training)
        {
            currentProfile.training = training;
        }
        void queClick(int length)
        {
            currentProfile.que = length;
        }

        void queInfo()
        { 
            RichBoxContent content = new RichBoxContent();

            content.text("Will keep traning soldiers until the que is empty");

            player.hud.tooltip.create(player, content, true);
        }
    }


    struct ConsriptProfile
    {
        public const int MaxQue = 5;

        public MainWeapon weapon;
        public ArmorLevel armorLevel;
        
        public Training training;
        public int city;
        public int que;
    }

    enum ArmorLevel
    { 
        None,
        Light,
        Medium,
        Heavy,
        NUM
    }

    enum MainWeapon
    { 
        SharpStick,
        Sword,
        Bow,
        NUM
    }

    enum Training
    { 
        Minimal,
        Basic,
        Skillful,
        Professional,
        NUM
    }
}
