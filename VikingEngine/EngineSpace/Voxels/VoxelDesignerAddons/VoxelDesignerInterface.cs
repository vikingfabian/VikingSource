using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Voxels
{
    //public interface IVoxelDesignerInterfaceParent
    //{
    //    void SetPencilBounds(ref Vector3 freePencilGridPos);
    //    void NewBlockPosEvent(IntVector3 newCoord, IntVector3 posDiff);
    //}

    public class VoxelDesignerInterface
    {
        static readonly Color EmptySelection = Color.White;
        static readonly Color ContactSelection = new Color(255, 200, 255);
        static readonly Color InsideSelection = new Color(255, 255, 120);

        AbsVoxelDesigner parent;
        public VikingEngine.Voxels.VoxelDesignerSettings settings = new Voxels.VoxelDesignerSettings();
        
        List<IDeleteable> createdObjects;

        public Graphics.Mesh freePencil;
        public Graphics.Mesh pencilCube;
        public VolumeGUI volumeGUI;
        
        public PencilShadow pencilShadow;
        

        protected Graphics.Mesh xline;
        protected Graphics.Mesh zline;

        //Mesh forwardArrow;
        Graphics.GeneratedObjColor gridOutlineModel;

        public IntervalIntV3 selectionArea;
        public IntVector3 drawCoord = IntVector3.Zero;
        public Vector3 freePencilGridPos;
        public IntVector3 keyDownDrawCoord = IntVector3.Zero;
        public IntVector3 drawSize = IntVector3.One;

        public InputDisplay inputDisplay = null;
        Image crossHair, crossHairColor;
        /// <summary>
        /// Where the grid origo starts
        /// </summary>
        public Vector3 offSet = Vector3.Zero;

        public Dimensions toolDir = Dimensions.NON;
        public IntVector3 mostRecentMoveXZ = IntVector3.Zero;

        Engine.PlayerData pData;
        public Graphics.ImageGroup hudElements;

        bool useDrawLimits;

        public void setPencilColor(int state_empty0_contact1_inside2)
        {
            Color col = Color.Red;
            switch (state_empty0_contact1_inside2)
            {
                case 0: col = EmptySelection; break;
                case 1: col = ContactSelection; break;
                case 2: col = InsideSelection; break;
            }

            pencilCube.Color = col;
            freePencil.Color = col;
            xline.Color = ColorExt.MultiplyRGBA( col, 0.3f);
            zline.Color = xline.Color;
        }

        public VoxelDesignerInterface(AbsVoxelDesigner parent, EditorInputMap input, int playerIx, Vector3 startPos, Vector3 offset, 
            bool transparentHelpLines, IntervalIntV3? drawLimits,
            Graphics.AbsCamera cam)
        {
            float FreePencilWidth = 0.24f;
            if (transparentHelpLines)
            {
                FreePencilWidth = 0.2f;
            }
            float HelpLineWidth = FreePencilWidth * 0.4f;

            
            this.offSet = offset;
            useDrawLimits = drawLimits != null;
            this.parent = parent;
            this.pData = Engine.XGuide.GetPlayer(playerIx);
            pData.view.Camera = cam;
            float helpLinesTransparentsy = transparentHelpLines ? 0.4f : 1;

            freePencilGridPos = startPos;
            freePencil = new Graphics.Mesh(LoadedMesh.sphere,
                startPos, new Vector3(FreePencilWidth),
                TextureEffectType.Flat, SpriteName.WhiteArea, Color.White);
            
            freePencil.Opacity = helpLinesTransparentsy;

            pencilCube = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, new Vector3(1.16f),
                TextureEffectType.Flat, SpriteName.EditorPencilCube, Color.White);
            pencilCube.Opacity = 0.8f;


            xline = new Graphics.Mesh(LoadedMesh.cube_repeating, startPos + offSet, new Vector3(HelpLineWidth),
                TextureEffectType.Flat, SpriteName.WhiteArea, Color.LightGray);
            xline.Opacity = helpLinesTransparentsy;
            zline = (Graphics.Mesh)xline.CloneMe();

            zline.RotateWorld(new Vector3(MathHelper.PiOver2, 0, 0));

            const float BasicLineLength = 32;
            xline.ScaleX = BasicLineLength;
            zline.ScaleX = BasicLineLength;
            
            pencilShadow = new PencilShadow();
            volumeGUI = new VolumeGUI();

            crossHair = new Image(SpriteName.IconBuildArrow,
                Engine.Screen.CenterScreen + new Vector2(Engine.Screen.BorderWidth),
                Engine.Screen.IconSizeV2, ImageLayers.AbsoluteTopLayer);

            crossHairColor = new Image(SpriteName.VoxelEditorColorCube,
                crossHair.position + Engine.Screen.IconSizeV2 * 0.2f,
                Engine.Screen.SmallIconSizeV2, ImageLayers.AbsoluteTopLayer);

            createdObjects = new List<IDeleteable>{
                freePencil, pencilCube,
                xline, zline, crossHair, crossHairColor };

            hudElements = new ImageGroup(8);

            hudElements.Add(crossHair);
            hudElements.Add(crossHairColor);
            hudElements.Add(freePencil);
            hudElements.Add(pencilCube);
            hudElements.Add(xline);
            hudElements.Add(zline);
        }
        
        public void Update(bool hasSelection, EditorDrawTools drawTools, bool cameraMode, Color materialColor)
        {
            pData.view.Camera.LookTarget = freePencil.Position;

            SpriteName icon = SpriteName.IconBuildArrow;

            crossHairColor.Visible = false;

            if (cameraMode)
            {
                icon = SpriteName.InterfaceIconCamera;
            }
            else if (hasSelection)
            {
                icon = SpriteName.IconBuildMoveSelection;
            }
            else if (drawTools.currentDrawAction != null)
            {
                switch (drawTools.currentDrawAction.fill)
                {
                    case PaintFillType.Delete:
                        icon = SpriteName.IconBuildRemove;
                        break;
                    case PaintFillType.Fill:
                        icon = SpriteName.IconBuildAdd;
                        break;
                    case PaintFillType.Select:
                        icon = SpriteName.IconBuildSelection;
                        break;
                }
            }
            else if (pencilShadow.visible)
            {
                crossHairColor.Visible = true;
                crossHairColor.Color = materialColor;
            }

            crossHair.SetSpriteName(icon);
        }
        
        public void moveFreePencil(Vector3 dirTime, bool yMoveToggle)
        {
            Vector3 startVal = dirTime;

            Vector2 xz = new Vector2(dirTime.X, dirTime.Z);
            if (VectorExt.HasValue(xz))
            {
                Rotation1D camrot = Rotation1D.FromDirection(xz);
                camrot.Radians += pData.view.Camera.TiltX - MathHelper.PiOver2;

                if (yMoveToggle)
                {
                    camrot.snapToNearest90Degrees();
                }

                Vector2 rotatedXZmove = camrot.Direction(xz.Length());
                dirTime.X = rotatedXZmove.X;
                dirTime.Z = rotatedXZmove.Y;
            }

            if (dirTime != Vector3.Zero)
            {
                freePencilGridPos += dirTime;

                parent.SetPencilBounds(ref freePencilGridPos);

                freePencil.Position = freePencilGridPos + offSet;

                xline.Z = freePencil.Z;
                xline.Y = freePencil.Y;
                zline.X = freePencil.X;
                zline.Y = freePencil.Y;
                if (!useDrawLimits)
                {
                    xline.X = freePencil.X;
                    zline.Z = freePencil.Z;
                }

                IntVector3 newCoord = IntVector3.FromV3(freePencilGridPos);
                
                IntVector3 posDiff = newCoord - drawCoord;
                

                if (posDiff.HasValue())
                {
                    drawCoord = newCoord;

                    pencilCube.Position = drawCoord.Vec + offSet;
                    parent.NewBlockPosEvent(newCoord, posDiff);
                }
            }
        }

        public void setPencilPosition(Vector3 pos)
        {
            freePencilGridPos = pos;
            moveFreePencil(Vector3.Zero, false);
        }

        public void UpdateMultiSelectionPencil(PaintFillType fill, IntVector3 drawCoordPosDiff)
        {
            selectionArea = IntervalIntV3.FromTwoPoints(drawCoord, keyDownDrawCoord);
            drawSize = selectionArea.Add;

            refreshVolumeGui(fill);
            //calc draw tool dir
            int numExpandedDirs = 0;
            if (drawSize.X > 0)
                numExpandedDirs++;
            if (drawSize.Y > 0)
                numExpandedDirs++;
            if (drawSize.Z > 0)
                numExpandedDirs++;

            if (numExpandedDirs == 2)
            {
                if (drawSize.X == 0)
                    toolDir = Dimensions.X;
                else if (drawSize.Y == 0)
                    toolDir = Dimensions.Y;
                else
                    toolDir = Dimensions.Z;
            }

            if (drawCoordPosDiff.X != 0)
            {
                mostRecentMoveXZ.Z = 0;
                mostRecentMoveXZ.X = lib.ToLeftRight(drawCoordPosDiff.X);
            }
            if (drawCoordPosDiff.Z != 0)
            {
                mostRecentMoveXZ.X = 0;
                mostRecentMoveXZ.Z = lib.ToLeftRight(drawCoordPosDiff.Z);
            }

        }

        public void refreshVolumeGui(PaintFillType fill)
        {
            volumeGUI.refresh(selectionArea, this.offSet, fill, parent.SelectedMaterial.flatcolor);
        }

        public void SetDrawLimits(IntervalIntV3 drawLimits, bool resetWhiteLines)
        {
            IntVector3 gidSize = drawLimits.Add + 1;

            if (resetWhiteLines)
            {
                xline.Position = drawLimits.Center + offSet;
                zline.Position = xline.Position;
            }
            xline.ScaleX = gidSize.X;
            zline.ScaleX = gidSize.Z;
        }

        public void createGrid(IntervalIntV3 volume)
        {
            //if (forwardArrow == null)
            //{
            //    forwardArrow = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(8f),
            //        TextureEffectType.Flat, SpriteName.EditorForwardArrow, Color.White);
            //    forwardArrow.Opacity = 0.3f;
            //}

            //forwardArrow.Position = new Vector3(volume.Max.X * PublicConstants.Half, -0.5f, (volume.Max.Z + 2) * 1f);
            //forwardArrow.Scale = VectorExt.V3(volume.Max.X + 1) * 0.25f;
            var forwardArrow = Graphics.PolygonColor.QuadXZ(new Vector3(volume.Max.X * PublicConstants.Half, -0.55f, (volume.Max.Z + 2) * 1f),
                /*VectorExt.V2(volume.Max.X + 1) **/ new Vector2( 1.5f), 0, SpriteName.EditorForwardArrow, Dir4.E, ColorExt.MultiplyRGBA(Color.White, 0.3f));


            IntVector3 gidSize = volume.Size;

            //backgrund grid
            const float Alpha = 0.4f;
            Sprite gridTexture = DataLib.SpriteCollection.Get(SpriteName.RectangleOutline_2pix_64w);
            Color gridColorX = ColorExt.MultiplyRGBA(Color.Blue, Alpha);

            Color gridColorZ = ColorExt.MultiplyRGBA(Color.Green, Alpha);         

            Color gridColorBottom = ColorExt.MultiplyRGBA(Color.Brown, Alpha);
            //gridColorBottom.A = 50;

            Graphics.Face[] faces = Graphics.PolygonLib.createFaces(AbsVoxelDesigner.BlockScale);
            List<Graphics.PolygonColor> gridPolys = new List<Graphics.PolygonColor>(128);
            gridPolys.Add(forwardArrow);

            Vector3 frontStartPos = volume.Min.Vec * AbsVoxelDesigner.BlockScale + offSet;
            Vector3 backStartPos = frontStartPos;
            Vector3 rightStartPos = frontStartPos;
            Vector3 bottomStartPos = frontStartPos;


            frontStartPos.Z -= AbsVoxelDesigner.BlockScale;
            backStartPos.Z += AbsVoxelDesigner.BlockScale * gidSize.Z;

            Vector3 leftStartPos = rightStartPos;
            rightStartPos.X -= AbsVoxelDesigner.BlockScale;
            leftStartPos.X += AbsVoxelDesigner.BlockScale * gidSize.X;

            bottomStartPos.Y -= AbsVoxelDesigner.BlockScale;
            for (int y = 0; y < gidSize.Y; y++)
            {
                for (int x = 0; x < gidSize.X; x++)
                {
                    //front
                    Graphics.Face frontCopy = faces[(int)CubeFace.Zpositive];
                    frontCopy.Move(frontStartPos + new Vector3(x * AbsVoxelDesigner.BlockScale, y * AbsVoxelDesigner.BlockScale, 0));
                    gridPolys.Add(new Graphics.PolygonColor(
                        frontCopy, gridTexture, gridColorX));

                    //back
                    Graphics.Face backCopy = faces[(int)CubeFace.Znegative];
                    backCopy.Move(backStartPos + new Vector3(x * AbsVoxelDesigner.BlockScale, y * AbsVoxelDesigner.BlockScale, 0));
                    gridPolys.Add(new Graphics.PolygonColor(
                        backCopy, gridTexture, gridColorX));
                }
                for (int z = 0; z < gidSize.Z; z++)
                {
                    //right
                    Graphics.Face right = faces[(int)CubeFace.Xpositive];
                    right.Move(rightStartPos + new Vector3(0, y * AbsVoxelDesigner.BlockScale, z * AbsVoxelDesigner.BlockScale));
                    gridPolys.Add(new Graphics.PolygonColor(
                        right, gridTexture, gridColorZ));

                    //left
                    Graphics.Face left = faces[(int)CubeFace.Xnegative];
                    left.Move(leftStartPos + new Vector3(0, y * AbsVoxelDesigner.BlockScale, z * AbsVoxelDesigner.BlockScale));
                    gridPolys.Add(new Graphics.PolygonColor(
                        left, gridTexture, gridColorZ));
                }
            }
            //bottom
            for (int z = 0; z < gidSize.Z; z++)
            {
                for (int x = 0; x < gidSize.X; x++)
                {
                    Graphics.Face bottom = faces[(int)CubeFace.Ypositive];
                    bottom.Move(bottomStartPos + new Vector3(x * AbsVoxelDesigner.BlockScale, 0, z * AbsVoxelDesigner.BlockScale));
                    gridPolys.Add(new Graphics.PolygonColor(
                        bottom, gridTexture, gridColorBottom));
                }
            }
            if (gridOutlineModel != null)
            {
                gridOutlineModel.DeleteMe();
            }
            gridOutlineModel = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                gridPolys, null), LoadedTexture.SpriteSheet, true);

            moveFreePencil(Vector3.Zero, false);

           
        }

        public static SpriteName ToolIcon(PaintToolType tool)
        {
            switch (tool)
            {
                case PaintToolType.Cone: return SpriteName.EditorToolCone;
                case PaintToolType.Cylinder: return SpriteName.EditorToolCylinder;
                case PaintToolType.Pencil: return SpriteName.EditorToolPencil;
                case PaintToolType.Pyramid: return SpriteName.EditorToolPyramid;
                case PaintToolType.ReColor: return SpriteName.EditorToolReColorPencil;
                case PaintToolType.Rectangle: return SpriteName.EditorToolCube;
                case PaintToolType.Road: return SpriteName.EditorToolRoad;
                case PaintToolType.Sphere: return SpriteName.EditorToolSphere;
                case PaintToolType.Triangle: return SpriteName.EditorToolWedge;
                case PaintToolType.Bucket: return SpriteName.VoxelEditorBucket;
            }

            return SpriteName.MissingImage;
        }
        

        public void ShowHUD(bool show)
        {
            hudElements.SetVisible(show);
            
            pencilShadow.visible = show;
            pencilShadow.hide();
            
            
            if (gridOutlineModel != null)
            {
                gridOutlineModel.Visible = show;
                //forwardArrow.Visible = show;
            }
        }

        public void DeleteMe()
        {
            if (createdObjects != null)
            {
                foreach (IDeleteable del in createdObjects)
                    del.DeleteMe();
            }
            hudElements.DeleteAll();
            pencilShadow.DeleteMe();

            if (gridOutlineModel != null)
            {
                gridOutlineModel.DeleteMe();
                //forwardArrow.DeleteMe();
            }
        }
    }

    
}
