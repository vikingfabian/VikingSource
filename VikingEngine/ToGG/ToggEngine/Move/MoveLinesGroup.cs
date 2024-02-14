using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class MoveLinesGroup
    {        
        public static readonly Vector3 ModelScale = new Vector3(0.4f);

        Graphics.Mesh currentPosDot;
        public List<MoveLine> lines;
        public Graphics.ImageGroup moveActionIcons = new Graphics.ImageGroup();

        public int availableMoveLength = int.MaxValue, regularMoveLength = int.MaxValue;

        public bool isLocked = false;
        ToggEngine.Display2D.MoveLengthToolTip toolTip = null;
        public IntVector2 start;
        public bool isLocal = true;
        public AbsUnit unit;
        
        public MoveLinesGroup(AbsUnit u)
        {
            if (u != null)
            {
                regularMoveLength = u.MoveLengthWithModifiers(true);
                availableMoveLength = regularMoveLength;
            }
            init(u);
            updateAttackTargets();
        }

        public void onPlayerPickUp(MapControls mapcontrols, AbsUnit u)
        {
            toolTip = new ToggEngine.Display2D.MoveLengthToolTip(mapcontrols);
            toolTip.refresh(u);
        }

        public MoveLinesGroup(AbsUnit u, System.IO.BinaryReader r)
        {
            init(u);
            Read(r);
        }

        public MoveLinesGroup(AbsUnit u, WalkingPath wp)
        {
            u.movelines = this;
            init(u);

            start = u.squarePos;
            IntVector2 prev = u.squarePos;
            for (int i = wp.squaresEndToStart.Count - 1; i >= 0; --i)
            {
                lines.Add(new MoveLine(wp.squaresEndToStart[i], prev, arraylib.Last(lines), lines.Count >= regularMoveLength, u));
                prev = wp.squaresEndToStart[i];
            }

            refreshCurrentPosDot();
        }
        void removeToolTip()
        {
            if (toolTip != null)
            {
                toolTip.remove();
                toolTip = null;
            }
        }

        void init(AbsUnit u)
        {
            this.unit = u;
            IntVector2 squarePos = IntVector2.Zero;

            if (u != null)
            {
                u.movelines = this;
                squarePos = u.soldierModel.SquarePos;
            }

            start = squarePos;
            lines = new List<MoveLine>(0);

            currentPosDot = new Graphics.Mesh(LoadedMesh.plane, toggLib.ToV3(squarePos), ModelScale,
                Graphics.TextureEffectType.Flat, SpriteName.cmdMoveArrowDotMy, Color.White);
            currentPosDot.Y = ToggEngine.Map.SquareModelLib.TerrainY_MoveArrow;
        }

        public IntVector2 CurrentSquarePos()
        {
            if (lines.Count == 0)
            {
                return start;
            }
            return lines[lines.Count - 1].toPos;
        }

        public void refreshCurrentPosDot()
        {
            currentPosDot.Position = toggLib.ToV3(CurrentSquarePos(), ToggEngine.Map.SquareModelLib.TerrainY_MoveArrow);

            if (MoveLength > regularMoveLength)
            {
                currentPosDot.SetSpriteName(SpriteName.cmdMoveArrowDotStamina);
            }
            else
            {
                if (isLocal)
                {
                    currentPosDot.SetSpriteName(SpriteName.cmdMoveArrowDotMy);
                }
                else
                {
                    currentPosDot.SetSpriteName(SpriteName.cmdMoveArrowDotAlly);
                }
            }
        }

        public NewSquareResult NewSquare(IntVector2 toSquare)
        {
            NewSquareResult result = NewSquareResult.Other;
            bool addMove = false;
            bool createdShortcut;
            IntVector2 prev; //squarePos;
            
            do
            {
                createdShortcut = false;
                //squarePos = unit.soldierModel.SquarePos;
                prev = CurrentSquarePos();
                int posDiffLength = (toSquare - prev).SideLength();

                if (posDiffLength == 1)
                {
                    if (toggRef.board.MovementRestriction(toSquare, unit) ==
                        ToggEngine.Map.MovementRestrictionType.Impassable)
                    { //Cant go there
                        addMove = false;
                    }
                    else
                    {
                        addMove = true;
                        createdShortcut = checkForShortCut(toSquare, ref prev, ref addMove);
                    }
                }
                else if (posDiffLength > 1)
                {
                    result = NewSquareResult.TooFarOffset;

                    addMove = false;
                    createdShortcut = checkForShortCut(toSquare, ref prev, ref addMove);                    
                }
            } while (createdShortcut);

            ToggEngine.Map.MovementRestrictionType leaveRestriction = toggRef.board.MovementRestriction(prev, unit);
            refeshMoveStatus(leaveRestriction);

            if (status_totalMovesLeft <= 0)
            {
                addMove = false;
            }

            if (leaveRestriction == ToggEngine.Map.MovementRestrictionType.MustStop && 
                MoveLength > 0)
            {
                result = NewSquareResult.MustStop;
                addMove = false;
            }

            if (addMove)
            {
                lines.Add(new MoveLine(toSquare, prev, arraylib.Last(lines), lines.Count >= regularMoveLength, unit));

                if (toolTip != null)
                {
                    toolTip.refresh(unit);
                }

                result = NewSquareResult.Added;
            }

            updateAttackTargets();

            refreshCurrentPosDot();

            return result;
        }

        public int staminaCost()
        {
            int stamina = 0;
            for (int i = lines.Count - 1; i >= 0; --i)
            {
                if (lines[i].partialLock)
                {
                    break;
                }
                else
                {
                    stamina += lines[i].staminaCost;
                }
            }

            return stamina;
        }


        int status_regularMoves = 0;
        int status_lockedLength = 0;
        int status_regularMovesLeft;
        int status_totalMovesLeft;
        int status_availableStamina;
        int status_activeStaminaCost = 0;
        int status_nextMoveStaminaCost;

        void refeshMoveStatus(ToggEngine.Map.MovementRestrictionType leaveRestriction)
        {
            status_regularMoves = 0;
            status_lockedLength = 0;
            status_activeStaminaCost = 0;
            status_nextMoveStaminaCost = 0;

            for (int i = 0; i < lines.Count; ++i)
            {
                var l = lines[i];

                if (!l.boostedMove)
                {
                    ++status_regularMoves;
                }

                if (l.partialLock)
                {
                    ++status_lockedLength;
                }
                else
                {
                    status_activeStaminaCost += l.staminaCost;
                }
            }

            status_regularMovesLeft = regularMoveLength - status_regularMoves;
            status_totalMovesLeft = status_regularMovesLeft;

            if (unit.hasStamina())
            {
                status_availableStamina = unit.hq().data.hero.stamina.Value;

                if (leaveRestriction == ToggEngine.Map.MovementRestrictionType.CostStamina)
                {
                    status_nextMoveStaminaCost += 1;
                }
                
                if (status_regularMovesLeft <= 0)
                {
                    status_nextMoveStaminaCost += 1;
                }

                int staminaLeft = status_availableStamina - status_activeStaminaCost;

                if (staminaLeft - status_nextMoveStaminaCost >= 0)
                {
                    status_totalMovesLeft = staminaLeft - status_nextMoveStaminaCost + 1;
                }
                else
                {
                    status_totalMovesLeft = 0;
                }

            }
            else
            {
                status_availableStamina = 0;
            }
        }

        bool checkForShortCut(IntVector2 squarePos, ref IntVector2 prev, ref bool addMove)
        {
            bool createdShortcut = false;

            for (int i = lines.Count - 1; i >= 0; --i)
            {
                if (lines[i].partialLock == false)
                {
                    MoveLine pos = lines[i];
                    if (squarePos == pos.fromPos)
                    {
                        //Return to a previous pos
                        addMove = false;
                        removesLines(i);                        
                        createdShortcut = true;
                        break;
                    }
                    else if ((squarePos - pos.fromPos).SideLength() == 1)
                    {
                        //Found shortcut
                        addMove = true;
                        removesLines(i);
                        prev = pos.fromPos;
                        createdShortcut = true;
                        break;
                    }

                }
            }

            return createdShortcut;
        }

        public void checkOnMoveCompleted()
        {
            while (lines.Count > 0 &&
                toggRef.board.MovementRestriction(arraylib.Last(lines).toPos, unit) == ToggEngine.Map.MovementRestrictionType.WalkThroughCantStop)
            {
                arraylib.PullLastMember(lines).DeleteMe();
                refreshCurrentPosDot();
            }
        }

        public int MoveLength
        {
            get { return lines == null? 0 : lines.Count; }
        }

        private void updateAttackTargets()
        {
            if (unit != null)
            {
                new UnitMoveAndAttackGUI(unit, End, false, true);
            }
        }

        public void DeleteMe()
        {
            removeToolTip();
            removesLines();
            currentPosDot.DeleteMe();
            moveActionIcons.DeleteAll();
        }

        public void clearIcons()
        {
            foreach (var m in lines)
            {
                m.clearBackstabs();
            }

            moveActionIcons.DeleteAll();
        }

        public void removesLines(int fromIndex = 0)
        {
            if (fromIndex < lines.Count)
            {
                for (int i = fromIndex; i < lines.Count; ++i)
                {
                    lines[i].DeleteMe();
                }
                lines.RemoveRange(fromIndex, lines.Count - fromIndex);
            }
        }

        public void setFocus(int state_0Selected_1Hover_2Hidden)
        {
            bool view = state_0Selected_1Hover_2Hidden != 2;
            float opacity = state_0Selected_1Hover_2Hidden == 1 ? 0.7f: 1f;

            foreach (var l in lines)
            {
                if (l.dotModel != null)
                {
                    l.setFocus(unit, view, opacity);
                    if (l.backStabbers != null)
                    {
                        l.backStabbers.SetVisible(view);
                    }
                }
            }

            currentPosDot.Visible = view;
            currentPosDot.Opacity = opacity;

            if (state_0Selected_1Hover_2Hidden == 1)
            {
                currentPosDot.SetSpriteName(SpriteName.cmdMoveArrowDotLocked);
            }
            else
            {
                currentPosDot.SetSpriteName(SpriteName.cmdMoveArrowDotMy);
            }

            //if (targets != null)
            //{
            //    targets.SetVisible(state_0Selected_1Hover_2Hidden == 0);
            //}
        }

        public bool HadBackstabbers()
        {
            foreach (var line in lines)
            {
                if (arraylib.HasMembers(line.backstabberUnits))
                {
                    return true;
                }
            }

            return false;
        }

        public List<MoveLine> getBackstabbers()
        {
            List<MoveLine> result = new List<MoveLine>();
            foreach (var line in lines)
            {
                if (line.backstabberUnits != null)
                {
                    result.Add(line);
                }
            }
            return result;
        }

        public void onMovementAnimationComplete(AbsGenericPlayer p)
        {
            removeToolTip();

            for (int i = lines.Count - 1; i >= 0; --i)
            {
                if (lines[i].backstabberUnits != null)
                {
                    //p.movementPhaseIsLocked = true;
                    isLocked = true;
                    return;
                }
            }
        }

        public int backStabbersFullCount()
        {
            int result = 0;
            foreach (var line in lines)
            {
                result += line.BackStabCount;
            }

            return result;
        }

        public void Write(System.IO.BinaryWriter w)
        {
            start.writeByte(w);
            w.Write((byte)lines.Count);
            foreach (var l in lines)
            {
                l.Write(w);
            }
        }

        public void setPartialLock()
        {
            foreach (var l in lines)
            {
                l.partialLock = true;
            }
        }

        public void Read(System.IO.BinaryReader r)
        {
            isLocal = false;
            removesLines();

            start.readByte(r);
            int count = r.ReadByte();
            for (int i = 0; i < count; ++i)
            {
                lines.Add(new MoveLine(unit, r));
            }

            currentPosDot.Position = toggLib.ToV3(CurrentSquarePos());

            currentPosDot.SetSpriteName(SpriteName.cmdMoveArrowDotAlly);
            currentPosDot.Y = ToggEngine.Map.SquareModelLib.TerrainY_MoveArrowLow;
        }

        public bool hasPartialLock()
        {
            return lines.Count > 0 && lines[0].partialLock;
        }

        public bool Contains(IntVector2 pos)
        {
            if (lines.Count > 0 && lines[0].fromPos == pos)
            {
                return true;
            }

            foreach (var l in lines)
            {
                if (l.toPos == pos)
                {
                    return true;
                }
            }

            return false;
        }

        public IntVector2 End
        {
            get
            {
                if (HasMoved)
                {
                    return arraylib.Last(lines).toPos;
                }
                else
                {
                    return start;
                }
            }
        }

        public int CountBoostedMoves(bool excludeLockedMoves)
        {
            int length = 0;
            foreach (var line in lines)
            {
                if (!(line.partialLock && excludeLockedMoves) && line.boostedMove)
                {
                    ++length;
                }
            }

            return length;
       }

        public bool HasMoved
        {
            get { return lines.Count > 0; }
        }

        public bool HasNewMoves()
        {
            return lines.Count > 0 && !arraylib.Last(lines).partialLock;
        }

        public override string ToString()
        {
            return "Move(" + lines.Count.ToString() + ") " + start.ToString() + " - " + End.ToString();
        }
    }

    enum NewSquareResult
    {
        Added,
        MustStop,
        TooFarOffset,
        Other,
    }
}
