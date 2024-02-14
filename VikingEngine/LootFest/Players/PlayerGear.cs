using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest.Players
{
    /// <summary>
    /// A manager class for what kind of suit and gear the player is wearing
    /// </summary>
    class PlayerGearManager
    {
        public List<PlayerGearSetup> setupStack;

        public PlayerGearManager(Player player)
        {
            setupStack = new List<PlayerGearSetup>
            {
                new  PlayerGearSetup(player)
            };
        }
    }

    class PlayerGearSetup
    {
        Player localPlayer;
        AbsPlayer player;

        public GO.AbsSuit suit;
        public GO.Gadgets.Item itemSlot;

        int specAttAmmo = 0;

        public PlayerGearSetup(AbsPlayer player)
        {
            this.player = player;
            localPlayer = player as Player;
            suit = new VikingEngine.LootFest.GO.BasicSuit(player);
        }


        public int SpecialAttackAmmo 
        { 
            get { return specAttAmmo; } 
            set 
            { 
                specAttAmmo = value;
                if (localPlayer.statusDisplay != null)
                { localPlayer.statusDisplay.specialAttackHUD.UpdateAmount(value, suit.MaxSpecialAmmo); }
            } 
        }

        //int mainAttAmmo = 20;
        //public int MainAttackAmmo
        //{
        //    get { return mainAttAmmo; }
        //    set { mainAttAmmo = value; player.statusDisplay.primaryAttackHUD.UpdateAmount(value, suit.MaxMainAmmo); }
        //}

        public void dressInSuit(SuitType type)
        {
            if (suit != null)
                suit.DeleteMe();

            switch (type)
            {
                case SuitType.Basic:
                    suit = new BasicSuit(this.player);
                    break;

                case SuitType.Swordsman:
                    suit = new SwordsmanSuit(this.player);
                    break;
                case SuitType.Archer:
                    suit = new ArcherSuit(this.player);
                    break;
                case SuitType.BarbarianDane:
                    suit = new BarbarianDaneSuit(this.player);
                    break;
                case SuitType.BarbarianDual:
                    suit = new BarbarianDualSuit(this.player);
                    break;
                case SuitType.SpearMan:
                    suit = new SpearManSuit(this.player);
                    break;
                case SuitType.ShapeShifter:
                    suit = new ShapeShifter(this.player);
                    break;
                case SuitType.FutureSuit:
                    suit = new FutureSuit(this.player);
                    break;
                case SuitType.Emo:
                    suit = new EmoSuit(this.player);
                    break;
                default:
                    throw new NotImplementedException("Cant dress in suit: " + type.ToString());
            }

            if (localPlayer != null)
            {
                player.hero.UpdateAppearance(true);
                if (localPlayer.statusDisplay != null)
                {
                    localPlayer.statusDisplay.onNewSuit(suit);
                }
            }
        }

        public void onNewHero()
        {
            //refresh suit
            dressInSuit(suit.Type);
        }
    }
}
