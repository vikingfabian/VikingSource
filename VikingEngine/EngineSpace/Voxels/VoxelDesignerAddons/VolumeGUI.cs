using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.Voxels
{
    public class VolumeGUI
    {
        List<Graphics.PolygonColor> gridPolys = new List<Graphics.PolygonColor>(64);
        Graphics.GeneratedObjColor model;
        //Graphics.Mesh pencilMultiSelection;
        Graphics.Face[] faces;
        public VolumeGUI()
        {
            //pencilMultiSelection = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, Vector3.One,
            //    TextureEffectType.Flat, SpriteName.EditorMultiSelection, Color.Purple);
            //   //new Graphics.TextureEffect(TextureEffectType.Flat, SpriteName.EditorMultiSelection), 1f);
            //pencilMultiSelection.Color = Color.White;
            //pencilMultiSelection.Visible = false;
           faces  = Graphics.PolygonLib.createFaces(AbsVoxelDesigner.BlockScale * 1.01f);
        }

        public void refresh(IntervalIntV3 volume, Vector3 offset, PaintFillType fill, Color color)
        {

            if (volume.Volume() > 0)
            {
                gridPolys.Clear();
                Sprite gridTexture;
                switch (fill)
                {
                    default:
                    case PaintFillType.Fill:
                        gridTexture = DataLib.SpriteCollection.Get(SpriteName.EditorDottedOutline_Draw);
                        break;
                    case PaintFillType.Select:
                        gridTexture = DataLib.SpriteCollection.Get(SpriteName.EditorDottedOutline_Select);
                        break;
                    case PaintFillType.Delete:
                        gridTexture = DataLib.SpriteCollection.Get(SpriteName.EditorDottedOutline_Delete);
                        break;

                }
                IntVector3 pos = IntVector3.Zero;
                for (pos.Z = volume.Min.Z; pos.Z <= volume.Max.Z; pos.Z++)
                {
                    for (pos.Y = volume.Min.Y; pos.Y <= volume.Max.Y; pos.Y++)
                    {
                        for (pos.X = volume.Min.X; pos.X <= volume.Max.X; pos.X++)
                        {
                            Vector3 center = pos.Vec;
                            if (pos.X == volume.Min.X)
                            {
                                addface(CubeFace.Xnegative);
                            }
                            if (pos.X == volume.Max.X)
                            {
                                addface(CubeFace.Xpositive);
                            }
                            if (pos.Y == volume.Min.Y)
                            {
                                addface(CubeFace.Ynegative);
                            }
                            if (pos.Y == volume.Max.Y)
                            {
                                addface(CubeFace.Ypositive);
                            }
                            if (pos.Z == volume.Min.Z)
                            {
                                addface(CubeFace.Znegative);
                            }
                            if (pos.Z == volume.Max.Z)
                            {
                                addface(CubeFace.Zpositive);
                            }
                            //for (int faceIx = 0; faceIx < faces.Length; faceIx++)
                            //{
                            //    Graphics.Face faceCopy = faces[faceIx];
                            //    faceCopy.Move(center);
                            //    gridPolys.Add(new Graphics.PolygonColor(
                            //        faceCopy, gridTexture, Color.Purple));
                            //}
                            void addface(CubeFace face)
                            {
                                Graphics.Face faceCopy = faces[(int)face];
                                faceCopy.Move(center);
                                gridPolys.Add(new Graphics.PolygonColor(
                                    faceCopy, gridTexture, color));
                            }
                        }
                    }
                }

                Ref.update.AddSyncAction(() =>
                {
                    model?.DeleteMe();
                    model = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                        gridPolys, null), LoadedTexture.SpriteSheet, false);
                    model.AddToRender(1);
                });
            }
            else
            {
                hide();
            }
            //pencilMultiSelection.Visible = volume.Volume() > 0;
            //pencilMultiSelection.Color = Color.Purple;
            //if (pencilMultiSelection.Visible)
            //{
            //    const float MultiSelAddRadius = 0.1f;
            //    pencilMultiSelection.Position = volume.Center + offset;
            //    pencilMultiSelection.Scale = (volume.Add.Vec + Vector3.One) + VectorExt.V3(MultiSelAddRadius * 2);
            //}
        }

        public void hide()
        {
            model?.Visible = false;
            //pencilMultiSelection.Visible = false;
        }
        
    }
}