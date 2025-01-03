using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    static class toggLib
    {
        public static void Init()
        {
            Vector2 upDir = ModelCamFacingAngle.Direction(1f);
            UpVector = new Vector3(0, -upDir.Y, upDir.X);
            TowardCamVector = new Vector3(0, -upDir.Y, -upDir.X);
            {
                float oneMulti = 1f / Math.Abs(upDir.Y);
                TowardCamVector_Yis1 = TowardCamVector * oneMulti;
        } }

        const bool DebugQuickAnimate = true;
        
        #region INPUT
        
        
        public const SpriteName ButtonIcon_MoreInfo = SpriteName.cmdSpyglass;
        #endregion

        public const bool ViewDebugInfo = true;

        
        public static readonly Color HealthCol = new Color(36, 216, 27);
        public static readonly Color StaminaCol = new Color(236, 205, 39);
        public static readonly Color StaminaTextCol = new Color(251, 242, 54);
        public static readonly Color BloodRageTextCol = new Color(255,216,0);
        public static readonly Color NegativeTextCol = new Color(244, 34, 0);

        #region BATTLE

        public const float CloseCombatHitChance = 0.4f;
        public const float RangedCombatHitChance = 0.3f;
        public const float CloseCombatHitChance_LightUnit = CloseCombatHitChance * 0.5f;

        public const float CloseCombatRetreatChance = 0.2f;
        public const float RangedCombatRetreatChance = 0.1f;
        
        public const float CloseCombatRetreatChance_LightUnit = 0.1f;

        public const float DamagingTerrainHitChance = 0.3f;

        public const int AimPropertyBonus = 2;
        public const float ShieldDashPropertyRetreatBonus = 0.1f;
        public const float LeaderPropertyRetreatBonus = 0.05f;

        public const float FearSupportPropertyRetreatBonus = 0.1f;
        public const float LevelUpHitChanceBonus = 0.1f;

        public const float CatapultPlusCenterHitChance = 0.6f;

        #endregion

        #region VP
        public const int VP_DestroyValuableUnit = 4;
        public const int VP_DestroyUnit = 2;
        public const int VP_TacticalBanner = 1;
        public const int VP_DestroyEnemyBase = 4;
        public const int WinnerScore = 14;
        
        #endregion

        #region NETWORK

        public const int NetworkGamestatePropertyIndex = Network.Session.NumReservedProperties;
        public const int NetworkProtperty_Lobby = 0;
        public const int NetworkProtperty_Playing = 1;
        public const int NetworkProtperty_Other = 2;
        #endregion

        #region SAVE
        public const string MapsSaveFolder = "ToGG Map Save";
        public const string SaveStateFolder = "ToGG Save State";
        public const string ContentFolder = "ToggContent\\";
        public const string ContentMapsFolder = ContentFolder + "Maps";
        #endregion

        public static readonly IntervalF BoardSizeLimits_Width = new IntervalF(6, 50);
        public static readonly IntervalF BoardSizeLimits_Height = new IntervalF(4, 50);
        public const bool MouseInputMode = true;

        const float UnitPlaneRotY = 0.5f;
        public static readonly RotationQuarterion PlaneTowardsCam = new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(0, UnitPlaneRotY, 0f));
        public static Vector3 UpVector, TowardCamVector, TowardCamVector_Yis1;

        public static float AnimTime(float ms)
        {
            if (PlatformSettings.DevBuild && DebugQuickAnimate)
            {
                return 1;
            }
            else
            {
                return ms;
            }
        }

        public static RotationQuarterion PlaneTowardsCamWithRotation(float rotation)
        {
            RotationQuarterion result = RotationQuarterion.Identity;
            result.RotateWorldX(-rotation);
            result.RotateWorldY(UnitPlaneRotY);

            return result;
        }

        public static Vector3 ToV3(Vector2 pos)
        {
            Vector3 result = Vector3.Zero;
            result.X = pos.X;
            result.Z = pos.Y;
            return result;
        }
        public static Vector3 ToV3(Vector2 pos, float height)
        {
            Vector3 result = Vector3.Zero;
            result.X = pos.X;
            result.Y = height;
            result.Z = pos.Y;
            return result;
        }
        public static Vector3 ToV3(IntVector2 pos)
        {
            Vector3 result = Vector3.Zero;
            result.X = pos.X;
            result.Z = pos.Y;
            return result;
        }

        public static Vector3 ToV3(IntVector2 pos, float height)
        {
            Vector3 result = Vector3.Zero;
            result.X = pos.X;
            result.Y = height;
            result.Z = pos.Y;
            return result;
        }

        public static Vector2 ToV2(Vector3 pos)
        {
            Vector2 result = Vector2.Zero;
            result.X = pos.X;
            result.Y = pos.Z;
            return result;
        }

        public static Dir8 GeneralDirection(IntVector2 from, IntVector2 to, bool randomIfNone)
        {
            IntVector2 dir = to - from;
            Dir8 result = conv.ToDir8(dir);

            if (result == Dir8.NO_DIR && randomIfNone)
            {
                result = Ref.rnd.Dir8();
            }

            return result;
        }

        public static bool BottomPlayer(int playerNumber)
        {
            return playerNumber == Commander.Players.LocalPlayer.PlayerNumberOne;
        }

        public static readonly Graphics.CustomEffect ModelEffect = new  Graphics.CustomEffect("FlatVerticeColor", false);
        public static readonly Rotation1D ModelCamFacingAngle = Rotation1D.FromDegrees(-36);
       
        public static Graphics.PolygonColor CamFacingPolygon(Vector3 place, Vector2 size, Graphics.Sprite imageFile, Color color)
        {
            Vector3 up = UpVector * size.Y;

            Vector3 sw = place;
            sw.X -= size.X * 0.5f;
            Vector3 se = sw;
            se.X += size.X;

            Vector3 nw = sw;
            nw.Y += up.Y;
            nw.Z += up.Z;

            Vector3 ne = nw;
            ne.X += size.X;

            return new Graphics.PolygonColor(nw, ne, sw, se,
                  imageFile, color);
        }

        public static Vector3 MoveInCamFacePlane(Vector2 move)
        {
            Vector3 result = UpVector * -move.Y;
            result.X += move.X;

            return result;
        }

        public static RotationQuarterion Rotation1DToQuaterion(float rotation)
        {
            RotationQuarterion result = RotationQuarterion.Identity;
            result.RotateWorldX(MathHelper.TwoPi - rotation);
            return result;
        }

        public static Graphics.ImageGroupParent2D VpSquareCard()
        {
            Graphics.Image icon;
            var images = HudLib.HudCardBasics(null, "Strategic point", SpriteName.cmdBanner, 1f, out icon);

            return images;
        }
        public static Graphics.ImageGroupParent2D EscapePointCard()
        {
            Graphics.Image icon;
            var images = HudLib.HudCardBasics(null, "Escape route", SpriteName.EditorForwardArrow, 1f, out icon);

            return images;
        }

        public static List<IntVector2> ToPositions(List<AbsUnit> units)
        {
            List<IntVector2> result = new List<IntVector2>(units.Count);
            foreach (var m in units)
            {
                result.Add(m.squarePos);
            }

            return result;
        }

        //public static void TextAnimation(string text, Color textCol, Color bgCol, IntVector2 pos)
        //{
        //    Graphics.Text3DBillboard textbb = new Graphics.Text3DBillboard(LoadedFont.Regular, text, textCol, bgCol,
        //        toggRef.board.toWorldPos_Center(pos, 1.2f),//VectorExt.AddY(soldierModel.Position, 1.2f),
        //        0.26f, 1f, true);

        //    const float ViewTime = 2000;
        //    new Graphics.Motion3d(Graphics.MotionType.MOVE, textbb, VectorExt.V3FromZ(-0.6f), Graphics.MotionRepeate.NO_REPEAT, ViewTime, true);
        //    new Graphics.Motion3d(Graphics.MotionType.OPACITY, textbb, VectorExt.V3NegOne, Graphics.MotionRepeate.NO_REPEAT, ViewTime, true);
        //    new Timer.Terminator(ViewTime, textbb);
        //}
    }
    
    enum PlayerRelation
    {
        Myself,
        Allied,
        Opponent,
        Neutral,
    }

    enum LevelEnum
    {
        NONE,
        OrderMoveAttack = 1,
        FirstBattle = 2,
        RangeSupport = 3,
        Backstap = 4,
        MoveAllStrategy = 5,
        SupportChallenge = 6,
        StrategyCardsChallenge = 7,
        Resting = 8,
        RestingChallenge = 9,
        UseSpyglass = 10,

        Story1Practice1 = 11,
        Story1Overruned = 12,
        Story1CitySupport = 13,
        Story1BreakSiege = 14,
        Story1SaveTheEngineer = 15,
        Story1TheGate = 16,
        Story1Skirmish = 17,
        Story1StormTheCastle = 18,
        Story1Boss = 19,
        Story1Practice2 = 20,

        TestAi,
        testmix,
        NUM,
    }

    enum GameCategory
    {
        QuickMatch,
        Tutorial,
        Story1,        
    }

    enum ArmyScale
    {
        Standard,
        Double,
    }

    enum ValueType
    {
        Health,
        Stamina,
        BloodRage,
    }

    enum DamageAnimationType
    {
        None,
        AttackSlash,
    }

    enum UnitPropertyType
    {
        Light, //lägre träff chans på CC, kan fly igenom terräng
        Block,
        Arrow_Block,
        Over_shoulder, //Skjuter över axeln på intilliggande
        Aim, //Stå still o skjuta ger extra attack
        Flank_support, //Ger två tärningar support
        Leader, //Boostar intill liggander enheter
        Valuable, //Ger mer VP att slå ut
        Base, //Tält som fienden får VP för att slå ut
        Shield_dash, //pressar fienden till att backa

        Charge, //+1 attack om går innan attack
        Frenzy, //Extra attack ifall man follow up
        Slippery, //Hits blir retreat
        Ignore_terrain, //can gå igenom "must stop" utan att stanna
        Cant_retreat, //Tar hits istället för att backa
        Fear, //Flippar hit och retreat chance
        Fear_support, //Extra retreat chance till intilliggande vän
        Necromancer, //om intilliggande motståndare dör, omvandlas till 2? skelett
        Sucide_attack, //20% chans att offra sig och döda en moståndare
        Static_target, //Impossible to miss
        Backstab_expert, //Wont miss backstabs
        Unknown1,
        Unknown2,
        Unknown3,
        Unknown4,
        Level_2,//+10% hit 
        Level_3,//+10% hit, +1 attack
        Max_Level,//+10% hit, +1 attack, 1 block
        Catapult, //Bomb attack, and cant move and attack
        Catapult_Plus,
        Spawn_point,
        Pierce,//Ignore block
        Flying,

        Pet,
        Spotter,
        Scratchy,
        Bark,
        AutoAttack,
        Dull_Weapon,
        SlowAttack,
        Sleepy,
        FireBreath,
        PushSpecial,
        Swing,
        TargetX,
        OpenTarget,
        Undead,
        //Grapple,
        MonsterCommander,
        MonsterBoss,
        Parry,
        NetspitSpecial,
        Retaliate,
        HeavyShield,
        DoubleAttack,

        SurgeOptionGain_group,
        Action_group,

        Regenerate,
        DeliveryService,
        LootCollector,
        LootFinder,
        SlipThrough,

        CarryItem,
        UnitAiObjective,
        UnitAiObjectiveAlways,
        UnitAiIdle,
        DeathPoisionArea,

        Expendable,
        Body_snatcher,

        Num_Non,
    }
}
