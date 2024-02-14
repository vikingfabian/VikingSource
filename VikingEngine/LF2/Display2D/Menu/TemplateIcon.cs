//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;
//using Game1.Voxels;
//using Game1.HUD;

//namespace VikingEngine.LF2
//{
//    struct TemplateIconData : HUD.IMemberData
//    {
//        string Path;
//        HUD.Link link;
//        bool storage;

//        public TemplateIconData(string path, HUD.Link link, bool storage)
//        {
//            this.Path = path;
//            this.link = link;
//            this.storage = storage;
//        }
//        public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
//        {
//            return new TemplateIcon(args, link, Path, storage);

//        }
//        public string LinkCaption { get { return null; } }
//    }

//    /// <summary>
//    /// An icon of a saved voxel model, seen from sides
//    /// </summary>
//    class TemplateIcon : HUD.AbsBlockMenuMember, IQuedObject, IUpdateable
//    {
//        protected AbsDraw2D icon;
//        protected ImageAdvanced dataIcon = null;
//        const float IconEdge = 4;
//        const float EdgeTwice = IconEdge * PublicConstants.Twice;
//        static readonly Vector2 PosAdd = new Vector2(IconEdge);
//        string path;
//        bool storage;
//        List<byte> uncompressed = new List<byte>();
//        RenderTargetDrawContainer renderContainer;

//        public TemplateIcon(MemberDataArgs args, HUD.Link link, string path, bool storage)
//            : base(args, link, null)
//        {
//            this.renderContainer = args.renderList;
//            this.storage = storage;
//            this.path = path;
//           // this.description = description;
//            background.Height = args.menu.BlockHeight;
//            this.icon = new Image(SpriteName.NO_IMAGE, background.Position + PosAdd, new Vector2(args.menu.BlockHeight - EdgeTwice), args.layer);

//            Engine.Storage.AddToSaveQue(StartQuedProcess, false);
//        }

//        public override UpdateType UpdateType { get { return UpdateType.OneTimeTrigger; } }
//        public override void Time_Update(float time)
//        { //Just triggerd one time when the model is loaded
//            if (!this.IsDeleted)
//            {
//                Ref.draw.AddToContainer = renderContainer;

//                IntVector3 gridSize = new IntVector3(uncompressed[0], uncompressed[1], uncompressed[2]);
//                uncompressed.RemoveRange(0, 3);
//                VoxelObjGridData grid = new VoxelObjGridData(gridSize);
//                grid.FromCompressedData(uncompressed);

//                gridSize += 1;
//                IntVector3 pos = IntVector3.Zero;
//                List<AbsDraw> images = new List<AbsDraw>();
//                Vector2 imgStartPos = Vector2.Zero;
//                const float IconSize = 60;
//                const float IconColWidth = IconSize + 10;
//                Vector2 tileSize = new Vector2(IconSize / lib.LargestOfTwoValues(gridSize.X, gridSize.Z));

//                //TOP view
//                for (pos.Z = 0; pos.Z < gridSize.Z; pos.Z++)
//                {
//                    for (pos.X = 0; pos.X < gridSize.X; pos.X++)
//                    {
//                        for (pos.Y = gridSize.Y - 1; pos.Y >= 0; pos.Y--)
//                        {
//                            byte material = grid.Get(pos);
//                            if (material != 0)
//                            {
//                                images.Add(makeTile(material, imgStartPos, tileSize,
//                                    pos.X, pos.Z, gridSize.Y - pos.Y, gridSize.Y));
//                                break;
//                            }
//                        }
//                    }
//                }

//                tileSize = new Vector2(IconSize / lib.LargestOfTwoValues(gridSize.X, gridSize.Y));
//                imgStartPos.X += IconColWidth;
//                //SIDE VIEW 1
//                for (pos.Y = 0; pos.Y < gridSize.Y; pos.Y++)
//                {
//                    for (pos.X = 0; pos.X < gridSize.X; pos.X++)
//                    {
//                        for (pos.Z = 0; pos.Z < gridSize.Z; pos.Z++)
//                        {
//                            byte material = grid.Get(pos);
//                            if (material != 0)
//                            {
//                                images.Add(makeTile(material, imgStartPos, tileSize,
//                                    pos.X, gridSize.Y - pos.Y, pos.Z, gridSize.Z));
//                                break;
//                            }
//                        }
//                    }
//                }
//                imgStartPos.X += IconColWidth;
//                //SIDE VIEW 2
//                tileSize = new Vector2(IconSize / lib.LargestOfTwoValues(gridSize.Z, gridSize.Y));
//                for (pos.Y = gridSize.Y - 1; pos.Y >= 0; pos.Y--)
//                {
//                    for (pos.Z = 0; pos.Z < gridSize.Z; pos.Z++)
//                    {
//                        for (pos.X = 0; pos.X < gridSize.X; pos.X++)
//                        {
//                            byte material = grid.Get(pos);
//                            if (material != 0)
//                            {
//                                images.Add(makeTile(material, imgStartPos, tileSize,
//                                    pos.Z, gridSize.Y - pos.Y, pos.X, gridSize.X));
//                                break;
//                            }
//                        }
//                    }
//                }

//                icon.DeleteMe();
//                //icon = new 
//                RenderTargetImage target = new RenderTargetImage(Vector2.Zero, new Vector2(IconColWidth * 3, IconSize), ImageLayers.Foreground4);                
                
//                target.DrawImagesToTarget(images, true);
//                //target.Xpos = icon.Xpos;
//                target.Position = icon.Position;
//                target.Transparentsy = icon.Transparentsy;
//                target.Visible = icon.Visible;

//                icon = target;

//                //foreach (AbsDraw img in images)
//                //{ img.DeleteMe(); }

//                uncompressed = null;
//                if (this.IsDeleted)
//                {
//                    target.DeleteMe();
//                }

//                Ref.draw.AddToContainer = null;
//            }
//        }
//        public void Time_LasyUpdate(float time) {  }
//        public override bool SavingThread { get { return false; } }

//        public void StartQuedProcess(bool saveThread)
//        {
//            uncompressed.AddRange(DataLib.SaveLoad.LoadByteArray(path, storage));

//            Ref.update.AddToUpdate(this, true);
//        }
//        Image makeTile(byte material, Vector2 imgStartPos, Vector2 tileSize, int Xpos, int Ypos, int depthPos, int maxDepth)
//        {
//            Ref.draw.DontAddNextRenderObject();
//            Image tile = new Image(Data.MaterialBuilder.MaterialTile(material),
//                                new Vector2(imgStartPos.X + tileSize.X * Xpos, imgStartPos.Y + tileSize.Y * Ypos), tileSize + Vector2.One, ImageLayers.Foreground1);
//            const float MaxDarkess = 0.8f;
//            float col = 1 - ((float)depthPos / maxDepth * MaxDarkess);
//            tile.Color = new Color(col, col, col);
//            return tile;
//        }

//        public override void GoalY(float y, bool set)
//        {
//            base.GoalY(y, set);
//            icon.Ypos = y;
//            if (dataIcon != null)
//                dataIcon.Ypos = y;
//        }
        
//        public override bool Visible
//        {
//            set
//            {
//                base.Visible = value;
//                icon.Visible = value;
//                if (dataIcon != null)
//                    dataIcon.Visible = value;
//            }
//        }
//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            icon.DeleteMe();
//            if (dataIcon != null)
//                dataIcon.DeleteMe();
//        }
//        public override HUD.ClickFunction Function
//        {
//            get { return HUD.ClickFunction.Link; }
//        }
//    }
    
//}
