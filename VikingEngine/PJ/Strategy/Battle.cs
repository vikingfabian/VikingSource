using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Strategy
{
    class Battle
    {
        public MapArea fromArea, toArea;
        public bool attackerWon;
        public int winnerLoss = 0;
        public int moveAmount;

        Graphics.Image bg, scatterBg, centerLine, scatterLine;
        public BattleMembersGroup attackers, defenders, winners;

        float speed;
        float slowDownSpeed;
        int dir;

        Time slowDownTimer = new Time(Ref.rnd.Float(300, 1200), TimeUnit.Milliseconds);
        Vector2 center;
        Vector2 hitMarkersSz;
        bool viewWinnerState = false;
        Time winnerStateTimer = new Time(1.2f, TimeUnit.Seconds);


        public Battle(MapArea fromArea, MapArea toArea, int moveAmount)
        {
            StrategyLib.SetHudLayer();

            this.fromArea = fromArea;
            this.toArea = toArea;
            this.moveAmount = moveAmount;
            Vector2 iconSize = new Vector2(Engine.Screen.IconSize * 1f);
            center = Engine.Screen.CenterScreen;

            attackers = new BattleMembersGroup(fromArea.owner, true, center, iconSize);
            attackers.addSoldiers(moveAmount);

            defenders = new BattleMembersGroup(toArea.owner, false, center, iconSize);
            defenders.addSoldiers(toArea.ArmyCount);
            defenders.addMember(new BattleBarDefenceBonus());
            if (toArea.type == AreaType.Castle)
            {
                defenders.addMember(new BattleBarDefenceBonus());
            }

            BattleMembersGroup largestGroup, smallestGroup;
            if (attackers.soldierCount > defenders.soldierCount)
            {
                largestGroup = attackers;
                smallestGroup = defenders;
            }
            else
            {
                largestGroup = defenders;
                smallestGroup = attackers;
            }

            int sizeMultiplier = largestGroup.soldierCount / Bound.Min(smallestGroup.soldierCount, 1);
            if (sizeMultiplier >= 5)
            {
                sizeMultiplier = 5;
            }
            else if (sizeMultiplier >= 3)
            {
                sizeMultiplier = 3;
            }
            else if (sizeMultiplier >= 2)
            {
                sizeMultiplier = 2;
            }
            else
            {
                sizeMultiplier = 0;
            }

            if (sizeMultiplier > 0)
            {
                largestGroup.addMember(new BattleBarMultiplierBonus(sizeMultiplier));
            }

            bg = new Graphics.Image(SpriteName.WhiteArea, center,
                new Vector2(attackers.totalWidth + defenders.totalWidth, iconSize.Y * 1.2f),
                ImageLayers.Top8);
            bg.Color = Color.LightGray;
            bg.Xpos -= attackers.totalWidth;
            bg.Ypos -= bg.Height * 0.5f;

            scatterBg = new Graphics.Image(SpriteName.WhiteArea, bg.LeftBottom,
                new Vector2(bg.Width, Engine.Screen.IconSize * 0.5f),
                ImageLayers.Top9);
            scatterBg.Color = Color.Black;

            centerLine = new Graphics.Image(SpriteName.WhiteArea, center, new Vector2(Engine.Screen.IconSize * 0.06f, bg.Height * 1.2f),
                 ImageLayers.AbsoluteBottomLayer, true);
            centerLine.Color = Color.Red;
            centerLine.LayerAbove(bg);

            scatterLine =  new Graphics.Image(SpriteName.WhiteArea, center, new Vector2(Engine.Screen.IconSize * 0.1f, Engine.Screen.IconSize * 0.5f),
                 ImageLayers.Top5);
            scatterLine.Color = Color.LightBlue;
            scatterLine.origo = new Vector2(0.5f, 0f);
            scatterLine.Ypos = bg.Bottom;

            Vector2 hitMarkersCenter = new Vector2(center.X, bg.Bottom);
            hitMarkersSz = iconSize * 0.5f;
            attackers.createTakeHitsMarkers(hitMarkersCenter, hitMarkersSz);
            defenders.createTakeHitsMarkers(hitMarkersCenter, hitMarkersSz);


            speed = iconSize.X * 0.4f;
            slowDownSpeed = speed * Ref.rnd.Float(0.01f, 0.02f);
            dir = Ref.rnd.LeftRight();
        }

        public bool update()
        {
            if (viewWinnerState)
            {
                return winnerStateTimer.CountDown();
            }
            else
            {
                scatterLine.Xpos += speed * dir;

                if (dir < 0)
                {
                    if (scatterLine.Xpos < scatterBg.Xpos)
                    {
                        scatterLine.Xpos = scatterBg.Xpos;
                        dir = -dir;
                    }
                }
                else
                {
                    if (scatterLine.Xpos > scatterBg.Right)
                    {
                        scatterLine.Xpos = scatterBg.Right;
                        dir = -dir;
                    }
                }

                if (slowDownTimer.CountDown())
                {
                    speed -= slowDownSpeed;

                    if (speed <= 0)
                    {
                        //Announce winner
                        speed = 0;
                        attackerWon = scatterLine.Xpos < center.X;

                        
                        float centerDiff = Math.Abs(scatterLine.Xpos - center.X);
                        int winnerLossPos = (int)(centerDiff / hitMarkersSz.X);

                        BattleMembersGroup losers;
                        if (attackerWon)
                        {
                            winners = attackers;
                            losers = defenders;
                        }
                        else
                        {
                            winners = defenders;
                            losers = attackers;
                        }


                        if (winnerLossPos < winners.canTakeHitCount)
                        {
                            winnerLoss = winners.canTakeHitCount - winnerLossPos;
                        }

                        if (attackerWon && winnerLoss > 0)
                        {
                            lib.DoNothing();
                        }

                        winners.viewHits(winnerLoss);
                        losers.viewHits(int.MaxValue);

                        viewWinnerState = true;
                    }
                }
            }
            return false;
        }


        public void DeleteMe()
        {
            StrategyLib.SetHudLayer();

            attackers.DeleteMe(); 
            defenders.DeleteMe();
            bg.DeleteMe(); 
            scatterBg.DeleteMe();
            centerLine.DeleteMe();
            scatterLine.DeleteMe();
        }
    }

    class BattleMembersGroup
    {
        Gamer gamer;
        public int canTakeHitCount;
        List<BattleBarMember> members;
        bool attacker;
        Vector2 currentPos;
        Vector2 iconSize;
        public float totalWidth = 0;
        public int soldierCount = 0, survivorCount;
        List<Graphics.Image> hitMarkers = new List<Graphics.Image>();

        public BattleMembersGroup(Gamer gamer, bool attacker, Vector2 center, Vector2 iconSize)
        {
            this.gamer = gamer;
            this.attacker = attacker;
            this.currentPos = center;
            this.iconSize = iconSize;

            members = new List<BattleBarMember>();
        }

        public void createTakeHitsMarkers(Vector2 center, Vector2 markerSize)
        {
            canTakeHitCount = Bound.Min(soldierCount - 1, 0);
            int dir = -lib.BoolToLeftRight(attacker);

            center.X += markerSize.X * 0.5f * dir;
            center.Y += markerSize.Y * 0.3f;

            for (int i = 0; i < canTakeHitCount; ++i)
            {
                Graphics.Image mark = new Graphics.Image(SpriteName.birdPlayerFrameClose, center,
                    markerSize * 1.2f, ImageLayers.Top7, true);
                hitMarkers.Add(mark);
                center.X += markerSize.X * dir;
            }
        }

        public void viewHits(int damage)
        {
            survivorCount = Bound.Min(soldierCount - damage, 0);

            foreach (var m in members)
            {
                m.takeDamage(ref damage);
            }
           

            
        }

        public void addSoldiers(int count)
        {
            soldierCount += count;
            for (int i = 0; i < count; ++i)
            {
                addMember(new BattleBarSoldier());
            }
        }

        public void addMember(BattleBarMember member)
        {
            Vector2 memCenter = currentPos;
            int dir = -lib.BoolToLeftRight(attacker);
            memCenter.X += dir * member.Width * iconSize.X * 0.5f;

            currentPos.X += dir * member.Width * iconSize.X;

            totalWidth += member.Width * iconSize.X;

            member.placeVisuals(memCenter, iconSize, attacker, gamer);
            members.Add(member);            
        }

        public void DeleteMe()
        {
            foreach (var m in hitMarkers)
            {
                m.DeleteMe();
            }
            foreach (var m in members)
            {
                m.DeleteMe();
            }
        }
    }

    abstract class BattleBarMember
    {
        abstract public void placeVisuals(Vector2 pos, Vector2 iconSize, bool attacker, Gamer gamer);

        abstract public float Width { get; }

        abstract public void DeleteMe();

        virtual public void takeDamage(ref int damage)
        {
            //do nothing
        }
    }

    class BattleBarSoldier : BattleBarMember
    {
        Graphics.Image image;
        SpriteName damageTile;

        public override void placeVisuals(Vector2 pos, Vector2 iconSize, bool attacker, Gamer gamer)
        {
            image = new Graphics.Image(gamer.animalSetup.wingUpSprite, pos, iconSize * 1.2f, ImageLayers.Top5, true);
            damageTile = gamer.animalSetup.deadSprite;
            if (!attacker)
            {
                image.spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override void takeDamage(ref int damage)
        {
            if (damage > 0)
            {
                damage--;

                image.SetSpriteName(damageTile);
            }
        }

        public override float Width
        {
            get { return 1f; }
        }

        public override void DeleteMe()
        {
            image.DeleteMe();
        }
    }
    class BattleBarDefenceBonus : BattleBarMember
    {
        Graphics.Image image;

        public override void placeVisuals(Vector2 pos, Vector2 iconSize, bool attacker, Gamer gamer)
        {
            image = new Graphics.Image(SpriteName.BirdShieldIcon, pos, iconSize * 0.8f, ImageLayers.Top5, true);
            if (!attacker)
            {
                image.spriteEffects = SpriteEffects.FlipHorizontally;
            }
        }

        public override float Width
        {
            get { return 0.6f; }
        }

        public override void DeleteMe()
        {
            image.DeleteMe();
        }
    }

    class BattleBarMultiplierBonus : BattleBarMember
    {
        Graphics.TextG text;
        int multiCount;
        float w;

        public BattleBarMultiplierBonus(int multiCount)
        {
            this.multiCount = multiCount;
            switch (multiCount)
            {
                case 2:
                    w = 1.2f;
                    break;
                case 3:
                    w = 1.8f;
                    break;
                case 5:
                    w = 2.5f;
                    break;

                default:
                    throw new Exception();
            }
        }

        public override void placeVisuals(Vector2 pos, Vector2 iconSize, bool attacker, Gamer gamer)
        {
            text = new Graphics.TextG(LoadedFont.Regular, pos, new Vector2(iconSize.Y / Engine.Screen.IconSize * Engine.Screen.TextSize),
                Graphics.Align.CenterAll, "x" + multiCount.ToString(), Color.Blue, ImageLayers.Top5);
            
        }

        public override float Width
        {
            get { return w; }
        }

        public override void DeleteMe()
        {
            text.DeleteMe();
        }
    }
}
