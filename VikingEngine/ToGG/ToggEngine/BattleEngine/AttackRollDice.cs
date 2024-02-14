using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
   
    class AttackRollDice
    {
        bool hasRollingTexture;
        public DiceModel model;
        BattleDice dice;
        int index;
        public AttackRollDiceState state = 0;
        public BattleDiceSide dieSide = BattleDiceSide.NoResult;
        VectorRect hudArea;
        Graphics.RectangleLines selectionOutline = null;

        public int rotationType_0non_1idle_2full = 0;
        DiceRoll rnd = new DiceRoll();
        CirkleCounterUp currentHiddenFace;
        int facesBeforeResult = 4;
        float rotationAdd;
        float fullRotationSpeed = 17f;

        bool idleBumpMotion = false;
        float idleBumpRadians;

        Display2D.DiceTooltip tooltip = null;
        public AttackRollDiceDisplay diceDisplay;

        public AttackRollDice(AttackRollDiceDisplay diceDisplay, BattleDice dice, Vector2 position, int index)
        {
            this.diceDisplay = diceDisplay;
            this.dice = dice;
            this.index = index;

            model = new DiceModel(position);
            setDieIconTexture(dice);
            hudArea = model.drawcontainer.Area;
            hudArea.AddRadius(-hudArea.Width * 0.06f);
            selectionOutline = new RectangleLines(hudArea, HudLib.SelectionOutlineThickness, 0.5f, HudLib.AttackWheelLayer - 1);
            selectionOutline.Visible = false;
        }

        public void Update()
        {
            if (toggRef.mode == GameMode.HeroQuest &&
                state == AttackRollDiceState.Waiting)
            {

                bool selected = hudArea.IntersectPoint(Input.Mouse.Position);

                if (selected != selectionOutline.Visible)
                {
                    selectionOutline.Visible = selected;
                    if (selected)
                    {
                        setDieTexture(dice);

                        if (tooltip == null)
                        {
                            tooltip = new Display2D.DiceTooltip(hudArea, dice);
                        }
                    }
                    else
                    {
                        removeTooltip();
                        setDieIconTexture(dice);
                    }
                }
            }

            if (state < AttackRollDiceState.EndBounce)
            {
                updateDieRotation();
            }
            else if (state == AttackRollDiceState.EndBounce)
            {
                updateRotationBounce();
            }
        }

        void removeTooltip()
        {
            tooltip?.DeleteAll();
            tooltip = null;
        }

        void updateDieRotation()
        {
            if (rotationType_0non_1idle_2full == 0)
            {
                if (idleBumpMotion)
                {
                    idleBumpRadians += 6f * Ref.DeltaTimeSec;

                    if (idleBumpRadians >= MathExt.TauOver2)
                    {
                        idleBumpMotion = false;
                        model.rotation.radians = 0;
                    }
                    else
                    {
                        model.rotation.radians = MathExt.Sinf(idleBumpRadians) * 0.14f;
                    }
                }
            }
            else if (rotationType_0non_1idle_2full == 1 ||
                rotationType_0non_1idle_2full == 2)
            {
                idleBumpMotion = false;

                if (rotationType_0non_1idle_2full == 1)
                {
                    rotationAdd += Ref.DeltaTimeSec * 2f;
                }
                else
                {
                    rotationAdd += Ref.DeltaTimeSec * fullRotationSpeed;

                    if (fullRotationSpeed > 10f)
                    {
                        if (state == AttackRollDiceState.GotResult)
                        {
                            fullRotationSpeed -= Ref.DeltaTimeSec * 5f;
                        }
                        else
                        {
                            fullRotationSpeed -= Ref.DeltaTimeSec * 2f;
                        }
                    }
                }

                if (rotationAdd >= MathHelper.PiOver2)
                {
                    rotationAdd -= MathHelper.PiOver2;
                    
                    nextDiceSide(currentHiddenFace.value);

                    if (state == AttackRollDiceState.GotResult)
                    {
                        facesBeforeResult--;

                        if (facesBeforeResult == 3)
                        {
                            SpriteName icon = BattleDice.ResultIcon(dieSide.result);
                            model.setFaceTexture(currentHiddenFace.value, icon);
                        }
                        else if (facesBeforeResult == 0)
                        {
                            //rotationAdd = 0;
                            rotationType_0non_1idle_2full = 0;
                            state = AttackRollDiceState.EndBounce;
                            new Graphics.Motion2d(MotionType.MOVE, model.drawcontainer, VectorExt.V2FromY(DiceModel.Size.Y * 0.16f), MotionRepeate.BackNForwardOnce, 80, true);
                        }

                    }

                    model.refreshModel();

                    currentHiddenFace.Next();
                }
                model.rotation.radians = rotationAdd + currentHiddenFace.value * MathHelper.PiOver2;
            }

            model.update();
        }

        void updateRotationBounce()
        {
            for (int i = 0; i < Ref.GameTimePassed16ms; ++i)
            {
                var force = -rotationAdd * 8f;
                fullRotationSpeed = fullRotationSpeed * 0.6f + force;
            }
            rotationAdd += Ref.DeltaTimeSec * fullRotationSpeed;

            model.rotation.radians = rotationAdd + currentHiddenFace.value * MathHelper.PiOver2;
            model.update();
        }

        

        public void setDieIconTexture(BattleDice dice)
        {
            hasRollingTexture = false;
            this.dice = dice;

            rotationType_0non_1idle_2full = 0;
            model.rotation = Rotation1D.D0;
            rotationAdd = 0;

            SpriteName front, side;
            dice.textures(out front, out side);

            model.side1.setSprite(side, Dir4.N);
            model.side2.setSprite(side, Dir4.N);

            model.setFaceTexture(0, front);
            model.setFaceTexture(1, side);
            model.setFaceTexture(2, side);
            model.setFaceTexture(3, side);

            model.refreshModel();
        }

        public void beginRoll(BattleDice dice, int index)
        {
            if (!hasRollingTexture)
            {
                hasRollingTexture = true;
                setDieTexture(dice);

                rotationAdd = index * 0.4f + Ref.rnd.Plus_MinusF(0.1f);
            }
        }

        public void setDieTexture(BattleDice dice)
        {
            this.dice = dice;

            rotationType_0non_1idle_2full = 1;
            SpriteName sideTex = toggRef.mode == GameMode.Commander ? SpriteName.cmdDieTexSide : SpriteName.hqDieTexSide ;


            model.side1.setSprite(sideTex, Dir4.N);
            model.side2.setSprite(sideTex, Dir4.N);

            for (int modelFace = 0; modelFace < 4; modelFace++)
            {
                nextDiceSide(modelFace);
            }
            currentHiddenFace = new CirkleCounterUp(0, 3);

            model.refreshModel();
        }

        void nextDiceSide(int modelface)
        {
            SpriteName icon = BattleDice.ResultIcon(dice.NextRandom(rnd).result);
            model.setFaceTexture(modelface, icon);
        }

        public void SetResult(BattleDiceSide result)
        {
            //Debug.Log("result: " + result.ToString());
            this.dieSide = result;
            state =  AttackRollDiceState.GotResult;
            
        }

        public void idleBump()
        {
            idleBumpMotion = true;
            idleBumpRadians = 0;
        }

        public void DeleteMe()
        {
            model.DeleteMe();
            selectionOutline.DeleteMe();
            removeTooltip();
        }
    }

    enum AttackRollDiceState
    {
        Waiting,
        RollButtonHover,
        Rolling,
        GotResult,
        EndBounce,
        ViewResult,
    }

    
}
