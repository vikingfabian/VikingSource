using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class MoveLine
    {
        public IntVector2 toPos, fromPos;
        public Graphics.Mesh dotModel, arrowModel;
        public Graphics.ImageGroup backStabbers;
        public List<AbsUnit> backstabberUnits = new List<AbsUnit>();
        public int BackStabCount => backstabberUnits.Count;
        public bool partialLock = false;
        public bool boostedMove, prevBoostedMove;
        bool isLocal = true;
        public int staminaCost = 0;

        public MoveLine(IntVector2 toPos, IntVector2 fromPos, MoveLine prevline, bool boostedMove, AbsUnit u)
        {
            this.fromPos = fromPos;
            this.boostedMove = boostedMove;
            if (boostedMove)
            {
                staminaCost += 1;
            }
            prevBoostedMove = prevline != null && prevline.boostedMove;

            this.toPos = toPos;

            createModel(u);

            if (u != null)
            {
                if (toggRef.board.MovementRestriction(fromPos, u) == ToggEngine.Map.MovementRestrictionType.CostStamina)
                {
                    staminaCost += 1;
                }

                if (toggRef.mode == GameMode.Commander)
                {
                    collectBackStabbers(u, fromPos);
                }
            }

        }

        public MoveLine(AbsUnit u, System.IO.BinaryReader r)
        {
            isLocal = false;
            Read(r);
            createModel(u);
        }

        void createModel(AbsUnit u)
        {
            //IntVector2 posDiff = toPos - fromPos;
            //float dir = lib.V2ToAngle(posDiff.Vec);


            SpriteName arrow, dot; float y;
            arrowSprite(u, out arrow, out dot, out y);

            //dotModel = new Graphics.Mesh(LoadedMesh.plane, toggLib.ToV3(fromPos, y), MoveLinesGroup.ModelScale,
            //    Graphics.TextureEffectType.Flat, dot, Color.White);// 1f);

            //Vector2 mid = (toPos.Vec + fromPos.Vec) * VectorExt.V2Half;
            //arrowModel = new Graphics.Mesh(LoadedMesh.plane, toggLib.ToV3(mid, y), MoveLinesGroup.ModelScale, 
            //    Graphics.TextureEffectType.Flat, arrow, Color.White);// 1f);
            //arrowModel.Rotation = toggLib.Rotation1DToQuaterion(dir);

            dotModel = DotModel(fromPos, y, dot);
            arrowModel = ArrowModel(fromPos, toPos, y, arrow);
        }

        public static Graphics.Mesh DotModel(IntVector2 pos, float y, SpriteName sprite)
        {
            return new Graphics.Mesh(LoadedMesh.plane, toggLib.ToV3(pos, y), MoveLinesGroup.ModelScale,
               Graphics.TextureEffectType.Flat, sprite, Color.White);// 1f);
        }

        public static Graphics.Mesh ArrowModel(IntVector2 fromPos, IntVector2 toPos, float y, SpriteName sprite)
        {
            Vector2 mid = (toPos.Vec + fromPos.Vec) * VectorExt.V2Half;
            IntVector2 posDiff = toPos - fromPos;
            float dir = lib.V2ToAngle(posDiff.Vec);

            var arrowModel = new Graphics.Mesh(LoadedMesh.plane, toggLib.ToV3(mid, y), MoveLinesGroup.ModelScale,
                Graphics.TextureEffectType.Flat, sprite, Color.White);// 1f);
            arrowModel.Rotation = toggLib.Rotation1DToQuaterion(dir);

            return arrowModel;
        }


        void collectBackStabbers(AbsUnit u, IntVector2 prev)
        {
            //backStabCount = 0;
            backstabberUnits.Clear();

            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                IntVector2 nPos = dir + prev;
                if (toggRef.board.tileGrid.InBounds(nPos))
                {
                    ToggEngine.Map.BoardSquareContent square = toggRef.board.tileGrid.Get(nPos);
                    if (square.unit != null &&
                        toggRef.absPlayers.IsOpponent(square.unit, u) &&
                        square.unit.cmd().data.canBackStab)
                    {
                        //Moving from a tile next to an enemy unit
                        backstabberUnits.Add(square.unit);
                        Vector2 iconPosV2 = (prev.Vec + nPos.Vec) / 2f;

                        Graphics.Mesh stabArrow = new Graphics.Mesh(LoadedMesh.plane, toggLib.ToV3(iconPosV2), new Vector3(0.8f),
                            Graphics.TextureEffectType.Flat, SpriteName.BackStabArrow, Color.White);
                        stabArrow.Y = 0.3f;

                        Graphics.Mesh stabIcon = new Graphics.Mesh(LoadedMesh.plane, stabArrow.Position, new Vector3(0.5f),
                            Graphics.TextureEffectType.Flat, SpriteName.BackStabIcon, Color.White);
                        stabIcon.Y += 0.05f;

                        stabArrow.Rotation = toggLib.Rotation1DToQuaterion(lib.V2ToAngle(-dir.Vec));

                        if (backStabbers == null)
                        {
                            backStabbers = new Graphics.ImageGroup(2);
                        }

                        backStabbers.Add(stabIcon);
                        backStabbers.Add(stabArrow);

                        //backStabCount++;
                    }
                }
            }
        }

        public void setFocus(AbsUnit unit, bool view, float opacity)
        {
            dotModel.Visible = view;
            arrowModel.Visible = view;
           
            dotModel.Opacity = opacity;
            arrowModel.Opacity = opacity;

            SpriteName arrow, dot; float y;
            arrowSprite(unit, out arrow, out dot, out y);

            dotModel.SetSpriteName(dot);
            arrowModel.SetSpriteName(arrow);
            dotModel.Y = y;
            arrowModel.Y = y;
        }

        void arrowSprite(AbsUnit unit, out SpriteName arrow, out SpriteName dot, out float y)
        {

            Engine.PlayerType player = Engine.PlayerType.NON;

            if (unit != null)
            {
                player = unit.Player.pData.Type;
            }

            if (isLocal && !partialLock)
            {
                y = ToggEngine.Map.SquareModelLib.TerrainY_MoveArrow;
            }
            else
            {
                y = ToggEngine.Map.SquareModelLib.TerrainY_MoveArrowLow;
            }

            if (partialLock)
            {
                arrow = SpriteName.cmdMoveArrowLocked;
                dot = SpriteName.cmdMoveArrowDotLocked;
            }
            else
            {
                arrow = GetArrowSprite(boostedMove, player == Engine.PlayerType.Remote);
                dot = GetDotSprite(prevBoostedMove,  player == Engine.PlayerType.Remote);

                //if (boostedMove)
                //{
                //    arrow = SpriteName.cmdMoveArrowStamina;
                //}
                //else if (player == Engine.PlayerType.Remote)
                //{
                //    arrow = SpriteName.cmdMoveArrowAlly;
                //}
                //else
                //{
                //    arrow = SpriteName.cmdMoveArrowMy;
                //}

                //if (prevBoostedMove)
                //{
                //    dot = SpriteName.cmdMoveArrowDotStamina;
                //}
                //else if (player == Engine.PlayerType.Remote)
                //{
                //    dot = SpriteName.cmdMoveArrowDotAlly;
                //}
                //else
                //{
                //    dot = SpriteName.cmdMoveArrowDotMy;
                //}
            }
        }

        public static SpriteName GetArrowSprite(bool boosted, bool ally)
        {
            if (boosted)
            {
                return SpriteName.cmdMoveArrowStamina;
            }
            else if (ally)
            {
                return SpriteName.cmdMoveArrowAlly;
            }
            else
            {
                return SpriteName.cmdMoveArrowMy;
            }
        }

        public static SpriteName GetDotSprite(bool boosted, bool ally)
        {
            if (boosted)
            {
                return SpriteName.cmdMoveArrowDotStamina;
            }
            else if (ally)
            {
                return SpriteName.cmdMoveArrowDotAlly;
            }
            else
            {
                return SpriteName.cmdMoveArrowDotMy;
            }
        }

        public void Write(System.IO.BinaryWriter w)
        {
            fromPos.writeByte(w);
            toPos.writeByte(w);
        }
        void Read(System.IO.BinaryReader r)
        {
            fromPos.readByte(r);
            toPos.readByte(r);
        }

        public void DeleteMe()
        {
            dotModel.DeleteMe();
            arrowModel.DeleteMe();
            clearBackstabs();
        }

        public void clearBackstabs()
        {
            backStabbers?.DeleteAll();
            backStabbers = null;
        }

        public override string ToString()
        {
            return "Move " + conv.ToDir8(toPos -fromPos).ToString() + ": to" + toPos.ToString();
        }
    }
}
