using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ.Bagatelle;
using VikingEngine.PJ.GameState;
using VikingEngine.ToGG.HeroQuest.Data.UnitAction;
using VikingEngine.ToGG.HeroQuest.GO;

namespace VikingEngine.DSSWars.Data
{
    struct MapObjectTag
    {
        public static ushort Tag_LevelMaster;
        public static ushort Tag_SpecializeTradition; //Specialize_Tradition
        public static ushort Tag_Faction;//IconFaction

        public static ushort Tag_ItemResourceTypeFood;
        public static ushort Tag_ItemResourceTypeRawFood;
        public static ushort Tag_ItemResourceTypeBow;
        public static ushort Tag_ItemResourceTypeLongBow;
        public static ushort Tag_ItemResourceTypeMithrilBow;

        public CityTagBack backType;
        public ushort artId;

        public MapObjectTag(CityTagBack back, ushort art)
        { 
            this.backType = back;
            this.artId = art;
        }

        public MapObjectTag()
        { }


        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)backType);
            if (backType != CityTagBack.NONE)
            {
                w.Write(artId);
            }
        }

        public void read(System.IO.BinaryReader r)/*, int subversion)*/
        {
            backType = (CityTagBack)r.ReadByte();
            if (backType != CityTagBack.NONE)
            {
                artId = r.ReadUInt16();
            }
        }

        public SpriteName TagBack()
        { 
            return TagLib.BackSprite(backType);
        }
        public SpriteName TagArt()
        {
            return TagLib.artTagDictionary[artId];
        }

        public void tagSprites(out SpriteName back, out SpriteName art)
        {
            back = TagBack();//Data.TagLib.BackSprite(tagBack);
            art = TagArt();//Data.TagLib.ArtSprite(tagArt);
        }
        public bool tagToHud(RichBoxContent content)
        {
            tagSprites(out SpriteName back, out SpriteName art);
            if (back != TagLib.NoBackSprite)
            {
                if (art == SpriteName.NO_IMAGE)
                {
                    content.Add(new RbImage(back));
                }
                else
                {
                    content.Add(new RbOverlapImage(
                        new RbImage(back),
                        art, Vector2.Zero, 0.8f));
                }
                return true;
            }

            return false;
        }
    }

    static class TagLib
    {
        public const SpriteName NoBackSprite = SpriteName.BluePrintSquareFull;
        public static int CityOnlyStart;
        public static SpriteName[] artTagDictionary;

        public static void Init()
        {
            SpriteName[] icons = {
                SpriteName.WarsHammer,
                SpriteName.WarsWorkMove,
                SpriteName.WarsWorkCollect,
                SpriteName.WarsWorkMine,
                SpriteName.WarsWorkCasting,
                SpriteName.WarsWorkFarm,
                SpriteName.WarsBedIcon,
                SpriteName.WarsMapIcon,
                SpriteName.WarsGovernmentIcon,
                SpriteName.unitEmoteThumbUp,
                SpriteName.unitEmoteThumbDown,
                SpriteName.unitEmoteLove,

                SpriteName.WarsSpecializeField,
                SpriteName.WarsSpecializeSea,
                SpriteName.WarsSpecializeSiege,
                SpriteName.WarsSpecializeTradition,
                SpriteName.WarsUnitLevelMinimal,
                SpriteName.WarsUnitLevelBasic,
                SpriteName.WarsUnitLevelSkillful,
                SpriteName.WarsUnitLevelProfessional,
                SpriteName.WarsUnitLevelMaster,
                SpriteName.WarsUnitLevelLegend,

                SpriteName.warsArmyTag_Lightning,
                SpriteName.warsArmyTag_Fire,
                SpriteName.warsArmyTag_Hit,
                SpriteName.warsArmyTag_HitExpress,
                SpriteName.warsArmyTag_Retreat,
                SpriteName.warsArmyTag_Return,
                SpriteName.warsArmyTag_Anchor,
                SpriteName.warsArmyTag_Shield,
                SpriteName.warsArmyTag_GoldShield,
                SpriteName.warsArmyTag_BrokenShield,

                SpriteName.WarsRelationAlly,
                SpriteName.WarsRelationGood,
                SpriteName.WarsRelationPeace,
                SpriteName.WarsRelationNeutral,
                SpriteName.WarsRelationTruce,
                SpriteName.WarsRelationEnemy,
                SpriteName.WarsRelationWar,
                SpriteName.WarsRelationTotalWar,

                SpriteName.pjNum0,
                SpriteName.pjNum1,
                SpriteName.pjNum2,
                SpriteName.pjNum3,
                SpriteName.pjNum4,
                SpriteName.pjNum5,
                SpriteName.pjNum6,
                SpriteName.pjNum7,
                SpriteName.pjNum8,
                SpriteName.pjNum9,

                SpriteName.HudPin_Falcon0,
                SpriteName.HudPin_Falcon1,
                SpriteName.HudPin_Falcon2,
                SpriteName.HudPin_Falcon3,
                SpriteName.HudPin_Falcon4,
                SpriteName.HudPin_Falcon5,
                SpriteName.HudPin_Falcon6,
                SpriteName.HudPin_Falcon7,
                SpriteName.HudPin_Falcon8,
                SpriteName.HudPin_Falcon9,

                SpriteName.HudPin_Castle0,
                SpriteName.HudPin_Castle1,
                SpriteName.HudPin_Castle2,
                SpriteName.HudPin_Castle3,
                SpriteName.HudPin_Castle4,
                SpriteName.HudPin_Castle5,
                SpriteName.HudPin_Castle6,
                SpriteName.HudPin_Castle7,
                SpriteName.HudPin_Castle8,
                SpriteName.HudPin_Castle9,

                SpriteName.HudPin_Horse0,
                SpriteName.HudPin_Horse1,
                SpriteName.HudPin_Horse2,
                SpriteName.HudPin_Horse3,
                SpriteName.HudPin_Horse4,
                SpriteName.HudPin_Horse5,
                SpriteName.HudPin_Horse6,
                SpriteName.HudPin_Horse7,
                SpriteName.HudPin_Horse8,
                SpriteName.HudPin_Horse9,

                SpriteName.HudPin_Ship0,
                SpriteName.HudPin_Ship1,
                SpriteName.HudPin_Ship2,
                SpriteName.HudPin_Ship3,
                SpriteName.HudPin_Ship4,
                SpriteName.HudPin_Ship5,
                SpriteName.HudPin_Ship6,
                SpriteName.HudPin_Ship7,
                SpriteName.HudPin_Ship8,
                SpriteName.HudPin_Ship9,

                SpriteName.HudPin_Cannon0,
                SpriteName.HudPin_Cannon1,
                SpriteName.HudPin_Cannon2,
                SpriteName.HudPin_Cannon3,
                SpriteName.HudPin_Cannon4,
                SpriteName.HudPin_Cannon5,
                SpriteName.HudPin_Cannon6,
                SpriteName.HudPin_Cannon7,
                SpriteName.HudPin_Cannon8,
                SpriteName.HudPin_Cannon9,

                SpriteName.pjNumQuestion,
                SpriteName.pjNumExpression,
                SpriteName.pjNumEquals,
                SpriteName.pjNumArrowR,
            };


            

            const int IconsBuffer = 10;

            //ItemResourceType[] MenAnimalsWagons = {

            //};

            //const int MenAnimalsWagonsBuffer = 10;

            //ItemResourceType[] items = Resource.ResourceLib.MovableCityResource_Animals
            List< ItemResourceType> items = new List< ItemResourceType >(256);
            for (ResourceGroupType resourceGroup = 0; resourceGroup < ResourceGroupType.NUM; resourceGroup++)
            {
                items.AddRange(ResourceLib.ResourceGroupList(resourceGroup));
                
            }

            const int ItemsBuffer = 10;

            BuildAndExpandType[] buildings = {
        BuildAndExpandType.         ImmigrationTent,
        BuildAndExpandType.        WorkerHut,

        BuildAndExpandType.SoldiersQuarter_Small,
        BuildAndExpandType.SoldiersQuarter_Large,
        BuildAndExpandType.ServiceHouse_Small,        
        BuildAndExpandType.GuardHouse_Small,
        
        BuildAndExpandType.Postal,
        BuildAndExpandType.Recruitment,
       
        BuildAndExpandType.Noblehouse,
        BuildAndExpandType.Tavern,
        BuildAndExpandType.Storehouse,
        BuildAndExpandType.Brewery,
        BuildAndExpandType.Cook,
        BuildAndExpandType.Butcher,
        BuildAndExpandType.Smoker,
        BuildAndExpandType.Dryer,
        BuildAndExpandType.CoalPit,
        BuildAndExpandType.WorkBench,
        BuildAndExpandType.Pottery,
        BuildAndExpandType.DryingPan,
        
        BuildAndExpandType.Smith,
        BuildAndExpandType.Carpenter,
        BuildAndExpandType.WheatFarm,
        BuildAndExpandType.LinenFarm,
        BuildAndExpandType.HempFarm,
        BuildAndExpandType.RapeSeedFarm,
        
        BuildAndExpandType.Statue_ThePlayer,
        BuildAndExpandType.PavementStreet,
        BuildAndExpandType.PavementFlower,

        BuildAndExpandType.Logistics,
        BuildAndExpandType.Bank,
        BuildAndExpandType.CoinMinter,

        BuildAndExpandType.WoodCutter,
        BuildAndExpandType.StoneCutter,
        BuildAndExpandType.Embassy,
        BuildAndExpandType.WaterWell,
        BuildAndExpandType.WaterResovoir,
        BuildAndExpandType.SoldierBarracks,
        BuildAndExpandType.ArcherBarracks,
        BuildAndExpandType.WarmachineBarracks,
        BuildAndExpandType.GunBarracks,
        BuildAndExpandType.CannonBarracks,

        BuildAndExpandType.Smelter,
        BuildAndExpandType.Foundry,
        BuildAndExpandType.Armory,
        BuildAndExpandType.ShieldMaker,
        BuildAndExpandType.Chemist,
        BuildAndExpandType.Gunmaker,
        BuildAndExpandType.School,

        
        BuildAndExpandType.GoldDeliveryLvl1,
        
        BuildAndExpandType.DirtRoad,
        BuildAndExpandType.GravelRoad,
        BuildAndExpandType.CobblestoneRoad,
        BuildAndExpandType.PavedRoad,

        BuildAndExpandType.DirtWall,
        BuildAndExpandType.DirtTower,
        BuildAndExpandType.WoodWall,
        BuildAndExpandType.WoodTower,
        BuildAndExpandType.StoneWall,
        BuildAndExpandType.StoneTower,
        BuildAndExpandType.StoneWallGreen,
        BuildAndExpandType.StoneWallBlueRoof,
        BuildAndExpandType.StoneWallWoodHouse,
        BuildAndExpandType.StoneGate,
        BuildAndExpandType.StoneHouse,
        
        BuildAndExpandType.CitySquare,
        BuildAndExpandType.Statue_Leader,
        BuildAndExpandType.FlagPole_LongBanner,
        BuildAndExpandType.FlagPole_Banner,
        BuildAndExpandType.FlagPole_SlimBanner,

        BuildAndExpandType.FlagPole_Flag,
        BuildAndExpandType.FlagPole_FlagRound,
        BuildAndExpandType.FlagPole_FlagLarge,
        BuildAndExpandType.FlagPole_Streamer,
        BuildAndExpandType.FlagPole_Triangle,

        BuildAndExpandType.Palisade,
        BuildAndExpandType.ResearchCenter,
        BuildAndExpandType.BookPress,

        BuildAndExpandType.MithrilMine,
        BuildAndExpandType.SulfurMine,
        BuildAndExpandType.WorkerTent,

        BuildAndExpandType.ManorLord,
        BuildAndExpandType.GreatHall,

        BuildAndExpandType.OrchardApple,
        BuildAndExpandType.OrchidBanana,


        BuildAndExpandType.MaterialStorage, 
        BuildAndExpandType.FoodStorage,  
        BuildAndExpandType.WeaponStorage, 
        BuildAndExpandType.ArmorStorage, 
        BuildAndExpandType.AnimalStorage,
        BuildAndExpandType.Cesspit,

        BuildAndExpandType.TrapperHut,

        BuildAndExpandType.BoarPen,
        BuildAndExpandType.PigPen,
        BuildAndExpandType.FowlPen,
        BuildAndExpandType.HenPen,
        BuildAndExpandType.OxenPen,
        BuildAndExpandType.KineOxenPen,

        BuildAndExpandType.DogCage,
        BuildAndExpandType.HoundCage,

        BuildAndExpandType.PonyPen,
        BuildAndExpandType.HorsePen,
        BuildAndExpandType.WarHorsePen,
        BuildAndExpandType.DraftHorsePen,
        BuildAndExpandType.WildPigPen,
        BuildAndExpandType.WildHogPen,
        BuildAndExpandType.WarHogPen,
        BuildAndExpandType.StagHogPen,
        BuildAndExpandType.WolfCage,
        BuildAndExpandType.WargCage,
        BuildAndExpandType.AlphaWargCage,
        BuildAndExpandType.WildCatCage,
        BuildAndExpandType.LionCage,
        BuildAndExpandType.WarLionCage,
        BuildAndExpandType.ElephantCage,
        BuildAndExpandType.WarElephantCage,
        BuildAndExpandType.OliphantCage,        
            };

            const int BuildingsBuffer = 10;

            int length = 1 + icons.Length + IconsBuffer + items.Count + ItemsBuffer + buildings.Length + BuildingsBuffer;

            artTagDictionary = new SpriteName[length];

            Dictionary<SpriteName, int> iconRegister = new Dictionary<SpriteName, int>(icons.Length);
            Dictionary<ItemResourceType, int> itemRegister = new Dictionary<ItemResourceType, int>(items.Count);

            int index = 1;

            foreach (var icon in icons)
            {
#if DEBUG
                if (iconRegister.ContainsKey(icon))
                {
                    throw new Exception();
                }
#endif
                iconRegister.Add(icon, index);
                artTagDictionary[index++] = icon;
            }

            index += IconsBuffer;

            foreach (var item in items)
            {
#if DEBUG
                if (itemRegister.ContainsKey(item))
                {
                    throw new Exception();
                }
#endif
                itemRegister.Add(item, index);
                IconName.Item(item, out SpriteName itemIcon, out _);
                artTagDictionary[index++] = itemIcon;
            }

            index += ItemsBuffer;
            CityOnlyStart = index;

            foreach (var build in buildings)
            {
                IconName.Building(build, out SpriteName icon, out _);
                artTagDictionary[index++] = icon;
            }

            //Setup constants
            MapObjectTag.Tag_LevelMaster = (ushort)iconRegister[SpriteName.WarsUnitLevelMaster];
            MapObjectTag.Tag_SpecializeTradition = (ushort)iconRegister[SpriteName.WarsSpecializeTradition];
            MapObjectTag.Tag_Faction = (ushort)iconRegister[SpriteName.WarsGovernmentIcon];

            MapObjectTag.Tag_ItemResourceTypeFood = (ushort)itemRegister[ItemResourceType.Food_G];
            MapObjectTag.Tag_ItemResourceTypeRawFood = (ushort)itemRegister[ItemResourceType.RawFood_G];
            MapObjectTag.Tag_ItemResourceTypeBow = (ushort)itemRegister[ItemResourceType.Bow];
            MapObjectTag.Tag_ItemResourceTypeLongBow = (ushort)itemRegister[ItemResourceType.LongBow];
            MapObjectTag.Tag_ItemResourceTypeMithrilBow = (ushort)itemRegister[ItemResourceType.MithrilBow];

            //     public static ushort Tag_ItemResourceTypeFood;
            //public static ushort Tag_ItemResourceTypeRawFood;
            //public static ushort Tag_ItemResourceTypeLongBow;
            //public static ushort Tag_ItemResourceTypeMithrilBow;
        }

        public static SpriteName BackSprite(CityTagBack back)
        {
            switch (back)
            {
                case CityTagBack.White:
                    return SpriteName.warsFolder_white;
                case CityTagBack.Carton:
                    return SpriteName.warsFolder_carton;
                case CityTagBack.Yellow:
                    return SpriteName.warsFolder_yellow;
                case CityTagBack.Orange:
                    return SpriteName.warsFolder_orange;
                case CityTagBack.Pink:
                    return SpriteName.warsFolder_pink;
                case CityTagBack.Cyan:
                    return SpriteName.warsFolder_cyan;
                case CityTagBack.Blue:
                    return SpriteName.warsFolder_blue;
                case CityTagBack.Green:
                    return SpriteName.warsFolder_green;

                default: return NoBackSprite;
            }
        }

        public static void TagsToMenu(RichBoxContent content, LocalPlayer player, AbsMapObject mapObj)
        {
            HudLib.Label(content, DssRef.lang.ObjectUi_ViewOnMap + string.Format(" ({0})", DssRef.lang.Hud_AllArmies));
            content.newLine();
            player.armyHudSettings.toHud(content, false, player.profile.casualControls);

            content.newParagraph();

            for (CityTagBack back = CityTagBack.NONE; back < CityTagBack.NUM; back++)
            {
                var button = new ArtToggle(back == mapObj.Tag.backType, new List<AbsRichBoxMember> {
                    new RbImage(Data.TagLib.BackSprite(back))
                }, new RbAction1Arg<CityTagBack>((CityTagBack back) => { mapObj.Tag.backType = back; }, back, back == CityTagBack.NONE ? RbSoundType.Deselect : RbSoundType.Option));
                content.Add(button);

                if (back == CityTagBack.NONE)
                {
                    content.newLine();
                }

            }

            if (mapObj.Tag.backType != CityTagBack.NONE)
            {
                content.newParagraph();
                //for (ArmyTagArt art = ArmyTagArt.None; art < ArmyTagArt.NUM; art++)
                int end = mapObj.IsCity() ? artTagDictionary.Length : CityOnlyStart;

                var noArtbutton = new ArtToggle(mapObj.Tag.artId == 0, new List<AbsRichBoxMember> {
                        new RbImage(NoBackSprite)
                        }, new RbAction(() => { mapObj.Tag.artId = 0; }, RbSoundType.Deselect));
                content.Add(noArtbutton);

                for (int i = 0; i < end; i++)
                {
                    if (artTagDictionary[i] != SpriteName.NO_IMAGE)
                    {
                        var button = new ArtToggle(i == mapObj.Tag.artId, new List<AbsRichBoxMember> {
                            new RbImage(artTagDictionary[i])
                            }, new RbAction1Arg<int>((int art) => { mapObj.Tag.artId = (ushort)art; }, i, /*art == ArmyTagArt.None ? RbSoundType.Deselect : */RbSoundType.Option));
                        content.Add(button);
                    }
                }
            }
        }

    }
    enum CityTagBack
    {
        NONE,
        White,
        Carton,
        Yellow,
        Orange,
        Pink,
        Cyan,
        Blue,
        Green,
        NUM,
    }

}
