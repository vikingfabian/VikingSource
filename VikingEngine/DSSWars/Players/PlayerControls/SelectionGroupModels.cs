using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Players
{
    class SelectionGroupModels
    {
        public List<Mesh> groupModels = new List<Mesh>();
        int layer;
        int playerCam;
        //public SelectionGroupModels(bool 

        public SelectionGroupModels(int playerCam, bool detailLayer)
        {
            this.playerCam = playerCam;
            layer = detailLayer ? DrawGame.UnitDetailLayer : DrawGame.TerrainLayer;
        }

        public void BeginGroupModel()
        {
            clear();
        }

        public void clear()
        {
            
            foreach (var gm in groupModels)
            {
                gm.Visible = false;
            }
            
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
                model.AddToRender(layer);
                model.setVisibleCamera(playerCam);
                model.Visible = false;

                groupModels.Add(model);
            }

            var unitModel = groupModels[index];
            unitModel.LoadedMeshType = mesh;
            unitModel.Visible = true;
            unitModel.position = pos;
            unitModel.scale = scale;


            unitModel.Color = main ? Color.White : Color.LightGray;
        }

        public void OneFrameModel(Vector3 pos, Vector3 scale, bool hover, bool squareSelection)
        {
            BeginGroupModel();
            setGroupModel(0, pos, scale, hover, true, squareSelection);
        }
    }
}
