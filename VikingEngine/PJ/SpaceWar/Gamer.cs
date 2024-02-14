using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.PJ.SpaceWar.SpaceShip;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.PJ.SpaceWar
{
    class Gamer
    {
        const float Zoom = 40;
        const int MaxNoseBombs = 2;
        const int MaxTailSegments = 10;
        const int MaxTotalSegments = MaxTailSegments + 1;

        SafeCollectAsynchList<AbsGameObject> gameobjects = new SafeCollectAsynchList<AbsGameObject>(16);
        List2<AbsBodySegment> allSegments = new List2<AbsBodySegment>(MaxTotalSegments);
        List<GameObjects.NoseBomb> noseBombs = new List<GameObjects.NoseBomb>(MaxNoseBombs);
        Head head;
        //int playerIx;
        Graphics.TopDownCamera camera;        
        
        bool turnRight = false;
        int ticketsUsed = 0; 
        public int ticketsAvailable = 0;
        public int money = 0;
        GamerHUD hud;
        Physics.CircleBound quickBound = new Physics.CircleBound();

        CirkularList<PositionHistory> moveHistory = new CirkularList<PositionHistory>(MaxTailSegments * BodySegment.ExpandedSegmentFramesDist);

        GameObjects.ShopSquare onShop = null;
        bool buyEvent = false;
        Time buyLockTime = 0;
        Time viewDeathTimer = 0;
        Timer.Basic ignoreTerrainDamageTimer;

        public GamerData gamerdata;
        public Engine.PlayerData pData;

        public Gamer(int playerIx, int gamersCount, GamerData gamerdata)
        {
            this.gamerdata = gamerdata;
            
            //this.playerIx = playerIx;
            pData = Engine.XGuide.GetOrCreatePlayer(playerIx);
            
            camera = new Graphics.TopDownCamera(Zoom);
            //Ref.draw.Camera = camera;
            pData.view.ScreenIndex = playerIx;

            Ref.draw.AddPlayerScreen(pData);
            pData.view.Camera = camera;

            pData.view.SetDrawArea(gamersCount, playerIx, false, null);

            //pData.view.Camera = camera;
            camera.GoalLookTarget = Vector3.Zero;
            camera.FarPlane = 5000;

            hud = new GamerHUD(this);
            reset();
            
            ignoreTerrainDamageTimer = new Timer.Basic(900, false);
            ignoreTerrainDamageTimer.SetZeroTimeLeft();
        }

        public void update()
        {
            if (viewDeathTimer.HasTime)
            {
                //camera.Zoom += Ref.DeltaTimeSec * 2f;
                if (viewDeathTimer.CountDown())
                {
                    reset();
                }
            }
            else
            {
                updateInput();
                updateMovement();
                collisionCheck();

                if (PlatformSettings.DevBuild)
                {
                    if (Input.Keyboard.KeyDownEvent(Keys.D1))
                    {
                        addTailSegment();
                    }
                    if (Input.Keyboard.KeyDownEvent(Keys.D2))
                    {
                        if (allSegments.Count > 3)
                        {
                            removeSegment(allSegments[2]);
                        }
                    }
                    if (Input.Keyboard.KeyDownEvent(Keys.D3))
                    {
                        addNoseBomb();
                    }
                    if (Input.Keyboard.KeyDownEvent(Keys.D4))
                    {
                        addMoney(100);
                    }
                    if (Input.Keyboard.KeyDownEvent(Keys.D5))
                    {
                        addShield();
                    }
                    if (Input.Keyboard.KeyDownEvent(Keys.D6))
                    {
                        addTailSegment();
                        addWeapon();
                    }
                }

                updateBuy();

                moveHistory.AddLast(new PositionHistory(head.model.Position, head.rotation.radians), true);
                ignoreTerrainDamageTimer.Update();
            }
        }        

        public void asynchUpdate(float time)
        {
            if (gameobjects.ReadyForAsynchProcessing())
            {
                gameobjects.processList.Clear();
                quickBound.radius = allSegments.Count * BodySegment.BodyWidth + BodySegment.BodyWidth * 6f;

                if (allSegments.Count > 1)
                {
                    Vector3 start = head.model.Position;
                    Vector3 end = allSegments.Last().model.Position;
                    quickBound.Center =  VectorExt.V3XZtoV2(end + start) * 0.5f;

                    quickBound.radius += (start - end).Length();
                }
                else
                {
                    quickBound.Center = VectorExt.V3XZtoV2(head.model.Position);
                }
                
                SpaceRef.go.gameObjectsAsynchCounter.Reset();
                while (SpaceRef.go.gameObjectsAsynchCounter.Next())
                {
                    if (SpaceRef.go.gameObjectsAsynchCounter.sel.bound.ExtremeRadiusColl(quickBound))
                    {
                        gameobjects.processList.Add(SpaceRef.go.gameObjectsAsynchCounter.sel);
                    }
                }

                gameobjects.onAsynchProcessComplete();
            }

            //Shop
            if (buyLockTime.CountDown(time))
            {
                GameObjects.ShopSquare foundShop = null;
                
                foreach (var m in SpaceRef.go.shops)
                {
                    if (m.area.IntersectPoint(head.bound.MainBound.Center))
                    {
                        foundShop = m;
                        //break;
                    }

                    bool bCanBuy = m != foundShop && canBuy(m);
                    m.updateVisuals(bCanBuy, pData.localPlayerIndex);
                }

                buyEvent = foundShop != null && foundShop != onShop && canBuy(foundShop);
                buyLockTime.MilliSeconds = 800;
                onShop = foundShop;
            }

            
        }

        void updateBuy()
        {
            if (buyEvent)
            {
                buyEvent = false;

                if (onShop.price <= ticketsAvailable)
                {
                    //addMoney(-onShop.price);
                    switch (onShop.type)
                    {
                        case GameObjects.ShopSquareType.AddTail:
                            addTailSegment();
                            break;

                        case GameObjects.ShopSquareType.TailExpansion:
                            addShield();
                            break;

                        case GameObjects.ShopSquareType.TailKnife:
                            addWeapon();
                            break;

                        case GameObjects.ShopSquareType.NoseBomb:
                            addNoseBomb();
                            break;
                    }
                }
                refreshTickets();
            }
        }

        

        void updateMovement()
        {
            head.updateMove();
            updateSegments();

            camera.GoalLookTarget = head.model.Position;
            camera.Time_Update(Ref.DeltaTimeMs);
        }

        void addNoseBomb()
        {
            if (noseBombs.Count < MaxNoseBombs)
            {
                var bomb = new GameObjects.NoseBomb(noseBombs.Count);
                bomb.update(head);

                buyEffect(bomb.model, GameObjects.NoseBomb.Width * 0.5f);
                noseBombs.Add(bomb);
            }
        }

        void addTailSegment()
        {
            if (allSegments.Count < MaxTotalSegments)
            {
                BodySegment segment = new BodySegment(allSegments.Last());
                allSegments.Add(segment);
                refreshSegmentIndex();
                updateSegments();

                buyEffect(segment.model, AbsBodySegment.BodyWidth * 0.2f);
            }
        }

        void addShield()
        {
            var next = nextUnshieldedSegment();
            if (next != null)
            {
                next.refreshShield(true);
                buyEffect(next.shieldModel, AbsBodySegment.BodyWidth * 0.34f);
            }
        }

        void addWeapon()
        {
            var segment = nextWeaponSpot();
            if (segment != null)
            {
                segment.weapon = new SpaceShip.TailKnife(turnRight, segment.HasShield);
            }
        }

        AbsBodySegment nextUnshieldedSegment()
        {
            for (int i = 0; i < allSegments.Count; ++i)
            {
                if (!allSegments[i].HasShield)
                {
                    return allSegments[i];
                }
            }

            return null;
        }

        BodySegment nextWeaponSpot()
        {
            for (int i = 1; i < allSegments.Count; ++i)
            {
                if (allSegments[i].weapon == null)
                {
                    return (BodySegment)allSegments[i];
                }
            }

            return null;
        }

        void buyEffect(Graphics.Mesh model, float radius)
        {
            new Graphics.VisualFlash(model, 4, 160);
            Effects.EffectLib.BuyItemEffect(model.Position, radius);
        }

        bool canBuy(GameObjects.ShopSquare shop)
        {
            if (ticketsAvailable < shop.price)
            {
                return false;
            }

            switch (shop.type)
            {
                case GameObjects.ShopSquareType.AddTail:
                    return allSegments.Count < MaxTotalSegments;

                case GameObjects.ShopSquareType.TailExpansion:
                    return nextUnshieldedSegment() != null;

                case GameObjects.ShopSquareType.TailKnife:
                    return nextWeaponSpot() != null;

                case GameObjects.ShopSquareType.NoseBomb:
                    return noseBombs.Count < MaxNoseBombs;

                default:
                    throw new NotImplementedException();
            }
        }

        void removeSegment(AbsBodySegment segment)
        {
            segment.DeleteMe();

            Coin.DropAmount(GameObjects.ShopSquare.TailCost, segment.model.Position, this);

            allSegments.RemoveAt(segment.index);
            int next = segment.index + 1;
            AbsBodySegment nextSegment;
            if (arraylib.TryGet(allSegments, next, out nextSegment))
            {
                nextSegment.animateDistance += segment.frameHalfDist * 2;
            }

            refreshSegmentIndex();
        }

        void refreshSegmentIndex()
        {
            for (int i = 0; i < allSegments.Count; ++i)
            {
                allSegments[i].index = i;
            }
        }
        


        void updateInput()
        {
            if (gamerdata.button.IsDown)
            {
                head.turn(turnRight);
                //head.arrowModel.Opacity = 0.5f;
               
                //head.arrowModel.SetSpriteName(SpriteName.spaceWarTurnArrowPointing);
            }
            else if (gamerdata.button.UpEvent)
            {
                turnRight = !turnRight;
                //head.arrowModel.Opacity = 1f;
                //head.arrowModel.Color = Color.White;
                refreshTurnArrow();
               
                //head.arrowModel.SetSpriteName(SpriteName.spaceWarTurnArrow);
            }
        }

        void refreshTurnArrow()
        {
            head.arrowModel.ScaleX = lib.BoolToLeftRight(turnRight) * head.model.ScaleX;
        }

        void updateSegments()
        {
            int frameDist = 0;
            foreach (var m in allSegments)
            {
                m.update(moveHistory, 0, ref frameDist);
            }

            foreach (var m in noseBombs)
            {
                m.update(head);
            }
        }

        void collisionCheck()
        {
            gameobjects.checkForUpdatedList();

            if (noseBombs.Count > 0)
            {
                var bomb = noseBombs[noseBombs.Count - 1];
                bodyPartCollisionUpdate(bomb.bound, null, bomb, null);
            }

            for (int i = allSegments.Count-1; i >= 0; --i)
            {
                var segment = allSegments[i];
                if (segment.weapon != null && segment.weapon.Active)
                {
                    bodyPartCollisionUpdate(segment.weapon.bound, null, null, segment.weapon);
                }
                bodyPartCollisionUpdate(segment.bound, segment, null, null);
            }
            
            if (!SpaceRef.gamestate.map.PlayerBounds.IntersectPoint(head.bound.MainBound.Center))
            {
                onDeath();
            }
        }

        void bodyPartCollisionUpdate(Physics.Bound2DWrapper bound, AbsBodySegment segment, GameObjects.NoseBomb nosebomb, AbsTailWeapon weapon)
        {
            for (int i = gameobjects.list.Count - 1; i >= 0; --i)
            {
                var obj = gameobjects.list[i];
                if (!obj.isDeleted)
                {
                    if (obj.bound.Intersect(bound))
                    {
                        switch (obj.CollisionType)
                        {
                            default:

                                if (segment != null)
                                {
                                    if (ignoreTerrainDamageTimer.TimeOut ||
                                        segment.ActiveShield)
                                    {
                                        ignoreTerrainDamageTimer.Reset();

                                        if (segment.ActiveShield)
                                        {
                                            segment.takeShieldDamage();
                                            //BodySegment tail = segment as BodySegment;
                                            //Effects.EffectLib.SplitModelExplosion(tail.shieldModel, AbsBodySegment.ExplosionModelSplits);
                                            //tail.refreshShield(false);
                                        }
                                        else if (segment is Head)
                                        {
                                            if (doubleCheckNoseBombColl(obj))
                                            {
                                                useNoseBomb();
                                            }
                                            else
                                            {
                                                onDeath();
                                            }
                                        }
                                        else
                                        {
                                            Effects.EffectLib.SplitModelExplosion(segment.model, AbsBodySegment.ExplosionModelSplits);
                                            removeSegment(segment);
                                            onRemovedPart();
                                        }
                                    }
                                   
                                }
                                else if (weapon != null)
                                {
                                    weapon.onUse();
                                }
                                else if (nosebomb != null)
                                {
                                    useNoseBomb();
                                }
                                obj.takeDamage(VectorExt.V2toV3XZ(bound.MainBound.Center));
                                
                                return;

                            case CollisionType.PickUp:
                                if (segment != null)
                                {
                                    var coin = obj as Coin;
                                    if (coin.blockGamer == this)
                                    {
                                        coin.setGamerBlock(this);
                                    }
                                    else
                                    {
                                        collectMoney(Coin.GetCoinValue(coin.value));
                                        obj.DeleteMe();
                                        Effects.EffectLib.CoinEffect(coin);
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        bool doubleCheckNoseBombColl(AbsGameObject obj)
        {
            if (noseBombs.Count > 0)
            {
                Vector2 dirToObj = obj.bound.MainBound.Center - head.bound.MainBound.Center;
                float angle = this.head.rotation.AngleDifference(lib.V2ToAngle(dirToObj));

                if (Math.Abs(angle) <= MathExt.Tau * 0.2f)
                {
                    return true;
                }
            }

            return false;
        }

        void useNoseBomb()
        {
            arraylib.PullLastMember(noseBombs).DeleteMe();
            ignoreTerrainDamageTimer.Reset();
            onRemovedPart();
        }

        void collectMoney(int amount)
        {
            addMoney(amount);
        }

        void onRemovedPart()
        {
            refreshTickets();
        }

        void onDeath()
        {
            int removeCoins = money / 2;

            int segmentCoins = removeCoins / allSegments.Count;

            for (int i = allSegments.Count - 1; i >= 0; --i)
            {
                Effects.EffectLib.SplitModelExplosion(allSegments[i].model, AbsBodySegment.ExplosionModelSplits);
                allSegments[i].DeleteMe();

                int drop = segmentCoins;
                if (i != 0)
                {
                    drop += GameObjects.ShopSquare.TailCost;
                }
                Coin.DropAmount(drop, allSegments[i].model.Position, this);
            }

            allSegments.Clear();

            viewDeathTimer.Seconds = 2f;
            addMoney(-removeCoins);

            arraylib.DeleteAndClearArray(noseBombs);
        }

        void reset()
        {
            

            //init new
            head = new Head();
            allSegments.Add(head);

            switch (SpaceRef.gamestate.joinedLocalGamers.Count)
            {
                case 1:
                    camera.targetZoom = Zoom;
                    break;
                case 2:
                    camera.targetZoom = Zoom * 1.4f;
                    break;
                default: //3 or 4
                    camera.targetZoom = Zoom * 1.1f;
                    break;
            }

            refreshTurnArrow();
        }

        void setMoney(int amount)
        {
            money = amount;
            refreshMoney();//moneyText.TextString = money.ToString();
        }

        void addMoney(int add)
        {
            money += add;
            refreshMoney(); //moneyText.TextString = money.ToString();
        }

        void refreshMoney()
        {
            int tickets = money / 25;
            ticketsAvailable = tickets - ticketsUsed;
            hud.refesh();
        }

        void refreshTickets()
        {
            int used = 0;

            allSegments.loopBegin();
            
            while(allSegments.loopNext())
            {
                used += allSegments.sel.TicketValue();
            }

            used += GameObjects.ShopSquare.NoseBombCost * noseBombs.Count;
            
            if (lib.ChangeValue(ref ticketsUsed, used))
            {
                refreshMoney();
            }
        }
    }

    struct PositionHistory
    {
        public Vector3 pos;
        public float rotation;

        public PositionHistory(Vector3 pos, float rotation)
        {
            this.pos = pos;
            this.rotation = rotation;
        }
    }
}
