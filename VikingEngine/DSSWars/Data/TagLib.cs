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

        public void read(System.IO.BinaryReader r, int subversion)
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
        BuildAndExpandType.Pavement,
        BuildAndExpandType.PavementFlower,

        BuildAndExpandType.Logistics,
        BuildAndExpandType.Bank,
        BuildAndExpandType.CoinMinter,

        BuildAndExpandType.WoodCutter,
        BuildAndExpandType.StoneCutter,
        BuildAndExpandType.Embassy,
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
            MapObjectTag.Tag_ItemResourceTypeRawFood = (ushort)itemRegister[ItemResourceType.RawFood_Group];
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

        //public static SpriteName ArtSprite(TagArt art)
        //{
        //    if (art == TagArt.None)
        //    {
        //        return NoBackSprite;
        //    }
        //    else if (art <= TagArt.ItemResourceTypeMithril)
        //    {
        //        switch (art)
        //        {
        //            case TagArt.ItemResourceTypeGoldOre: return SpriteName.WarsResource_GoldOre;
        //            case TagArt.ItemResourceTypeGold: return SpriteName.rtsMoney;
        //            case TagArt.ItemResourceTypeWater: return SpriteName.WarsResource_Water;
        //            case TagArt.ItemResourceTypeWood: return SpriteName.WarsResource_Wood;
        //            case TagArt.ItemResourceTypeFuel: return SpriteName.WarsResource_Fuel;
        //            case TagArt.ItemResourceTypeStone: return SpriteName.WarsResource_Stone;
        //            // case CityTagArt.ItemResourceTypeRaw_Coal: return SpriteName.GoodsCoal; // Uncomment if Raw_Coal is added to enum
        //            case TagArt.ItemResourceTypeRaw_Meat: return SpriteName.WarsResource_RawMeat;
        //            case TagArt.ItemResourceTypeRaw_Wheat: return SpriteName.WarsResource_Wheat;
        //            case TagArt.ItemResourceTypeRaw_Linen: return SpriteName.WarsResource_Linen;
        //            case TagArt.ItemResourceTypeRaw_Hemp: return SpriteName.WarsResource_Hemp;
        //            case TagArt.ItemResourceTypeRaw_Rapeseed: return SpriteName.WarsResource_Rapeseed;

        //            case TagArt.ItemResourceTypeRawFood: return SpriteName.WarsResource_RawFood;
        //            case TagArt.ItemResourceTypeFood: return SpriteName.WarsResource_Food;
        //            case TagArt.ItemResourceTypeBeer: return SpriteName.WarsResource_Beer;
        //            case TagArt.ItemResourceTypeCoolingFluid: return SpriteName.WarsResource_CoolingFluid;
        //            case TagArt.ItemResourceTypeSkinLinen: return SpriteName.WarsResource_LinenCloth;
        //            case TagArt.ItemResourceTypeToolkit: return SpriteName.WarsResource_Toolkit;
        //            case TagArt.ItemResourceTypeWagon2Wheel: return SpriteName.WarsResource_Wagon2Wheel;
        //            case TagArt.ItemResourceTypeWagon4Wheel: return SpriteName.WarsResource_Wagon4Wheel;
        //            case TagArt.ItemResourceTypeBlackPowder: return SpriteName.WarsResource_BlackPowder;
        //            case TagArt.ItemResourceTypeGunPowder: return SpriteName.WarsResource_GunPowder;
        //            case TagArt.ItemResourceTypeLedBullet: return SpriteName.WarsResource_Bullets;

        //            case TagArt.ItemResourceTypeIronOre: return SpriteName.WarsResource_IronOre;
        //            case TagArt.ItemResourceTypeTinOre: return SpriteName.WarsResource_TinOre;
        //            case TagArt.ItemResourceTypeCopperOre: return SpriteName.WarsResource_CopperOre;
        //            case TagArt.ItemResourceTypeLeadOre: return SpriteName.WarsResource_LeadOre;
        //            case TagArt.ItemResourceTypeSilverOre: return SpriteName.WarsResource_SilverOre;

        //            case TagArt.ItemResourceTypeIron: return SpriteName.WarsResource_Iron;
        //            case TagArt.ItemResourceTypeTin: return SpriteName.WarsResource_Tin;
        //            case TagArt.ItemResourceTypeCopper: return SpriteName.WarsResource_Copper;
        //            case TagArt.ItemResourceTypeLead: return SpriteName.WarsResource_Lead;
        //            case TagArt.ItemResourceTypeSilver: return SpriteName.WarsResource_Silver;
        //            case TagArt.ItemResourceTypeRawMithril: return SpriteName.WarsResource_Mithril;

        //            case TagArt.ItemResourceTypeBronze: return SpriteName.WarsResource_Bronze;
        //            case TagArt.ItemResourceTypeCastIron: return SpriteName.WarsResource_CastIron;
        //            case TagArt.ItemResourceTypeBloomeryIron: return SpriteName.WarsResource_BloomeryIron;
        //            case TagArt.ItemResourceTypeSteel: return SpriteName.WarsResource_Steel;
        //            case TagArt.ItemResourceTypeMithril: return SpriteName.WarsResource_Mithril;
        //        }
        //    }
        //    else if (art <= TagArt.ItemResourceTypeMithrilArmor)
        //    {
        //        switch (art)
        //        {
        //            case TagArt.ItemResourceTypeSharpStick: return SpriteName.WarsResource_Sharpstick;
        //            case TagArt.ItemResourceTypeBronzeSword: return SpriteName.WarsResource_BronzeSword;
        //            case TagArt.ItemResourceTypeShortSword: return SpriteName.WarsResource_ShortSword;
        //            case TagArt.ItemResourceTypeSword: return SpriteName.WarsResource_Sword;
        //            case TagArt.ItemResourceTypeLongSword: return SpriteName.WarsResource_Longsword;

        //            case TagArt.ItemResourceTypeWarHammer: return SpriteName.WarsResource_Warhammer;
        //            case TagArt.ItemResourceTypeTwoHandSword: return SpriteName.WarsResource_TwoHandSword;
        //            case TagArt.ItemResourceTypeMithrilSword: return SpriteName.WarsResource_MithrilSword;

        //            case TagArt.ItemResourceTypeSlingshot: return SpriteName.WarsResource_Slingshot;
        //            case TagArt.ItemResourceTypeThrowingSpear: return SpriteName.WarsResource_ThrowSpear;
        //            case TagArt.ItemResourceTypeBow: return SpriteName.WarsResource_Bow;
        //            case TagArt.ItemResourceTypeLongBow: return SpriteName.WarsResource_Longbow;
        //            case TagArt.ItemResourceTypeCrossbow: return SpriteName.WarsResource_Crossbow;
        //            case TagArt.ItemResourceTypeMithrilBow: return SpriteName.WarsResource_Mithrilbow;

        //            case TagArt.ItemResourceTypeHandCannon: return SpriteName.WarsResource_BronzeRifle;
        //            case TagArt.ItemResourceTypeHandCulverin: return SpriteName.WarsResource_BronzeShotgun;
        //            case TagArt.ItemResourceTypeRifle: return SpriteName.WarsResource_IronRifle;
        //            case TagArt.ItemResourceTypeBlunderbus: return SpriteName.WarsResource_IronShotgun;

        //            case TagArt.ItemResourceTypeBallista: return SpriteName.WarsResource_Ballista;
        //            case TagArt.ItemResourceTypeManuBallista: return SpriteName.WarsResource_Manuballista;
        //            case TagArt.ItemResourceTypeCatapult: return SpriteName.WarsResource_Catapult;
        //            case TagArt.ItemResourceTypeSiegeCannonBronze: return SpriteName.WarsResource_BronzeSiegeCannon;
        //            case TagArt.ItemResourceTypeManCannonBronze: return SpriteName.WarsResource_BronzeManCannon;
        //            case TagArt.ItemResourceTypeSiegeCannonIron: return SpriteName.WarsResource_IronSiegeCannon;
        //            case TagArt.ItemResourceTypeManCannonIron: return SpriteName.WarsResource_IronManCannon;

        //            case TagArt.ItemResourceTypePaddedArmor: return SpriteName.WarsResource_PaddedArmor;
        //            case TagArt.ItemResourceTypeHeavyPaddedArmor: return SpriteName.WarsResource_HeavyPaddedArmor;
        //            case TagArt.ItemResourceTypeBronzeArmor: return SpriteName.WarsResource_BronzeArmor;
        //            case TagArt.ItemResourceTypeMailArmor: return SpriteName.WarsResource_IronArmor;
        //            case TagArt.ItemResourceTypeHeavyMailArmor: return SpriteName.WarsResource_HeavyIronArmor;
        //            case TagArt.ItemResourceTypeLightPlateArmor: return SpriteName.WarsResource_LightPlateArmor;
        //            case TagArt.ItemResourceTypeFullPlateArmor: return SpriteName.WarsResource_FullPlateArmor;

        //            case TagArt.ItemResourceTypeMithrilArmor: return SpriteName.WarsResource_MithrilArmor;

        //            case TagArt.ItemResourceTypeConservedFood: return SpriteName.WarsResource_ConservedFood;
        //            case TagArt.ItemResourceTypeClay: return SpriteName.WarsResource_Clay;
        //            case TagArt.ItemResourceTypeBrick: return SpriteName.WarsResource_Brick;
        //            case TagArt.ItemResourceTypeContainer: return SpriteName.WarsResource_Container;
        //            case TagArt.ItemResourceTypeWagonClosed: return SpriteName.WarsResource_WagonClosed;
        //            case TagArt.ItemResourceTypeWagonIron: return SpriteName.WarsResource_WagonIron;
        //            case TagArt.ItemResourceTypeWagonSteel: return SpriteName.WarsResource_WagonSteel;

        //            case TagArt.ItemResourceTypeBucklerShield: return SpriteName.ShieldBuckle;
        //            case TagArt.ItemResourceTypeRoundShield: return SpriteName.cmdRoundShield;
        //            case TagArt.ItemResourceTypeHeaterShield: return SpriteName.WarsResource_HeaterShield;
        //            case TagArt.ItemResourceTypeTowerShield: return SpriteName.WarsResource_TowerShield;

        //            case TagArt.ItemResourceTypeMountBronzeArmor: return SpriteName.WarsResource_MountBronzeArmor;
        //            case TagArt.ItemResourceTypeMountPaddedArmor: return SpriteName.WarsResource_MountPaddedArmor;
        //            case TagArt.ItemResourceTypeMountHeavyPaddedArmor: return SpriteName.WarsResource_MountHeavyPaddedArmor;
        //            case TagArt.ItemResourceTypeMountIronArmor: return SpriteName.WarsResource_MountIronArmor;
        //            case TagArt.ItemResourceTypeMountHeavyIronArmor: return SpriteName.WarsResource_MountHeavyIronArmor;
        //            case TagArt.ItemResourceTypeMountLightPlateArmor: return SpriteName.WarsResource_MountLightPlateArmor;
        //            case TagArt.ItemResourceTypeMountFullPlateArmor: return SpriteName.WarsResource_MountFullPlateArmor;
        //            case TagArt.ItemResourceTypeMountMithrilArmor: return SpriteName.WarsResource_MountMithrilArmor;
        //        }
        //    }
        //    else if (art <= TagArt.BuildStatue_ThePlayer)
        //    {
        //        switch (art)
        //        {
        //            case TagArt.BuildWorkerHuts: return SpriteName.WarsBuild_WorkerHuts;
        //            case TagArt.BuildPostal: return SpriteName.WarsBuild_Postal;
        //            case TagArt.BuildRecruitment: return SpriteName.WarsBuild_Recruitment;
        //            case TagArt.BuildNobelhouse: return SpriteName.WarsBuild_Nobelhouse;
        //            case TagArt.BuildTavern: return SpriteName.WarsBuild_Tavern;
        //            case TagArt.BuildStorehouse: return SpriteName.WarsBuild_Storehouse;
        //            case TagArt.BuildBrewery: return SpriteName.WarsBuild_Brewery;
        //            case TagArt.BuildCook: return SpriteName.WarsBuild_Cook;
        //            case TagArt.BuildCoalPit: return SpriteName.WarsBuild_CoalPit;
        //            case TagArt.BuildWorkBench: return SpriteName.WarsBuild_WorkBench;
        //            case TagArt.BuildSmith: return SpriteName.WarsBuild_Smith;
        //            case TagArt.BuildCarpenter: return SpriteName.WarsBuild_Carpenter;
        //            case TagArt.BuildWheatFarm: return SpriteName.WarsBuild_WheatFarms;
        //            case TagArt.BuildLinenFarm: return SpriteName.WarsBuild_LinenFarms;
        //            case TagArt.BuildHempFarm: return SpriteName.WarsBuild_HempFarms;
        //            case TagArt.BuildRapeSeedFarm: return SpriteName.WarsBuild_RapeseedFarms;
        //            case TagArt.BuildPigPen: return SpriteName.WarsBuild_PigPen;
        //            case TagArt.BuildHenPen: return SpriteName.WarsBuild_HenPen;
        //            case TagArt.BuildStatue_ThePlayer: return SpriteName.WarsBuild_Statue;
        //            case TagArt.BuildPavement: return SpriteName.WarsBuild_Pavement;
        //            case TagArt.BuildPavementFlower: return SpriteName.WarsBuild_PavementFlowers;

        //            case TagArt.Embassy: return SpriteName.WarsBuild_Embassy;
        //            case TagArt.Bank: return SpriteName.WarsBuild_Bank;
        //            case TagArt.CoinMinter: return SpriteName.WarsBuild_Coinminter;
        //            case TagArt.BuildWaterResovoir: return SpriteName.WarsBuild_WaterReservoir;
        //            case TagArt.BuildSmelter: return SpriteName.WarsBuild_Smelter;
        //            case TagArt.BuildFoundry: return SpriteName.WarsBuild_Foundry;
        //            case TagArt.BuildChemist: return SpriteName.WarsBuild_Chemist;
        //            case TagArt.BuildGunmaker: return SpriteName.WarsBuild_Gunmaker;
        //            case TagArt.BuildArmory: return SpriteName.WarsBuild_Armory;

        //            case TagArt.BuildSoldierBarracks: return SpriteName.WarsBuild_Barracks;
        //            case TagArt.BuildArcherBarracks: return SpriteName.WarsBuild_ArcherBarracks;
        //            case TagArt.BuildWarmachineBarracks: return SpriteName.WarsBuild_WarmachineBarracks;
        //            case TagArt.BuildGunBarracks: return SpriteName.WarsBuild_GunBarracks;
        //            case TagArt.BuildCannonBarracks: return SpriteName.WarsBuild_CannonBarracks;
        //            case TagArt.BuildKnightsBarracks: return SpriteName.WarsBuild_KnightBarrack;

        //            case TagArt.BuildWoodCutter: return SpriteName.WarsBuild_WoodCutter;
        //            case TagArt.BuildStoneCutter: return SpriteName.WarsBuild_StoneCutter;

        //            case TagArt.BuildTrapperHut: return SpriteName.WarsBuild_Trapper;
        //            case TagArt.BuildBoarPen: return SpriteName.WarsBuild_BoarPen;
        //            case TagArt.BuildFowlPen: return SpriteName.WarsBuild_FowlPen;
        //            case TagArt.BuildOxenPen: return SpriteName.WarsBuild_OxenPen;
        //            case TagArt.BuildKineOxenPen: return SpriteName.WarsBuild_KineOxenPen;

        //            case TagArt.BuildDogCage: return SpriteName.WarsBuild_DogCage;
        //            case TagArt.BuildHoundCage: return SpriteName.WarsBuild_HoundCage;

        //            case TagArt.BuildPonyPen: return SpriteName.WarsBuild_PonyPen;
        //            case TagArt.BuildHorsePen: return SpriteName.WarsBuild_HorsePen;
        //            case TagArt.BuildWarHorsePen: return SpriteName.WarsBuild_WarHorsePen;
        //            case TagArt.BuildDraftHorsePen: return SpriteName.WarsBuild_DraftHorsePen;
        //            case TagArt.BuildWildPigPen: return SpriteName.WarsBuild_WildPigPen;
        //            case TagArt.BuildWildHogPen: return SpriteName.WarsBuild_WildHogPen;
        //            case TagArt.BuildWarHogPen: return SpriteName.WarsBuild_WarHogPen;
        //            case TagArt.BuildStagHogPen: return SpriteName.WarsBuild_StagHogPen;
        //            case TagArt.BuildWolfCage: return SpriteName.WarsBuild_WolfPen;
        //            case TagArt.BuildWargCage: return SpriteName.WarsBuild_WargPen;
        //            case TagArt.BuildAlphaWargCage: return SpriteName.WarsBuild_AlphaWargPen;
        //            case TagArt.BuildWildCatCage: return SpriteName.WarsBuild_WildCatPen;
        //            case TagArt.BuildLionCage: return SpriteName.WarsBuild_LionPen;
        //            case TagArt.BuildWarLionCage: return SpriteName.WarsBuild_WarLionPen;
        //            case TagArt.BuildElephantCage: return SpriteName.WarsBuild_ElephantPen;
        //            case TagArt.BuildWarElephantCage: return SpriteName.WarsBuild_WarElephantPen;
        //            case TagArt.BuildOliphantCage: return SpriteName.WarsBuild_OliphantPen;

        //            case TagArt.BuildPottery: return SpriteName.WarsBuild_Pottery;
        //            case TagArt.BuildDryingPan: return SpriteName.WarsBuild_DryingPan;
        //            case TagArt.BuildButcher: return SpriteName.WarsBuild_Butcher;
        //            case TagArt.BuildSmoker: return SpriteName.WarsBuild_Smoker;
        //            case TagArt.BuildDryer: return SpriteName.WarsBuild_Dryer;
        //            case TagArt.BuildShieldMaker: return SpriteName.WarsBuild_Shieldmaker;

        //            case TagArt.BuildMaterialStorage: return SpriteName.WarsBuild_MaterialStorage; 
        //            case TagArt.BuildFoodStorage: return SpriteName.WarsBuild_FoodStorage; 
        //            case TagArt.BuildWeaponStorage: return SpriteName.WarsBuild_WeaponStorage; 
        //            case TagArt.BuildArmorStorage: return SpriteName.WarsBuild_ArmorStorage; 
        //            case TagArt.BuildAnimalStorage: return SpriteName.WarsBuild_AnimalStorage;
        //            case TagArt.BuildCesspit: return SpriteName.WarsBuild_Cesspit;
        //        }
        //    }
        //    else if (art <= TagArt.UnitType_Viking)
        //    {
        //        switch (art)
        //        {
        //            case TagArt.Worker: return SpriteName.WarsWorker;
        //            case TagArt.UnitType_SharpStick: return SpriteName.WarsUnitIcon_Folkman;
        //            case TagArt.UnitType_Sword: return SpriteName.WarsUnitIcon_Soldier;
        //            case TagArt.UnitType_LongSword: return SpriteName.WarsUnitIcon_Longsword;

        //            case TagArt.UnitType_Warhammer: return SpriteName.WarsUnitIcon_Hammerknight;
        //            case TagArt.UnitType_TwohandSword: return SpriteName.WarsUnitIcon_TwoHand;
        //            case TagArt.UnitType_Knight: return SpriteName.WarsUnitIcon_Knight;
        //            case TagArt.UnitType_MithrilKnight: return SpriteName.WarsUnitIcon_MithrilMan;
        //            case TagArt.UnitType_MithrilArcher: return SpriteName.WarsUnitIcon_MithrilArcher;

        //            case TagArt.UnitType_Slingshot: return SpriteName.WarsUnitIcon_Slingshot;
        //            case TagArt.UnitType_Javelin: return SpriteName.WarsUnitIcon_Javelin;
        //            case TagArt.UnitType_Bow: return SpriteName.WarsUnitIcon_Archer;
        //            case TagArt.UnitType_Crossbow: return SpriteName.LittleUnitIconCrossBowman;

        //            case TagArt.UnitType_Rifle: return SpriteName.WarsUnitIcon_BronzeRifle;
        //            case TagArt.UnitType_Shotgun: return SpriteName.WarsResource_BronzeShotgun;

        //            case TagArt.UnitType_Ballista: return SpriteName.WarsUnitIcon_Ballista;
        //            case TagArt.UnitType_ManuBallista: return SpriteName.WarsUnitIcon_Manuballista;
        //            case TagArt.UnitType_Catapult: return SpriteName.WarsUnitIcon_Catapult;

        //            case TagArt.UnitType_SiegeBronzeCannon: return SpriteName.WarsUnitIcon_BronzeSiegeCannon;
        //            case TagArt.UnitType_ManBronzeCannon: return SpriteName.WarsUnitIcon_BronzeManCannon;
        //            case TagArt.UnitType_SiegeIronCannon: return SpriteName.WarsUnitIcon_IronSiegeCannon;
        //            case TagArt.UnitType_ManIronCannon: return SpriteName.WarsUnitIcon_IronManCannon;

        //            case TagArt.UnitType_HonourGuard: return SpriteName.WarsUnitIcon_Honorguard;
        //            case TagArt.UnitType_Viking: return SpriteName.WarsUnitIcon_Viking;
        //        }
        //    }
        //    else if (art <= TagArt.IconHeart)
        //    {
        //        switch (art)
        //        {
        //            case TagArt.IconBuild: return SpriteName.WarsHammer;
        //            case TagArt.IconMovebox: return SpriteName.WarsWorkMove;
        //            case TagArt.IconHandCollect: return SpriteName.WarsWorkCollect;
        //            case TagArt.IconMine: return SpriteName.WarsWorkMine;
        //            case TagArt.IconSmelt: return SpriteName.WarsWorkCasting;
        //            case TagArt.IconDig: return SpriteName.WarsWorkFarm;
        //            case TagArt.IconBed: return SpriteName.WarsBedIcon;
        //            case TagArt.IconMap: return SpriteName.WarsMapIcon;
        //            case TagArt.IconFaction: return SpriteName.WarsGovernmentIcon;
        //            case TagArt.IconThumbsUp: return SpriteName.unitEmoteThumbUp;
        //            case TagArt.IconThumbsDown: return SpriteName.unitEmoteThumbDown;
        //            case TagArt.IconHeart: return SpriteName.unitEmoteLove;
        //        }
        //    }
        //    else
        //    {
        //        switch (art)
        //        {
        //            case TagArt.Num0: return SpriteName.pjNum0;
        //            case TagArt.Num1: return SpriteName.pjNum1;
        //            case TagArt.Num2: return SpriteName.pjNum2;
        //            case TagArt.Num3: return SpriteName.pjNum3;
        //            case TagArt.Num4: return SpriteName.pjNum4;
        //            case TagArt.Num5: return SpriteName.pjNum5;
        //            case TagArt.Num6: return SpriteName.pjNum6;
        //            case TagArt.Num7: return SpriteName.pjNum7;
        //            case TagArt.Num8: return SpriteName.pjNum8;
        //            case TagArt.Num9: return SpriteName.pjNum9;
        //            case TagArt.NumQuestion: return SpriteName.pjNumQuestion;
        //            case TagArt.NumExpression: return SpriteName.pjNumExpression;
        //            case TagArt.NumEqual: return SpriteName.pjNumEquals;
        //            case TagArt.NumArrow: return SpriteName.pjNumArrowR;
        //        }
        //    }

        //    return SpriteName.NO_IMAGE;
        //}

        //    public static SpriteName ArtSprite(ArmyTagArt art)
        //    {
        //        if (art == ArmyTagArt.None)
        //        {
        //            return NoBackSprite;
        //        }
        //        else if (art <= ArmyTagArt.UnitType_ManIronCannon)
        //        {
        //            switch (art)
        //            {
        //                case ArmyTagArt.UnitType_SharpStick: return SpriteName.WarsUnitIcon_Folkman;
        //                case ArmyTagArt.UnitType_Sword: return SpriteName.WarsUnitIcon_Soldier;
        //                case ArmyTagArt.UnitType_LongSword: return SpriteName.WarsUnitIcon_Longsword;

        //                case ArmyTagArt.UnitType_Warhammer: return SpriteName.WarsUnitIcon_Hammerknight;
        //                case ArmyTagArt.UnitType_TwohandSword: return SpriteName.WarsUnitIcon_TwoHand;
        //                case ArmyTagArt.UnitType_Knight: return SpriteName.WarsUnitIcon_Knight;
        //                case ArmyTagArt.UnitType_MithrilKnight: return SpriteName.WarsUnitIcon_MithrilMan;
        //                case ArmyTagArt.UnitType_MithrilArcher: return SpriteName.WarsUnitIcon_MithrilArcher;

        //                case ArmyTagArt.UnitType_Slingshot: return SpriteName.WarsUnitIcon_Slingshot;
        //                case ArmyTagArt.UnitType_Javelin: return SpriteName.WarsUnitIcon_Javelin;
        //                case ArmyTagArt.UnitType_Bow: return SpriteName.WarsUnitIcon_Archer;
        //                case ArmyTagArt.UnitType_Crossbow: return SpriteName.LittleUnitIconCrossBowman;

        //                case ArmyTagArt.UnitType_Rifle: return SpriteName.WarsUnitIcon_BronzeRifle;
        //                case ArmyTagArt.UnitType_Shotgun: return SpriteName.WarsResource_BronzeShotgun;

        //                case ArmyTagArt.UnitType_Ballista: return SpriteName.WarsUnitIcon_Ballista;
        //                case ArmyTagArt.UnitType_ManuBallista: return SpriteName.WarsUnitIcon_Manuballista;
        //                case ArmyTagArt.UnitType_Catapult: return SpriteName.WarsUnitIcon_Catapult;

        //                case ArmyTagArt.UnitType_SiegeBronzeCannon: return SpriteName.WarsUnitIcon_BronzeSiegeCannon;
        //                case ArmyTagArt.UnitType_ManBronzeCannon: return SpriteName.WarsUnitIcon_BronzeManCannon;
        //                case ArmyTagArt.UnitType_SiegeIronCannon: return SpriteName.WarsUnitIcon_IronSiegeCannon;
        //                case ArmyTagArt.UnitType_ManIronCannon: return SpriteName.WarsUnitIcon_IronManCannon;

        //                case ArmyTagArt.UnitType_HonourGuard: return SpriteName.WarsUnitIcon_Honorguard;
        //            }
        //        }
        //        else if (art <= ArmyTagArt.ItemResourceTypeMithrilArmor)
        //        {
        //            switch (art)
        //            {
        //                case ArmyTagArt.ItemResourceTypeSharpStick: return SpriteName.WarsResource_Sharpstick;
        //                case ArmyTagArt.ItemResourceTypeBronzeSword: return SpriteName.WarsResource_BronzeSword;
        //                case ArmyTagArt.ItemResourceTypeShortSword: return SpriteName.WarsResource_ShortSword;
        //                case ArmyTagArt.ItemResourceTypeSword: return SpriteName.WarsResource_Sword;
        //                case ArmyTagArt.ItemResourceTypeLongSword: return SpriteName.WarsResource_Longsword;

        //                case ArmyTagArt.ItemResourceTypeWarHammer: return SpriteName.WarsResource_Warhammer;
        //                case ArmyTagArt.ItemResourceTypeTwoHandSword: return SpriteName.WarsResource_TwoHandSword;
        //                case ArmyTagArt.ItemResourceTypeKnightsLance: return SpriteName.WarsResource_KnightsLance;
        //                case ArmyTagArt.ItemResourceTypeMithrilSword: return SpriteName.WarsResource_MithrilSword;

        //                case ArmyTagArt.ItemResourceTypeSlingshot: return SpriteName.WarsResource_Slingshot;
        //                case ArmyTagArt.ItemResourceTypeThrowingSpear: return SpriteName.WarsResource_ThrowSpear;
        //                case ArmyTagArt.ItemResourceTypeBow: return SpriteName.WarsResource_Bow;
        //                case ArmyTagArt.ItemResourceTypeLongBow: return SpriteName.WarsResource_Longbow;
        //                case ArmyTagArt.ItemResourceTypeCrossbow: return SpriteName.WarsResource_Crossbow;
        //                case ArmyTagArt.ItemResourceTypeMithrilBow: return SpriteName.WarsResource_Mithrilbow;

        //                case ArmyTagArt.ItemResourceTypeHandCannon: return SpriteName.WarsResource_BronzeRifle;
        //                case ArmyTagArt.ItemResourceTypeHandCulverin: return SpriteName.WarsResource_BronzeShotgun;
        //                case ArmyTagArt.ItemResourceTypeRifle: return SpriteName.WarsResource_IronRifle;
        //                case ArmyTagArt.ItemResourceTypeBlunderbus: return SpriteName.WarsResource_IronShotgun;

        //                case ArmyTagArt.ItemResourceTypeBallista: return SpriteName.WarsResource_Ballista;
        //                case ArmyTagArt.ItemResourceTypeManuBallista: return SpriteName.WarsResource_Manuballista;
        //                case ArmyTagArt.ItemResourceTypeCatapult: return SpriteName.WarsResource_Catapult;
        //                case ArmyTagArt.ItemResourceTypeSiegeCannonBronze: return SpriteName.WarsResource_BronzeSiegeCannon;
        //                case ArmyTagArt.ItemResourceTypeManCannonBronze: return SpriteName.WarsResource_BronzeManCannon;
        //                case ArmyTagArt.ItemResourceTypeSiegeCannonIron: return SpriteName.WarsResource_IronSiegeCannon;
        //                case ArmyTagArt.ItemResourceTypeManCannonIron: return SpriteName.WarsResource_IronManCannon;

        //                case ArmyTagArt.ItemResourceTypePaddedArmor: return SpriteName.WarsResource_PaddedArmor;
        //                case ArmyTagArt.ItemResourceTypeHeavyPaddedArmor: return SpriteName.WarsResource_HeavyPaddedArmor;
        //                case ArmyTagArt.ItemResourceTypeBronzeArmor: return SpriteName.WarsResource_BronzeArmor;
        //                case ArmyTagArt.ItemResourceTypeMailArmor: return SpriteName.WarsResource_IronArmor;
        //                case ArmyTagArt.ItemResourceTypeHeavyMailArmor: return SpriteName.WarsResource_HeavyIronArmor;
        //                case ArmyTagArt.ItemResourceTypeLightPlateArmor: return SpriteName.WarsResource_LightPlateArmor;
        //                case ArmyTagArt.ItemResourceTypeFullPlateArmor: return SpriteName.WarsResource_FullPlateArmor;

        //                case ArmyTagArt.ItemResourceTypeMithrilArmor: return SpriteName.WarsResource_MithrilArmor;
        //            }
        //        }
        //        else if (art <= ArmyTagArt.Specialize_Tradition)
        //        {
        //            switch (art)
        //            {
        //                case ArmyTagArt.Specialize_Field:
        //                    return SpriteName.WarsSpecializeField;
        //                case ArmyTagArt.Specialize_Sea:
        //                    return SpriteName.WarsSpecializeSea;
        //                case ArmyTagArt.Specialize_Siege:
        //                    return SpriteName.WarsSpecializeSiege;
        //                case ArmyTagArt.Specialize_Tradition:
        //                    return SpriteName.WarsSpecializeTradition;

        //            }
        //        }
        //        else if (art <= ArmyTagArt.LevelLegend)
        //        {
        //            switch (art)
        //            {
        //                case ArmyTagArt.Specialize_Field:
        //                    return SpriteName.WarsSpecializeField;
        //                case ArmyTagArt.Specialize_Sea:
        //                    return SpriteName.WarsSpecializeSea;
        //                case ArmyTagArt.Specialize_Siege:
        //                    return SpriteName.WarsSpecializeSiege;
        //                case ArmyTagArt.Specialize_Tradition:
        //                    return SpriteName.WarsSpecializeTradition;
        //                case ArmyTagArt.LevelMinimal:
        //                    return SpriteName.WarsUnitLevelMinimal;
        //                case ArmyTagArt.LevelBasic:
        //                    return SpriteName.WarsUnitLevelBasic;
        //                case ArmyTagArt.LevelSkillful:
        //                    return SpriteName.WarsUnitLevelSkillful;
        //                case ArmyTagArt.LevelProfessional:
        //                    return SpriteName.WarsUnitLevelProfessional;
        //                case ArmyTagArt.LevelMaster:
        //                    return SpriteName.WarsUnitLevelMaster;
        //                case ArmyTagArt.LevelLegend:
        //                    return SpriteName.WarsUnitLevelLegend;
        //            }
        //        }
        //        else if (art <= ArmyTagArt.icon_BrokenShield)
        //        {
        //            switch (art)
        //            {
        //                case ArmyTagArt.icon_Lightning:
        //                    return SpriteName.warsArmyTag_Lightning;
        //                case ArmyTagArt.icon_Fire:
        //                    return SpriteName.warsArmyTag_Fire;
        //                case ArmyTagArt.icon_Hit:
        //                    return SpriteName.warsArmyTag_Hit;
        //                case ArmyTagArt.icon_HitExpress:
        //                    return SpriteName.warsArmyTag_HitExpress;
        //                case ArmyTagArt.icon_Retreat:
        //                    return SpriteName.warsArmyTag_Retreat;
        //                case ArmyTagArt.icon_Return:
        //                    return SpriteName.warsArmyTag_Return;
        //                case ArmyTagArt.icon_Anchor:
        //                    return SpriteName.warsArmyTag_Anchor;
        //                case ArmyTagArt.icon_Shield:
        //                    return SpriteName.warsArmyTag_Shield;
        //                case ArmyTagArt.icon_GoldShield:
        //                    return SpriteName.warsArmyTag_GoldShield;
        //                case ArmyTagArt.icon_BrokenShield:
        //                    return SpriteName.warsArmyTag_BrokenShield;
        //                case ArmyTagArt.icon_RoundShield:
        //                    return SpriteName.warsArmyTag_RoundShield;
        //            }
        //        }
        //        else if (art <= ArmyTagArt.WarsRelationTotalWar)
        //        {
        //            switch (art)
        //            {
        //                case ArmyTagArt.WarsRelationAlly:
        //                    return SpriteName.WarsRelationAlly;
        //                case ArmyTagArt.WarsRelationGood:
        //                    return SpriteName.WarsRelationGood;
        //                case ArmyTagArt.WarsRelationPeace:
        //                    return SpriteName.WarsRelationPeace;
        //                case ArmyTagArt.WarsRelationNeutral:
        //                    return SpriteName.WarsRelationNeutral;
        //                case ArmyTagArt.WarsRelationTruce:
        //                    return SpriteName.WarsRelationTruce;
        //                case ArmyTagArt.WarsRelationWar:
        //                    return SpriteName.WarsRelationWar;
        //                case ArmyTagArt.WarsRelationTotalWar:
        //                    return SpriteName.WarsRelationTotalWar;
        //            }
        //        }
        //        else
        //        {
        //            switch (art)
        //            {
        //                case ArmyTagArt.Num0: return SpriteName.pjNum0;
        //                case ArmyTagArt.Num1: return SpriteName.pjNum1;
        //                case ArmyTagArt.Num2: return SpriteName.pjNum2;
        //                case ArmyTagArt.Num3: return SpriteName.pjNum3;
        //                case ArmyTagArt.Num4: return SpriteName.pjNum4;
        //                case ArmyTagArt.Num5: return SpriteName.pjNum5;
        //                case ArmyTagArt.Num6: return SpriteName.pjNum6;
        //                case ArmyTagArt.Num7: return SpriteName.pjNum7;
        //                case ArmyTagArt.Num8: return SpriteName.pjNum8;
        //                case ArmyTagArt.Num9: return SpriteName.pjNum9;
        //                case ArmyTagArt.NumQuestion: return SpriteName.pjNumQuestion;
        //                case ArmyTagArt.NumExpression: return SpriteName.pjNumExpression;
        //                case ArmyTagArt.NumEqual: return SpriteName.pjNumEquals;
        //                case ArmyTagArt.NumArrow: return SpriteName.pjNumArrowR;
        //            }
        //        }
        //        return SpriteName.NO_IMAGE;
        //    }
        //}
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

    //enum ArmyTagArt
    //{
    //    None = 0,
    //    UnitType_SharpStick,
    //    UnitType_Sword,
    //    UnitType_LongSword,

    //    UnitType_Warhammer,
    //    UnitType_TwohandSword,
    //    UnitType_Knight,
    //    UnitType_MithrilKnight,
    //    UnitType_MithrilArcher,
    //    UnitType_HonourGuard,

    //    UnitType_Slingshot,
    //    UnitType_Javelin,
    //    UnitType_Bow,
    //    UnitType_Crossbow,

    //    UnitType_Rifle,
    //    UnitType_Shotgun,

    //    UnitType_Ballista,
    //    UnitType_ManuBallista,
    //    UnitType_Catapult,

    //    UnitType_SiegeBronzeCannon,
    //    UnitType_ManBronzeCannon,
    //    UnitType_SiegeIronCannon,
    //    UnitType_ManIronCannon,


    //    ItemResourceTypeSharpStick,
    //    ItemResourceTypeBronzeSword,
    //    ItemResourceTypeShortSword,
    //    ItemResourceTypeSword,
    //    ItemResourceTypeLongSword,

    //    ItemResourceTypeWarHammer,
    //    ItemResourceTypeTwoHandSword,
    //    ItemResourceTypeKnightsLance,
    //    ItemResourceTypeMithrilSword,
    //    ItemResourceTypeMithrilBow,

    //    ItemResourceTypeSlingshot,
    //    ItemResourceTypeThrowingSpear,
    //    ItemResourceTypeBow,
    //    ItemResourceTypeLongBow,
    //    ItemResourceTypeCrossbow,

    //    ItemResourceTypeHandCannon,
    //    ItemResourceTypeHandCulverin,
    //    ItemResourceTypeRifle,
    //    ItemResourceTypeBlunderbus,

    //    ItemResourceTypeBallista,
    //    ItemResourceTypeManuBallista,
    //    ItemResourceTypeCatapult,
    //    ItemResourceTypeSiegeCannonBronze,
    //    ItemResourceTypeManCannonBronze,
    //    ItemResourceTypeSiegeCannonIron,
    //    ItemResourceTypeManCannonIron,

    //    ItemResourceTypePaddedArmor,
    //    ItemResourceTypeHeavyPaddedArmor,
    //    ItemResourceTypeBronzeArmor,
    //    ItemResourceTypeMailArmor,
    //    ItemResourceTypeHeavyMailArmor,
    //    ItemResourceTypeLightPlateArmor,
    //    ItemResourceTypeFullPlateArmor,
    //    ItemResourceTypeMithrilArmor,

    //    Specialize_Field,
    //    Specialize_Sea,
    //    Specialize_Siege,
    //    Specialize_Tradition,

       

    //    Num0,
    //    Num1,
    //    Num2,
    //    Num3,
    //    Num4,
    //    Num5,
    //    Num6,
    //    Num7,
    //    Num8,
    //    Num9,
    //    NumQuestion,
    //    NumExpression,
    //    NumEqual,
    //    NumArrow,

    //    NUM,
    //}

    //enum TagArt
    //{
    //    None = 0,

        
    //    //UnitType_SharpStick,
    //    //UnitType_Sword,
    //    //UnitType_LongSword,

    //    //UnitType_Warhammer,
    //    //UnitType_TwohandSword,
    //    //UnitType_Knight,
    //    //UnitType_MithrilKnight,
    //    //UnitType_MithrilArcher,
    //    //UnitType_HonourGuard,

    //    //UnitType_Slingshot,
    //    //UnitType_Javelin,
    //    //UnitType_Bow,
    //    //UnitType_Crossbow,

    //    //UnitType_Rifle,
    //    //UnitType_Shotgun,

    //    //UnitType_Ballista,
    //    //UnitType_ManuBallista,
    //    //UnitType_Catapult,

    //    //UnitType_SiegeBronzeCannon,
    //    //UnitType_ManBronzeCannon,
    //    //UnitType_SiegeIronCannon,
    //    //UnitType_ManIronCannon,

    //    //UnitType_Viking,

    //    IconBuild,
    //    IconMovebox,
    //    IconHandCollect,
    //    IconMine,
    //    IconSmelt,
    //    IconDig,
    //    //IconState,
    //    IconBed,
    //    IconMap,
    //    IconFaction,
    //    IconThumbsUp,
    //    IconThumbsDown,
    //    IconHeart,

    //    LevelMinimal,
    //    LevelBasic,
    //    LevelSkillful,
    //    LevelProfessional,
    //    LevelMaster,
    //    LevelLegend,

    //    icon_Lightning,
    //    icon_Fire,
    //    icon_Hit,
    //    icon_HitExpress,
    //    icon_Retreat,
    //    icon_Return,
    //    icon_Anchor,
    //    icon_Shield,
    //    icon_GoldShield,
    //    icon_RoundShield,
    //    icon_BrokenShield,

    //    WarsRelationAlly,
    //    WarsRelationGood,
    //    WarsRelationPeace,
    //    WarsRelationNeutral,
    //    WarsRelationTruce,
    //    WarsRelationEnemy,
    //    WarsRelationWar,
    //    WarsRelationTotalWar,

    //    Num0,
    //    Num1,
    //    Num2,
    //    Num3,
    //    Num4,
    //    Num5,
    //    Num6,
    //    Num7,
    //    Num8,
    //    Num9,

    //    HudPin_Falcon0,
    //    HudPin_Falcon1,
    //    HudPin_Falcon2,
    //    HudPin_Falcon3,
    //    HudPin_Falcon4,
    //    HudPin_Falcon5,
    //    HudPin_Falcon6,
    //    HudPin_Falcon7,
    //    HudPin_Falcon8,
    //    HudPin_Falcon9,

    //    HudPin_Castle0,
    //    HudPin_Castle1,
    //    HudPin_Castle2,
    //    HudPin_Castle3,
    //    HudPin_Castle4,
    //    HudPin_Castle5,
    //    HudPin_Castle6,
    //    HudPin_Castle7,
    //    HudPin_Castle8,
    //    HudPin_Castle9,

    //    HudPin_Horse0,
    //    HudPin_Horse1,
    //    HudPin_Horse2,
    //    HudPin_Horse3,
    //    HudPin_Horse4,
    //    HudPin_Horse5,
    //    HudPin_Horse6,
    //    HudPin_Horse7,
    //    HudPin_Horse8,
    //    HudPin_Horse9,

    //    HudPin_Ship0,
    //    HudPin_Ship1,
    //    HudPin_Ship2,
    //    HudPin_Ship3,
    //    HudPin_Ship4,
    //    HudPin_Ship5,
    //    HudPin_Ship6,
    //    HudPin_Ship7,
    //    HudPin_Ship8,
    //    HudPin_Ship9,

    //    HudPin_Cannon0,
    //    HudPin_Cannon1,
    //    HudPin_Cannon2,
    //    HudPin_Cannon3,
    //    HudPin_Cannon4,
    //    HudPin_Cannon5,
    //    HudPin_Cannon6,
    //    HudPin_Cannon7,
    //    HudPin_Cannon8,
    //    HudPin_Cannon9,


    //    NumQuestion,
    //    NumExpression,
    //    NumEqual,
    //    NumArrow,

    //    Worker,
    //    Nobelman,

    //    AnimalFowl,
    //    AnimalHen,
    //    AnimalBoar,
    //    AnimalPig,
    //    AnimalOxen,
    //    AnimalKineOxen,

    //    AnimalDog,
    //    AnimalHound,

    //    AnimalPony,
    //    AnimalHorse,
    //    AnimalWarHorse,
    //    AnimalDraftHorse,

    //    AnimalWildPig,
    //    AnimalWildHog,
    //    AnimalWarHog,
    //    AnimalStagHog,

    //    AnimalWolf,
    //    AnimalWarg,
    //    AnimalAlphaWarg,

    //    AnimalWildCat,
    //    AnimalLion,
    //    AnimalWarLion,

    //    AnimalElephant,
    //    AnimalWarElephant,
    //    AnimalOliphant,

    //    CITYONLY_START,

    //    ItemResourceTypeGoldOre,
    //    ItemResourceTypeGold,
    //    ItemResourceTypeWater,

    //    ItemResourceTypeWood,
    //    ItemResourceTypeStone,
    //    ItemResourceTypeClay,
    //    ItemResourceTypeBrick,
    //    ItemResourceTypeRaw_Meat,
    //    ItemResourceTypeRaw_Wheat,
    //    ItemResourceTypeRaw_Linen,
    //    ItemResourceTypeRaw_Hemp,
    //    ItemResourceTypeRaw_Rapeseed,
    //    ItemResourceTypeRawFood,
    //    ItemResourceTypeFuel,
    //    ItemResourceTypeSkinLinen,
    //    ItemResourceTypeFood,
    //    ItemResourceTypeConservedFood,
        
        
       
    //    ItemResourceTypeBeer,
    //    ItemResourceTypeCoolingFluid,

    //    ItemResourceTypeContainer,
    //    ItemResourceTypeToolkit,
    //    ItemResourceTypeWagon2Wheel,
    //    ItemResourceTypeWagon4Wheel,
    //    ItemResourceTypeWagonClosed,
    //    ItemResourceTypeWagonIron,
    //    ItemResourceTypeWagonSteel,
    //    ItemResourceTypeBlackPowder,
    //    ItemResourceTypeGunPowder,
    //    ItemResourceTypeLedBullet,

    //    ItemResourceTypeIronOre,
    //    ItemResourceTypeTinOre,
    //    ItemResourceTypeCopperOre,
    //    ItemResourceTypeLeadOre,
    //    ItemResourceTypeSilverOre,

    //    ItemResourceTypeIron,
    //    ItemResourceTypeTin,
    //    ItemResourceTypeCopper,
    //    ItemResourceTypeLead,
    //    ItemResourceTypeSilver,
    //    ItemResourceTypeRawMithril,

    //    ItemResourceTypeBronze,
    //    ItemResourceTypeCastIron,
    //    ItemResourceTypeBloomeryIron,
    //    ItemResourceTypeSteel,
    //    ItemResourceTypeMithril,

    //    ItemResourceTypeSharpStick,
    //    ItemResourceTypeBronzeSword,
    //    ItemResourceTypeShortSword,
    //    ItemResourceTypeSword,
    //    ItemResourceTypeLongSword,

    //    ItemResourceTypeWarHammer,
    //    ItemResourceTypeTwoHandSword,
    //    ItemResourceTypeMithrilSword,
    //    ItemResourceTypeMithrilBow,

    //    ItemResourceTypeSlingshot,
    //    ItemResourceTypeThrowingSpear,
    //    ItemResourceTypeBow,
    //    ItemResourceTypeLongBow,
    //    ItemResourceTypeCrossbow,
    //    ItemResourceTypeHandCannon,
    //    ItemResourceTypeHandCulverin,
    //    ItemResourceTypeRifle,
    //    ItemResourceTypeBlunderbus,
    //    ItemResourceTypeBallista,
    //    ItemResourceTypeManuBallista,
    //    ItemResourceTypeCatapult,
    //    ItemResourceTypeSiegeCannonBronze,
    //    ItemResourceTypeManCannonBronze,
    //    ItemResourceTypeSiegeCannonIron,
    //    ItemResourceTypeManCannonIron,

    //    ItemResourceTypeBucklerShield, 
    //    ItemResourceTypeRoundShield, 
    //    ItemResourceTypeHeaterShield, 
    //    ItemResourceTypeTowerShield,

    //    ItemResourceTypePaddedArmor,
    //    ItemResourceTypeHeavyPaddedArmor,
    //    ItemResourceTypeBronzeArmor,
    //    ItemResourceTypeMailArmor,
    //    ItemResourceTypeHeavyMailArmor,
    //    ItemResourceTypeLightPlateArmor,
    //    ItemResourceTypeFullPlateArmor,
    //    ItemResourceTypeMithrilArmor,

    //    ItemResourceTypeMountBronzeArmor,
    //    ItemResourceTypeMountPaddedArmor,
    //    ItemResourceTypeMountHeavyPaddedArmor,
    //    ItemResourceTypeMountIronArmor,
    //    ItemResourceTypeMountHeavyIronArmor,
    //    ItemResourceTypeMountLightPlateArmor,
    //    ItemResourceTypeMountFullPlateArmor,
    //    ItemResourceTypeMountMithrilArmor,

    //    BuildWorkerHuts,
    //    BuildAppleOrchard,
    //    BuildWheatFarm,
    //    BuildLinenFarm,
    //    BuildRapeSeedFarm,
    //    BuildHempFarm,

    //    BuildTrapperHut,
    //    BuildBoarPen,
    //    BuildPigPen,
    //    BuildFowlPen,
    //    BuildHenPen,
    //    BuildOxenPen,
    //    BuildKineOxenPen,

    //    BuildDogCage,
    //    BuildHoundCage,

    //    BuildPonyPen,
    //    BuildHorsePen,
    //    BuildWarHorsePen,
    //    BuildDraftHorsePen,
    //    BuildWildPigPen,
    //    BuildWildHogPen,
    //    BuildWarHogPen,
    //    BuildStagHogPen,
    //    BuildWolfCage,
    //    BuildWargCage,
    //    BuildAlphaWargCage,
    //    BuildWildCatCage,
    //    BuildLionCage,
    //    BuildWarLionCage,
    //    BuildElephantCage,
    //    BuildWarElephantCage,
    //    BuildOliphantCage,
        
    //    BuildNobelhouse,
    //    Embassy,
    //    Bank,
    //    CoinMinter,
    //    BuildPostal,
    //    BuildRecruitment,
    //    BuildStorehouse,

    //    BuildTavern,
    //    BuildBrewery,
    //    BuildWaterResovoir,
    //    BuildCoalPit,
    //    BuildWorkBench,
    //    BuildPottery,
    //    BuildDryingPan,
    //    BuildButcher,
    //    BuildSmoker,
    //    BuildDryer,
    //    BuildCook,
    //    BuildSmelter,
    //    BuildFoundry,
    //    BuildSmith,
    //    BuildCarpenter,
    //    BuildChemist,
    //    BuildGunmaker,
    //    BuildArmory,
    //    BuildShieldMaker,

    //    BuildMaterialStorage,
    //    BuildFoodStorage,
    //    BuildWeaponStorage,
    //    BuildArmorStorage,
    //    BuildAnimalStorage,
    //    BuildCesspit,

    //    BuildSoldierBarracks,
    //    BuildArcherBarracks,
    //    BuildWarmachineBarracks,
    //    BuildGunBarracks,
    //    BuildCannonBarracks,
    //    BuildKnightsBarracks,

    //    BuildWoodCutter,
    //    BuildStoneCutter,

    //    BuildPavement,
    //    BuildPavementFlower,
    //    BuildStatue_ThePlayer,
    //    CITYONLY_END,//281

    //    NUM    
    //}
}
