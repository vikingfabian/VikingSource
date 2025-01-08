using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.Battle;
using VikingEngine.ToGG.HeroQuest.Data.UnitAction;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.Data;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.HeroStrategy
{
    abstract class AbsHeroStrategy : AbsStrategyCard
    {
        protected const string UnltimateTitle = "Ultimate";

        protected const string TwoAttacks = "Attack twice.";
        protected const string NoMove = "No movement.";

        public string name, description;
        public SpriteName cardSprite;
        public int staminaCost = 0;
        public CooldownCounter coolDownTurns = CooldownCounter.NoCoolDown;
        public bool useBloodRage = false;
        public ValueBar attacks;
        public int movement = 0;
        public bool groupAttack = false;

        int baseMoveActions, baseAttackActions;

        abstract public void ApplyToHero(Unit heroUnit, bool commit);

        protected void setMoveAttackCount(Unit heroUnit, int moves, int attacks)
        {
            baseMoveActions = moves;
            baseAttackActions = attacks;

            this.attacks = new ValueBar(attacks, attacks);
            movement = heroUnit.data.move * moves;
        }

        public override List<AbsRichBoxMember> actionsTooltip(AbsUnit unit)
        {
            ApplyToHero(unit.hq(), false);
            List<AbsRichBoxMember> result = new List<AbsRichBoxMember>(8);
            if (movement > 0)
            {
                result.Add(new RbText(movement.ToString()));
                result.Add(new RbImage(SpriteName.cmdUnitMoveGui_Small));
                result.Add(new RbText(" squares"));                
            }

            if (baseAttackActions > 0)
            {
                addNewLine();

                var wep = unit.hq().data.WeaponStats;
                int strength; bool melee;
                wep.strongestAttack(out strength, out melee);
                strength = modifiedAttackStrength(strength);

                result.Add(new RbText(baseAttackActions.ToString()));
                result.Add(new RbImage(melee? SpriteName.cmdUnitMeleeGui : SpriteName.cmdUnitRangedGui));

                var die = new RbImage(SpriteName.cmdDiceAttack);
                for (int i = 0; i < strength; ++i)
                {
                    result.Add(die);
                }
            }

            return result;

            void addNewLine()
            {
                if (result.Count > 0)
                {
                    result.Add(new RbNewLine());
                }
            }
        }

        virtual protected int modifiedAttackStrength(int baseStrength)
        {
            return baseStrength;
        }

        public static AbsHeroStrategy GetStrategy(HeroStrategyType type)
        {
            switch (type)
            {
                case HeroStrategyType.Advance:
                    return new Advance();

                case HeroStrategyType.FromTheShadows:
                    return new FromTheShadows();

                case HeroStrategyType.Trapper:
                    return new Trapper();

                case HeroStrategyType.PoisionBomb:
                    return new PoisionBomb();

                case HeroStrategyType.StunBomb:
                    return new StunBomb();

                case HeroStrategyType.KnifeDance:
                    return new KnifeDance();

                case HeroStrategyType.AimedShot:
                    return new AimedShot();

                case HeroStrategyType.Fight:
                    return new Fight();

                case HeroStrategyType.LineOfFire:
                    return new LineOfFire();

                case HeroStrategyType.LeapAttack:
                    return new LeapAttack();

                case HeroStrategyType.ArrowRain:
                    return new ArrowRain();

                case HeroStrategyType.Run:
                    return new Run();

                case HeroStrategyType.RunAndHide:
                    return new RunAndHide();

                case HeroStrategyType.Rest:
                    return new Rest();

                case HeroStrategyType.Swing3:
                    return new Swing3();

                case HeroStrategyType.UltimateHeroicCry:
                    return new UltimateHeroicCry();

                case HeroStrategyType.UltimatePiercingShot:
                    return new UltimatePiercingShot();

                case HeroStrategyType.UltimateShieldWall:
                    return new UltimateShieldWall();

                    case HeroStrategyType.UltimateEarthShake:
                    return new UltimateEarthShake();

                case HeroStrategyType.UltimateLootrun:
                    return new UltimateLootrun();
                                       
                default: throw new NotImplementedException();
            }
        }

        //virtual public void battleModifiers(BattleSetup coll)
        //{
        //}

        //public static SpriteName ActionIcon(ActionType type)
        //{
        //    switch (type)
        //    {
        //        case ActionType.Attack: return SpriteName.cmdUnitMeleeGui;
        //        case ActionType.Move: return SpriteName.cmdUnitMoveGui_Small;

        //        default: throw new NotImplementedException();
        //    }
        //}

        public void toDoList(Unit unit, List<Display.AbsToDoAction> list)
        {
            if (baseMoveActions > 0)
            {
                list.Add(new Display.ToDoMove(unit));
            }

            if (baseAttackActions > 0)
            {
                list.Add(new Display.ToDoAttack(unit));
            }
        }

        virtual public void collectActions(List<Data.UnitAction.AbsUnitAction> unitActions)
        {
            if (baseAttackActions > 0)
            {
                unitActions.Add(new Data.UnitAction.AttackTerrain());
            }
        }

        public void onTurnStart()
        {
            coolDownTurns.CountDown();
        }

        virtual public void onTurnEnd(Unit heroUnit) { }

        virtual public void onAttackCommit(AttackDisplay attack) { }

        abstract public HeroStrategyType Type { get; }

        override public int Id { get { return (int)Type; } }
        override public string Name { get { return name; } }
        override public string Description { get { return description; } }
        override public SpriteName CardSprite { get { return cardSprite; } }

        virtual public void collectBoardUiTargets(Unit heroUnit, List<IntVector2> boardUiTargets)
        {
            //return false;
            //boardUiTargets = heroUnit.attackTargets.targetPositions();
            //heroUnit.attackTargets.refr esh(heroUnit.squarePos);
        }

        virtual public List<AttackTarget> collectAttackGroup(Unit heroUnit, IntVector2 startPos, out List<AttackTarget> validTargets)
        {
            throw new NotImplementedExceptionExt("attackGroup from" + Type.ToString(), (int)Type);
        }

        protected bool tryAddTargetToGroup(IntVector2 pos, Unit heroUnit, IntVector2 startPos,
           List<AttackTarget> group, List<AttackTarget> validTargets, bool needRangeAndSight)
        {
            AttackTarget target = new AttackTarget(pos);

            if (AttackTargetCollection.CollectGroupTarget(heroUnit, startPos, ref target, needRangeAndSight))
            {
                if (target.unit != null)
                {
                    if (validTargets.Count == 0)
                    {
                        foreach (var m in group)
                        {
                            m.groupHasValidTarget = true;
                        }
                    }

                    validTargets.Add(target);
                }

                target.groupHasValidTarget = validTargets.Count > 0;
                group.Add(target);

                return true;
            }
            else
            {
                return false;
            }
        }

        public override List<StrategyCardResource> Resources(object tag, out bool available)
        {
            Unit heroUnit = tag as Unit;
            available = true;

            if (useBloodRage)
            {
                available = heroUnit.data.hero.bloodrage.IsMax;
                string desc = LanguageLib.BloodRage;
                if (!available)
                {
                    desc += TextLib.Parentheses(heroUnit.data.hero.bloodrage.BarText(), true); 
                }

                return new List<StrategyCardResource>
                {
                    new StrategyCardResource(SpriteName.cmdIconBloodrageSmall, 
                        SpriteName.cmdIconBloodrageSmallGray, 
                        desc,
                        new ValueBar(lib.BoolToInt01(available),1), 
                        available)
                };
            }
            else if (staminaCost > 0 || coolDownTurns.HasCooldown)
            {
                List<StrategyCardResource> result = new List<StrategyCardResource>(2);
                
                if (staminaCost > 0)
                {
                    string desc = LanguageLib.Stamina + 
                        TextLib.Parentheses(
                        TextLib.Divition(staminaCost, heroUnit.data.hero.stamina.Value), true);

                    if (heroUnit.data.hero.stamina.Value < staminaCost)
                    {
                        available = false;
                    }

                    result.Add(new StrategyCardResource(SpriteName.cmdIconStaminaSmall, 
                        SpriteName.cmdIconStaminaSmallGray, 
                        desc,
                        new ValueBar(heroUnit.data.hero.stamina.Value, staminaCost), 
                        available));                    
                }

                if (coolDownTurns.HasCooldown)
                {
                    string desc = "Cooldown turns";

                    if (coolDownTurns.IsReady())
                    {
                        desc += TextLib.Parentheses(coolDownTurns.cooldownTurns.ToString(), true);
                    }
                    else
                    {
                        available = false;
                        desc += TextLib.Parentheses(coolDownTurns.ValueBar().BarText(), true);
                    }

                    result.Add(new StrategyCardResource(SpriteName.cmdIconHourglassSmall, 
                        SpriteName.cmdIconHourglassSmallGray,
                        desc,
                        coolDownTurns.ValueBar(),
                        available));
                }

                return result;
            }

            return null;
        }

        //ValueBar healthStored, staminaStored;
        //int healthAdded, staminaAdded;

        virtual public void payForStrategy(bool pay_notReturn, Unit heroUnit)
        {
            var hero = heroUnit.data.hero;

            if (pay_notReturn)
            { coolDownTurns.SetNow(); }
            else
            { coolDownTurns.SetReady(); }

            if (useBloodRage)
            {
                if (pay_notReturn)
                { hero.bloodrage.Value = 0; }
                else
                { hero.bloodrage.setMax(); }
            }

            if (staminaCost > 0)
            {
                if (pay_notReturn)
                { hero.stamina.spend(staminaCost); }
                else
                { hero.stamina.add(staminaCost); }
            }
        }

        //protected void StoreHealth(bool store_notReset, Unit heroUnit)
        //{
        //    if (store_notReset)
        //    {
        //        healthStored = heroUnit.health;
        //    }
        //}
        override public bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return false;
        }

        public override void modLabel(BattleModifierLabel label)
        {
            throw new NotImplementedException();
        }

        
        public override void applyMod(BattleSetup setup)
        {
            throw new NotImplementedException();
        }

        public override List<HUD.RichBox.AbsRichBoxMember> menuToolTip(List<StrategyCardResource> resources)
        {
            var members = new List<HUD.RichBox.AbsRichBoxMember>
            {
                new HUD.RichBox.RbBeginTitle(),
                new HUD.RichBox.RbText(Name),
                new HUD.RichBox.RbNewLine(false),
                new HUD.RichBox.RbText(Description)
            };

            var unit = HeroQuest.hqRef.players.localHost.HeroUnit;
            var actions = actionsTooltip(unit);
            if (actions != null)
            {
                members.Add(new HUD.RichBox.RbNewLine(true));
                members.AddRange(actions);
            }

            if (arraylib.HasMembers(resources))
            {
                members.Add(new HUD.RichBox.RbNewLine(true));
                members.Add(new HUD.RichBox.RbBeginTitle());
                members.Add(new HUD.RichBox.RbText("Requirements"));
                members.Add(new HUD.RichBox.RbNewLine(false));

                for (int i = 0; i < resources.Count; ++i)
                {
                    var r = resources[i];
                    members.Add(new HUD.RichBox.RbImage(r.sprite));
                    members.Add(new HUD.RichBox.RbText(r.toolTipDesc,
                        r.available ? Color.White : HudLib.UnavailableRedCol));

                    members.Add(new HUD.RichBox.RbNewLine(false));
                }

                specialRequirements(unit, members);
            }

            //addTooltipText(members, Dir4.W);
            return members;
        }

        public override string ToString()
        {
            return "Hero Strategy - " + this.name;
        }

        public override BattleModificationType ModificationType => BattleModificationType.HeroStrategy;

        override public int ModificationUnderType { get { return (int)Type; } }
    }

    class Advance : AbsHeroStrategy
    {
        public Advance()
        {
            cardSprite = SpriteName.hqStrategyMoveAttack;
            name = "Advance";
            description = "Move and attack";
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 1, 1);
        }

        //public override List<ActionType> actionList()
        //{
        //    return new List<ActionType>() { ActionType.Move, ActionType.Attack };
        //}
        override public HeroStrategyType Type { get { return HeroStrategyType.Advance; } }
    }

    class AimedShot : AbsHeroStrategy
    {
        const int StrengthAdd = 2;

        public AimedShot()
        {
            cardSprite = SpriteName.hqStrategyAimedShot;
            name = "Aimed shot";
            description = "Make one " + TextLib.ValuePlusMinus(StrengthAdd) + " attack.";
            coolDownTurns = new CooldownCounter(1);
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 0, 1);
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return setup.targets.IsProjectile;
        }

        public override void applyMod(BattleSetup setup)
        {
            setup.AttackerSetup.modAttackStrength(StrengthAdd);
        }

        public override void modLabel(BattleModifierLabel label)
        {
            label.modSource(this);
            label.attackModifier(StrengthAdd);
        }

        //public override void battleModifiers(BattleSetup setup)
        //{
        //    if (setup.targets.IsProjectile)
        //    {
        //        setup.AttackerSetup.modAttackStrength(StrengthAdd);//coll.attackStrength += AttacksAdd;

        //        var label = setup.attackerSetup.beginModLabel();
        //        label.modSource(this);
        //        label.attackModifier(StrengthAdd);

        //        //coll.addModLabel(true,
        //        //                new Battle.BattleModifierIcon_StrategyCard2(cardSprite),
        //        //                new Battle.BattleModifierIcon_Arrow(),
        //        //                new Battle.BattleModifierIcon_Text(TextLib.ValuePlusMinus(StrengthAdd)),
        //        //                new Battle.BattleModifierIcon_Image(SpriteName.cmdDiceAttack));
        //    }
        //    base.battleModifiers(setup);
        //}

        protected override int modifiedAttackStrength(int baseStrength)
        {
            return baseStrength + StrengthAdd;
        }

        //public override List<ActionType> actionList()
        //{
        //    return new List<ActionType>() { ActionType.Attack };
        //}

        override public HeroStrategyType Type { get { return HeroStrategyType.AimedShot; } }
    }

    class Fight : AbsHeroStrategy
    {
        public Fight()
        {
            cardSprite = SpriteName.hqStrategyAttackAttack;
            name = "Fight";
            description = TwoAttacks + " " + NoMove;//"Attack twice. No movement.";
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 0, 2);
        }

        //public override List<ActionType> actionList()
        //{
        //    return new List<ActionType>() { ActionType.Attack };
        //}

        override public HeroStrategyType Type { get { return HeroStrategyType.Fight; } }
    }

    class Run : AbsHeroStrategy
    {
        public Run()
        {
            cardSprite = SpriteName.hqStrategyMoveMove;
            name = "Run";
            description = "Move double length. No attacks.";
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 2, 0);
        }

        //public override List<ActionType> actionList()
        //{
        //    return new List<ActionType>() { ActionType.Move };
        //}

        override public HeroStrategyType Type { get { return HeroStrategyType.Run; } }
    }

    class Rest : AbsHeroStrategy
    {
        public Rest()
        {
            cardSprite = SpriteName.hqStrategyRest;
            name = "Rest";
            description = "Move once, no attacks. You may rearrange your equipment." +
                Environment.NewLine +
                "At the end of turn: Gain a Rest Action. " +
                Environment.NewLine +
                HeroData.RestDesc();
        }

        public override List<AbsRichBoxMember> actionsTooltip(AbsUnit unit)
        {
            List <AbsRichBoxMember> result = base.actionsTooltip(unit);

            result.Add(new RbNewLine());
            result.Add(new RbImage(SpriteName.cmdBackpack));
            result.Add(new RbText("backpack"));

            HeroData.RestActionsDesc(result);
            return result;
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 1, 0);
        }

        public override void onTurnEnd(Unit heroUnit)
        {
            heroUnit.data.hero.rest(heroUnit);
        }

        override public HeroStrategyType Type { get { return HeroStrategyType.Rest; } }
    }
    
    class UltimatePiercingShot : AbsHeroStrategy
    {
        const int PierceAdd = 2;
        const int AttackCount = 2;

        public UltimatePiercingShot()
        {
            cardSprite = SpriteName.hqStrategyPiercingShot;
            name = UnltimateTitle;//"Piercing shot";
            description = TwoAttacks + " Ranged attacks gain " + LanguageLib.AccValue(PierceAdd) + "Pierce. " + NoMove;
            useBloodRage = true;
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 0, AttackCount);
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return setup.targets.IsProjectile;
        }

        public override void applyMod(BattleSetup setup)
        {
            setup.attackerSetup.pierce += PierceAdd;
        }

        public override void modLabel(BattleModifierLabel label)
        {
            label.modSource(this);
            label.content.Add(new RbText(PierceAdd.ToString()));
            label.content.Add(new RbImage(SpriteName.cmdPierce));
        }

        //public override void battleModifiers(BattleSetup setup)
        //{
        //    if (setup.targets.IsProjectile)
        //    {
        //        setup.attackerSetup.pierce += PierceAdd;

        //        var label = setup.attackerSetup.beginModLabel();
        //        label.modSource(this);
        //        label.members.Add(new RichBoxText(PierceAdd.ToString()));
        //        label.members.Add(new RichBoxImage(SpriteName.cmdPierce));

        //        //coll.addModLabel(true,
        //        //    new Battle.BattleModifierIcon_StrategyCard2(cardSprite),
        //        //    new Battle.BattleModifierIcon_Arrow(),
        //        //    new Battle.BattleModifierIcon_Text(PierceAdd.ToString()),
        //        //    new Battle.BattleModifierIcon_Image(SpriteName.cmdPierce));                
        //    }

        //    base.battleModifiers(setup);
        //}

        //public override List<ActionType> actionList()
        //{
        //    return new List<ActionType>() { ActionType.Attack };
        //}

        override public HeroStrategyType Type { get { return HeroStrategyType.UltimatePiercingShot; } }
    }

    class UltimateHeroicCry : AbsHeroStrategy
    {
        const int StrengthAdd = 1;

        int healthAdd, staminaAdd;

        public UltimateHeroicCry()
        {
            cardSprite = SpriteName.hqStrategyHeroic;
            name = UnltimateTitle;
            description = "Restore all health and Stamina. Move and make one " + TextLib.ValuePlusMinus(StrengthAdd) + " attack.";
            useBloodRage = true;
        }

        public override void ApplyToHero(Unit heroUnit, bool commit)
        {
            setMoveAttackCount(heroUnit, 1, 1);
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return true;
        }

        public override void applyMod(BattleSetup setup)
        {
            setup.AttackerSetup.modAttackStrength(StrengthAdd);
        }

        public override void modLabel(BattleModifierLabel label)
        {
            label.modSource(this);
            label.attackModifier(StrengthAdd);
        }

        //public override void battleModifiers(BattleSetup coll)
        //{
        //    coll.AttackerSetup.modAttackStrength(StrengthAdd);

        //    var label = coll.attackerSetup.beginModLabel();
        //    label.modSource(this);
        //    label.attackModifier(StrengthAdd);

        //    //coll.addModLabel(true,
        //    //    new Battle.BattleModifierIcon_StrategyCard2(cardSprite),
        //    //    new Battle.BattleModifierIcon_Arrow(),
        //    //    new Battle.BattleModifierIcon_Text(TextLib.ValuePlusMinus(StrengthAdd)),
        //    //    new Battle.BattleModifierIcon_Image(SpriteName.cmdDiceAttack));

        //    base.battleModifiers(coll);
        //}

        protected override int modifiedAttackStrength(int baseStrength)
        {
            return baseStrength + StrengthAdd;
        }

        public override void payForStrategy(bool pay_notReturn, Unit heroUnit)
        {
            base.payForStrategy(pay_notReturn, heroUnit);

            if (pay_notReturn)
            {
                var prevHealth = heroUnit.health;
                var prevStamina = heroUnit.data.hero.stamina;

                //heroUnit.heal(new HealSettings(byte.MaxValue, HealType.Will));
                new HealUnit(heroUnit, byte.MaxValue, HealType.Will, false);
                heroUnit.addStamina(byte.MaxValue);

                healthAdd = heroUnit.health.CompareValueAdded(prevHealth);
                staminaAdd = heroUnit.data.hero.stamina.CompareValueAdded(prevStamina);
            }
            else
            {
                heroUnit.health.add(-healthAdd);
                heroUnit.data.hero.stamina.add(-staminaAdd);
            }
        }
        
        override public HeroStrategyType Type { get { return HeroStrategyType.UltimateHeroicCry; } }
    }

   
    enum ActionType
    {
        Move,
        Attack,
        UseSkill,
    }
}
