using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
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

        //public Mesh frameModel;
        public List<Mesh> groupModels;
        public Graphics.ImageGroup guiModels = new Graphics.ImageGroup(32);
        bool currentUnitDetailLayer = false;
        Mesh targetLine;
        /// <summary>
        /// Only for controller input
        /// </summary>
        public bool menuFocus = false;

        int playerCam;
        //public List<string> menuState = new List<string>();
        //public Army sendUnitsToArmy;
        //public bool menuStateChange = false;

        public Selection(LocalPlayer player, bool isHover)
        {
            playerCam = player.playerData.localPlayerIndex;
            //frameModel = new Mesh(LoadedMesh.SelectSquareDotted, Vector3.Zero, Vector3.One,
            //   TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);
            //frameModel.AddToRender(DrawGame.TerrainLayer);
            //frameModel.AddToRender(DrawGame.UnitDetailLayer);
            //frameModel.setVisibleCamera(player.playerData.localPlayerIndex);
            //frameModel.Visible = false;

            subTile = new SelectedSubTile(player, isHover);
        }

        public void ClearSelectionModels()
        {
            targetLine?.DeleteMe();
            targetLine = null;
            guiModels.DeleteAll();
            //frameModel.Visible = false;

            if (groupModels != null)
            {
                foreach (var gm in groupModels)
                {
                    gm.Visible = false;
                }
            }
        }

       

        public void BeginGroupModel(bool unitDetail)
        {
            if (currentUnitDetailLayer != unitDetail)
            {
                ClearSelectionModels();
                currentUnitDetailLayer = unitDetail;
            }

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

        public void TargetLine(ref Vector3 from, ref Vector3 to)
        {
            Vector3 dir =to - from;
           

            if (targetLine == null)
            {
                targetLine = new Mesh(LoadedMesh.cube_repeating,
                    Vector3.Zero, new Vector3(0.05f), TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);
                targetLine.AddToRender(DrawGame.UnitDetailLayer);
            }

            dir = VectorExt.Normalize(dir, out float l);
            Vector3 defaultForward = Vector3.Forward;

            // If dir is not already aligned with defaultForward
            //if (dir != defaultForward && dir != -defaultForward)
            //{
            //    Vector3 axis = Vector3.Cross(defaultForward, dir);
            //    float angle = (float)Math.Acos(Vector3.Dot(defaultForward, dir));

            //    if (axis.LengthSquared() > 0.0001f) // Ensure valid axis
            //    {
            //        axis = Vector3.Normalize(axis);
                    targetLine.Rotation.QuadRotation = Quaternion.CreateFromAxisAngle(dir, 0);
            //    }
            //}
            //targetLine.Rotation.QuadRotation = Quaternion.(Vector3.Forward, dir); //is this correct?
            targetLine.scale = new Vector3(0.05f);
            targetLine.ScaleY = l;
            targetLine.position = from + l * 0.5f * dir;
            targetLine.position.Y += 0.05f;
        }

        public void setGroupModel(int index, Vector3 pos, Vector3 scale, bool hover, bool main, bool squareSelection)
        {
            LoadedMesh mesh;
            if (squareSelection)
            {
                mesh = hover ? LoadedMesh.SelectSquareDotted : LoadedMesh.SelectSquareSolid;
            }
            else
            {
                mesh = hover ? LoadedMesh.SelectCircleDotted : LoadedMesh.SelectCircleSolid;
            }

            while (index >= groupModels.Count)
            {                
                var model = new Mesh(mesh, Vector3.Zero, scale,
                TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);
                model.AddToRender(currentUnitDetailLayer? DrawGame.UnitDetailLayer : DrawGame.TerrainLayer);
                model.setVisibleCamera(playerCam);
                model.Visible = false;

                groupModels.Add(model);
            }

            var soldierModel = groupModels[index];
            soldierModel.LoadedMeshType = mesh;
            soldierModel.Visible = true;
            soldierModel.position = pos;
            soldierModel.scale = scale;


            soldierModel.Color = main? Color.White : Color.LightGray;
        }

        public void OneFrameModel(bool unitDetail, Vector3 pos, Vector3 scale, bool hover, bool squareSelection)
        {
            BeginGroupModel(unitDetail);
            setGroupModel(0, pos, scale, hover, true, squareSelection);
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
            //frameModel.Visible = false;
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
