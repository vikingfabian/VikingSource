using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
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

        public Mesh frameModel;
        public List<Mesh> groupModels;
        public Graphics.ImageGroup guiModels = new Graphics.ImageGroup(32);

        /// <summary>
        /// Only for controller input
        /// </summary>
        public bool menuFocus = false;
        //public List<string> menuState = new List<string>();
        //public Army sendUnitsToArmy;
        //public bool menuStateChange = false;

        public Selection(LocalPlayer player, bool isHover)
        {
            frameModel = new Mesh(LoadedMesh.SelectSquareDotted, Vector3.Zero, Vector3.One,
               TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);
            frameModel.AddToRender(DrawGame.TerrainLayer);
            //frameModel.AddToRender(DrawGame.UnitDetailLayer);
            frameModel.setVisibleCamera(player.playerData.localPlayerIndex);
            frameModel.Visible = false;

            subTile = new SelectedSubTile(player, isHover);
        }

        public void ClearSelectionModels()
        {
            guiModels.DeleteAll();
            frameModel.Visible = false;

            if (groupModels != null)
            {
                foreach (var gm in groupModels)
                {
                    gm.Visible = false;
                }
            }
        }

        public void BeginGroupModel()
        {
            if (groupModels == null)
            {
                groupModels = new List<Mesh>();
            }
            else
            {
                foreach (var m in groupModels)
                { 
                    m.Visible = false;
                }
            }
        }

        public void setGroupModel(int index, Vector3 pos, Vector3 scale, bool hover, bool main)
        {
            while (index >= groupModels.Count)
            {
                var model = new Mesh(hover ? LoadedMesh.SelectCircleDotted : LoadedMesh.SelectCircleSolid, Vector3.Zero, scale,
                TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);
                model.AddToRender(DrawGame.UnitDetailLayer);
                model.inPlayerCamera = frameModel.inPlayerCamera;
                model.Visible = false;

                groupModels.Add(model);
            }

            var soldierModel = groupModels[index];
            soldierModel.Visible = true;
            soldierModel.position = pos;
            soldierModel.scale = scale;


            soldierModel.Color = main? Color.White : Color.LightGray;
        }

        //public bool isNew_Detail()
        //{
        //    return detailObj != prevDetailObj;
        //}

        public void begin(bool hover)
        {
            prevObj = obj;
            //prevDetailObj = detailObj;
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
            menuFocus = false;
            isNew = false;
            frameModel.Visible = false;
            if (groupModels != null)
            {
                foreach (var model in groupModels)
                { 
                    model.DeleteMe();
                }
                groupModels = null;
            }
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
