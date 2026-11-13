using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Core.Wand.Data;
using VikingEngine.Core.Wand.Equipment;
using VikingEngine.Core.Wand.GO;
using VikingEngine.Core.Wand.GO.Item;
using VikingEngine.Core.Wand.MagicEffect;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Core.Wand.Scene;
using VikingEngine.DebugExtensions;
using VikingEngine.EngineSpace;
using VikingEngine.LootFest.Map;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.Core.Wand.Laws
{
    class TurnOrderManager
    {
        Queue<AttackData> queuedAttacks = new Queue<AttackData>(8);
        StructList<IntVector2> pathResult = new StructList<IntVector2>(128);
        public AStarPathfinder pathfinder = new AStarPathfinder();
        Time waitTime = Time.Zero;
        public TurnState turnState = 0;
        AnimateMagicState magicState = 0;

        public List2<MagicCastEffect> magicCasts = new List2<MagicCastEffect>(2);

        Queue<AbsGameObject> magicTargetObjects = new Queue<AbsGameObject>();

        float moveTween = 0;
        public const float MoveTime = 220;
        float time = 0;

        bool waitForAsyncProcess = false;

        public void MovePlayer(IntVector2 move)
        {
            if (checkMoveInput(ref move))
            {
                
                var toPos = WandRef.world.hero.currentPos.MovePos(move);
                if (WandRef.world.map.CanMove_WallCheck(WandRef.world.hero.currentPos.tilePos, toPos.tilePos, true, out var door))
                {
                    turnState = TurnState.BeginTurn;

                    if (WandRef.world.map.CanMove_CreatureCheck(toPos.tilePos, out var pcreature))
                    {
                        //MOVE
                        WandRef.world.hero.SetPos(toPos);

                        if (door.wallType == Map.ThinWall.Door)
                        {
                            door.wallPos.RemoveWall();
                        }

                        //Item pick check
                        Tile tile = WandRef.world.map.tileGrid.Get(toPos.tilePos);
                        if (tile.item.HasGameObject())
                        {
                            switch (tile.item.itemType)
                            {
                                case GO.ObjectPointer.TileItemType.MagicWand:
                                    if (WandRef.PlayScene.player.equipment.wands.Count < Player.EquipmentSetup.MaxWandCount)
                                    {
                                        var wand = ((MagicWandItem)ObjectLifeTime.DestroyItem(toPos.tilePos)).wand;
                                        WandRef.PlayScene.player.equipment.wands.Add(wand);
                                    }
                                    break;
                                case GO.ObjectPointer.TileItemType.HealthPotion:
                                    {
                                        var tier = ObjectLifeTime.DestroyItem(toPos.tilePos).itemTier;
                                        WandRef.world.hero.Heal(2 * tier);
                                    }
                                    break;
                                case GO.ObjectPointer.TileItemType.Book:
                                    {
                                        var tier = ObjectLifeTime.DestroyItem(toPos.tilePos).itemTier;
                                        WandRef.PlayScene.player.FoundMagic(tier);
                                    }
                                    break;

                            }
                        }

                        WandRef.PlayScene.player.MenuNeedRefresh();
                    }
                    else
                    { 
                        //ATTACK
                         WandRef.world.hero.TurnTowards(toPos.tilePos);
                        queuedAttacks.Enqueue(new AttackData(WandRef.world.hero, pcreature.Get()));
                    }
                }
            }
        }

        public  void OnPlayerDeath()
        {
            waitTime = new Time(1, TimeUnit.Seconds);
            //turnState = TurnState.PlayerDeath;
        }

        public bool UseMagicWeapon(MagicWand weapon, IntVector2 dir)
        {
            if (checkMoveInput(ref dir))
            {
                var toPos = WandRef.world.hero.currentPos.MovePos(dir);
                if (WandRef.world.map.CanMove_WallCheck(WandRef.world.hero.currentPos.tilePos, toPos.tilePos, false, out var door))
                {
                    WandRef.world.hero.TurnTowards(toPos.tilePos);
                    weapon.charges--;
                    turnState = TurnState.AnimateMagicWeapon;
                    magicState = 0;
                    magicCasts.Add(new MagicCastEffect(weapon, toPos));
                    WandRef.PlayScene.player.MenuNeedRefresh();

                    return true;
                }
            }
            return false;
        }

        bool checkMoveInput(ref IntVector2 move)
        {
            if (move.HasValue())
            {
                if (move.X != 0 && move.Y != 0)
                {
                    move.Y = 0;
                }
                return true;
            }
            return false;
        }

        public void update()
        {
            if (waitForAsyncProcess)
            {
                return;
            }

            if (turnState > TurnState.WaitingForInput)
            {
                if (waitTime.CountDown())
                {
                    if (WandRef.world.hero.health.IsZero)
                    {
                        new WandPlayScene();
                        return;
                    }

                    switch (turnState)
                    {
                        case TurnState.AnimateMagicWeapon:
                            AnimateMagic();
                            break;
                        case TurnState.BeginTurn:
                            waitTime = new Time(0.2f, TimeUnit.Seconds);
                            turnState = TurnState.AnimatingEnemies;
                            break;
                        case TurnState.AnimatingEnemies:                           
                            MoveEnemies();
                            turnState++;
                            break;
                        case TurnState.Attacks:
                            while (queuedAttacks.TryDequeue(out var attack))
                            {
                                BattleManager.Attack(attack);
                            }
                            turnState++;
                            break;
                        case TurnState.EndTurn:
                            turnState = 0;
                            if (++WandRef.world.SpawnTimer > 30)
                            {
                                if (WandRef.world.map.tileGrid.Get(WandRef.world.map.playerEnterPos).creature.HasGameObject() == false)
                                {
                                    ObjectLifeTime.SpawnCreature(WandRef.world.map.playerEnterPos, GO.ObjectPointer.CreatureType.BlackKnight, true);
                                    WandRef.world.SpawnTimer = 5;
                                }
                            }
                            break;

                        case TurnState.PlayerDeath:
                            
                            break;
                    }

                    
                }
            }
        }

        public void AnimateMagic()
        {
            if (magicCasts.Count > 0)
            {
                switch (magicState)
                {
                    case AnimateMagicState.Move:
                        time += Ref.DeltaGameTimeMs;
                        moveTween = time / MoveTime;
                        if (moveTween >= 1f)
                        {
                            time = 0;
                            moveTween = 1f;
                            magicState++;
                        }

                        foreach (var m in magicCasts)
                        {
                            m.TweenUpdate(moveTween);
                        }
                        break;
                    case AnimateMagicState.CheckTrigger:
                        for (int i = magicCasts.Count-1; i >=0 ; --i)
                        {                            
                            magicCastTriggerCheck(i);
                        }
                        WandRef.PlayScene.player.MenuNeedRefresh();
                        magicState++;
                        break;
                    case AnimateMagicState.NUM:
                        magicState = 0;
                        break;
                }
            }
            else
            {
                turnState = TurnState.BeginTurn;
            }
        }

        private void magicCastTriggerCheck(int magicCastIndex)
        {
            var cast = magicCasts[magicCastIndex];
            cast.currentPos = cast.nextPos;
            cast.Next(cast.currentPos.ForwardPos());

            bool triggerEnd = false;
            bool wallHinder = !WandRef.world.map.CanMove_WallCheck(cast.currentPos.tilePos, cast.nextPos.tilePos, false, out var thinWall);

            var tile = WandRef.world.map.tileGrid.Get(cast.currentPos.tilePos);

            int start = 0;
            int exend = cast.magic.magicEffects.Count;

            if (cast.magic.magicEffects[0].effect == Data.MagicEffect.Activate)
            {
                if (cast.activated)
                {
                    start = 1;
                }
                else
                {
                    exend = 1;
                }
            }

            for (int effectIx = start; effectIx < exend; effectIx++)
            {
                bool willTrigger = false;
                bool removeFluid = false;
                bool hasTriggered = false;

                switch (cast.magic.magicEffects[effectIx].trigger)
                {
                    case MagicTrigger.Fire_north:
                        willTrigger = cast.currentPos.direction == Dir4.N;
                        hasTriggered = willTrigger;
                        break;
                    case MagicTrigger.Fire_south:
                        willTrigger = cast.currentPos.direction == Dir4.S;
                        hasTriggered = willTrigger;
                        break;
                }

                switch (cast.magic.magicEffects[effectIx].triggerObject)
                {
                    case MagicTrigger_ObjectType.Any:
                        willTrigger = tile.creature.HasGameObject() || tile.item.HasGameObject();
                        break;
                    case MagicTrigger_ObjectType.Blood:
                        willTrigger = tile.fluid.HasValue && tile.fluid.fluidType == FluidType.Blood;
                        removeFluid = true;
                        break;
                    case MagicTrigger_ObjectType.Creature:
                        if (tile.creature.HasGameObject())
                        {
                            willTrigger = true;
                            magicTargetObjects.Enqueue(tile.creature.Get());
                        }
                        break;
                    case MagicTrigger_ObjectType.Any_Wall:
                        if (cast.magic.magicEffects[effectIx].charges > 0)
                        {
                            if (thinWall.wallType == ThinWall.None || thinWall.wallType == ThinWall.Wall)
                            {
                                wallHinder = false;
                            }
                        }
                        willTrigger = thinWall.wallType == ThinWall.Wall || tile.tileType == TileType.ThickWall;
                        break;
                    case MagicTrigger_ObjectType.Thin_Wall:
                        if (cast.magic.magicEffects[effectIx].charges > 0)
                        {
                            if (thinWall.wallType == ThinWall.Wall)
                            {
                                willTrigger = true;
                                wallHinder = false;
                            }
                        }
                        break;
                    
                }


                

                if (willTrigger)
                {
                    cast.magic.magicEffects[effectIx].charges--;
                    switch (cast.magic.magicEffects[effectIx].effect)
                    {
                        case Data.MagicEffect.Activate:
                            hasTriggered = true;
                            cast.activated = true;
                            break;
                        case Data.MagicEffect.Damage:
                            {
                                int damage = cast.magic.magicEffects[effectIx].PowerAndBoost;
                                while (magicTargetObjects.TryDequeue(out var target))
                                {
                                    hasTriggered =target.TakeDamage(damage);
                                }
                            }
                            break;
                        case Data.MagicEffect.Leech:
                            if (tile.creature.HasGameObject())
                            {
                                if (!WandRef.world.hero.health.IsMax)
                                {
                                    var target = tile.creature.Get();
                                    int leech = lib.SmallestValue(WandRef.world.hero.health.ValueRemoved, target.health.Value, cast.magic.magicEffects[effectIx].PowerAndBoost);

                                    if (target.TakeDamage(leech))
                                    {
                                        WandRef.world.hero.Heal(leech);

                                        hasTriggered = true;
                                    }
                                }
                            }
                            break;
                        case Data.MagicEffect.Execute_damaged:
                            if (tile.creature.HasGameObject())
                            {
                                var target = tile.creature.Get();
                                if (!target.health.IsMax)
                                {
                                    hasTriggered = target.TakeDamage(target.health.Value);
                                }
                            }
                            break;
                        case Data.MagicEffect.Open_wall:
                            if (thinWall.wallType == ThinWall.Wall)
                            {
                                thinWall.wallPos.RemoveWall();
                                hasTriggered = true;
                            }
                            else
                            {
                                tile.tileType = TileType.Floor;
                                RevealRoom.Reveal(cast.currentPos.tilePos);
                                hasTriggered = true;
                            }
                            WandRef.PlayScene.mapmodel.floorNeedsUpdate = true;
                            break;
                        case Data.MagicEffect.Explode:
                            PhysicsManager.Explosion(cast.currentPos.tilePos, cast.magic.magicEffects[effectIx].PowerAndBoost);
                            hasTriggered = true;
                            break;
                        case Data.MagicEffect.Heal_target:
                            if (tile.creature.HasGameObject())
                            {
                                var target = tile.creature.Get();
                                if (!target.health.IsMax)
                                {
                                    hasTriggered = true;
                                    target.Heal(cast.magic.magicEffects[effectIx].PowerAndBoost);
                                }
                            }
                            break;
                        case Data.MagicEffect.Heal_user:
                            if (!WandRef.world.hero.health.IsMax)
                            {
                                hasTriggered = true;
                                WandRef.world.hero.Heal(cast.magic.magicEffects[effectIx].PowerAndBoost);
                            }
                            break;
                        case Data.MagicEffect.Magic_up:
                            for (int othereffectIx = 0; othereffectIx < cast.magic.magicEffects.Count; othereffectIx++)
                            {
                                hasTriggered = true;
                                if (othereffectIx != effectIx)
                                {
                                    cast.magic.magicEffects[othereffectIx].MagicUp();
                                }
                            }
                            break;
                        case Data.MagicEffect.Clone:
                            if (tile.creature.HasGameObject())
                            {
                                var target = tile.creature.Get();
                                hasTriggered = ObjectLifeTime.CloneObject(target);
                            }
                            if (tile.item.HasGameObject())
                            {
                                var target = tile.item.Get();
                                hasTriggered = ObjectLifeTime.CloneObject(target);
                            }
                            break;
                        case Data.MagicEffect.Level_up:
                            if (tile.creature.HasGameObject())
                            {
                                var target = tile.creature.Get();
                                hasTriggered = ObjectLifeTime.LevelUpObject(target, cast.magic.magicEffects[effectIx].PowerAndBoost);
                            }
                            if (tile.item.HasGameObject())
                            {
                                var target = tile.item.Get();
                                hasTriggered = ObjectLifeTime.LevelUpObject(target, cast.magic.magicEffects[effectIx].PowerAndBoost);
                            }
                            break;
                        case Data.MagicEffect.End:
                            triggerEnd = true;
                            hasTriggered = true;
                            break;
                    }
                }

                if (hasTriggered)
                {
                    cast.magic.magicEffects[effectIx].isFound = true;

                    if (removeFluid)
                    {
                        tile.fluid = Fluid.Empty;
                        WandRef.PlayScene.mapmodel.decalsNeedsUpdate = true;
                    }
                }
            }

            if (wallHinder || triggerEnd)
            {
                //RUN ON_COMPLETE
                if (cast.activated)
                {
                    for (int effectIx = 0; effectIx < cast.magic.magicEffects.Count; effectIx++)
                    {
                        if (cast.magic.magicEffects[effectIx].trigger == MagicTrigger.OnComplete)
                        {
                            bool isFound = false;
                            switch (cast.magic.magicEffects[effectIx].effect)
                            {
                                case Data.MagicEffect.Explode:
                                    PhysicsManager.Explosion(cast.currentPos.tilePos, cast.magic.magicEffects[effectIx].PowerAndBoost);
                                    isFound = true;
                                    break;
                                case Data.MagicEffect.Teleport_user:
                                    if (tile.creature.HasGameObject() == false)
                                    {
                                        WandRef.world.hero.SetPos(new MapPlacement(cast.currentPos.tilePos, WandRef.world.hero.currentPos.direction));
                                        isFound = true;
                                    }
                                    break;
                                case Data.MagicEffect.Back_fire:
                                    WandRef.world.hero.TakeDamage(cast.magic.magicEffects[effectIx].PowerAndBoost);
                                    isFound = true;
                                    break;
                                case Data.MagicEffect.Heal_user:
                                    if (!WandRef.world.hero.health.IsMax)
                                    {
                                        WandRef.world.hero.Heal(cast.magic.magicEffects[effectIx].PowerAndBoost);
                                        isFound = true;
                                    }
                                    break;
                            }

                            cast.magic.magicEffects[effectIx].isFound = isFound;
                        }
                    }
                }
                magicCasts[magicCastIndex].DeleteMe();
                magicCasts.RemoveAt(magicCastIndex);
            }
        }

        public void MoveEnemies()
        {
            waitForAsyncProcess = true;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var creaturesC = WandRef.world.creatures.counter();
                    while (creaturesC.Next())
                    {
                        if (creaturesC.sel.summoningSickness)
                        {
                            creaturesC.sel.summoningSickness = false;
                        }
                        else if (creaturesC.sel.ChaseHero() &&
                            pathfinder.FindPath(WandRef.world.map,
                             creaturesC.sel.currentPos.tilePos, WandRef.world.hero.currentPos.tilePos, ref pathResult) && pathResult.Count > 1)
                        {
                            IntVector2 toPos = pathResult.Array[1];
                            if (WandRef.world.map.CanMove_CreatureCheck(toPos, out var pcreature))
                            {
                                //MOVE
                                creaturesC.sel.SetPos(creaturesC.sel.currentPos.MoveTo(toPos));
                            }
                            else 
                            {
                                creaturesC.sel.TurnTowards(toPos);
                                if (pcreature.creatureType == GO.ObjectPointer.CreatureType.Hero)
                                {
                                    //ATTACK
                                    queuedAttacks.Enqueue(new AttackData(creaturesC.sel, pcreature.Get()));
                                }
                            }
                        }
                    }
                    waitForAsyncProcess = false;
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }

            });
        }
    }

    enum TurnState
    { 
        WaitingForInput,
        AnimateMagicWeapon,
        BeginTurn,
        AnimatingPlayer,
        AnimatingEnemies,
        Attacks,
        EndTurn,
        PlayerDeath,
    }

    enum AnimateMagicState
    { 
        CheckTrigger,
        Move,
        NUM
    }
}
