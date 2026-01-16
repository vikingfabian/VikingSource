using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.Voxels
{
    class PencilShadow
    {
        const float LayerAboveSurface = 0.05f;

        int currentModel;
        List<Graphics.Mesh> models = new List<Graphics.Mesh>(8);
        public bool visible = true;

        public PencilShadow()
        { }

        public void update(AbsVoxelDesigner designer)
        {
            hide();

            var limits = designer.voxelProject.drawLimits;
            int frame = designer.CurrentFrame;
            if (visible)
            {
                {
                    IntVector3 pos = designer.designerInterface.drawCoord;
                    for (pos.X =limits.Min.X; pos.X <=limits.Max.X; ++pos.X)
                    {
                        bool isBlock = designer.Get(pos) != LootFest.Map.HDvoxel.BlockHD.EmptyBlock;

                        dirCheck(designer, isBlock, pos, Dimensions.X, -1);
                        dirCheck(designer, isBlock, pos, Dimensions.X, 1);
                    }
                }
                {
                    IntVector3 pos = designer.designerInterface.drawCoord;
                    for (pos.Y =limits.Min.Y; pos.Y <=limits.Max.Y; ++pos.Y)
                    {
                        bool isBlock = designer.Get(pos) != LootFest.Map.HDvoxel.BlockHD.EmptyBlock;

                        dirCheck(designer, isBlock, pos, Dimensions.Y, -1);
                        dirCheck(designer, isBlock, pos, Dimensions.Y, 1);
                    }
                }
                {
                    IntVector3 pos = designer.designerInterface.drawCoord;
                    for (pos.Z =limits.Min.Z; pos.Z <=limits.Max.Z; ++pos.Z)
                    {
                        bool isBlock = designer.Get(pos) != LootFest.Map.HDvoxel.BlockHD.EmptyBlock;

                        dirCheck(designer, isBlock, pos, Dimensions.Z, -1);
                        dirCheck(designer, isBlock, pos, Dimensions.Z, 1);
                    }
                }
            }
        }
        

        void dirCheck(AbsVoxelDesigner designer, bool isBlock, IntVector3 pos, Dimensions d, int dir)
        {
            IntVector3 adjPos = pos;
            adjPos.AddDimension(d, dir);

            //bool outsideBound;
            //if (dir < 0)
            //{
            //    outsideBound = adjPos.GetDimension(d) < designer.drawLimits.Min.GetDimension(d);
            //}
            //else
            //{
            //    outsideBound = adjPos.GetDimension(d) > designer.drawLimits.Max.GetDimension(d);
            //}

            if (designer.voxelProject.drawLimits.pointInBounds(adjPos))
            {
                if (designer.Get(adjPos) == LootFest.Map.HDvoxel.BlockHD.EmptyBlock)
                {
                    if (isBlock)
                    {
                        addShadow(designer, pos, d, dir, 0.5f + LayerAboveSurface, false);
                    }
                }
            }
            else
            {
                if (!isBlock)
                {
                    addShadow(designer, pos, d, dir, 0.5f - LayerAboveSurface, true);
                }
            }

            //if (outsideBound)
            //{
            //    if (!isBlock)
            //    {
            //        addShadow(designer, pos, d, dir, 0.5f - LayerAboveSurface, true);
            //    }
            //}
            //else if (designer.GetVoxel(adjPos) == LootFest.Map.HDvoxel.BlockHD.EmptyBlock)
            //{
            //    if (isBlock)
            //    {
            //        addShadow(designer, pos, d, dir, 0.5f + LayerAboveSurface, false);
            //    }
            //}
        }

        void addShadow(AbsVoxelDesigner designer, IntVector3 pos, Dimensions d, int dir, float yAdj, bool boundEdge)
        {
            SpriteName image = SpriteName.EditorPencilShadow;

            if (!boundEdge)
            {
                int diff = designer.designerInterface.drawCoord.GetDimension(d) - pos.GetDimension(d);
                bool sameSide = lib.SameDirection(dir, diff);

                int distance = Math.Abs(diff);
                if (!sameSide)
                {
                    distance += 1;
                }

                switch (distance)
                {
                    case 3: image = SpriteName.EditorPencilShadow2; break;
                    case 4: image = SpriteName.EditorPencilShadow3; break;
                    case 5: image = SpriteName.EditorPencilShadow4; break;
                    case 6: image = SpriteName.EditorPencilShadow5; break;
                    case 7: image = SpriteName.EditorPencilShadow6; break;
                }
            }
            Graphics.Mesh shadow;

            if (currentModel >= models.Count)
            {
                shadow = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, Vector3.One,
                    TextureEffectType.Flat, image, Color.White);
                    //new Graphics.TextureEffect(
                    //TextureEffectType.Flat,
                    //image), 1f);
                shadow.Opacity = 0.25f;
                models.Add(shadow);
            }
            else
            {
                shadow = models[currentModel];
                shadow.SetSpriteName(image);
                shadow.Visible = true;
            }
            currentModel++;

            shadow.Position = pos.Vec + designer.worldPos.PositionV3;
            shadow.Position = VectorExt.AddToDimention(shadow.Position, dir * yAdj, d);

            shadow.Rotation = RotationQuarterion.Identity;
            if (d == Dimensions.X)
            {
                shadow.Rotation.RotateWorldZ(MathHelper.PiOver2);
            }
            else if (d == Dimensions.Z)
            {
                shadow.Rotation.RotateWorldY(MathHelper.PiOver2);
            }
        }

        public void hide()
        {
            currentModel = 0;

            foreach (var m in models)
            { m.Visible = false; }
        }

        public void DeleteMe()
        {
            foreach (var m in models)
            { m.DeleteMe(); }
        }
    }
}
