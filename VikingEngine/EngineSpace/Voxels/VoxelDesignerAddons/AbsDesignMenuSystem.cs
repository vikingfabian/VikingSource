using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using VikingEngine.Input;
using Microsoft.Xna.Framework;

namespace VikingEngine.Voxels
{
    class AbsDesignMenuSystem
    {
        public HUD.Gui menu;
        AbsVoxelDesigner designer;

        public AbsDesignMenuSystem(AbsVoxelDesigner designer)
        {
            this.designer = designer;
        }

        public bool Update()
        {
            return menu.Update();
        }

        virtual public void openMenu() { throw new NotImplementedException(); }
        //virtual protected DrawTool drawTool
        //{
        //    get { return designer.tool; }
        //    set { designer.tool = value; }
        //}

        virtual public void selectionMenu()
        {

            // HUD.File file = new HUD.File();
            var layout = new GuiLayout("Selection Menu", menu);
            {
                selectionMenuBase(layout);
            } layout.End();
            //menu.Visible = true;
            // menu.File = file;
        }

        protected void selectionMenuBase(GuiLayout layout)
        {
            new GuiTextButton("Cancel", null, closeMenu, false, layout);
            new GuiTextButton("Replace Material", null, linkFindReplaceSelectionMaterials, true, layout);
            new GuiTextButton("Copy", null, new GuiAction1Arg<bool>(designer.copySelectedVoxels, false), false, layout);
            new GuiTextButton("Cut", null, new GuiAction1Arg<bool>(designer.copySelectedVoxels, true), false, layout);
            //new GuiTextButton("Save as template", null, designer.LinkSelSaveTemplate, true, layout);
            new GuiTextButton("Make Stamp", null, new GuiAction1Arg<bool>(designer.stampSelection, true), false, layout);
            new GuiTextButton("Rotate/Flip", null, LinkRotateFlip, true, layout);
        }

        protected void LinkRotateFlip()
        {
            var layout = new GuiLayout("Rotate/Flip", menu);
            {
                roatateFlipMenu(layout);
                allFramesChkBox(layout);
            } layout.End();
        }

        protected void allFramesChkBox(HUD.GuiLayout layout)
        {
            if (designer is LootFest.Editor.VoxelDesigner)
            {
                if (((LootFest.Editor.VoxelDesigner)designer).inGame)
                    return;
            }
            new GuiCheckbox("All frames", "Make the same action on all frames", designer.bRepeateOnAllFramesProperty, layout);
        }

        protected void roatateFlipMenu(HUD.GuiLayout layout)
        {
            new HUD.GuiTextButton("Rotate C", null, designer.LinkSelRotateC, false, layout);
            new HUD.GuiTextButton("Rotate CC", null, designer.LinkSelRotateCC, false, layout);
            new HUD.GuiTextButton("Mirror", "Will flip all blocks from left to rigth, it is depending on what angle you have the camera",
                designer.mirrorSelection, false, layout);
            new HUD.GuiTextButton("Flip Up N Down", null, designer.LinkSelFlipY, false, layout);
            new HUD.GuiTextButton("Flip lying/standing", null, designer.flipLyingToStanding, false, layout);
        }

        void linkFindReplaceSelectionMaterials()//HUD.File file, Action<Data.MaterialType> callBack, bool showMoreMenusArrow)
        {
            //bool[] haveMaterial = selectedVoxels.ContainMaterials();

            var used = designer.materialsInSelection();



            var layout = new GuiLayout(SpriteName.NO_IMAGE, "Model materials", menu, HUD.GuiLayoutMode.MultipleColumns);
            {
                new GuiTextButton("Empty", null, new GuiAction1Arg<ushort>(designer.linkReplaceSelectionMaterials, VikingEngine.LootFest.Map.HDvoxel.BlockHD.EmptyBlock),
                    true, layout);
                //for (MaterialType type = (MaterialType)1; type < MaterialType.NUM; type++)
                //{
                //    if (haveMaterial[(int)type])
                //    {
                //        new GuiIcon(BlockTextures.MaterialTile(type), TextLib.EnumName(type.ToString()),
                //            new GuiAction1Arg<MaterialType>(linkReplaceSelectionMaterials, type), false, layout);
                //    }
                //}

                foreach (var m in used)
                {
                    Color col = VikingEngine.LootFest.Map.HDvoxel.BlockHD.ToColor(m);

                    var icon = new GuiIcon(SpriteName.WhiteArea, col.ToString(),
                        new GuiAction1Arg<ushort>(designer.linkReplaceSelectionMaterials, m), false, layout);
                    icon.iconImage.Color = col;
                }

                allFramesChkBox(layout);
            } layout.End();
        }

        virtual public void closeMenu()
        {
            if (menu != null)
            {
                menu.DeleteMe();
                menu = null;
                designer.updateFrameInfo();
                Input.Mouse.Visible = false;

                //designer.inputMap.SetGameStateLayout(ControllerActionSetType.EditorControls);
            }
        }
        public bool InMenu
        {
            get { return menu != null && menu.Visible; }
        }
    }
}
