using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Core.Wand.Data;
using VikingEngine.Core.Wand.Equipment;
using VikingEngine.Core.Wand.GO;
using VikingEngine.Core.Wand.MagicEffect;
using VikingEngine.Core.Wand.Map;
using VikingEngine.DebugExtensions;
using VikingEngine.EngineSpace;
using VikingEngine.LootFest.Map;

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

        List2<MagicWandZap> magicEffects = new List2<MagicWandZap>(2);

        Queue<AbsGameObject> magicTargetObjects = new Queue<AbsGameObject>();

        float moveTween = 0;
        public const float MoveTime = 300;
        float time = 0;

        bool waitForAsyncProcess = false;

        public void MovePlayer(IntVector2 move)
        {
            if (checkMoveInput(ref move))
            {
                
                var toPos = WandRef.world.hero.currentPos.MovePos(move);
                if (WandRef.world.map.CanMove_WallCheck(WandRef.world.hero.currentPos.tilePos, toPos.tilePos, out var door))
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

        public bool UseMagicWeapon(MagicWand weapon, IntVector2 dir)
        {
            if (checkMoveInput(ref dir))
            {
                var toPos = WandRef.world.hero.currentPos.MovePos(dir);
                if (WandRef.world.map.CanMove_WallCheck(WandRef.world.hero.currentPos.tilePos, toPos.tilePos, out var door) && door.wallType == ThinWall.None)
                {
                    WandRef.world.hero.TurnTowards(toPos.tilePos);
                    weapon.charges--;
                    turnState = TurnState.AnimateMagicWeapon;
                    magicState = 0;
                    magicEffects.Add(new MagicWandZap(weapon, toPos));
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
                        case TurnState.NUM:
                            turnState = 0;
                            break;
                    }
                }
            }
        }

        public void AnimateMagic()
        {
            if (magicEffects.Count > 0)
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

                        foreach (var m in magicEffects)
                        {
                            m.TweenUpdate(moveTween);
                        }
                        break;
                    case AnimateMagicState.CheckTrigger:
                        for (int i = magicEffects.Count-1; i >=0 ; --i)
                        {
                            var effect = magicEffects[i];
                            effect.currentPos = effect.nextPos;
                            effect.Next(effect.currentPos.ForwardPos());

                            bool wallHinder = !WandRef.world.map.CanMove_WallCheck(effect.currentPos.tilePos, effect.nextPos.tilePos, out var thinWall);

                            var tile = WandRef.world.map.tileGrid.Get(effect.currentPos.tilePos);
                            for (int effectIx = 0; effectIx < effect.magic.magicEffects.Count; effectIx++)
                            {
                                bool willTrigger = false;

                                switch (effect.magic.magicEffects[effectIx].triggerObject)
                                {
                                    case MagicTrigger_ObjectType.Creature:
                                        if (tile.creature.HasGameObject())
                                        {
                                            willTrigger = true;
                                            magicTargetObjects.Enqueue(tile.creature.Get());
                                        }
                                        break;
                                    case MagicTrigger_ObjectType.Any_Wall:
                                        if (effect.magic.magicEffects[effectIx].charges > 0)
                                        {
                                            if (thinWall.wallType == ThinWall.None || thinWall.wallType == ThinWall.Wall)
                                            {
                                                wallHinder = false;
                                            }
                                        }
                                        willTrigger = thinWall.wallType == ThinWall.Wall || tile.tileType == TileType.ThickWall;
                                        break;
                                    case MagicTrigger_ObjectType.Thin_Wall:
                                        if (effect.magic.magicEffects[effectIx].charges > 0)
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
                                    effect.magic.magicEffects[effectIx].charges--;
                                    switch (effect.magic.magicEffects[effectIx].effect)
                                    {
                                        case Data.MagicEffect.Damage:
                                            int damage = effect.magic.magicEffects[effectIx].triggerTier;
                                            while (magicTargetObjects.TryDequeue(out var target))
                                            {
                                                target.TakeDamage(damage);
                                            }
                                            break;
                                        case Data.MagicEffect.Open_wall:
                                            if (thinWall.wallType == ThinWall.Wall)
                                            {
                                                thinWall.wallPos.RemoveWall();
                                            }
                                            else
                                            {
                                                tile.tileType = TileType.Floor;
                                                RevealRoom.Reveal(effect.currentPos.tilePos);
                                            }
                                            WandRef.PlayScene.mapmodel.floorNeedsUpdate = true;
                                            break;
                                    }
                                }
                            }

                            if (wallHinder)
                            {
                                magicEffects[i].DeleteMe();
                                magicEffects.RemoveAt(i);
                            }
                        }

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
                        if (creaturesC.sel.ChaseHero() &&
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
                                    //BattleManager.Attack(creaturesC.sel, pcreature.Get()); 
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
        NUM
    }

    enum AnimateMagicState
    { 
        CheckTrigger,
        Move,
        NUM
    }
}
