using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.Timer;

namespace VikingEngine.DSSWars.Players
{
    class SelectionGroupModels
    {
        public List<Mesh> groupModels = new List<Mesh>();
        int layer;
        int playerCam;
        int count = 0;

        public bool Visible => count > 0;
        //public SelectionGroupModels(bool 

        public SelectionGroupModels(int playerCam, bool detailLayer)
        {
            this.playerCam = playerCam;
            layer = detailLayer ? DrawGame.UnitDetailLayer : DrawGame.MidLayer;
        }

        public void Draw(int cameraIndex)
        {
            for (int i = 0; i < count; i++)
            {
                groupModels[i].Draw(cameraIndex);
            }
        }

        public void BeginGroupModel()
        {
            clear();
        }

        public void clear()
        {
            if (Input.Keyboard.Ctrl)
            {
                lib.DoNothing();
            }

            for (int i = 0; i < count; i++)
            {
                groupModels[i].Visible = false;
            }
            count = 0;
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
                //model.AddToRender(layer);
                //model.setVisibleCamera(playerCam);
                model.Visible = false;

                groupModels.Add(model);
            }

            var unitModel = groupModels[index];
            if (index >= count)
            {
                count = index + 1;
            }
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
