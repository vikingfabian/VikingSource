using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG
{
    class FollowUp
    {
        Commander.Battle.AttackRoll2 attackAnimation;
        IntVector2 fromPos;
        IntVector2 toPos;
        Commander.Players.LocalPlayer player;
        bool bFollowUp;
        FollowUpToolTip tooltip;
        Graphics.Mesh arrowGUI, dotGUI;
        //MoveLine arrowGUI;

        public FollowUp(Commander.Battle.AttackRoll2 attackAnimation, Commander.Players.LocalPlayer player)
        {
            this.attackAnimation = attackAnimation;
            this.player = player;

            fromPos = attackAnimation.MainAttacker.squarePos;
            toPos = attackAnimation.attackPosition;

            //player.mapControls.availableTiles = new AvailableTilesGUI(new List<IntVector2>{ toPos }, null);
            player.mapControls.SetAvailableTiles(new List<IntVector2> { toPos });

            tooltip = new FollowUpToolTip(player.mapControls);

            //arrowGUI = new Graphics.Mesh(LoadedMesh.plane,
            //    toggLib.ToV3((fromPos.Vec + toPos.Vec) / 2f, 0.1f), Vector3.One,
            //    Graphics.TextureEffectType.Flat, SpriteName.cmdIconFollowUp, Color.White);

            //Vector2 dir = toPos.Vec - fromPos.Vec;
            //arrowGUI.Rotation = toggLib.Rotation1DToQuaterion(lib.V2ToAngle(dir));

            var arrowY = ToggEngine.Map.SquareModelLib.TerrainY_MoveArrow;
            var arrowSprite = MoveLine.GetArrowSprite(true, false);
            var dotSprite = MoveLine.GetDotSprite(true, false);

            arrowGUI = MoveLine.ArrowModel(fromPos, toPos, arrowY, arrowSprite);
            dotGUI = MoveLine.DotModel(toPos, arrowY, dotSprite);
            //arrowGUI = new MoveLine(toPos, fromPos, null, true, null);

            onNewTile();
        }

        public bool update()
        {
            player.mapControls.updateMapMovement(true);

            if (toggRef.inputmap.click.DownEvent)//player.inputMap.DownEvent(Input.ButtonActionType.MenuClick))
            {
                if (bFollowUp)
                {
                    Commit(attackAnimation);
                }
                return true;
                //player.gamePhase.endAttackAnimation();
            }
            return false;
        }

        public void onNewTile()
        {
            bFollowUp = player.mapControls.selectionIntV2 == toPos;
            player.mapControls.setAvailable(bFollowUp);

            tooltip.refresh(bFollowUp);

            arrowGUI.Color = bFollowUp ? Color.White : ColorExt.VeryDarkGray;
            dotGUI.Color = arrowGUI.Color;
        }

        //public void commit()
        //{
        //    attackAnimation.attacker.SlideToSquare(toPos, true);

        //    new NetActionFollowUp_Host(attackAnimation.attacker, toPos);

        //    if (attackAnimation.attacker.HasProperty(UnitPropertyType.Frenzy) && !attackAnimation.attacker.UsedFrenzy)
        //    {
        //        attackAnimation.attacker.UsedFrenzy = true;
        //        attackAnimation.attacker.AttackedThisTurn = false;
        //    }
        //}

        public static void Commit(Commander.Battle.AttackRoll2 attackAnimation)
        {
            var toPos = attackAnimation.attackPosition;

            attackAnimation.MainAttacker.SlideToSquare(toPos, true);

            //new NetActionFollowUp_Host(attackAnimation.attacker, toPos);

            if (attackAnimation.MainAttacker.HasProperty(UnitPropertyType.Frenzy) && !attackAnimation.MainAttacker.UsedFrenzy)
            {
                attackAnimation.MainAttacker.UsedFrenzy = true;
                attackAnimation.MainAttacker.AttackedThisTurn = false;
            }
        }

        public void DeleteMe()
        {
            player.mapControls.removeAvailableTiles();
            player.mapControls.removeToolTip();
            arrowGUI.DeleteMe();
            dotGUI.DeleteMe();
        }
    }

    class FollowUpToolTip : ToGG.ToggEngine.Display2D.AbsToolTip
    {
        //Graphics.Image checkBoxIcon;
        RbText yesNoText;

        public FollowUpToolTip(MapControls mapcontrols)
            : base(mapcontrols)
        {
            RichBoxContent members = new RichBoxContent();
            members.Add(new RbImage(SpriteName.cmdMoveArrowStamina));
            members.Add(new RbBeginTitle());
            members.Add(new RbText("Follow Up?"));

            members.Add(new RbNewLine());
            yesNoText = new RbText("---", HudLib.UnavailableRedCol);
            members.Add(yesNoText);
            
            AddRichBox(members);

            refresh(false);
            //Graphics.TextG title = new Graphics.TextG(LoadedFont.Regular,
            //    Vector2.Zero, Engine.Screen.TextSizeV2 * 1.2f, Graphics.Align.Zero,
            //    "Follow Up?", Color.White, Layer);
            ////title.border = Graphics.TextBorderType.ShadowSouthEast;
            //Add(title);

            //checkBoxIcon = new Graphics.Image(SpriteName.NO_IMAGE,
            //   new Vector2(title.Xpos, title.MeasureBottomPos()),
            //   Engine.Screen.SmallIconSizeV2, Layer);
            //Add(checkBoxIcon);

            //yesNoText = new Graphics.TextG(LoadedFont.Regular,
            //    checkBoxIcon.RightTop, Vector2.One, Graphics.Align.Zero,
            //    "XX", Color.White, Layer);
            //yesNoText.SetHeight(checkBoxIcon.Height);
            ////yesNoText.border = Graphics.TextBorderType.ShadowSouthEast;
            //Add(yesNoText);

            //VectorRect area = new VectorRect(title.Position, new Vector2(title.MeasureText().X, checkBoxIcon.Bottom));
            //area.AddRadius(Engine.Screen.IconSize * 0.1f);
            //Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea,
            //    area.Position, area.Size, ImageLayers.AbsoluteBottomLayer);
            //bg.Color = Color.Black;
            //bg.LayerBelow(title);
            //bg.Opacity = 0.7f;
            //Add(bg);

            //completeSetup(area.Size);
        }

        public void refresh(bool yes)
        {
            if (yes)
            {
                //checkBoxIcon.SetSpriteName(SpriteName.LfCheckYes);
                //checkBoxIcon.Color = Color.LightGreen;
                yesNoText.pointer.Color = HudLib.AvailableGreenCol;
                yesNoText.pointer.TextString = "YES";
            }
            else
            {
                //checkBoxIcon.SetSpriteName(SpriteName.LfCheckNo);
                //checkBoxIcon.Color = Color.LightPink;
                yesNoText.pointer.Color = HudLib.UnavailableRedCol;
                yesNoText.pointer.TextString = "NO";
            }

            //yesNoText.Color = checkBoxIcon.Color;
        }
    }
}
