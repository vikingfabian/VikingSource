using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Core.DSSWars;
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
        PathVisuals testPath3Visuals;
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
            testPath3Visuals  = new PathVisuals(player.playerData.localPlayerIndex);
            groupModels_terrian = new SelectionGroupModels(playerCam, false);
            groupModels_detail = new SelectionGroupModels(playerCam, true);
        }

        public void ClearSelectionModels()
        {
            targetLine?.DeleteMe();
            targetLine = null;
            guiModels.DeleteAll();
            groupPath.DeleteMe();
            testPath3Visuals.DeleteMe();

            groupModels_detail.clear();
            groupModels_terrian.clear();
        }

        DetailWalkingPathResult prevpath = null;
        public void viewGroupPath(DetailWalkingPathResult path)
        {
            if (path != null)
            {
                groupPath.refresh(path, isHover);
            }
            else
            {
                prevpath = null;
                groupPath.DeleteMe();
            }

            //Test the same path
            if (path != null && path != prevpath && path.nodes.Count >= 3)
            {
                prevpath = path;
                IntVector2 fromPos = path.nodes.Last().position;
                IntVector2 toPos = path.nodes.First().position;

                //var layerPath = DssRef.world.GetPath3FromPool();
                var highPath = DssRef.world.path3Thread_test.FindHighPath(WP.MaptileToWorldPosXZ(fromPos), WP.MaptileToWorldPosXZ(toPos), 0, false, false);
                var layPath = DssRef.world.path3Thread_test.FindDetailPath(highPath, WP.MaptileToWorldPosXZ(fromPos), WP.MaptileToWorldPosXZ(toPos), Rotation1D.D0, false, false, false);

                testPath3Visuals.refresh2(layPath, isHover);
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

        public GameObjectType objectType()
        {
            if (obj != null)
            {
                return obj.gameobjectType();
            }

            return GameObjectType.NONE;
        }
                
    }

    
}
