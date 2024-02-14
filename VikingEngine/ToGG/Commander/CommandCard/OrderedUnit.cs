using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    class OrderedUnit : ICheckListMember
    {
        public const SpriteName OrderSpriteFull = SpriteName.cmdOrderCheckFlat;
        public const SpriteName OrderSpriteHollow = SpriteName.cmdOrderCheckOutline;

        public static readonly Color OrderActionReadyCol = Color.White;//new Color(116, 234, 255);
        public static readonly Color OrderActionCompleteCol = ColorExt.GrayScale(0.3f);

        static readonly Vector3 CheckMarkScale_Large = new Vector3(0.22f);
        static readonly Vector3 CheckMarkScale_Small = CheckMarkScale_Large * 0.7f;

        static readonly Vector3 PhaseMarkScale = new Vector3(0.18f);

        public AbsUnit unit;
        public Graphics.Mesh checkMark;
        public Graphics.Mesh gamePhaseMark;
        public CheckState state;
        public AttackTargetCollection attackTargets = null;

        public OrderedUnit(AbsUnit unit, CheckState state = CheckState.Enabled)
        {
            this.unit = unit;
            unit.orderStartPos = unit.squarePos;

            if (unit.order != null)
            {
                throw new Exception("Order duplicate");
            }
            unit.order = this;
            this.state = state; 

            checkMark = new Graphics.Mesh(LoadedMesh.plane, new Vector3(0.3f, ModelLayers.CheckMarkY, 0.48f), CheckMarkScale_Large, 
                Graphics.TextureEffectType.Flat, SpriteName.MissingImage, Color.White);            
            unit.soldierModel.getOrCreateChildModels().AddAndUpdate(checkMark);

            gamePhaseMark = new Graphics.Mesh(LoadedMesh.plane, toggLib.TowardCamVector_Yis1 * ModelLayers.GamePhaseMark, PhaseMarkScale,
                Graphics.TextureEffectType.Flat, SpriteName.cmdUnitMoveGui, Color.White);
            gamePhaseMark.Rotation = toggLib.PlaneTowardsCam;
            gamePhaseMark.Visible = false;
            unit.soldierModel.getOrCreateChildModels().AddAndUpdate(gamePhaseMark);

            refreshCheckMark();

            if (state == CheckState.Enabled)
            {
                new Effects.OrderUnitEffect(checkMark, true);

                writeOrder(true);
            }
        }

        public void DeleteMe()
        {
            checkMark.DeleteMe();
            writeOrder(false);

            gamePhaseMark.DeleteMe();

            unit.deleteMoveLines();
            unit.order = null;
        }

        void writeOrder(bool addOrder)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.cmdOrderUnit, Network.PacketReliability.Reliable);
            unit.writeIndex(w);
            w.Write(addOrder);
        }

        //bool enabled = false;
        public bool CheckList_Enabled
        {
            get { return state == CheckState.Enabled; }
            set
            {
                state = value? CheckState.Enabled : CheckState.Disabled;

                refreshCheckMark();
            }
        }

        public void SetState(CheckState state)
        {
            this.state = state;
            refreshCheckMark();
        }

        public void PhaseMark(SpriteName phaseSprite, float scale = 1f)
        {
            //checkMark.Visible = false;
            checkMark.scale = CheckMarkScale_Small;
            gamePhaseMark.Visible = true;
            gamePhaseMark.SetSpriteName(phaseSprite);
            gamePhaseMark.scale = PhaseMarkScale * scale;
        }

        public void PhaseMarkVisible(bool visible)
        {
            //checkMark.Visible = visible;
            gamePhaseMark.Visible = visible;
        }

        void refreshCheckMark()
        {
            
            if (unit.Alive)
            {
                switch (state)
                {
                    case CheckState.Enabled:
                        checkMark.SetSpriteName(OrderSpriteFull);
                        checkMark.Color = Color.White;
                        gamePhaseMark.Color = OrderActionReadyCol;
                        break;
                    case CheckState.Disabled:
                        checkMark.SetSpriteName(OrderSpriteHollow);
                        checkMark.Color = Color.DarkGray;
                        gamePhaseMark.Color = OrderActionCompleteCol;                        
                        break;
                    case CheckState.Suggested:
                        checkMark.SetSpriteName(OrderSpriteHollow);
                        checkMark.Color = Color.White;
                        gamePhaseMark.Color = Color.White;
                        break;
                }
            }

            //gamePhaseMark.Color = checkMark.Color;
        }        

        public override string ToString()
        {
            return unit.ToString() + " order { enabled(" + CheckList_Enabled.ToString() + "), made attack(" + unit.AttackedThisTurn.ToString() + ") }";
        }
        
    }

    enum CheckState
    {
        Suggested,
        Enabled,
        Disabled,
    }
}
