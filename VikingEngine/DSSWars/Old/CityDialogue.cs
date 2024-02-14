//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace Game1.RTS.Display
//{
//    ///// <summary>
//    ///// Create a bar with info about the city
//    ///// </summary>
//    //class CityDialogue : AbsSelectedObjDialogue
//    //{
//    //    const float IconSz = 64;

//    //    GameObject.City city;
//    //    float rapidPurchaseTimer = 0;
//    //    Vector2 unitButtonCenter;

//    //    static readonly GameObject.TroopType[] unitButtonTypes = new GameObject.TroopType[]
//    //    {
//    //        GameObject.TroopType.Spearmen,
//    //        GameObject.TroopType.Cavalry,
//    //        GameObject.TroopType.SeaWarrior,
//    //        GameObject.TroopType.Ballista,
//    //    };

//    //    int? selectedUnitType = null;
//    //    Graphics.Image[] unitButtons;
//    //    Graphics.TextS[] unitCosts;
//    //    Graphics.Image unitTypeSelectionImg;

//    //    public CityDialogue(GameObject.City city, WorldData world, ButtonsOverview buttonsOverview, VectorRect playerSafeArea)
//    //        :base(world, buttonsOverview)
//    //    {
//    //        //this.p = p;
//    //        this.city = city;

//    //        buttonsOverview.Update(new List<ButtonOption>
//    //        {
//    //            new ButtonOption(false, TileName.LeftStick, "Select"),
//    //            new ButtonOption(false, TileName.ButtonA, "Buy"),
//    //            new ButtonOption(false, TileName.ButtonB, "Exit"),

//    //        });

            
//    //        unitButtonCenter = playerSafeArea.BottomLeft + new Vector2(IconSz * 1.5f, -IconSz * 1.5f);
//    //        unitButtons = new Graphics.Image[IntVector2.From4Dirs.Length];
//    //        unitCosts = new Graphics.TextS[IntVector2.From4Dirs.Length];

//    //        for (int i = 0; i < IntVector2.From4Dirs.Length; ++i)
//    //        {
//    //            Vector2 pos = unitButtonCenter + IntVector2.From4Dirs[i].Vec * IconSz;
//    //            unitButtons[i] = new Graphics.Image(GameObject.SoldierUnits.TypeIcon(unitButtonTypes[i]), 
//    //                pos, new Vector2(IconSz), RTSlib.LayerUnitDialogue, true);
//    //            unitCosts[i] = new Graphics.TextS(LoadedFont.Lootfest, pos, new Vector2(1.0f), Graphics.Align.Zero,
//    //                city.ArmyUnitCost.Get(unitButtonTypes[i]).ToString(), Color.White, RTSlib.LayerUnitDialogue - 1);
//    //        }
            
//    //        unitTypeSelectionImg = new Graphics.Image(TileName.InterfaceBorder, unitButtonCenter, 
//    //            new Vector2(IconSz), RTSlib.LayerUnitDialogueSelection, true);
//    //    }

//    //    /// <returns>Close dialogue</returns>
//    //    override public bool UpdateInput(Input.AbsControllerInstance controller)
//    //    {
//    //        JoyStickValue leftPad = controller.JoyStickValue(Stick.Left);
//    //        if (leftPad.Direction.Length() > 0.5f)
//    //        {
//    //            if (Math.Abs(leftPad.Direction.X) > Math.Abs(leftPad.Direction.Y))
//    //            {
//    //                if (leftPad.Direction.X > 0) selectedUnitType = 1;
//    //                else selectedUnitType = 3;
//    //            }
//    //            else
//    //            {
//    //                if (leftPad.Direction.Y > 0) selectedUnitType = 2;
//    //                else selectedUnitType = 0;
//    //            }
//    //            unitTypeSelectionImg.Position = unitButtons[selectedUnitType.Value].Position;

//    //            //BUY UNITS
//    //            if (controller.IsButtonDown(Buttons.A))
//    //            {
//    //                if (rapidPurchaseTimer >= 200f || controller.KeyDownEvent(Buttons.A))
//    //                {
//    //                    rapidPurchaseTimer = 0;
//    //                    GameObject.Army a = city.faction.BuySoldiers(unitButtonTypes[selectedUnitType.Value], city, world);
//    //                    if (a != null)
//    //                    {//animate the purchase
//    //                        Vector2 start = unitButtons[selectedUnitType.Value].Position;
//    //                        Engine.PlayerData p = Engine.XGuide.GetPlayer(controller.Index);
//    //                        Vector2 end = p.view.ImageContainer.LayerToScreenPos(a.SelectionCenter) + p.view.DrawAreaF.Position;

//    //                        Vector2 diff = end - start;

//    //                        RTSlib.SetHUDLayer();
//    //                        Graphics.Image icon = new Graphics.Image(
//    //                            GameObject.SoldierUnits.TypeIcon(unitButtonTypes[selectedUnitType.Value]),
//    //                            start, new Vector2(IconSz * 0.8f), RTSlib.LayerUnitDialogue, true);
//    //                        const float animTime = 600;
//    //                        new Graphics.Motion2d(Graphics.MotionType.MOVE, icon, diff, Graphics.MotionRepeate.NO_REPEATE,
//    //                            animTime, true);
//    //                        new Timer.Terminator(animTime - 33, icon);
//    //                    }
//    //                }
//    //                rapidPurchaseTimer += Ref.DeltaTimeMs;
//    //            }
//    //        }
//    //        else
//    //        {
//    //            selectedUnitType = null;
//    //            unitTypeSelectionImg.Position = unitButtonCenter;
//    //        }

                
//    //        if (controller.KeyDownEvent(Buttons.B, Buttons.Back, Buttons.Start))
//    //        {
//    //            return true;
//    //        }
//    //        return false;
//    //    }

       

//    //    public override void DeleteMe()
//    //    {
//    //        base.DeleteMe();
//    //        foreach (Graphics.Image img in unitButtons)
//    //        {
//    //            img.DeleteMe();
//    //        }
//    //        foreach (Graphics.TextS img in unitCosts)
//    //        {
//    //            img.DeleteMe();
//    //        }
//    //        unitTypeSelectionImg.DeleteMe();
//    //    }
//    //}
//}
