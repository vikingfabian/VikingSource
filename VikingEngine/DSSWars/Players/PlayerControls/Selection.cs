using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;
using static System.Collections.Specialized.BitVector32;

namespace VikingEngine.DSSWars.Players
{
    class Selection
    {
        public AbsGameObject obj = null;
        public SelectedSubTile subTile;
        AbsGameObject prevObj = null;
        public bool isNew = false;

        public SelectionGroupModels groupModels_terrian, groupModels_detail;
        public Graphics.ImageGroup guiModels = new Graphics.ImageGroup(32);
        bool currentUnitDetailLayer = false;
        Line targetLine;
        PathVisuals groupPath;
        /// <summary>
        /// Only for controller input
        /// </summary>
        //public bool menuFocus = false;

        int playerCam;
        bool isHover;

        public Selection(LocalPlayer player, bool isHover)
        {
            this.isHover = isHover;
            playerCam = player.playerData.localPlayerIndex;
            
            subTile = new SelectedSubTile(player, isHover);
            groupPath = new PathVisuals(player.playerData.localPlayerIndex);
            groupModels_terrian = new SelectionGroupModels(playerCam, false);
            groupModels_detail = new SelectionGroupModels(playerCam, true);
        }

        public void ClearSelectionModels()
        {
            targetLine?.DeleteMe();
            targetLine = null;
            guiModels.DeleteAll();
            groupPath.DeleteMe();

            groupModels_detail.clear();
            groupModels_terrian.clear();
        }

        public void viewGroupPath(DetailWalkingPath path)
        {
            if (path != null)
            {
                groupPath.refresh(path, isHover);
            }
            else
            {
                groupPath.DeleteMe();
            }
        }

        public void TargetLine(ref Vector3 from, ref Vector3 to)
        {
            if (targetLine == null)
            {
                targetLine = new Line(2, HudLib.IngameUiLayer, Color.Pink);
            }

            targetLine.UpdateLine(Ref.draw.ActivePlayerScreens[playerCam].view.From3DToScreenPos(from), Ref.draw.ActivePlayerScreens[playerCam].view.From3DToScreenPos(to));

        }

        public void hideTargetLine()
        {
            targetLine?.DeleteMe();
            targetLine = null;
        }

        public void begin(bool hover)
        {
            prevObj = obj;

            if (hover)
            {
                obj = null;
            }
        }

        public void end()
        {
            isNew = prevObj != obj;
        }

        public bool clear()
        {
            isNew = false;

            groupModels_detail.clear();
            groupModels_terrian.clear();
            guiModels.DeleteAll();

            if (obj != null)
            {
                obj = null;
                return true;
            }

            return false;
        }
                
    }

    
}
