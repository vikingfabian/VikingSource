using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Voxels;
//
using VikingEngine.DataStream;

namespace VikingEngine.LF2.Editor
{

    class VoxelDesigner : Voxels.AbsVoxelDesigner, Timer.IEventTriggerCallBack, IStreamIOCallback
    {
        static readonly bool SaveByteArray
#if WINDOWS
 = true;
#else
            = true;
#endif
        static readonly bool LoadByteArray
#if WINDOWS
 = true;
#else
            = true;
#endif

        Players.PlayerMode mode = Players.PlayerMode.Creation;
        public Players.PlayerMode Mode
        {
            get
            {
                if (HaveSelection) return Players.PlayerMode.CreationSelection;
                if (InMenu) return Players.PlayerMode.InMenu;
                return mode;
            }
        }

        int previousMenu = -1;
        Players.PlayerSettings settings;
        MergeModelsOption mergeModelsOption = new MergeModelsOption().StandardInit();
        Players.Player parent = null;

        bool inGame { get { return parent != null; } }

        public static readonly IntVector3 StandardDrawlimit = new IntVector3(
           Data.Block.NumObjBlocksPerTerrainBlock * 2,
           Data.Block.NumObjBlocksPerTerrainBlock * 3,
           Data.Block.NumObjBlocksPerTerrainBlock * 2) - 1;
        public static readonly RangeIntV3 StandardDrawLimitRange =
            new RangeIntV3(IntVector3.Zero, StandardDrawlimit);
        public const string UserVoxelObjFolder = "VoxelObjSave";
        string currentObjName = randomName();

        //byte material = 1;
        byte swapMaterialFrom;
        TextG infoText;
        Image crossHair;
        float sphereRadius = 2.5f;

        GameObjects.EnvironmentObj.Door door;
        Graphics.Mesh doorOutline;

        const int MessageMaxLength = 250;

        const string LinkCopyAll = "Select And Copy All";
        const string LinkPaste = "Paste";
        const string SelectMaterial = "Select Material";
        const string PickMaterial = "Pick Material(";
        const string X = "X";
        const string Y = "Y";
        const string Z = "Z";

        const string LinkRotateFlip = "Rotate/Flip";


        const string LinkMoveEverything = "Move everything";
        const string MoveX = "Move +X";
        const string MoveY = "Move Up";
        const string MoveZ = "Move +Z";
        const string MoveNX = "Move -X";
        const string MoveNY = "Move Down";
        const string MoveNZ = "Move -Z";


        override protected byte selectedMaterial
        {
            get { return settings.Material; }
            set
            {
                settings.Material = value;
                if (parent != null)
                    parent.SettingsChanged();
            }
        }
        override protected byte secondaryMaterial
        {
            get { return settings.SecondaryMaterial; }
            set
            {
                settings.SecondaryMaterial = value;
                if (parent != null)
                    parent.SettingsChanged();
            }
        }
        static string randomName()
        {
            return "VX" + Ref.rnd.Int(9999).ToString();
        }


        public static Map.WorldPosition HeroPosToCreationStartPos(IntVector2 heroScreen)
        {
            Map.WorldPosition result = Map.WorldPosition.EmptyPos;
            result.ChunkGrindex = heroScreen;
            result.ChunkGrindex -= Editor.VoxelDesigner.CreationChunkWidth / PublicConstants.Twice;

            result.LocalBlockGrindex = new IntVector3(1, 0, 1);
            return result;
        }

        public const int CreationChunkWidth = 3;
        public const int CreationXZSize = Map.WorldPosition.ChunkWidth * CreationChunkWidth - 2;

        public static readonly RangeIntV3 CreationSizeLimit = new RangeIntV3(IntVector3.Zero,
            new IntVector3(CreationXZSize, Map.WorldPosition.ChunkHeight, CreationXZSize) - 1);

        bool[] hasChatergory;

        public VoxelDesigner(Map.WorldPosition voxelDesignerStartPos,
            Graphics.AbsCamera camera, VectorRect menuArea, Players.Player parent)
            : base(CreationSizeLimit,
            new Vector3(voxelDesignerStartPos.ToV3().X, 0, voxelDesignerStartPos.ToV3().Z), parent.inputMap, true)
        { //IN GAME 
            bUpdateDrawLimits = false;
            ZoomBounds.Max = 75;
            this.parent = parent;
            //this.player = player;
            settings = parent.Settings;
            worldPos = voxelDesignerStartPos;
            basicInit(menuArea);

            //place the selection in center

            designerInterface.freePencilGridPos = new Vector3(drawLimits.Max.X * PublicConstants.Half, voxelDesignerStartPos.WorldGrindex.Y + 16,
                drawLimits.Max.Z * PublicConstants.Half);
            designerInterface.moveFreePencil(Vector3.Up * 0.1f);

            playCam.Zoom = camera.Zoom;
            playCam.Tilt = camera.Tilt;
            playCam.ViewAngle = camera.ViewAngle;
            playCam.aspectRatio = camera.aspectRatio;
        }

        public VoxelDesigner(int pIx)
            : base(StandardDrawLimitRange, Vector3.Zero, Input.Controller.Instance(pIx), false)
        { //IN EDITOR
            if (LF2.Data.RandomSeed.Empty)
                LF2.Data.RandomSeed.NewWorld();

            settings = new Players.PlayerSettings(null, false, TextLib.EmptyString);
            //settings.ViewMaterialName = false;
            basicInit(new VectorRect(
                Engine.Screen.SafeArea.Position, new Vector2(300, Engine.Screen.SafeArea.Height)));
            Ref.draw.Camera.Zoom = 150;
        }
        override protected bool viewDrawLimitGrid { get { return !inGame; } }

        public void StartQuedProcess(ThreadedLoad type, int part, int menuId)
        {
            switch (type)
            {
                case ThreadedLoad.StartUp:
                    for (int i = 0; i < (int)SaveCategory.NUM; i++)
                    {
                        hasChatergory[i] = DataLib.SaveLoad.FolderExistAndHaveFilesInit(categoryDir(i));
                    }
                    break;
                case ThreadedLoad.ListTemplates:
                    if (menu != null && menu.Visible && menuId == menu.MenuId)
                    {
                        listTemplates();
                    }
                    break;
                case ThreadedLoad.ListTemplatesCategory:
                    if (menu != null && menu.Visible && menuId == menu.MenuId)
                    {
                        previousMenu = (int)Link.LoadTemplate;
                        currentCategory = (SaveCategory)part;
                        HUD.File file = new HUD.File();
                        listFilesInCategory(file, currentCategory);
                        //menu.File = file;
                        new Process.SynchOpenMenuFile(file, menu);
                    }
                    break;
            }

        }

        override protected bool allowSelectAll { get { return !inGame; } }

        public override void UpdateInput(Input.AbsControllerInstance controller)
        {
            if (inputDialogue == null)
            {
                base.UpdateInput(controller);
                if (InMenu && controller.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.B))
                {
                    if (previousMenu >= 0)
                    {
                        BlockMenuLinkEvent(new HUD.Link(previousMenu), Engine.XGuide.LocalHostIndex, numBUTTON.A);
                    }
                    else
                    {
                        openHeadMenu(false);
                    }
                }
            }
            else
            {
                inputDialogue.UpdateInput(controller);
            }
        }

        void basicInit(VectorRect menuArea)
        {

            menu = new LF2.Menu(menuArea, Engine.Screen.SafeArea, ImageLayers.Foreground5, parent == null ? -1 : parent.Index);
            menu.Visible = false;

            infoText = new TextG(LoadedFont.Lootfest, new Vector2(menuArea.X, menuArea.Bottom - 30),
                new Vector2(0.8f), Align.Zero, "info", Color.White, ImageLayers.Background6);
            HUDelements.Add(infoText);

            crossHair = new Image(SpriteName.LFHealIcon, Vector2.Zero, new Vector2(32), ImageLayers.AbsoluteTopLayer);

            hasChatergory = new bool[(int)SaveCategory.NUM];
            new VoxelDesignerQueLoader(ThreadedLoad.StartUp, this);
            bUpdateDrawLimits = true;
            UpdateDrawLimits();
        }
        protected override void colorMenu(int materialIndex)
        {
            HUD.File selMenu = new HUD.File();
            selMenu.AddTitle(SelectMaterial);
            listMaterials(selMenu, (int)Link.SelectMaterial_dialogue, false, settings, (int)Link.ShowHideMaterialNames, materialIndex);
            menu.File = selMenu;
        }
        void beginDrawGeometry(GeomitricType type)
        {
            designerInterface.selectionArea = drawSphere(type, sphereRadius, designerInterface.drawCoord, worldPos, selectedMaterial);

            startUpdateVoxelObj(false);

            designerInterface.selectionArea.Min = designerInterface.drawCoord;
            designerInterface.selectionArea.Max = designerInterface.drawCoord;

            //share
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_VoxelEditorDrawSphere,
                Network.PacketReliability.ReliableLasy, parent.Index);
            w.Write((byte)type);
            w.Write((byte)(sphereRadius * 2));
            designerInterface.drawCoord.NetworkWriteByte(w);
            worldPos.Write(w);//WorldGrindex.WriteStream(w);
            w.Write(selectedMaterial);
        }
        protected override void chagedTool(FillType tool)
        {
            infoText.TextString = "Tool: " + tool.ToString();
        }
        public static void NetworkReadSphere(System.IO.BinaryReader r)
        {
            GeomitricType type = (GeomitricType)r.ReadByte();
            float sphereRadius = r.ReadByte() * PublicConstants.Half;
            IntVector3 drawCoord = IntVector3.FromByteSzStream(r);
            Map.WorldPosition worldPos = Map.WorldPosition.NetworkRead(r);
            byte selectedMaterial = r.ReadByte();
            RangeIntV3 selectionArea = drawSphere(type, sphereRadius, drawCoord, worldPos, selectedMaterial);
            UpdateMapArea(worldPos, selectionArea);
        }

        static RangeIntV3 drawSphere(GeomitricType type, float radius, IntVector3 drawCoord,
            Map.WorldPosition worldPos, byte selectedMaterial)
        {
            int intRadius = (int)radius;
            IntVector3 pos = IntVector3.Zero;
            switch (type)
            {
                default: //sphere

                    for (pos.Z = drawCoord.Z - intRadius; pos.Z <= drawCoord.Z + intRadius; pos.Z++)
                    {
                        for (pos.Y = drawCoord.Y - intRadius; pos.Y <= drawCoord.Y + intRadius; pos.Y++)
                        {
                            for (pos.X = drawCoord.X - intRadius; pos.X <= drawCoord.X + intRadius; pos.X++)
                            {
                                if (CreationSizeLimit.WithinRange(pos) && (pos.Vec - drawCoord.Vec).Length() < radius)
                                {
                                    Map.WorldPosition wp = worldPos;
                                    wp.WorldGrindex.Add(pos);
                                    //wp.UpdateChunkPos();
                                    if (wp.WorldGrindex.Y >= 0 && wp.WorldGrindex.Y < Map.WorldPosition.ChunkHeight)
                                    { LfRef.chunks.SetIfOpen(wp, selectedMaterial); }
                                }
                            }
                        }
                    }
                    break;
                case GeomitricType.Cylinder:
                    const int Lenght = 3;
                    Vector2 planePos = Vector2.Zero;

                    for (pos.Z = drawCoord.Z - intRadius; pos.Z <= drawCoord.Z + intRadius; pos.Z++)
                    {
                        planePos.Y = pos.Z - drawCoord.Z;
                        for (pos.Y = drawCoord.Y; pos.Y <= drawCoord.Y + Lenght; pos.Y++)
                        {
                            for (pos.X = drawCoord.X - intRadius; pos.X <= drawCoord.X + intRadius; pos.X++)
                            {
                                planePos.X = pos.X - drawCoord.X;
                                if (CreationSizeLimit.WithinRange(pos) && planePos.Length() < radius)
                                {
                                    Map.WorldPosition wp = worldPos;
                                    wp.WorldGrindex.Add(pos);
                                    //wp.UpdateChunkPos();
                                    if (wp.WorldGrindex.Y >= 0 && wp.WorldGrindex.Y < Map.WorldPosition.ChunkHeight)
                                    { LfRef.chunks.SetIfOpen(wp, selectedMaterial); }
                                }
                            }
                        }
                    }
                    break;
                case GeomitricType.Pyramid:
                    pos.Y = drawCoord.Y;
                    while (pos.Y < Map.WorldPosition.ChunkHeight && radius > 0)
                    {
                        for (pos.Z = drawCoord.Z - intRadius; pos.Z <= drawCoord.Z + intRadius; pos.Z++)
                        {
                            for (pos.X = drawCoord.X - intRadius; pos.X <= drawCoord.X + intRadius; pos.X++)
                            {
                                if (CreationSizeLimit.WithinRange(pos))
                                {
                                    Map.WorldPosition wp = worldPos;
                                    wp.WorldGrindex.Add(pos);
                                    //wp.UpdateChunkPos();
                                    if (wp.WorldGrindex.Y >= 0 && wp.WorldGrindex.Y < Map.WorldPosition.ChunkHeight)
                                    { LfRef.chunks.SetIfOpen(wp, selectedMaterial); }
                                }
                            }
                        }
                        pos.Y++;
                        intRadius--;
                    }
                    intRadius = (int)radius;
                    break;
            }


            RangeIntV3 selectionArea = RangeIntV3.Zero;
            selectionArea.Min.X = Bound.Min(drawCoord.X - intRadius, CreationSizeLimit.Min.X);
            selectionArea.Min.Z = Bound.Min(drawCoord.Z - intRadius, CreationSizeLimit.Min.Z);

            selectionArea.Max.X = lib.SetMaxVal(drawCoord.X + intRadius, CreationSizeLimit.Max.X);
            selectionArea.Max.Z = lib.SetMaxVal(drawCoord.Z + intRadius, CreationSizeLimit.Max.Z);

            return selectionArea;
        }

        protected override void drawInArea(FillType fill, DrawTool tool, RangeIntV3 drawArea)
        {
            switch (fill)
            {
                default:
                    Music.SoundManager.PlayFlatSound(LoadedSound.block_place_1);
                    break;
                case FillType.Delete:
                    Music.SoundManager.PlayFlatSound(LoadedSound.tool_dig);
                    break;
                case FillType.Select:
                    Music.SoundManager.PlayFlatSound(LoadedSound.tool_select);
                    selectedVoxels.Voxels.Clear();
                    templateSent = false;
                    break;

            }
            base.drawInArea(fill, tool, drawArea);

            if (inGame && (fill == FillType.Delete || selectedMaterial != 0 || fill == FillType.Select))
            {
                startUpdateVoxelObj(false);
                byte m = selectedMaterial;
                if (fill == FillType.Delete || fill == FillType.Select)
                    m = 0;
                if (fill != FillType.Select || settings.SelectionCut)
                    NetworkWriteDrawRect(m, parent, drawArea);
            }
            if (fill == FillType.Select)
            {
                startUpdateVoxelObj(true);
            }

            designerInterface.drawSize = IntVector3.One;
            UpdatePencilInfo();
        }

        public override void SetVoxel(IntVector3 drawPoint, byte material)
        {
            if (inGame)
            {
                Map.WorldPosition pos = worldPos;
                pos.WorldGrindex.Add(drawPoint);
                // pos.UpdateChunkPos();
                LfRef.chunks.SetIfOpen(pos, material);
            }
            else
                base.SetVoxel(drawPoint, material);
        }
        public override byte GetVoxel(IntVector3 drawPoint)
        {
            if (inGame)
            {
                Map.WorldPosition pos = worldPos;
                pos.WorldGrindex.Add(drawPoint);
                // pos.UpdateChunkPos();
                return LfRef.chunks.Get(pos);

            }
            return base.GetVoxel(drawPoint);
        }

        public override void SetVoxelToPortal(LF2.Map.WorldPosition wp, byte material)
        {
            if (inGame)
            {
                LfRef.chunks.SetVoxelToPortal(wp, material);
            }
            else
                base.SetVoxelToPortal(wp, material);
        }
        public override byte GetVoxelFromPortal(LF2.Map.WorldPosition wp)
        {
            if (inGame)
            {
                return LfRef.chunks.GetVoxelFromPortal(wp);
            }
            return base.GetVoxelFromPortal(wp);
        }



        override protected DrawTool drawTool
        {
            get { return settings.DrawTool; }
            set { settings.DrawTool = value; }
        }
        protected override bool selectionCut
        {
            get
            {
                return settings.SelectionCut;
            }
        }




        public void SetRandomMaterial()
        {
            selectedMaterial = (byte)(Ref.rnd.Int((int)Data.MaterialType.pirate) + 1);
        }
        override protected void openHeadMenu(bool open)
        {
#if PCGAME
            Engine.Input.CenterMouse = !open;
#endif
            menu.Visible = open;
            if (open)
            {
                mainMenu();
                ShowHUD(true);

                if (inGame)
                    mode = Players.PlayerMode.InMenu;
            }
            else if (inGame)
            {
                mode = HaveSelection ? Players.PlayerMode.CreationSelection : Players.PlayerMode.Creation;
            }
        }

        override protected bool resetWhiteLines
        {
            get
            {
                return !inGame;
            }
        }


        public override void KeyPressEvent(Microsoft.Xna.Framework.Input.Keys key, bool keydown)
        {
            if (inputDialogue != null)
            {
                inputDialogue.KeyPressEvent(key, keydown);
            }
            else
            {
                if (key == Microsoft.Xna.Framework.Input.Keys.Tab && keydown)
                    parent.EndCreationMode();
                else
                    base.KeyPressEvent(key, keydown);
            }
        }

        //public override void Button_Event(ButtonValue e)
        //{
        //    if (inputDialogue != null)
        //    {
        //        inputDialogue.Button_Event(e);
        //    }
        //    else
        //    {
        //        //if (InMenu)
        //        //{
        //        //    if (e.KeyDown && mFile != null && mFile.IsInAlphabeticOrder)
        //        //    {
        //        //        if (e.Button == numBUTTON.LB)
        //        //        {
        //        //            menu.NextLetter(false);
        //        //        }
        //        //        else if (e.Button == numBUTTON.RB)
        //        //        {
        //        //            menu.NextLetter(true);
        //        //        }
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    if (e.Button == numBUTTON.A && (e.KeyDown || pencilKeyDown))
        //        //        e.Button = drawButton;
        //        //    else if (e.Button == numBUTTON.B && !HaveSelection && (e.KeyDown || pencilKeyDown))
        //        //        e.Button = deleteButton;
        //        //}
        //        //base.Button_Event(e);
        //        if (cameraCtlrKeyDown)
        //        {
        //            setIcon(SpriteName.InterfaceIconCamera);
        //        }
        //        else if (e.KeyDown)
        //        {
        //            switch (fillType)
        //            {
        //                case FillType.Delete:
        //                    setIcon(SpriteName.IconBuildRemove);
        //                    break;
        //                case FillType.Fill:
        //                    setIcon(SpriteName.IconBuildAdd);
        //                    break;
        //                case FillType.Select:
        //                    setIcon(SpriteName.IconBuildSelection);
        //                    break;
        //            }
        //        }
        //        else if (HaveSelection)
        //        {
        //            setIcon(SpriteName.IconBuildMoveSelection);
        //            if (inGame)
        //            {
        //                mode = Players.PlayerMode.CreationSelection;
        //            }
        //            else
        //            {
        //                updateSelectionObj();
        //            }
        //        }
        //        else
        //        {
        //            setIcon(SpriteName.IconBuildArrow);
        //        }
        //    }
        //}
        //protected override void drawKeyDown(bool keyDown, FillType fillType)
        //{
        //    if (keyDown)
        //    {
        //        switch (fillType)
        //        {
        //            case FillType.Delete:
        //                setIcon(SpriteName.IconBuildRemove);
        //                break;
        //            case FillType.Fill:
        //                setIcon(SpriteName.IconBuildAdd);
        //                break;
        //            case FillType.Select:
        //                setIcon(SpriteName.IconBuildSelection);
        //                break;
        //        }
        //    }
        //    base.drawKeyDown(keyDown, fillType);
        //}



        override protected void lostSelectionWhenMoving()
        {
            mode = Players.PlayerMode.Creation;
        }
        #region TextBlocks

        HUD.InputDialogue inputDialogue = null;
        public override void BeginInputDialogueEvent(KeyboardInputValues keyInputValues)
        {
            inputDialogue = new HUD.InputDialogue(menu, keyInputValues);
        }
        void deleteInputDialogue()
        {
            if (inputDialogue != null)
            {
                inputDialogue.DeleteMe();
                inputDialogue = null;
                openHeadMenu(false);
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (inGame)
            {
                SpriteName icon = SpriteName.IconBuildArrow;
                if (pencilKeyDown)
                {
                    switch (fillType)
                    {
                        case FillType.Delete:
                            icon = SpriteName.IconBuildRemove;
                            break;
                        case FillType.Fill:
                            icon = SpriteName.IconBuildAdd;
                            break;
                        case FillType.Select:
                            icon = SpriteName.IconBuildSelection;
                            break;
                    }
                }
                else if (cameraCtlrKeyDown)
                {
                    icon = SpriteName.InterfaceIconCamera;
                }

                crossHair.SetSpriteName(icon);
            }

        }

        public override void TextInputCancelEvent(PlayerIndex playerIndex)
        {
            deleteInputDialogue();
        }

        const byte EmptyLetter = (byte)Data.MaterialType.empty_letter;
        public override void TextInputEvent(PlayerIndex playerIndex, string input, int link)
        {
            deleteInputDialogue();
            WaitingForTextInput = false;
            if (inGame)
            {
                input = input.ToLower();
                input = TextLib.CheckBadLanguage(input);

                byte[] materials = new byte[input.Length];

                bool xdir = cameraInZdir();
                Vector2 dir = lib.AngleToV2(playCam.TiltX, 1);
                int leftToRightDir;
                if (xdir)
                    leftToRightDir = lib.FloatToDir(dir.X);
                else
                    leftToRightDir = lib.FloatToDir(dir.Y);


                for (int i = 0; i < input.Length; i++)
                {

                    materials[i] = (byte)lib.LetterBlockFromChar(input[i]);
                }

                LetterRows rows = lettersToRows(materials, designerInterface.drawCoord, xdir, leftToRightDir);
                designerInterface.selectionArea = rows.selectionArea;
                storeUndoableAction(UndoType.LetterBlocks);
                Map.WorldPosition startPos = worldPos;
                startPos.WorldGrindex += designerInterface.drawCoord;
                // startPos.UpdateChunkPos();
                printLetters(rows, materials, startPos, xdir, leftToRightDir);
                updateVoxelObj();

                NetworkWriteTextBlocks(xdir, leftToRightDir, materials);
            }
            else
            {
                currentObjName = input;
                save();
                openHeadMenu(false);
            }
        }
        void NetworkWriteTextBlocks(bool xdir, int leftToRightDir, byte[] materials)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_TextBlocks,
                Network.PacketReliability.Reliable, parent.Index);
            worldPos.WorldGrindex.WriteStream(w);
            designerInterface.drawCoord.WriteByteStream(w);
            w.Write(xdir);
            w.Write(leftToRightDir > 0);
            w.Write((byte)materials.Length);
            w.Write(materials);
        }
        public static void NetworkReadTextBlocks(System.IO.BinaryReader r, Players.ClientPlayer sender)
        {
            Map.WorldPosition worldPos = Map.WorldPosition.EmptyPos;
            worldPos.WorldGrindex.ReadStream(r);
            Map.WorldPosition startPos = worldPos;

            IntVector3 drawCoord = IntVector3.FromByteSzStream(r);
            startPos.WorldGrindex += drawCoord;
            //  startPos.UpdateChunkPos();

            bool xdir = r.ReadBoolean();
            int leftToRightDir = lib.BoolToDirection(r.ReadBoolean());
            int length = r.ReadByte();
            byte[] materials = r.ReadBytes(length);

            LetterRows rows = lettersToRows(materials, drawCoord, xdir, leftToRightDir);
            RangeIntV3 selectionArea = rows.selectionArea;
            sender.EditorStoreUndoableAction(selectionArea, worldPos, UndoType.LetterBlocks);
            printLetters(rows, materials, startPos, xdir, leftToRightDir);


            UpdateMapArea(worldPos, selectionArea);
            //    Map.WorldPosition minScreen = worldPos;
            //    minScreen.WorldGrindex += selectionArea.Min;
            //   // minScreen.UpdateChunkPos();
            //    Map.WorldPosition maxScreen = worldPos;
            //    maxScreen.WorldGrindex += selectionArea.Max;
            //    //maxScreen.UpdateChunkPos();

            //    //Update Chunks
            ////public static void UpdateMapArea(Map.WorldPosition worldPos, RangeIntV3 selectionArea)


            //    IntVector2 screenIx = IntVector2.Zero;
            //    for (screenIx.Y = minScreen.ChunkGrindex.Y; screenIx.Y <= maxScreen.ChunkGrindex.Y; screenIx.Y++)
            //    {
            //        for (screenIx.X = minScreen.ChunkGrindex.X; screenIx.X <= maxScreen.ChunkGrindex.X; screenIx.X++)
            //        {
            //            Map.Chunk screen = LfRef.chunks.GetScreenUnsafe(screenIx);
            //            if (screen != null)
            //            {
            //                if (screen.Openstatus >= Map.ScreenOpenStatus.MeshGeneratedDone)
            //                {
            //                    Map.World.ReloadChunkMesh(screenIx);
            //                }
            //                else
            //                {
            //                    LfRef.worldOverView.ChangedChunk(screenIx);
            //                }
            //            }
            //        }
            //    }
        }

        static void letterBlocksNewRow(ref int row, ref int length, ref int maxLength, List<int> lenghts)
        {
            lenghts.Add(length);
            row++;
            if (length > maxLength)
                maxLength = length;
            length = 0;
        }
        static LetterRows lettersToRows(byte[] letters, IntVector3 drawCoord, bool xdir, int leftToRightDir)
        {
            LetterRows result = new LetterRows();
            result.lengths = new List<int>();

            int row = 0;
            int length = 0;
            int maxLength = 0;
            int numEmpty = 0;


            for (int i = 0; i < letters.Length; i++)
            {

                if (letters[i] == EmptyLetter)
                {
                    numEmpty++;
                    if (numEmpty >= 3)
                    {
                        letterBlocksNewRow(ref row, ref length, ref maxLength, result.lengths);
                    }
                }

                int sidePos;
                length++;
                if (xdir) //xdir är fel?
                {
                    sidePos = drawCoord.X;
                }
                else
                {
                    sidePos = drawCoord.Z;
                }

                if (leftToRightDir > 0)
                {
                    if (sidePos + length >= CreationXZSize)
                    {
                        letterBlocksNewRow(ref row, ref length, ref maxLength, result.lengths);
                    }
                }
                else
                {
                    if (sidePos - length < 0)
                    {
                        letterBlocksNewRow(ref row, ref length, ref maxLength, result.lengths);
                    }
                }
            }

            if (length > 0)
            {
                if (length > maxLength)
                    maxLength = length;
                result.lengths.Add(length);
            }
            result.selectionArea.Min = drawCoord;
            if (xdir)
            {
                int endPosX = Bound.Set(drawCoord.X + maxLength * leftToRightDir, 0, CreationChunkWidth);
                result.selectionArea.Min.X = lib.SmallestOfTwoValues(endPosX, drawCoord.X);
                result.selectionArea.Max.X = lib.LargestOfTwoValues(endPosX, drawCoord.X);
                result.selectionArea.Max.Z = drawCoord.Z;
            }
            else
            {
                int endPosZ = Bound.Set(drawCoord.Z + maxLength * leftToRightDir, 0, CreationChunkWidth);
                result.selectionArea.Min.Z = lib.SmallestOfTwoValues(endPosZ, drawCoord.Z);
                result.selectionArea.Max.Z = lib.LargestOfTwoValues(endPosZ, drawCoord.Z);
                result.selectionArea.Max.X = drawCoord.X;
            }
            result.selectionArea.Max.Y = drawCoord.Y;
            result.selectionArea.Min.Y = Bound.Min(drawCoord.Y - row, 0);
            return result;
        }

        static void printLetters(LetterRows letterRows, byte[] letters, Map.WorldPosition startPos, bool xdir, int leftToRightDir)
        {
            Map.WorldPosition pos = startPos;
            int letter = 0;
            for (int row = 0; row < letterRows.lengths.Count; row++)
            {
                for (int col = 0; col < letterRows.lengths[row]; col++)
                {
                    LfRef.chunks.Set(pos, letters[letter]);
                    if (xdir)
                    {
                        pos.WorldGrindex.X += leftToRightDir;
                    }
                    else
                    {
                        pos.WorldGrindex.Z += leftToRightDir;
                    }
                    letter++;
                }
                pos = startPos;
                pos.WorldGrindex.Y += -row - 1;
            }
        }


        #endregion

        const string DoorPart1Text = "Door (open)";
        const string DoorPart2Text = "Door (closed)";

        void mainMenu()
        {

            HUD.File file = new HUD.File();
            file.AddIconTextLink(Data.MaterialBuilder.MaterialTile(selectedMaterial), SelectMaterial, new HUD.Link((int)Link.LinkSelectMaterial, 0));
            file.AddDescription("Tool: " + settings.DrawTool.ToString());
            file.AddTextLink("Select tool", "Change the shape of the drawing tool", (int)Link.ChangeTool);
            //TOOL OPTIONS
            if (drawTool == DrawTool.Pencil || drawTool == DrawTool.Road || drawTool == DrawTool.ReColor)
            {
                file.ValueOptionList2("Pencil size", (int)ValueLink.PencilSize, new IntervalF(1, 17), 2);
                file.Checkbox2("Round pencil", null, (int)ValueLink.RoundPencil);

                if (drawTool == DrawTool.Road)
                {
                    file.ValueOptionList2("Edge size", (int)ValueLink.RoadEdgeSize, new IntervalF(0, 5), 1);
                    file.AddIconTextLink(Data.MaterialBuilder.MaterialTile(secondaryMaterial), "Edge material", new HUD.Link((int)Link.LinkSelectMaterial, 1));
                    file.ValueOptionList2("Percent Fill", (int)ValueLink.PercentFill, new IntervalF(10, 100), 10);

                    file.ValueOptionList2("Clear above", (int)ValueLink.RoadUpwardClear, new IntervalF(0, 32), 1);
                    file.ValueOptionList2("Fill below", (int)ValueLink.RoadBelowFill, new IntervalF(0, 32), 1);
                }
            }

            if (PlatformSettings.ViewUnderConstructionStuff)
            {
                if (door != null)
                {
                    file.AddTextLink(DoorPart1Text, "Complete and save the door", (int)Link.CreateDoor2);
                    file.AddTextLink("Cancel Door", "Remove the selected door area", (int)Link.CancelCreateDoor);
                }
            }

            if (drawCoordMaterial != 0)
                file.AddIconTextLink(SpriteName.IconColorPick, ((Data.MaterialType)drawCoordMaterial).ToString(), (int)Link.LinkPickMaterial);

            if (inGame)
            {

                if (PlatformSettings.DebugOptions)
                {
                    file.AddTextLink("Template message", (int)Link.LoadTemplateMessage);
                }

                file.AddTextLink("Insert template", (int)Link.LoadTemplate);
                file.AddTextLink("Flatten area",
                    "Will fill everything to the level of the cursor, with the selected material, and remove all above it", (int)Link.FLattenArea);
            }

            file.AddTextOptionList("Move speed", (int)Link.MoveSpeed, designMoveSpeedOption,
                new List<string>
                {
                    "Slow", "Recommended", "Fast", "Very Fast"
                });

            if (inGame)
            {
                file.Checkbox2("Selection Cut", "Removes blocks you select with RT", (int)ValueLink.SelectionCut);

                if (PlatformSettings.ViewUnderConstructionStuff)
                    file.AddIconTextLink(SpriteName.IconInfo, "Create door", (int)Link.CreateDoorInfo);
                //
                file.AddTextLink("Letter blocks", "Add a row of blocks with letters on them", (int)Link.TypeTextBlocks);
                file.AddTextLink("Exit Creation", (int)Link.EXIT);
            }
            else
            {
                file.AddTextLink("Save", (int)Link.LinkSaveOptions);
                file.AddTextLink("Load", (int)Link.Load);
                file.AddTextLink("Canvas size", "Change the draw limits", (int)Link.CanvasSize);

                if (PlatformSettings.RunningWindows)
                    file.AddTextLink("Expand Draw Limits", (int)Link.LinkExpandLimits);

                file.AddTextLink(LinkRotateFlip, "Rotate or flip the whole model", (int)Link.LinkRotateFlip);
                file.AddTextLink(LinkMoveEverything, (int)Link.LinkMoveEverything);

                //if (animationFrames.Frames.Count < MaxFrames)
                file.AddTextLink("Add frame", "Add another frame to the animation", (int)Link.AnimAddFram);


                if (haveAnimation)
                {
                    file.AddTextLink("Remove current frame", (int)Link.AnimRemoveFrame);
                    file.AddTextLink("Clear Animation", "Remove all frames but this one", (int)Link.AnimRemoveAll);
                    file.AddTextLink("Move frame", "Change the order of frames in the animation", (int)Link.AnimMoveFrame);
                    if (lockFirstFrames == 0)
                    {
                        if (currentFrame.Value > 0)
                            file.AddTextLink("Lock as first frame", "Frames before this wont be seen when testing the animation", (int)Link.AnimLockFrame);
                    }
                    else
                    {
                        file.AddTextLink("Remove frame lock", "Will view the first frame again", (int)Link.AnimUnlockFrame);
                    }
                }
                file.AddTextLink("Clear all", "Removes all blocks and all frames", (int)Link.LinkClearAll);
                file.AddTextLink("Background Color", (int)Link.LinkBGcolor);
                file.AddTextLink("Hide HUD", "View only the model, great for screen capture", (int)Link.LinkHideHUD);

                if (!inGame)
                {
                    file.AddDescription(LootfestLib.ViewBackText + " for help");
                    file.AddTextLink("Exit", "Exit to main menu", (int)Link.EXIT);
                }

            }

            menu.File = file;
        }

        //List<UndoAction> undoActions = new List<UndoAction>();
        protected override void storeUndoableAction(UndoType type)
        {
            if (inGame)
                parent.EditorStoreUndoableAction(designerInterface.selectionArea, worldPos, type);
            else
                base.storeUndoableAction(type);
        }
        protected override bool undo()
        {
            if (inGame)
            {
                if (parent.EditorUndo())
                {
                    //need teleport
                    openHeadMenu(true);
                    mFile = new HUD.File();
                    mFile.AddTitle("To far away");
                    mFile.AddDescription("You need to be on the location");
                    mFile.AddTextLink("Teleport there", (int)Link.TeleportToUndo);
                    menu.File = mFile;

                }
            }
            else
            {
                bool result = base.undo();
                if (!result)
                {
                    LF2.Music.SoundManager.PlayFlatSound(LoadedSound.out_of_ammo);
                }
                return result;
            }
            return false;
        }

        void ShowControls(bool PC)
        {
            DataLib.DocFile headMenuFile = new DataLib.DocFile();
            if (PC)
            {

                headMenuFile.addText("PC controls (must press F2)", TextStyle.Body);

                headMenuFile.addText("Arrow/WASD: Move XZ", Align.Zero, TextStyle.Body);
                headMenuFile.addText("MouseScroll: MoveY", Align.Zero, TextStyle.Body);
                headMenuFile.addText("Shift+arrows: MoveY", Align.Zero, TextStyle.Body);
                headMenuFile.addText("Shift+scroll: Zoom", Align.Zero, TextStyle.Body);
                headMenuFile.addText("Space: Draw", Align.Zero, TextStyle.Body);
                headMenuFile.addText("X: Draw Selection", Align.Zero, TextStyle.Body);
                headMenuFile.addText("B: Delete", Align.Zero, TextStyle.Body);
                headMenuFile.addText("Ctrl+Z: Undo", Align.Zero, TextStyle.Body);
                headMenuFile.addText("Ctrl+Scroll: Animation", Align.Zero, TextStyle.Body);
            }
            else
            {

                headMenuFile.addText("LS: Move XZ", Align.Zero, TextStyle.Body);
                headMenuFile.addText("RS: MoveY", Align.Zero, TextStyle.Body);
                headMenuFile.addText("RB: Draw", Align.Zero, TextStyle.Body);
                headMenuFile.addText("RT: Draw Selection", Align.Zero, TextStyle.Body);
                headMenuFile.addText("LB: Delete", Align.Zero, TextStyle.Body);
                headMenuFile.addText("LT + LS or RS: Camera", Align.Zero, TextStyle.Body);
                headMenuFile.addText("LT + Dpad: Add/Remove Frame", TextStyle.Body);
                headMenuFile.addText("Y: Undo", Align.Zero, TextStyle.Body);
                headMenuFile.addText("Dpad: Next frame", Align.Zero, TextStyle.Body);
            }
            //headMenuFile.addText("Voxel designer version5", Align.Zero, TextStyle.Body);
            //headMenuFile.addText("by GAMEFARM", Align.Zero, TextStyle.Body);

            menu.OpenDocument(headMenuFile);
        }

        public override void BlockMenuListOptionEvent(int link, int option, int playerIx)
        {
            switch ((Link)link)
            {
                case Link.MoveSpeed:
                    designMoveSpeedOption = option;
                    break;
            }
        }
        public override void BlockMenuValueOptionEvent(int link, double value, int playerIx)
        {
            switch ((Link)link)
            {
                case Link.DrawGeometry:
                    sphereRadius = (float)value;
                    break;

            }
        }
        public override void BlockMenuCheckboxEvent(int link, bool value, int playerIx)
        {
            switch ((Link)link)
            {
                case Link.ChbxSelectionCut:
                    settings.SelectionCut = value;
                    if (parent != null)
                        parent.SettingsChanged();
                    break;

                case Link.CBRepeatOnAllFrames:
                    repeateOnAllFrames = value;
                    break;
            }
        }

        bool combineLoading = false;
        public override bool BlockMenuBoolValue(int playerIx, bool value, bool get, int valueIx)
        {
            switch ((ValueLink)valueIx)
            {
                case ValueLink.RepeateOnAllFrames:
                    value = getSetBool(ref repeateOnAllFrames, value, get);
                    break;
                case ValueLink.SelectionCut:
                    value = getSetBool(ref settings.SelectionCut, value, get);
                    break;
                case ValueLink.RoundPencil:
                    value = getSetBool(ref settings.RoundPencil, value, get);
                    break;
                case ValueLink.CombineLoadedModel:
                    value = getSetBool(ref combineLoading, value, get);
                    break;
                case ValueLink.bMergeKeepSize:
                    value = getSetBool(ref mergeModelsOption.KeepOldGridSize, value, get);
                    break;
                case ValueLink.bMergeNewOverride:
                    value = getSetBool(ref mergeModelsOption.NewBlocksReplaceOld, value, get);
                    break;

                default:
                    throw new NotImplementedException("Voxeldesigner BlockMenuBoolValue");
            }
            return value;
        }
        public override float BlockMenuFloatValue(int playerIx, float value, bool get, int valueLink)
        {
            switch ((ValueLink)valueLink)
            {
                case ValueLink.RoadUpwardClear:
                    if (get)
                        return settings.RoadUpwardClear;
                    else
                        settings.RoadUpwardClear = (int)value;
                    break;
                case ValueLink.RoadBelowFill:
                    if (get)
                        return settings.RoadBelowFill;
                    else
                        settings.RoadBelowFill = (int)value;
                    break;

                case ValueLink.PencilSize:
                    if (get)
                        return settings.PencilSize;
                    else
                        settings.PencilSize = (int)value;
                    break;
                case ValueLink.RoadEdgeSize:
                    if (get)
                        return settings.RoadEdgeSize;
                    else
                        settings.RoadEdgeSize = (int)value;
                    break;
                case ValueLink.PercentFill:
                    if (get)
                        return settings.RoadPercentFill;
                    else
                        settings.RoadPercentFill = (int)value;
                    break;
                default:
                    throw new NotImplementedException("Voxeldesigner BlockMenuFloatValue");
            }
            return value;

        }
        const string TemplateCategoryFolder = "Template";
        const string TemplateFileEnd = ".tpl";

        string categoryDir(SaveCategory c)
        {
            return this.categoryDir((int)c);
        }
        string categoryDir(int c)
        {
            return TemplateCategoryFolder + c.ToString();
        }
        SaveCategory currentCategory = SaveCategory.non;
        string currentFileName;

        //List<string> templateMessage;

        public static DataStream.FilePath CustomVoxelObjPath(string name)
        {
            return new DataStream.FilePath(UserVoxelObjFolder, name, Data.Images.VoxelObjByteArrayEnding);
        }

        HUD.File mFile;
        public bool WaitingForTextInput = false;
        public override void BlockMenuLinkEvent(HUD.IMenuLink link, int playerIx, numBUTTON button)
        {
            if (inputDialogue != null)
            {
                inputDialogue.BlockMenuLinkEvent(link, playerIx, button);
            }
            else
            {
                previousMenu = -1; //previousMenuDialogue = -1;
                switch ((Link)link.Value1)
                {
                    case Link.DeleteCreation_dialogue:
                        DataStream.DataStreamHandler.BeginUserRemoveFile(CustomVoxelObjPath(currentObjName));
                        mainMenu();
                        break;
                    case Link.SendAsMessagePart_dialogue:
                        VoxelObjGridData grid2 = SelectionToGrid();
                        string message = grid2.ToMessage();
                        int length = message.Length;
                        int removeLength = link.Value2 * MessageMaxLength;
                        message = message.Remove(0, removeLength);
                        message = TextLib.KeepFirstLetters(message, MessageMaxLength);
                        Engine.XGuide.SendFeedBack(playerIx, message);
                        break;
                    case Link.TemplateDeleteDialogue_dialogue:
                        mFile = new HUD.File();
                        mFile.AddTitle("Delete template?");
                        mFile.AddTextLink("Delete", (int)Link.TemplateDeleteOK);
                        mFile.AddTextLink("Cancel", new HUD.Link((int)Link.SelectTemplateFile_dialogue, link.Value2));//link.Value2, (int)Link.SelectTemplateFile);
                        menu.File = mFile;
                        break;
                    case Link.CanvasSize_dialogue:
                        RangeIntV3 newLimit = StandardDrawLimitRange;
                        switch ((DrawLimits)link.Value2)
                        {
                            case DrawLimits.W24H24:
                                newLimit.Max.X = 23;
                                newLimit.Max.Y = 23;
                                newLimit.Max.Z = 23;
                                break;
                            case DrawLimits.W24H32:
                                newLimit.Max.X = 23;
                                newLimit.Max.Y = 31;
                                newLimit.Max.Z = 23;
                                break;
                            case DrawLimits.W32H32:
                                newLimit.Max.X = 31;
                                newLimit.Max.Y = 31;
                                newLimit.Max.Z = 31;
                                break;
                            case DrawLimits.W32H48:
                                newLimit.Max.X = 31;
                                newLimit.Max.Y = 47;
                                newLimit.Max.Z = 31;
                                break;
                        }
                        setDrawLimit(newLimit);
                        break;
                    case Link.CanvasSizePlusOne_dialogue:
                        drawLimits.Max.AddDimention((Dimensions)link.Value2, 1);
                        UpdateDrawLimits();
                        updateVoxelObj();
                        break;
                    case Link.CanvasSizeSubOne_dialogue:
                        drawLimits.Max.AddDimention((Dimensions)link.Value2, -1);
                        UpdateDrawLimits();
                        updateVoxelObj();
                        break;
                    case Link.ImportObject_dialogue:
                        VoxelObjName loader;
                        if (PlatformSettings.DebugOptions)
                        {
                            loader = (VoxelObjName)link.Value2;
                        }
                        else
                        {
                            loader = loadableInGameObjects()[link.Value2];
                        }

                        if (inGame)
                        {
                            voxelGridToSelection(Editor.VoxelObjDataLoader.LoadVoxelObjGrid(loader)[0]);
                        }
                        else
                        {
                            currentObjName = loader.ToString();
                            loadVoxels(VoxelObjDataLoader.LoadVoxelObjGrid(loader));
                            updateVoxelObj();
                        }
                        break;
                    case Link.ChangeMaterialFrom_dialogue:
                        swapMaterialFrom = (byte)link.Value2;

                        HUD.File swapMenu2 = new HUD.File();
                        swapMenu2.AddTitle("Swap Material To");
                        listMaterials(swapMenu2, (int)Link.ChangeMaterialTo_dialogue, true, settings, (int)Link.ShowHideMaterialNames, 0);
                        menu.File = swapMenu2;
                        break;
                    case Link.SelectMaterial_dialogue:
                        if (link.Value3 == 0)
                            selectedMaterial = (byte)link.Value2;
                        else
                            secondaryMaterial = (byte)link.Value2;
                        openHeadMenu(false);
                        break;
                    case Link.ChangeMaterialTo_dialogue:
                        byte swapTo = (byte)link.Value2;
                        if (HaveSelection)
                        {
                            if (repeateOnAllFrames)
                            {
                                designerInterface.drawSize = designerInterface.selectionArea.Add;
                                for (int i = 0; i < animationFrames.Frames.Count; i++)
                                {
                                    swapMaterials(selectedVoxels, swapTo, false);
                                    makeStamp(false);
                                    selectedVoxels.Voxels.Clear();
                                    nextFrame(true);
                                    RunThreadedAction(ThreadedActionType.Rectangle, DrawTool.Rectangle, designerInterface.selectionArea, FillType.Select);
                                    // new ThreadedDrawAcion(ThreadedActionType.Rectangle, DrawTool.Rectangle, this, selectionArea, FillType.Select, false);
                                }
                            }
                            else
                                swapMaterials(selectedVoxels, swapTo, true);
                        }
                        else
                        {
                            swapMaterials(voxels, swapTo);
                        }
                        templateSent = false;
                        openHeadMenu(false);
                        break;
                    case Link.LoadCreation_dialogue:
                        currentObjName = UserMadeModelNameFromIndex(link.Value2);
                        if (LoadByteArray)
                        {

                            if (button == numBUTTON.X)
                            {
                                mFile = new HUD.File();
                                mFile.AddDescription("Remove " + currentObjName + "?");
                                mFile.AddTextLink("Remove", new HUD.Link((int)Link.DeleteCreation_dialogue, link.Value2));//link.Value2, (int)Link.DeleteCreation_dialogue);
                                mFile.AddTextLink("Cancel", (int)Link.LinkLoadUserMade);
                                menu.File = mFile;
                            }
                            else
                            {
                                //old
                                //new DataStream.ReadByteArrayObj(CustomVoxelObjPath(currentObjName), this, null);

                                //new
                                //new DataStream.ReadBinaryIO(CustomVoxelObjPath(currentObjName), animationFrames.ReadBinaryStream,
                                //    this);
                                new LoadCreatorImage(CustomVoxelObjPath(currentObjName), CreatorImageLoaded);
                            }

                        }
                        else
                        {

                            try
                            {
                                loadCreation(System.IO.Directory.GetCurrentDirectory() + TextLib.Dir + UserVoxelObjFolder +
                                    TextLib.Dir + currentObjName + TextLib.TextFileEnding);
                            }
                            catch
                            {
                                openHeadMenu(false);
                                print("ERROR: Loading Failed");
                            }
                            UpdateDrawLimits();
                        }

                        break;
                    case Link.SaveToCategory_dialogue:
                        hasChatergory[link.Value2] = true;
                        IntVector3 gridSize = designerInterface.selectionArea.Add;
                        VoxelObjGridData grid = SelectionToGrid();
                        List<byte> compressed = new List<byte> { (byte)gridSize.X, (byte)gridSize.Y, (byte)gridSize.Z };
                        compressed.AddRange(grid.ToCompressedData());

                        DataLib.SaveLoad.CreateFolder(categoryDir(link.Value2));
                        DataLib.SaveLoad.SaveByteArray(categoryDir(link.Value2) + TextLib.Dir +
                            DateTime.Now.Ticks.ToString() + TemplateFileEnd, compressed.ToArray());
                        openHeadMenu(false);
                        //visa save warning

                        if (inGame)
                        {
                            parent.Print(DataLib.AbsSaveToStorage.AbleToSave ?
                                "Template Saved" : "Saving Failed");
                        }
                        break;
                    case Link.OpenCategory_dialogue:
                        previousMenu = (int)Link.LoadTemplate;
                        loadingMenu();
                        new VoxelDesignerQueLoader(ThreadedLoad.ListTemplatesCategory, link.Value2, menu.MenuId, this);
                        break;
                    case Link.SelectTemplateFile_dialogue:
                        //open a bigger image of the rotating object
                        //be able to delete or change category, and see file date/size

                        previousMenu = (int)currentCategory; //previousMenuDialogue = (int)Link.OpenCategory_dialogue;

                        string fileName = DataLib.SaveLoad.FilesInStorageDir(
                            TemplateCategoryFolder + ((int)currentCategory).ToString())[link.Value2];
                        currentFileName = fileName + TemplateFileEnd;
                        HUD.File templateInfo = new HUD.File();
                        templateInfo.AddTitle(fileName);

                        long ticks = Convert.ToInt64(fileName);
                        DateTime saveDate = new DateTime().AddTicks(ticks);
                        templateInfo.AddDescription(saveDate.ToString());

                        templateInfo.AddTextLink("Use", (int)Link.TemplateUse);
                        templateInfo.AddTextLink("Delete", new HUD.Link((int)Link.TemplateDeleteDialogue_dialogue, link.Value2));
                        templateInfo.AddIcon(CategoryInfo(currentCategory), Color.White, HUD.Link.Empty);
                        menu.File = templateInfo;
                        break;
                    case Link.SelectThrallordFile_dialogue:
                        string name = DataLib.SaveLoad.FilesInContentDir(ThrallordPath)[link.Value2];
                        loadTemplateFile(new DataStream.FilePath(
                            ThrallordPath, name, TemplateFileEnd, false));
                        break;
                    case Link.SelectRaceTrackFile_dialogue:
                        string name2 = DataLib.SaveLoad.FilesInContentDir(RaceTrackPath)[link.Value2];
                        loadTemplateFile(new DataStream.FilePath(
                            RaceTrackPath, name2, TemplateFileEnd, false));
                        break;
                    //case Link.TemplateMessagePart_dialogue:
                    //    Engine.XGuide.SendFeedBack(playerIx, templateMessage[link.Value2]);
                    //    break;
                    case Link.SelectTool_dialogue:
                        settings.DrawTool = (DrawTool)link.Value2;
                        openHeadMenu(false);
                        break;
                    //default:
                    //    switch ((Link)link)
                    //    {
                    case Link.AnimUnlockFrame:
                        lockFirstFrames = 0;
                        openHeadMenu(false);
                        break;
                    case Link.AnimLockFrame:
                        lockFirstFrames = currentFrame.Value;
                        openHeadMenu(false);
                        break;
                    case Link.ChangeTool:
                        mFile = new HUD.File();
                        for (int i = 0; i < (int)DrawTool.NUM; i++)
                        {
                            mFile.AddTextLink(((DrawTool)i).ToString(), new HUD.Link((int)Link.SelectTool_dialogue, i));//i, (int)Link.SelectTool_dialogue);
                        }
                        menu.File = mFile;
                        break;
                    case Link.SetLimitsAfterSel:
                        if (HaveSelection)
                        {
                            lockFirstFrames = 0;
                            dropSelection(false);
                            bool repeatSave = repeateOnAllFrames;
                            repeateOnAllFrames = true;
                            moveAll(-designerInterface.selectionArea.Min);
                            repeateOnAllFrames = repeatSave;
                            drawLimits.Max = designerInterface.selectionArea.Add;
                            UpdateDrawLimits();
                            updateVoxelObj();
                        }
                        break;
                    case Link.StampAllFrames:
                        for (int i = 0; i < animationFrames.Frames.Count; i++)
                        {
                            nextFrame(true);
                            makeStamp(false);
                        }
                        openHeadMenu(false);
                        break;
                    case Link.CreateDoor1:
                        if (designerInterface.selectionArea.Add.SideLength() <= GameObjects.EnvironmentObj.Door.MinSize)
                        {
                            mFile = new HUD.File();
                            mFile.AddTitle("To small");
                            mFile.AddDescription("The selection must have a minmum of two blocks");
                            mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)Link.SelectionMenu);
                            menu.File = mFile;
                        }
                        if (designerInterface.selectionArea.Add.SideLength() <= GameObjects.EnvironmentObj.Door.MaxSize)
                        {
                            if (settings.SelectionCut)
                                makeStamp(true);
                            door = new GameObjects.EnvironmentObj.Door(worldPos.GetNeighborPos(designerInterface.selectionArea.Min), designerInterface.selectionArea.Add);
                            doorOutline = (Graphics.Mesh)designerInterface.pencilMultiSelection.CloneMe();
                            doorOutline.Position = designerInterface.multiSelImagePos();
                            doorOutline.TextureSource.SetSpriteName(SpriteName.InterfaceBorder);
                            doorOutline.Color = Color.LightBlue;
                            openHeadMenu(false);

                            removeSelection();
                        }
                        else
                        {
                            mFile = new HUD.File();
                            mFile.AddTitle("To large");
                            mFile.AddDescription("The selection can be max " + GameObjects.EnvironmentObj.Door.MaxSize.ToString() + " blocks wide and height");
                            mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)Link.SelectionMenu);
                            menu.File = mFile;
                        }
                        break;
                    case Link.CreateDoorInfo:
                        DataLib.DocFile doorInfo = new DataLib.DocFile();
                        doorInfo.addText("Create doors", TextStyle.LF_HeadTitle);
                        doorInfo.addText("The door is two snapshots of the map that you switch between.", TextStyle.LF_Description);
                        doorInfo.addText("Creating a door is a two step process", TextStyle.LF_UTitle);
                        doorInfo.addText("1. Design the door how you want it to be when it's closed. Select the blocks (RT) that will be included both when the door is opened and closed. Press START and select \"" +
                            DoorPart1Text + "\" in the menu", TextStyle.LF_Bread);
                        doorInfo.addText("2. Now build the door to look open, inside the blue area. Press START and select \"" + DoorPart2Text + "\" to complete the creation", TextStyle.LF_Bread);
                        doorInfo.addText("Edit and remove", TextStyle.LF_UTitle);
                        doorInfo.addText("You can edit the door when it is complete by simply changing the blocks.", TextStyle.LF_Bread);
                        doorInfo.addText("To remove the door, walk up to it with the charcter and press START to find the remove option in the menu.", TextStyle.LF_Bread);
                        menu.OpenDocument(doorInfo);
                        break;
                    case Link.CreateDoor2:
                        if (door.CompleteDoor(playerIx))
                        {
                            clearDoor();
                            openHeadMenu(false);
                        }
                        else
                        {
                            mFile = new HUD.File();
                            mFile.AddTitle("Not complete");
                            mFile.AddDescription("You must make changes to the door");
                            mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)Link.Cancel);
                            menu.File = mFile;
                        }
                        break;
                    case Link.CancelCreateDoor:
                        clearDoor();
                        openHeadMenu(false);
                        break;
                    case Link.SelectionMenu:
                        selectionMenu();
                        break;
                    case Link.SelSendAsMessage:
                        VoxelObjGridData grid3 = SelectionToGrid();
                        string message2 = grid3.ToMessage();

                        int NumMessages = message2.Length / MessageMaxLength;
                        if (MessageMaxLength * NumMessages < message2.Length)
                            NumMessages++;
                        mFile = new HUD.File();
                        mFile.AddDescription(TextLib.KeepFirstLetters(message2, 12));
                        for (int i = 0; i < NumMessages; i++)
                        {
                            mFile.AddTextLink("Message" + (i + 1).ToString(), new HUD.Link((int)Link.SendAsMessagePart_dialogue, i));//i, (int)Link.SendAsMessagePart_dialogue);
                        }
                        menu.File = mFile;
                        break;
                    case Link.TemplateDeleteOK:
                        if (inGame)
                        {
                            Engine.Storage.AddToSaveQue(new DataLib.RemoveFile(categoryDir(currentCategory) + TextLib.Dir + currentFileName).StartQuedProcess, true);//Engine.Storage.AddToSaveQue(new DataLib.RemoveFile(categoryDir(currentCategory) + TextLib.Dir + currentFileName));
                        }
                        //DataLib.SaveLoad.RemoveFile(categoryDir(currentCategory) + TextLib.Dir + currentFileName);
                        parent.Print("Template Deleted");
                        //listTemplates();
                        loadingMenu();
                        new VoxelDesignerQueLoader(ThreadedLoad.ListTemplates, this);
                        break;
                    case Link.TypeTextBlocks:
                        Engine.XGuide.BeginKeyBoardInput(new KeyboardInputValues("Type block letters", "Accepts a-z and 0-9, all other letters will be empty blocks", TextLib.EmptyString, playerIx));
                        openHeadMenu(false);
                        WaitingForTextInput = true;
                        break;
                    case Link.Load:
                        mFile = new HUD.File();
                        mFile.AddTextLink("My creations", (int)Link.LinkLoadUserMade);
                        mFile.AddTextLink("Template", (int)Link.LoadTemplate);
                        mFile.AddTextLink("In game", (int)Link.LinkImportInGameObj);
                        if (PlatformSettings.DebugOptions)
                        {
                            mFile.Checkbox2("Combine", "Will merge with the current model", (int)ValueLink.CombineLoadedModel);
                        }
                        //mFile.AddDescription("Other options");
                        //mFile.AddTextLink("Remove creation", (int)Link.ListMyCreationsForRemove);
                        menu.File = mFile;
                        break;
                    case Link.CanvasSize:
                        mFile = new HUD.File();
#if WINDOWS
                        mFile.AddTextLink("+X", new HUD.Link((int)Link.CanvasSizePlusOne_dialogue, (int)Dimensions.X));
                        mFile.AddTextLink("+Y", new HUD.Link((int)Link.CanvasSizePlusOne_dialogue, (int)Dimensions.Y));
                        mFile.AddTextLink("+Z", new HUD.Link((int)Link.CanvasSizePlusOne_dialogue, (int)Dimensions.Z));
                        mFile.AddTextLink("-X", new HUD.Link((int)Link.CanvasSizeSubOne_dialogue, (int)Dimensions.X));
                        mFile.AddTextLink("-Y", new HUD.Link((int)Link.CanvasSizeSubOne_dialogue, (int)Dimensions.Y));
                        mFile.AddTextLink("-Z", new HUD.Link((int)Link.CanvasSizeSubOne_dialogue, (int)Dimensions.Z));
#endif

                        mFile.AddDescription("Draw limits Width and Height");
                        for (DrawLimits dl = (DrawLimits)0; dl < DrawLimits.NUM; dl++)
                        {
                            mFile.AddTextLink(dl.ToString(), new HUD.Link((int)Link.CanvasSize_dialogue, (int)dl));
                        }
                        mFile.AddDescription("Will clear the undo list!");
                        menu.File = mFile;
                        break;
                    case Link.AnimMoveFrame:
                        mFile = new HUD.File();
                        mFile.AddTextLink("Forward", (int)Link.AnimMoveFrameForward);
                        mFile.AddTextLink("Back", (int)Link.AnimMoveFrameBack);
                        mFile.AddTextLink("To start", (int)Link.AnimMoveFrameTobeginning);
                        mFile.AddTextLink("To end", (int)Link.AnimMoveFrameToEnd);
                        menu.File = mFile;
                        break;
                    case Link.AnimMoveFrameForward:
                        moveFrame(MoveFrameType.Forward);
                        break;
                    case Link.AnimMoveFrameBack:
                        moveFrame(MoveFrameType.Back);
                        break;
                    case Link.AnimMoveFrameTobeginning:
                        moveFrame(MoveFrameType.ToStart);
                        break;
                    case Link.AnimMoveFrameToEnd:
                        moveFrame(MoveFrameType.ToEnd);
                        break;

                    case Link.ImportThrallords:
                        previousMenu = (int)Link.LoadTemplate;
                        mFile = new HUD.File();
                        listPremadeTemplates(mFile, ThrallordPath, Link.SelectThrallordFile_dialogue);
                        menu.File = mFile;
                        break;
                    case Link.ImportRaceTracks:
                        previousMenu = (int)Link.LoadTemplate;
                        mFile = new HUD.File();
                        listPremadeTemplates(mFile, RaceTrackPath, Link.SelectRaceTrackFile_dialogue);
                        menu.File = mFile;
                        break;
                    case Link.TeleportToUndo:
                        previousMenu = (int)Link.LoadTemplate;
                        parent.EndCreationMode();
                        parent.hero.JumpTo(parent.NextUndoPos());//parent.undoActions[0].WorldPos.ScreenIndex);
                        //openHeadMenu(false);
                        break;
                    case Link.FLattenArea:
                        if (parent.ClientPermissions == Players.ClientPermissions.Full)
                        {
                            //DrawTool tool = settings.DrawTool;
                            //settings.DrawTool = DrawTool.Rectangle;
                            designerInterface.selectionArea = new RangeIntV3(IntVector3.Zero, drawLimits.Max);

                            designerInterface.selectionArea.Max.Y = designerInterface.drawCoord.Y - 1;
                            storeUndoableAction(UndoType.Flatten);
                            //drawInArea(FillType.Fill);
                            new ThreadedDrawAcion(ThreadedActionType.Rectangle, DrawTool.Rectangle, this, designerInterface.selectionArea, FillType.Fill, false);
                            designerInterface.selectionArea.Max = drawLimits.Max;
                            designerInterface.selectionArea.Min.Y = designerInterface.drawCoord.Y;
                            storeUndoableAction(UndoType.Flatten);
                            //drawInArea(FillType.Delete);
                            new ThreadedDrawAcion(ThreadedActionType.Rectangle, DrawTool.Rectangle, this, designerInterface.selectionArea, FillType.Delete, false);

                            //settings.DrawTool = tool;
                        }
                        else
                        {
                            parent.Print("Need full permission");
                            openHeadMenu(false);
                        }
                        break;
                    //case Link.ShowHideMaterialNames:
                    //    settings.ViewMaterialName = !settings.ViewMaterialName;
                    //    mFile = new HUD.File();
                    //    listMaterials(mFile, (int)Link.SelectMaterial_dialogue, false, settings, (int)Link.ShowHideMaterialNames, 0);
                    //    menu.File = mFile;
                    //    if (parent != null)
                    //    {
                    //        parent.SettingsChanged();
                    //    }
                    //    break;

                    case Link.TemplateUse:
                        loadTemplateFile(new DataStream.FilePath(TemplateCategoryFolder + ((int)currentCategory).ToString(), currentFileName, null, true));
                        break;

                    case Link.SelSaveTemplate:
                        //list categories
                        HUD.File categories = new HUD.File();
                        categories.AddTitle("Select Category");
                        categories.AddTextLink("No category", new HUD.Link((int)Link.SaveToCategory_dialogue, 0));//0, (int)Link.SaveToCategory_dialogue);
                        for (int i = 1; i < (int)SaveCategory.NUM; i++)
                        {
                            SpriteName id = CategoryInfo((SaveCategory)i);
                            categories.AddIcon(id, Color.White, new HUD.Link((int)Link.SaveToCategory_dialogue, i));//i, (int)Link.SaveToCategory_dialogue);
                        }
                        menu.File = categories;
                        break;
                    case Link.LoadTemplate:
                        //listTemplates();
                        loadingMenu();
                        new VoxelDesignerQueLoader(ThreadedLoad.ListTemplates, 0, menu.MenuId, this);
                        break;
                    case Link.AnimAddFram:
                        const int MaxFrames =
#if WINDOWS
 140;
#else
                            10;
#endif
                        if (currentFrame.Max < MaxFrames)
                        {
                            AddFrame();
                        }
                        else
                        {
                            mFile = new HUD.File();
                            mFile.AddDescription("You've reached the maxmum frame length");
                            mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)Link.Cancel);
                            menu.File = mFile;
                        }
                        break;
                    case Link.Cancel:
                        openHeadMenu(true);
                        break;
                    case Link.AnimRemoveAll:
                        RemoveAllFramesButThis();
                        openHeadMenu(false);
                        break;
                    case Link.AnimRemoveFrame:
                        RemoveCurrentFrame();
                        break;

                    case Link.ShowControlsPC:
                        ShowControls(true);
                        break;
                    case Link.ShowControlsXBOX:
                        ShowControls(false);
                        break;
                    case Link.LinkSelectMaterial:
                        colorMenu(link.Value2);
                        break;
                    case Link.LinkPickMaterial:
                        //material = drawCoordMaterial;
                        pickColor();
                        openHeadMenu(false);
                        break;

                    case Link.LinkLoadUserMade:
                        menu.File = listUseMadeModels(true, (int)Link.LoadCreation_dialogue);
                        break;

                    case Link.DrawGeometry:
                        //openHeadMenu(false);
                        if (parent.ClientPermissions == Players.ClientPermissions.Full)
                        {
                            HUD.File sphereDialogue = new HUD.File();
                            sphereDialogue.AddValueOptionList("Radius", (int)Link.DrawGeometry, sphereRadius, new IntervalF(2.5f, Map.WorldPosition.ChunkWidth), 0.5f);

                            sphereDialogue.AddTextLink("Sphere", (int)Link.DrawSphere);
                            sphereDialogue.AddTextLink("Cylinder", (int)Link.DrawCylinder);
                            sphereDialogue.AddTextLink("Pyramid", (int)Link.DrawPyramid);
                            menu.File = sphereDialogue;
                        }
                        else
                        {
                            parent.Print("Need full permission");
                            openHeadMenu(false);
                        }
                        break;
                    case Link.DrawSphere:
                        openHeadMenu(false);
                        beginDrawGeometry(GeomitricType.Sphere);
                        break;
                    case Link.DrawCylinder:
                        openHeadMenu(false);
                        beginDrawGeometry(GeomitricType.Cylinder);
                        break;
                    case Link.DrawPyramid:
                        openHeadMenu(false);
                        beginDrawGeometry(GeomitricType.Pyramid);
                        break;

                    case Link.LinkClearAll:
                        this.NewCanvas();
                        currentObjName = randomName();
                        openHeadMenu(false);
                        break;
                    case Link.LinkSaveOptions:
                        mFile = new HUD.File();
                        mFile.AddDescription(currentObjName);
                        if (PlatformSettings.RunningWindows)
                        {
                            mFile.AddDescription("All files end up in \"" + UserVoxelObjFolder + "\", you can't replace the ingame models");
                        }
                        mFile.AddTextLink("Replace Old Save", "Will use the current name", (int)Link.LinkSaveReplace);
                        mFile.AddTextLink("Save As New", "Rename the model", (int)Link.SaveNew);
                        menu.File = mFile;
                        break;
                    case Link.SaveNew:
                        currentObjName = randomName();
                        Engine.XGuide.BeginKeyBoardInput(new KeyboardInputValues("Name", "Name your creation", currentObjName, playerIx));
                        break;
                    case Link.LinkSaveReplace:
                        save();
                        openHeadMenu(false);
                        break;
                    case Link.LinkImportInGameObj:
                        mFile = new HUD.File();
                        if (PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1)
                        {
                            for (int i = 0; i < (int)VoxelObjName.NUM_Empty; i++)
                            {

                                mFile.AddTextLink(((VoxelObjName)i).ToString(), new HUD.Link((int)Link.ImportObject_dialogue, i));//i, (int)Link.ImportObject_dialogue);
                            }
                        }
                        else
                        {
                            List<VoxelObjName> loadable = loadableInGameObjects();

                            for (int i = 0; i < loadable.Count; i++)
                            {
                                mFile.AddTextLink(loadable[i].ToString(), new HUD.Link((int)Link.ImportObject_dialogue, i));// i, (int)Link.ImportObject_dialogue);
                            }
                        }
                        mFile.AlphabeticOrder();
                        menu.File = mFile;
                        break;

                    case Link.LinkExpandLimits:
                        HUD.File file = new HUD.File();
                        file.AddTitle("Expand Dir");
                        file.AddTextLink(X, (int)Link.X);
                        file.AddTextLink(Y, (int)Link.Y);
                        file.AddTextLink(Z, (int)Link.Z);
                        menu.File = file;
                        break;
                    case Link.X:
                        ExpandDrawLimits(Dimensions.X, Data.Block.NumObjBlocksPerTerrainBlock);
                        break;
                    case Link.Y:
                        ExpandDrawLimits(Dimensions.Y, Data.Block.NumObjBlocksPerTerrainBlock);
                        break;
                    case Link.Z:
                        ExpandDrawLimits(Dimensions.Z, Data.Block.NumObjBlocksPerTerrainBlock);
                        break;

                    case Link.LinkSwapMaterial:
                        mFile = new HUD.File();
                        mFile.AddTitle("Swap Material From");
                        listSelectionMaterials(mFile, (int)Link.ChangeMaterialFrom_dialogue);
                        allFramesChkBox();
                        menu.File = mFile;
                        break;

                    case Link.LinkCancelSelection:
                        openHeadMenu(false);
                        break;
                    case Link.SelRotateC:
                        rotateHeader(true);
                        templateSent = false;
                        break;
                    case Link.SelRotateCC:
                        rotateHeader(false);
                        templateSent = false;
                        break;
                    case Link.SelRotateLieDown:

                        templateSent = false;
                        break;
                    case Link.SelMirror:
                        mirrorSelection();
                        break;
                    case Link.SelFlipY:
                        flip(Dimensions.Y);
                        templateSent = false;
                        break;


                    case Link.LinkAddFrame:
                        AddFrame();
                        openHeadMenu(false);
                        break;
                    case Link.LinkRemoveCurrentFrame:
                        RemoveCurrentFrame();
                        updateVoxelObj();
                        openHeadMenu(false);
                        break;
                    case Link.LinkHideHUD:
                        ShowHUD(false);
                        openHeadMenu(false);
                        break;
                    case Link.LinkBGcolor:
                        const string BGWhite = "White";
                        const string BGBlue = "Sky blue";
                        const string BGBlack = "Black";
                        HUD.File bgcol = new HUD.File();
                        bgcol.AddTextLink(BGWhite, (int)Link.BGWhite);
                        bgcol.AddTextLink(BGBlue, (int)Link.BGBlue);
                        bgcol.AddTextLink(BGBlack, (int)Link.BGBlack);
                        menu.File = bgcol;
                        break;
                    case Link.BGWhite:
                        Ref.draw.ClrColor = Color.White;
                        break;
                    case Link.BGBlue:
                        Ref.draw.ClrColor = Color.CornflowerBlue;
                        break;
                    case Link.BGBlack:
                        Ref.draw.ClrColor = Color.Black;
                        break;
                    case Link.LinkMoveEverything:
                        MoveAllMenu();
                        break;
                    case Link.MoveX:
                        moveAll(IntVector3.PlusX);
                        break;
                    case Link.MoveNX:
                        moveAll(IntVector3.NegativeX);
                        break;
                    case Link.MoveY:
                        moveAll(IntVector3.PlusY);
                        break;
                    case Link.MoveNY:
                        moveAll(IntVector3.NegativeY);
                        break;
                    case Link.MoveZ:
                        moveAll(IntVector3.PlusZ);
                        break;
                    case Link.MoveNZ:
                        moveAll(IntVector3.NegativeZ);
                        break;
                    case Link.SelCopy:
                        copySelectedVoxels(false);
                        break;
                    case Link.SelCut:
                        copySelectedVoxels(true);
                        openHeadMenu(false);
                        break;
                    case Link.SelMove:
                        MoveAllMenu();
                        break;
                    case Link.LinkRotateFlip:
                        mFile = roatateFlipMenu();
                        allFramesChkBox();
                        menu.File = mFile;
                        break;

                    case Link.LinkBackToSelectionMenu:
                        selectionMenu();
                        break;
                    case Link.Paste:
                        Paste();
                        break;
                    case Link.SelStamp:
                        makeStamp(true);
                        break;
                    case Link.EXIT:
                        if (inGame)
                            parent.EndCreationMode();
                        else
                        {
                            if (Ref.gamestate.previousGameState == null)
                                new MainMenuState(true);
                            else
                                Engine.StateHandler.PopGamestate();
                        }
                        break;
                    case Link.LoadTemplateMessage:
                        try
                        {
                            string templateMessageString = DataLib.SaveLoad.LoadTextFile("template_message.txt")[0];
                            voxelGridToSelection(new VoxelObjGridData(templateMessageString));
                        }
                        catch (Exception e)
                        {
                            Debug.LogError( "template_message.txt is missing");
                        }
                        break;
                }
            }
        }



        void rotateHeader(bool clockwise)
        {
            if (repeateOnAllFrames)
            {
                for (int i = 0; i < animationFrames.Frames.Count; i++)
                {
                    Rotate(clockwise);
                    nextFrame(true);
                }
            }
            else
            {
                Rotate(clockwise);
            }
        }

        void allFramesChkBox()
        {
            if (!inGame)
                mFile.Checkbox2("All frames", "Make the same action on all frames", (int)ValueLink.RepeateOnAllFrames);
        }

        static public string UserMadeModelNameFromIndex(int index)
        {
            List<string> files = DataLib.SaveLoad.FilesInStorageDir(UserVoxelObjFolder, searchPattern(false));
            return files[index];
        }

        static string searchPattern(bool save)
        {
            bool bytearray;
            if (save)
                bytearray = SaveByteArray;
            else
                bytearray = LoadByteArray;
            return "*" + (bytearray ? Data.Images.VoxelObjByteArrayEnding : TextLib.TextFileEnding);
        }
        VoxelObjGridData SelectionToGrid()
        {
            VoxelObjListData clone = selectedVoxels.Clone();
            clone.Move(designerInterface.selectionArea.Min * -1, drawLimits);

            IntVector3 gridSize = designerInterface.selectionArea.Add;
            VoxelObjGridData grid = new VoxelObjGridData(gridSize, clone.Voxels);
            return grid;
        }

        void voxelGridToSelection(VoxelObjGridData grid)
        {
            selectedVoxels.Voxels = grid.GetVoxelArray();

            selectedVoxels.Move(designerInterface.drawCoord, drawLimits);
            startUpdateVoxelObj(true);
            openHeadMenu(false);

            designerInterface.selectionArea = new RangeIntV3(designerInterface.drawCoord, designerInterface.drawCoord + grid.Limits);
            templateSent = false;
        }

        void listTemplates()
        {
            currentCategory = SaveCategory.non;
            HUD.File file = new HUD.File();

            if (hasChatergory[(int)SaveCategory.non])
                listFilesInCategory(file, SaveCategory.non);

            listTemplatesBase(file);

            new Process.SynchOpenMenuFile(file, menu);
        }
        void loadingMenu()
        {
            HUD.File file = new HUD.File();
            file.AddDescription("Loading...");
            menu.File = file;
        }
        void listTemplatesBase(HUD.File file)
        {
            for (int i = 1; i < (int)SaveCategory.NUM; i++)
            {
                if (hasChatergory[i])//DataLib.SaveLoad.FolderExistAndHaveFilesInit(categoryDir(i)))
                {

                    SpriteName id = CategoryInfo((SaveCategory)i);
                    file.AddIcon(id, Color.White, new HUD.Link((int)Link.OpenCategory_dialogue, i));//i, (int)Link.OpenCategory_dialogue);
                }
            }
            file.AddDescription("Premade templates");
            file.AddTextLink("Thrallords", (int)Link.ImportThrallords);
            //    file.AddTextLink("Race tracks", (int)Link.ImportRaceTracks);
            file.AddTextLink("Lootfest", (int)Link.LinkImportInGameObj);
        }

        void loadTemplateFile(DataStream.FilePath path)
        {
            templateSent = false;
            new LoadTemplateFile(path, this, designerInterface.drawCoord, drawLimits);
            openHeadMenu(false);
        }
        void clearSelectedArea()
        {
            RangeIntV3 area = designerInterface.selectionArea;
            dropSelection(false);
            drawInArea(FillType.Delete, DrawTool.Rectangle, area);
        }
        void clearSelectedArea_AllFrames()
        {
            for (int i = 0; i < animationFrames.Frames.Count; i++)
            {
                nextFrame(true);
                drawInArea(FillType.Delete, DrawTool.Rectangle, designerInterface.selectionArea);
            }
            clearSelectedArea();
        }
        void clearSelectedArea_AllFramesButThis()
        {
            RangeIntV3 area = designerInterface.selectionArea;
            dropSelection(false);
            int protectedFrame = currentFrame.Value;
            for (int i = 0; i < animationFrames.Frames.Count - 1; i++)
            {
                nextFrame(true);
                if (currentFrame.Value != protectedFrame)
                    drawInArea(FillType.Delete, DrawTool.Rectangle, area);
            }
        }
        public void InsertLoadedTemplate(VoxelObjListData selectedVoxels, IntVector3 gridSize, IntVector3 move)
        {
            this.selectedVoxels = selectedVoxels;
            designerInterface.selectionArea = new RangeIntV3(move, move + gridSize);
            startUpdateVoxelObj(true);

        }

        public static HUD.File listUseMadeModels(bool deleteOption, int link)
        {
            HUD.File listFiles = new HUD.File();
            if (deleteOption)
                listFiles.AddDescription("A:Load, X:Delete");
            List<string> files2 = DataLib.SaveLoad.FilesInStorageDir(UserVoxelObjFolder, searchPattern(false));

            for (int i = 0; i < files2.Count; i++)
            {
                listFiles.AddTextLink(files2[i], new HUD.Link(link, i));//i, dialogue);
            }
            return listFiles;
            //menu.File = listFiles;
        }



        public static string CustomObjectFromIndex(int index)
        {
            List<string> files = DataLib.SaveLoad.FilesInStorageDir(UserVoxelObjFolder, searchPattern(false));
            if (index < files.Count)
                return files[index];
            else
            {
                Debug.LogError( "CustomObjectFromIndex, index outside files length");
                return null;
            }


        }

        protected override int designMoveSpeedOption
        {
            get
            {
                if (settings == null)
                    return 1;
                return settings.DesignMoveSpeedOption;
                //if (inGame)
                //    return settings.
                //return base.designMoveSpeedOption;
            }
            set
            {
                settings.DesignMoveSpeedOption = value;
            }
        }

        protected override void makeStamp(bool threaded)
        {

            if (inGame)
            {
                if (HaveSelection)
                {
                    Music.SoundManager.PlayFlatSound(LoadedSound.block_place_1);
                    storeUndoableAction(UndoType.Paste);
                    foreach (Voxel v in selectedVoxels.Voxels)
                    {
                        LfRef.chunks.Set(worldPos.GetNeighborPos(v.Position), v.Material);
                    }
                    startUpdateVoxelObj(false);
                    NetworkWriteTemplate();
                }
            }
            else
            {
                base.makeStamp(threaded);
            }

        }
        List<VoxelObjName> loadableInGameObjects()
        {
            List<VoxelObjName> loadable = new List<VoxelObjName>
            {
                VoxelObjName.Apple,
                VoxelObjName.ApplePie,
                VoxelObjName.barrelX,
                VoxelObjName.bee,
                VoxelObjName.Character,
                VoxelObjName.chest_open,
                VoxelObjName.Coin,
                VoxelObjName.cook,
                VoxelObjName.crockodile1,
                VoxelObjName.ent,
                VoxelObjName.father,
                VoxelObjName.fire_goblin,
                VoxelObjName.frog1,
                VoxelObjName.ghost,
                VoxelObjName.granpa2,
                VoxelObjName.grunt,
                VoxelObjName.harpy,
                VoxelObjName.hog_lvl1,
                VoxelObjName.lizard1,
                VoxelObjName.Lumberjack,
                VoxelObjName.magician,
                VoxelObjName.mommy,
                VoxelObjName.orc_sword1,
                VoxelObjName.Pig,
                VoxelObjName.priest,
                VoxelObjName.scorpion1,
                VoxelObjName.sheep,
                VoxelObjName.spider1,
                VoxelObjName.squig_lvl1,
                //VoxelObjName.Statue,
                VoxelObjName.TM_Lumberjack,
                VoxelObjName.TM_shop,
                VoxelObjName.TM_spider,
                VoxelObjName.TM_Zeus,
                VoxelObjName.war_veteran,
                VoxelObjName.white_hen,
                VoxelObjName.wolf_lvl1,
                VoxelObjName.zombie1,
            };


            return loadable;
        }
        void listFilesInCategory(HUD.File file, SaveCategory c)
        {
            string folder = TemplateCategoryFolder + ((int)c).ToString();
            List<string> files = DataLib.SaveLoad.FilesInStorageDir(folder);
            for (int i = files.Count - 1; i >= 0; i--)
            {
                file.Add(new TemplateIconData(
                    TemplateCategoryFolder + ((int)currentCategory).ToString() + TextLib.Dir + files[i] + TemplateFileEnd,
                    new HUD.Link((int)Link.SelectTemplateFile_dialogue, i), true));
            }
        }

        const string ThrallordPath = LootfestLib.DataFolder + "Thrallords";
        const string RaceTrackPath = LootfestLib.DataFolder + "Data\\RaceTracks";
        void listPremadeTemplates(HUD.File file, string folder, Link dialogue)
        {
            List<string> files = DataLib.SaveLoad.FilesInContentDir(folder);
            for (int i = files.Count - 1; i >= 0; i--)
            {
                file.Add(new TemplateIconData(
                    Engine.LoadContent.Content.RootDirectory + TextLib.Dir + folder + TextLib.Dir + files[i] + TemplateFileEnd,
                    new HUD.Link((int)dialogue, i), false));
            }
        }

        void MoveAllMenu()
        {
            mFile = new HUD.File();
            mFile.AddTextLink(MoveX, (int)Link.MoveX);
            mFile.AddTextLink(MoveNX, (int)Link.MoveNX);
            mFile.AddTextLink(MoveZ, (int)Link.MoveZ);
            mFile.AddTextLink(MoveNZ, (int)Link.MoveNZ);
            mFile.AddTextLink(MoveY, (int)Link.MoveY);
            mFile.AddTextLink(MoveNY, (int)Link.MoveNY);
            if (HaveSelection)
            { mFile.AddTextLink("Back", (int)Link.LinkBackToSelectionMenu); }
            allFramesChkBox();

            menu.File = mFile;
        }


        protected override void pickColor()
        {
            base.pickColor();
            if (drawCoordMaterial != 0)
            {
                settings.Material = drawCoordMaterial;
                if (inGame)
                {
                    parent.Print("Picked: " + ((Data.MaterialType)settings.Material).ToString());
                }
            }
        }

        override protected byte Get(IntVector3 pos)
        {
            if (inGame)
            {
                Map.WorldPosition wp = Map.WorldPosition.EmptyPos;
                wp = worldPos.GetNeighborPos(pos);
                return LfRef.chunks.Get(wp);
            }
            else
            {
                return voxels.Get(pos);
            }
        }

        protected override void UpdatePencilInfo()
        {
            Map.WorldPosition wp = Map.WorldPosition.EmptyPos;
            drawCoordMaterial = Get(designerInterface.drawCoord);

            infoText.TextString = "X" + designerInterface.drawCoord.X.ToString() + " Y" + designerInterface.drawCoord.Y.ToString() + " Z" + designerInterface.drawCoord.Z.ToString();


            if (HaveSelection || pencilKeyDown)
            {
                IntVector3 size = designerInterface.selectionArea.Add + 1;
                infoText.TextString += " W" + size.X.ToString() + " H" + size.Y.ToString() + " L" + size.Z.ToString();

            }
            if (drawCoordMaterial != 0)
            {
                infoText.TextString += " " + ((Data.MaterialType)drawCoordMaterial).ToString();
            }
            //base.UpdatePencilInfo();
            if (inGame)
            {
                wp = worldPos;
                wp.WorldGrindex += designerInterface.drawCoord;

                crossHair.Position = parent.localPData.view.From3DToScreenPos(designerInterface.freePencil.Position);


                designerInterface.pencilSelection.Color = EmptySelection;

                if (drawCoordMaterial != 0)
                {
                    designerInterface.pencilSelection.Color = InsideSelection;
                }
                else if (checkSelectionContact(wp))
                {
                    designerInterface.pencilSelection.Color = ContactSelection;
                }
                else
                {
                    designerInterface.pencilSelection.Color = EmptySelection;
                }


                //set shadow

                bool gotYpos = false;

                wp.WorldGrindex.Y -= 1;

                if (drawCoordMaterial == 0)
                {
                    while (wp.WorldGrindex.Y > 0)
                    {
                        wp.WorldGrindex.Y -= 1;
                        if (LfRef.chunks.Get(wp) != 0)
                        {
                            gotYpos = true;
                            break;
                        }
                    }
                }
                if (!gotYpos)
                {
                    wp.Y = designerInterface.drawCoord.Y;
                    while (wp.WorldGrindex.Y < Map.WorldPosition.MaxChunkY)
                    {
                        if (LfRef.chunks.Get(wp) == 0)
                        {
                            wp.WorldGrindex.Y -= 1;
                            break;
                        }
                        wp.Y += 1;

                    }
                }

                designerInterface.pencilSelectionShadow.Y = wp.WorldGrindex.Y + 0.55f;

            }
            else
            {
                base.UpdatePencilInfo();
            }

        }

        bool checkSelectionContact(Map.WorldPosition wp)
        {
            for (Dimensions d = Dimensions.X; d <= Dimensions.Z; d++)
            {

                for (int dir = 0; dir < 2; dir++)
                {
                    IntVector3 pos = IntVector3.Zero;
                    pos.Set(d, lib.BoolToDirection(dir == 0));
                    if (LfRef.chunks.GetSafe(wp.GetNeighborPos(pos)) != 0)
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        static readonly List<char> Allowed = new List<char>
            {
                '0','1','2','3','4','5','6','7','8','9',
                SaveData.Dimension,
            };
        void loadCreation(string path)
        {
            List<string> lines = DataLib.SaveLoad.LoadTextFile(path);
            drawLimits.Max = lib.StringToIntV3(lines[0]);
            lines.Remove(lines[0]);
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length < 5)
                {
                    lines.Remove(lines[i]);
                    i--;
                }
                else
                {

                    for (int c = 0; c < lines[i].Length; c++)
                    {
                        if (!Allowed.Contains(lines[i][c]))
                        {
                            lines[i] = lines[i].Remove(c, 1);
                            c--;
                        }
                    }
                }
            }

            loadVoxels(Editor.VoxelObjDataLoader.LoadObjFromFile(lines, drawLimits.Max, SaveVersion.Ver2));
            updateVoxelObj();
            UpdateDrawLimits();
        }
        void loadVoxels(List<VoxelObjGridData> frames)
        {
            //if (combineLoading)
            //{
            //    animationFrames.Merge(new VoxelObjGridDataAnim(frames));
            //}
            //else
            //{
            //    animationFrames.Frames = frames;
            //    currentFrame = new CirkleCounter(frames.Count - 1);
            //    drawLimits.Max = frames[0].Limits;
            //    UpdateDrawLimits();
            //}
            CreatorImageLoaded(new VoxelObjGridDataAnim(frames), combineLoading);
        }

        public void CreatorImageLoaded(VoxelObjGridDataAnim creatorModel)
        {
            this.CreatorImageLoaded(creatorModel, combineLoading);
        }

        public void CreatorImageLoaded(VoxelObjGridDataAnim loadedModel
            , bool combineLoading)
        {
            if (combineLoading)
            {
                const string FramesTxt = "Frames: ";
                const string SizeTxt = "Size: ";

                //view merge options
                mFile = new HUD.File();
                mFile.AddTitle("Merge options");
                mFile.AddDescription("Current");
                mFile.AddDescription(FramesTxt + animationFrames.Frames.Count.ToString());
                mFile.AddDescription(SizeTxt + animationFrames.Size.ToString());
                mFile.AddDescription("Loaded model");
                mFile.AddDescription(FramesTxt + loadedModel.Frames.Count.ToString());
                mFile.AddDescription(SizeTxt + loadedModel.Size.ToString());
                mFile.Checkbox2("Override", "The new blocks will replace the old", (int)ValueLink.bMergeNewOverride);
                mFile.Checkbox2("Keep size", "keep the current grid size", (int)ValueLink.bMergeKeepSize);

                mFile.AddTextLink(MergeFramesOptions.NewFirstOnOldFrames.ToString(), "Take the first frame from the loaded model and stamp on all",
                    new HUD.Generic2ArgLink<MergeFramesOptions, VoxelObjGridDataAnim>(selectMergeOption, MergeFramesOptions.NewFirstOnOldFrames, loadedModel));

                mFile.AddTextLink(MergeFramesOptions.FrameByFrame.ToString(), "Merged frame by frame",
                    new HUD.Generic2ArgLink<MergeFramesOptions, VoxelObjGridDataAnim>(selectMergeOption, MergeFramesOptions.FrameByFrame, loadedModel));

                mFile.AddTextLink(MergeFramesOptions.OldFirstOnNewFrames.ToString(), "Only keep the first frame of current model and use on the loaded one",
                    new HUD.Generic2ArgLink<MergeFramesOptions, VoxelObjGridDataAnim>(selectMergeOption, MergeFramesOptions.OldFirstOnNewFrames, loadedModel));
                menu.File = mFile;
                //this.animationFrames.Merge(creatorModel,);
            }
            else
            {
                animationFrames = loadedModel;
                drawLimits.Max = animationFrames.Frames[0].Limits;
                EventTriggerCallBack(0);
            }
        }


        void selectMergeOption(MergeFramesOptions opt, VoxelObjGridDataAnim loadedModel)
        {
            mergeModelsOption.MergeFramesOptions = opt;
            this.animationFrames.Merge(loadedModel, mergeModelsOption);
            EventTriggerCallBack(0);
        }

        void FlipSelection(Dimensions dir)
        {
            openHeadMenu(false);
            voxels.FlipDir(dir, drawLimits);
            updateVoxelObj();

        }

        //void WriteBinaryStream(System.IO.BinaryWriter w)
        //{
        //    const byte SaveVersion = 3;
        //    w.Write(SaveVersion);
        //    w.Write((byte)animationFrames.Frames.Count);
        //    animationFrames.Frames[0].Size.WriteByteStream(w);
        //    for (int frame = 0; frame < animationFrames.Frames.Count; frame++)
        //    {
        //        Voxels.VoxelLib.WriteGrid(animationFrames.Frames[frame].MaterialGrid, w);
        //    }
        //}
        //void ReadBinaryStream(System.IO.BinaryReader r)
        //{
        //    byte saveVersion = r.ReadByte();
        //    if (saveVersion <= 2)
        //    {
        //        //old version, backwards working
        //        r.BaseStream.Position--;
        //        //byte[] dataArray = new byte[(int)(r.BaseStream.Length - r.BaseStream.Position)];
        //        byte[] dataArray = r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position));
        //        ByteArraySaveData = dataArray;
        //    }
        //    else
        //    {
        //        int numFrames = r.ReadByte();
        //        IntVector3 size = IntVector3.Zero;
        //        size.ReadByteStream(r);
        //        animationFrames = new VoxelObjGridDataAnim(new List<VoxelObjGridData>(numFrames));
        //        for (int frame = 0; frame < numFrames; frame++)
        //        {
        //            animationFrames.Frames.Add(new VoxelObjGridData(
        //                Voxels.VoxelLib.ReadByteGrid(r, size)));
        //        }
        //    }
        //}

        //public byte[] ByteArraySaveData 
        //{
        //    get
        //    {
        //        const byte SaveVersion = 2;
        //        List<byte> data = new List<byte> { SaveVersion, (byte)animationFrames.Frames.Count, 
        //            (byte)drawLimits.Max.X, (byte)drawLimits.Max.Y, (byte)drawLimits.Max.Z};

        //        for (int frame = 0; frame < animationFrames.Frames.Count; frame++)
        //        {
        //            //VoxelObjGridData grid = new VoxelObjGridData(drawLimits.Max, animationFrames[frame].Voxels);
        //            data.AddRange(animationFrames.Frames[frame].ToCompressedData());
        //        }
        //        return data.ToArray();
        //    }

        //    set
        //    {
        //        if (value.Length < 5)
        //        {
        //            //Corrupt file
        //            Engine.XGuide.Message("File Error", "The save file is corrupt, please remove it", Engine.XGuide.LastLocalHost, 
        //                Microsoft.Xna.Framework.GamerServices.MessageBoxIcon.Error);
        //            return;
        //        }

        //        List<VoxelObjGridData> old = animationFrames.Frames;

        //        animationFrames.Frames = new List<VoxelObjGridData>();//.Clear();
        //        byte version = value[0];
        //        byte numFrames = value[1];
        //        IntVector3 limits = new IntVector3(value[2], value[3], value[4]);


        //        List<byte> data = new List<byte>();
        //        data.AddRange(value);
        //        data.RemoveRange(0, 5);

        //        for (int frame = 0; frame < numFrames; frame++)
        //        {
        //            VoxelObjGridData grid = new VoxelObjGridData(limits);
        //            int pos = grid.FromCompressedData(data);
        //            animationFrames.Frames.Add(grid);
        //            data.RemoveRange(0, pos);
        //        }

        //        currentFrame = new CirkleCounter(numFrames - 1);
        //        drawLimits.Max = limits;

        //        if (combineLoading)
        //        {
        //            if (animationFrames.Frames.Count > old.Count)
        //            {
        //                foreach (VoxelObjGridData frame in animationFrames.Frames)
        //                {
        //                    frame.AddVoxels(old[0].GetVoxelArray());
        //                }
        //            }
        //            else
        //            {
        //                foreach (VoxelObjGridData o in old)
        //                {
        //                    o.AddVoxels(animationFrames.Frames[0].GetVoxelArray());
        //                }
        //                animationFrames.Frames = old;
        //            }
        //        }

        //        //unthread the update
        //        new Timer.EventTrigger(this, 0);
        //    }

        //}
        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            drawLimits.Max = animationFrames.Frames[0].Limits;
            if (!save)
                new Timer.EventTrigger(this, 0);
        }



        public void EventTriggerCallBack(int type)
        {
            //after loading a file
            updateFrameInfo();
            updateVoxelObj();
            UpdateDrawLimits();
        }

        public static void CreateVoxelObjFolder()
        {
            DataLib.SaveLoad.CreateFolder(UserVoxelObjFolder);
        }

        void save()
        {
            print("Saving...");
            if (SaveByteArray)
            {
                CreateVoxelObjFolder();

                //new
                new DataStream.WriteBinaryIO(new DataStream.FilePath(UserVoxelObjFolder, currentObjName, Data.Images.VoxelObjByteArrayEnding, true, false),
                    animationFrames.WriteBinaryStream, null);
                //old
                //new DataLib.ByteArray(true, UserVoxelObjFolder + TextLib.Dir + currentObjName + Data.Images.VoxelObjByteArrayEnding, 
                //    this, DataLib.ThreadType.FromThreadQue);
            }
            else
            {
                byte[, ,] materialGrid;


                List<string> lines = new List<string> { lib.Vec3Text(drawLimits.Max.Vec) };
                for (int frame = 0; frame < animationFrames.Frames.Count; frame++)
                {

                    materialGrid = animationFrames.Frames[frame].MaterialGrid;
                    IntVector3 pos = IntVector3.Zero;
                    string textVal = "";
                    int lastVal = materialGrid[0, 0, 0];
                    int numReperitions = 0;

                    for (pos.Z = 0; pos.Z <= drawLimits.Max.Z; pos.Z++)
                    {
                        for (pos.Y = 0; pos.Y <= drawLimits.Max.Y; pos.Y++)
                        {
                            for (pos.X = 0; pos.X <= drawLimits.Max.X; pos.X++)
                            {
                                if (lastVal == materialGrid[pos.X, pos.Y, pos.Z])
                                {
                                    numReperitions++;
                                }
                                else
                                {
                                    textVal += lastVal.ToString() + SaveData.Dimension + numReperitions.ToString() + SaveData.Dimension;
                                    numReperitions = 1;
                                    lastVal = materialGrid[pos.X, pos.Y, pos.Z];
                                }
                            }
                        }
                    }
                    lines.Add("\"" + textVal + lastVal.ToString() + SaveData.Dimension + numReperitions.ToString() + "\",");

                }
                DataLib.SaveLoad.CreateTextFile(System.IO.Directory.GetCurrentDirectory() + "\\" +
                 UserVoxelObjFolder + "\\" + currentObjName + ".txt", lines);
            }
        }

        const string EmptySpaceText = "Empty space";
        public static void listMaterials(HUD.File file, int dialogue, bool includeEmptySpace, Players.PlayerSettings settings, int showHideMaterialNamesLink, int materialIndex)
        {
            Dictionary<MaterialCathegory, List<Data.MaterialType>> cathegories = new Dictionary<MaterialCathegory, List<Data.MaterialType>>();
            cathegories.Add(MaterialCathegory.Nature, new List<Data.MaterialType>
            {
                Data.MaterialType.grass,
                Data.MaterialType.forest,
                Data.MaterialType.stony_grass,
                Data.MaterialType.stony_forest,
                Data.MaterialType.dirt,
                Data.MaterialType.sand,
                Data.MaterialType.desert_sand,
                Data.MaterialType.burnt_ground,
                Data.MaterialType.stone,
                Data.MaterialType.cobble_stone,
                Data.MaterialType.mossy_stone,
                Data.MaterialType.sand_stone,
                
                Data.MaterialType.wood,
                Data.MaterialType.wood_growing,
                Data.MaterialType.gray_wood,
                Data.MaterialType.black_wood,
                Data.MaterialType.patterned_wood,
                Data.MaterialType.patterned_growing_wood,
                Data.MaterialType.patterned_gray_wood,
                Data.MaterialType.patterned_mossy_stone,
                Data.MaterialType.leaves,
                Data.MaterialType.cactus,
            });

            cathegories.Add(MaterialCathegory.Animal, new List<Data.MaterialType>
            {
                Data.MaterialType.bone,
                Data.MaterialType.leather,
                Data.MaterialType.skin,
                Data.MaterialType.dark_skin,
                Data.MaterialType.zombie_skin,
                Data.MaterialType.orc_skin,
                Data.MaterialType.purple_skin,
                Data.MaterialType.blonde,
                
            });
            cathegories.Add(MaterialCathegory.Metal, new List<Data.MaterialType>
            {
                Data.MaterialType.iron,
                Data.MaterialType.copper,
                Data.MaterialType.bronze,
                Data.MaterialType.gold,
                Data.MaterialType.silver,
                Data.MaterialType.mithril,
                Data.MaterialType.green_gem,
                Data.MaterialType.red_gem,
                Data.MaterialType.blue_gem,
            });

            cathegories.Add(MaterialCathegory.Fabricated, new List<Data.MaterialType>
            {
                Data.MaterialType.red_bricks,
                Data.MaterialType.gray_bricks,
                Data.MaterialType.red_stones,
                Data.MaterialType.blue_stones,
                Data.MaterialType.red_roof,
                Data.MaterialType.blue_roof,
                Data.MaterialType.straw,
                Data.MaterialType.marble,
                Data.MaterialType.sand_stone_bricks,
                Data.MaterialType.patterned_marble, 
                Data.MaterialType.patterned_sand_stone,
                Data.MaterialType.patterned_stone,
                Data.MaterialType.runes,
                Data.MaterialType.red_carpet,
                Data.MaterialType.blue_carpet,
                Data.MaterialType.brown_carpet,
                Data.MaterialType.green_carpet,
            });
            cathegories.Add(MaterialCathegory.Color, new List<Data.MaterialType>
            {
                Data.MaterialType.black,
                Data.MaterialType.dark_gray,
                Data.MaterialType.blue_gray,
                Data.MaterialType.light_gray,
                Data.MaterialType.gray,
                Data.MaterialType.white,

                Data.MaterialType.light_brown,
                Data.MaterialType.brown,
                Data.MaterialType.red_brown,
                Data.MaterialType.mossy_green,
                Data.MaterialType.green,
                Data.MaterialType.dark_green,
                Data.MaterialType.yellow_green,

                Data.MaterialType.pink,
                Data.MaterialType.light_red,
                Data.MaterialType.red,
                Data.MaterialType.red_orange,
                Data.MaterialType.orange,
                Data.MaterialType.yellow,

                Data.MaterialType.magenta,
                Data.MaterialType.violet,
                Data.MaterialType.dark_blue,
                Data.MaterialType.blue,
                Data.MaterialType.cyan,  
            });

            cathegories.Add(MaterialCathegory.Flat, new List<Data.MaterialType>
            {
                Data.MaterialType.flat_black,
                Data.MaterialType.flat_dark_gray,
                Data.MaterialType.flat_gray,
                Data.MaterialType.flat_white,

                Data.MaterialType.flat_skin,
                Data.MaterialType.flat_sand,
                
                Data.MaterialType.flat_light_brown,
                Data.MaterialType.flat_brown,
                Data.MaterialType.flat_dark_brown,

                Data.MaterialType.flat_green,
                Data.MaterialType.flat_mossy_green,
                Data.MaterialType.flat_light_green,
                
                Data.MaterialType.flat_pink,
                Data.MaterialType.flat_red,
                Data.MaterialType.flat_red_orange,
                Data.MaterialType.flat_orange,
                Data.MaterialType.flat_yellow,

                Data.MaterialType.flat_purple,
                Data.MaterialType.flat_magenta,
                Data.MaterialType.flat_blue,
                Data.MaterialType.flat_dark_blue,
                Data.MaterialType.flat_sky_blue,
                Data.MaterialType.flat_cyan, 
            });
            List<Data.MaterialType> special = new List<Data.MaterialType>
            {
                Data.MaterialType.lava,
                Data.MaterialType.water,
                Data.MaterialType.lightning,
                Data.MaterialType.empty_letter,

            };
#if CMODE
            if (settings.UnlockedGirlyPack)
            { special.Add(Data.MaterialType.pink_heart); special.Add(Data.MaterialType.flowers); }
            if (settings.UnlockedPiratePack)
                special.Add(Data.MaterialType.pirate);
            if (settings.UnlockedTotalMinerPack)
            {
                special.AddRange(new List<Data.MaterialType>
                {
                    Data.MaterialType.TM_grass,
                    Data.MaterialType.TM_dirt,
                    Data.MaterialType.TM_leaves,
                    Data.MaterialType.TM_braided_leaves,
                    Data.MaterialType.TM_wood,
                    Data.MaterialType.TM_rock,
                    Data.MaterialType.TM_TNT,
                    Data.MaterialType.TM_work_bench,
                    Data.MaterialType.TM_shop,
                    Data.MaterialType.total_invader,//69
                });
            }
#endif
            if (PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1)
            {
                special.Add(Data.MaterialType.AntiBlock);
            }
            if (PlatformSettings.DebugOptions || settings.PlaytesterOptions)
                special.Add(Data.MaterialType.Gamefarm);

            cathegories.Add(MaterialCathegory.Special, special);

            if (includeEmptySpace)
                file.AddTextLink(EmptySpaceText, new HUD.Link(dialogue, 0));//0, dialogue);

            HUD.Link l = new HUD.Link(HUD.LinkType.ActionAndExit, dialogue, 0, materialIndex, 0);
            for (MaterialCathegory c = (MaterialCathegory)0; c < MaterialCathegory.NUM; c++)
            {
                file.AddDescription(c.ToString());

                foreach (Data.MaterialType type in cathegories[c])
                {
                    l.Value2 = (int)type;
                    //if (settings.ViewMaterialName)
                    //    file.AddIconTextLink(Data.MaterialBuilder.MaterialTile(type), type.ToString(), l);//(int)type, dialogue);
                    //else
                    file.AddIcon(Data.MaterialBuilder.MaterialTile(type), Color.White, l, type.ToString());
                }
            }
            //if (!includeEmptySpace)
            //    file.AddTextLink((settings.ViewMaterialName ? "Hide " : "Show ") + "names", showHideMaterialNamesLink);//(int)Link.ShowHideMaterialNames);

        }
        void listSelectionMaterials(HUD.File file, int dialogue)
        {
            bool[] haveMaterial = selectedVoxels.ContainMaterials();

            file.AddTextLink(EmptySpaceText, new HUD.Link(dialogue, 0));
            for (Data.MaterialType type = (Data.MaterialType)1; type < Data.MaterialType.NUM; type++)
            {
                if (haveMaterial[(int)type])
                    file.AddIcon(Data.MaterialBuilder.MaterialTile(type), Color.White, new HUD.Link(dialogue, (int)type), type.ToString());
            }
        }

        //public override void MouseClick_Event(Vector2 position, MouseButton button, bool keydown)
        //{
        //    //if (button == MouseButton.Right)
        //    //{
        //    //    if (keydown)
        //    //    {
        //    //        openHeadMenu(!menu.Visible);
        //    //    }
        //    //}
        //    //else 
        //    if (menu.Visible)
        //    {
        //        menu.MouseClick_Event(position, button, keydown);   
        //    }
        //    else
        //        base.MouseClick_Event(position, button, keydown);
        //}
        //public override void MouseScroll_Event(int scroll, Vector2 position)
        //{
        //    if (menu.Visible)
        //    {
        //        menu.MouseScroll(scroll);
        //    }
        //    else
        //    {
        //        base.MouseScroll_Event(scroll, position);
        //    }

        //}



        protected override void selectionMenu()
        {
            HUD.File file = new HUD.File();

            file.AddTitle("Selection Menu");
            file.AddTextLink("Cancel", "Exit menu", (int)Link.LinkCancelSelection);
            file.AddTextLink("Replace Material", "Replave the block color in the selected area", (int)Link.LinkSwapMaterial);
            file.AddTextLink("Save as template", (int)Link.SelSaveTemplate);
            file.AddTextLink("Make Stamp", (int)Link.SelStamp);
            file.AddTextLink("Clear", "Removes all blocks in the selected area", new HUD.Link(clearSelectedArea));
            if (animationFrames.Frames.Count > 1)
            {
                file.AddTextLink("Stamp All frames", "Paste the voxels on all frames", (int)Link.StampAllFrames);
                file.AddTextLink("Clear in frames", "Remove blocks in the selected area from all frames", new HUD.Link(clearSelectedArea_AllFrames));
                file.AddTextLink("Clear other Frames", "Remove blocks from all frames except this", new HUD.Link(clearSelectedArea_AllFramesButThis));
                file.AddTextLink("Move frame", "Move the selected blocks to another frame", (int)Link.AnimMoveFrame);
            }
            file.AddTextLink(LinkRotateFlip, (int)Link.LinkRotateFlip);


            if (!inGame && PlatformSettings.RunningWindows)
                file.AddTextLink("Set limits from selection", "Will shrink the draw limit to the selected area", (int)Link.SetLimitsAfterSel);


            string tag = Engine.XGuide.GetPlayer(int.Player1).PublicName;
            if (PlatformSettings.DebugOptions || LF2.Players.GamerName.Playtester(tag) || LF2.Players.GamerName.Creator(tag))
            {
                file.AddDescription("Playtesters only");
                file.AddTextLink("Send as message", (int)Link.SelSendAsMessage);
            }

            if (PlatformSettings.ViewUnderConstructionStuff && door == null)
                file.AddTextLink(DoorPart2Text, "Create a door", (int)Link.CreateDoor1);

            menu.Visible = true;
            menu.File = file;

        }

        static SpriteName CategoryInfo(SaveCategory c)
        {
            SpriteName result = SpriteName.NO_IMAGE;
            switch (c)
            {
                case SaveCategory.castle:
                    result = SpriteName.FolderCastle;
                    break;
                case SaveCategory.squares:
                    result = SpriteName.FolderSquares;
                    break;
                case SaveCategory.house:
                    result = SpriteName.FolderHouse;
                    break;
                case SaveCategory.veihcle:
                    result = SpriteName.FolderVeihcle;
                    break;
                case SaveCategory.art:
                    result = SpriteName.FolderArt;
                    break;
                case SaveCategory.animals:
                    result = SpriteName.FolderAnimals;
                    break;
                case SaveCategory.smiley:
                    result = SpriteName.FolderSmileys;
                    break;
                case SaveCategory.space:
                    result = SpriteName.FolderSpace;
                    break;
                case SaveCategory.roadSign:
                    result = SpriteName.FolderRoadSign;
                    break;
                case SaveCategory.temporary:
                    result = SpriteName.FolderTimer;
                    break;
                case SaveCategory.terrain:
                    result = SpriteName.FolderTerrain;
                    break;
                case SaveCategory.character:
                    result = SpriteName.FolderCharacer;
                    break;
                case SaveCategory.dontKnow:
                    result = SpriteName.FolderQuestion;
                    break;
                case SaveCategory.furniture:
                    result = SpriteName.FolderFurniture;
                    break;
                case SaveCategory.tools:
                    result = SpriteName.FolderTools;
                    break;
            }
            return result;
        }

        void swapMaterials(VoxelObjListData voxelList, byte swapTo, bool updateImage)
        {
            if (swapMaterialFrom == swapTo)
                return;
            if (swapMaterialFrom == 0)
            {
                //VoxelObjGridData grid = new VoxelObjGridData(drawLimits.Max, voxelList.Voxels);
                IntVector3 pos = IntVector3.Zero;
                for (pos.Z = designerInterface.selectionArea.Min.Z; pos.Z <= designerInterface.selectionArea.Max.Z; pos.Z++)
                {
                    for (pos.Y = designerInterface.selectionArea.Min.Y; pos.Y <= designerInterface.selectionArea.Max.Y; pos.Y++)
                    {
                        for (pos.X = designerInterface.selectionArea.Min.X; pos.X <= designerInterface.selectionArea.Max.X; pos.X++)
                        {
                            if (voxelList.GetValue(pos) == 0)
                            {
                                voxelList.Voxels.Add(new Voxel(pos, swapTo));
                            }
                        }

                    }
                }
            }
            else if (swapTo == 0)
            {
                for (int i = voxelList.Voxels.Count - 1; i >= 0; i--)
                {
                    if (voxelList.Voxels[i].Material == swapMaterialFrom)
                    {
                        voxelList.Voxels.RemoveAt(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < voxelList.Voxels.Count; i++)
                {
                    if (voxelList.Voxels[i].Material == swapMaterialFrom)
                    {
                        Voxel v = voxelList.Voxels[i];
                        v.Material = swapTo;
                        voxelList.Voxels[i] = v;
                    }
                }
            }

            if (updateImage)
                startUpdateVoxelObj(HaveSelection);
        }

        void swapMaterials(VoxelObjGridData grid, byte swapTo)
        {
            if (swapMaterialFrom == swapTo)
                return;
            //if (swapMaterialFrom == 0)
            //{
            //VoxelObjGridData grid = new VoxelObjGridData(drawLimits.Max, voxelList.Voxels);
            IntVector3 pos = IntVector3.Zero;
            for (pos.Z = designerInterface.selectionArea.Min.Z; pos.Z <= designerInterface.selectionArea.Max.Z; pos.Z++)
            {
                for (pos.Y = designerInterface.selectionArea.Min.Y; pos.Y <= designerInterface.selectionArea.Max.Y; pos.Y++)
                {
                    for (pos.X = designerInterface.selectionArea.Min.X; pos.X <= designerInterface.selectionArea.Max.X; pos.X++)
                    {
                        //if (voxelList.GetValue(pos)== 0)
                        //{
                        //    voxelList.Voxels.Add(new Voxel(pos, swapTo));
                        //}
                        if (grid.Get(pos) == swapMaterialFrom)
                        {
                            grid.Set(pos, swapTo);
                        }
                    }

                }
            }
            //}
            //else if (swapTo == 0)
            //{
            //    for (int i = voxelList.Voxels.Count -1; i >= 0; i--)
            //    {
            //        if (voxelList.Voxels[i].Material == swapMaterialFrom)
            //        {
            //            voxelList.Voxels.RemoveAt(i);
            //        }
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < voxelList.Voxels.Count; i++)
            //    {
            //        if (voxelList.Voxels[i].Material == swapMaterialFrom)
            //        {
            //            Voxel v = voxelList.Voxels[i];
            //            v.Material = swapTo;
            //            voxelList.Voxels[i] = v;
            //        }
            //    }
            //}
            startUpdateVoxelObj(HaveSelection);
        }

        override protected void removeSelection()
        {
            //Merge the selected group of voxels with the original group
            if (HaveSelection)
            {
                selectedVoxels.Voxels.Clear();
                if (inGame)
                {
                    mode = Players.PlayerMode.Creation;
                    //parent.ShowControls(Players.PlayerMode.Creation);
                }
            }
            if (selectionVoxelObj != null)
            {
                selectionVoxelObj.DeleteMe();
            }
            designerInterface.selectedVolumeLine.Visible = false;
        }

        protected override void LargeSelectionWarning()
        {
            if (inGame)
            {
                parent.Print("Large selection!");
            }
        }



        protected void NetworkWriteTemplate()
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginAsynchPacket();
            //System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.VoxelEditorAddTemplate, Network.PacketRelyability.RelyableLasy, parent.Index);

            // parent.NetworkWriteIdAndScreenIx(w);
            w.Write(templateSent);
            if (!templateSent)
            {
                VoxelObjListData clone = selectedVoxels.Clone();
                clone.Move(-designerInterface.selectionArea.Min, drawLimits);
                VoxelObjGridData grid = new VoxelObjGridData(designerInterface.selectionArea.Add, clone.Voxels);
                designerInterface.selectionArea.Add.NetworkWriteByte(w);
                List<byte> array = grid.ToCompressedData();
                w.Write((short)array.Count);
                w.Write(array.ToArray());
                templateSent = true;
            }

            IntVector3 pos = worldPos.WorldGrindex;
            pos += designerInterface.selectionArea.Min;
            pos.WriteStream(w);
            //Ref.netSession.EndWritingPacket(SendDataOptions.Reliable);

            Ref.netSession.EndAsynchPacket(w, Network.PacketType.LF2_VoxelEditorAddTemplate, Network.SendPacketTo.All, 0,
                Network.PacketReliability.ReliableLasy, null);
        }
        public static Map.WPRange NetworkReadTemplate(System.IO.BinaryReader r, Players.ClientPlayer sender)
        {
            if (!r.ReadBoolean())
            {
                sender.EditorTemplateSize = IntVector3.FromByteSzStream(r);
                VoxelObjGridData grid = new VoxelObjGridData(sender.EditorTemplateSize);

                int length = r.ReadInt16();
                byte[] array = new byte[length];
                r.Read(array, 0, length);
                List<byte> list = new List<byte>();
                list.AddRange(array);
                grid.FromCompressedData(list);

                sender.EditorTemplate = new VoxelObjListData(grid.GetVoxelArray());
            }
            Map.WorldPosition worldPos = Map.WorldPosition.EmptyPos;
            worldPos.WorldGrindex = IntVector3.FromStream(r);
            //worldPos.UpdateChunkPos();
            //storeUndoableAction();
            sender.EditorStoreUndoableAction(new RangeIntV3(IntVector3.Zero, sender.EditorTemplateSize), worldPos, UndoType.Paste);
            foreach (Voxel v in sender.EditorTemplate.Voxels)
            {
                LfRef.chunks.Set(worldPos.GetNeighborPos(v.Position), v.Material);
            }

            Map.WorldPosition max = worldPos;
            max.WorldGrindex += sender.EditorTemplateSize;
            //max.UpdateChunkPos();
            return new Map.WPRange(worldPos, max);
            //UpdateMapArea(worldPos, sender.EditorTemplateSize);
        }

        protected override void dottetLineKeyUp(bool add)
        {
            if (inGame)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_VoxelEditorDottedLine,
                   Network.PacketReliability.ReliableLasy, parent.Index);
                //Tool settings
                w.Write(selectedMaterial);
                w.Write((byte)pencilSize);
                w.Write((byte)roadPercentFill);//add);
                TwoHalfByte toolAndEdge = new TwoHalfByte((byte)drawTool, (byte)roadEdge); toolAndEdge.WriteStream(w);
                TwoHalfByte clearAndFillHeight = new TwoHalfByte((byte)roadAboveClear, (byte)roadBelowFill); clearAndFillHeight.WriteStream(w);
                EightBit addAndRoundPencil = new EightBit();
                addAndRoundPencil.Set(0, add);
                addAndRoundPencil.Set(1, roundPencil);
                addAndRoundPencil.WriteStream(w);

                if (roadEdge > 0)
                {
                    w.Write(secondaryMaterial);
                }

                //start position
                designerInterface.keyDownDrawCoord.WriteByteStream(w);
                //pencil movement
                w.Write((ushort)dottedLineMovement.Count);

                foreach (SByteVector3 move in dottedLineMovement)
                {
                    move.WriteStream(w);
                }
            }
            base.dottetLineKeyUp(add);

        }

        static public Map.WPRange NetworkReadDottedLine(System.IO.BinaryReader r, Players.ClientPlayer sender)
        {
#if WINDOWS
            if (r.BaseStream.Position > 0)
                throw new Exception();
#endif

            //Tool settings
            byte material1 = r.ReadByte();
            int pencilSize = r.ReadByte();
            int percentFill = r.ReadByte();
            TwoHalfByte toolAndEdge = TwoHalfByte.FromStream(r);
            TwoHalfByte clearAndFillHeight = TwoHalfByte.FromStream(r);
            EightBit addAndRoundPencil = EightBit.FromStream(r);

            DrawTool drawTool = (DrawTool)toolAndEdge.Value1;
            int edge = toolAndEdge.Value2;
            int clearHeight = clearAndFillHeight.Value1;
            int fillHeight = clearAndFillHeight.Value2;
            bool add = addAndRoundPencil.Get(0);
            bool roundPencil = addAndRoundPencil.Get(1);

            byte material2 = byte.MinValue;
            if (toolAndEdge.Value2 > 0)
            {
                material2 = r.ReadByte();
            }

            //start position
            Map.WorldPosition wp = HeroPosToCreationStartPos(sender.BuildingPos);
            //wp.UpdateWorldGridPos();

            RangeIntV3 drawLimits = new RangeIntV3(wp.WorldGrindex, wp.WorldGrindex + CreationSizeLimit.Max);

            IntVector3 startPos = ByteVector3.FromStream(r).IntVec;
            wp.WorldGrindex += startPos;

            int numDots = r.ReadUInt16();
            List<IntVector3> dots = new List<IntVector3>(numDots);
            RangeIntV3 updateArea = new RangeIntV3(wp.WorldGrindex);
            for (int i = 0; i < numDots; i++)
            {
                wp.WorldGrindex += SByteVector3.FromStream(r).IntVec;
                if (wp.WorldGrindex.X < updateArea.Min.X)
                    updateArea.Min.X = wp.WorldGrindex.X;
                else if (wp.WorldGrindex.X > updateArea.Max.X)
                    updateArea.Max.X = wp.WorldGrindex.X;

                if (wp.WorldGrindex.Y < updateArea.Min.Y)
                    updateArea.Min.Y = wp.WorldGrindex.Y;
                else if (wp.WorldGrindex.Y > updateArea.Max.Y)
                    updateArea.Max.Y = wp.WorldGrindex.Y;

                if (wp.WorldGrindex.Z < updateArea.Min.Z)
                    updateArea.Min.Z = wp.WorldGrindex.Z;
                else if (wp.WorldGrindex.Z > updateArea.Max.Z)
                    updateArea.Max.Z = wp.WorldGrindex.Z;

                dots.Add(wp.WorldGrindex);
            }

            //Calculate the affected draw area
            int radius = pencilSize / PublicConstants.Twice;
            if (drawTool == DrawTool.Road)
            {
                radius += edge;
            }
            Map.WorldPosition min = Map.WorldPosition.EmptyPos;
            min.WorldGrindex.X = updateArea.Min.X - radius;
            min.WorldGrindex.Z = updateArea.Min.Z - radius;
            Map.WorldPosition max = Map.WorldPosition.EmptyPos;
            max.WorldGrindex.X = updateArea.Max.X + radius;
            max.WorldGrindex.Z = updateArea.Max.Z + radius;

            if (drawTool == DrawTool.Road)
            {
                min.WorldGrindex.Y = updateArea.Min.Y - fillHeight;
                max.WorldGrindex.Y = updateArea.Max.Y + clearHeight;
            }
            else
            {
                min.WorldGrindex.Y = updateArea.Min.Y - radius;
                max.WorldGrindex.Y = updateArea.Max.Y + radius;
            }
            //min.UpdateChunkPos();
            // max.UpdateChunkPos();

            //undo store
            sender.EditorStoreUndoableAction(new RangeIntV3(IntVector3.Zero, max.WorldGrindex - min.WorldGrindex), min, add ? UndoType.DrawRect : UndoType.RemoveRect);

            //Add the voxels
            foreach (IntVector3 dot in dots)
            {
                paintDotLoop(LfRef.chunks, Map.WorldPosition.EmptyPos, pencilSize, toolAndEdge.Value2, dot, drawTool,
                    add, drawLimits, material1, material2, roundPencil, percentFill, fillHeight, clearHeight);
            }

            return new Map.WPRange(min, max);
        }

        protected void NetworkWriteDrawRect(byte material, Players.AbsPlayer sender, RangeIntV3 drawArea)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginAsynchPacket();

            w.Write(material);
            w.Write((byte)drawTool);

            Map.WorldPosition wp = worldPos.GetNeighborPos(drawArea.Min);
            wp.WorldGrindex.WriteStream(w);
            drawArea.Add.NetworkWriteByte(w);

            if (drawTool != DrawTool.Rectangle && drawTool != DrawTool.Sphere)
            {
                w.Write((byte)designerInterface.toolDir);
                if (designerInterface.toolDir == Dimensions.NON)
                    designerInterface.toolDir = Dimensions.Y;
                w.Write(designerInterface.keyDownDrawCoord.GetDimension(designerInterface.toolDir) == drawArea.Min.GetDimension(designerInterface.toolDir));
            }

            Ref.netSession.EndAsynchPacket(w, Network.PacketType.LF2_VoxelEditorDrawRect, Network.SendPacketTo.All,
                0, Network.PacketReliability.ReliableLasy, null);

        }
        static public Map.WPRange NetworkReadDrawRect(System.IO.BinaryReader r, Players.ClientPlayer sender)
        {
            byte material = r.ReadByte();
            DrawTool drawTool = (DrawTool)r.ReadByte();
            Dimensions toolDir = Dimensions.NON;
            IntVector3 keyDownDrawCoord = IntVector3.Zero;

            Map.WorldPosition minScreen = Map.WorldPosition.EmptyPos;
            minScreen.WorldGrindex = IntVector3.FromStream(r);
            //minScreen.UpdateChunkPos();
            IntVector3 size = IntVector3.FromByteSzStream(r);
            size += 1;

            Map.WorldPosition maxScreen = minScreen;
            maxScreen.WorldGrindex.Add(size);
            //maxScreen.UpdateChunkPos();

            RangeIntV3 selectionarea = new RangeIntV3(IntVector3.Zero, size - 1);

            if (drawTool != DrawTool.Rectangle && drawTool != DrawTool.Sphere)
            {
                toolDir = (Dimensions)r.ReadByte();
                if (r.ReadBoolean())
                {
                    keyDownDrawCoord = selectionarea.Min;
                }
                else
                {
                    keyDownDrawCoord = selectionarea.Max;
                }
            }


            sender.EditorStoreUndoableAction(new RangeIntV3(IntVector3.Zero, size), minScreen, material == 0 ? UndoType.RemoveRect : UndoType.DrawRect);
            //updatera pos!!

            AbsVoxelDesigner.FillArea(material, FillType.Fill, toolDir, drawTool, selectionarea, keyDownDrawCoord,
                new VoxelObjListData(new List<Voxel>()), false, new NetworkDraw(minScreen));

            return new Map.WPRange(minScreen, maxScreen);
            //UpdateMapArea(minScreen, maxScreen);
        }
        public static void UpdateMapArea(Map.WorldPosition worldPos, IntVector3 selectionSize)
        {
            UpdateMapArea(worldPos, new RangeIntV3(IntVector3.Zero, selectionSize));
        }

        public static void UpdateMapArea(Map.WorldPosition worldPos, RangeIntV3 selectionArea)
        {
            Map.WorldPosition minScreen = worldPos;
            minScreen.WorldGrindex.Add(selectionArea.Min);

            Map.WorldPosition maxScreen = worldPos;
            maxScreen.WorldGrindex.Add(selectionArea.Max);

            UpdateMapArea(minScreen, maxScreen);
        }
        public static void UpdateMapArea(Map.WorldPosition minScreen, Map.WorldPosition maxScreen)
        {
            IntVector2 screenIx = IntVector2.Zero;
            for (screenIx.Y = minScreen.ChunkGrindex.Y; screenIx.Y <= maxScreen.ChunkGrindex.Y; screenIx.Y++)
            {
                for (screenIx.X = minScreen.ChunkGrindex.X; screenIx.X <= maxScreen.ChunkGrindex.X; screenIx.X++)
                {
                    LfRef.worldOverView.ChangedChunk(screenIx);

                    Map.Chunk screen = LfRef.chunks.GetScreenUnsafe(screenIx);
                    if (screen != null)
                    {
                        if (screen.Openstatus >= Map.ScreenOpenStatus.MeshGeneratedDone)
                        {
                            Map.World.ReloadChunkMesh(screenIx);
                        }
                        //else
                        //{
                        //LfRef.worldOverView.ChangedChunk(screenIx);
                        //}
                    }
                    else if (Ref.netSession.IsHost)
                    {
                        //didnt recieve the change
                        throw new Debug.HostDataLostException("UpdateMapArea");
                    }
                }
            }
        }


        protected override void updateVoxelObj(RangeIntV3 updateArea)
        {
            if (inGame)
            {
                Map.WorldPosition minScreen = worldPos;
                minScreen.WorldGrindex.Add(updateArea.Min);
                //minScreen.UpdateChunkPos();
                Map.WorldPosition maxScreen = worldPos;
                maxScreen.WorldGrindex.Add(updateArea.Max);
                //maxScreen.UpdateChunkPos();

                //updatera pos!!

                IntVector2 screenIx = IntVector2.Zero;
                for (screenIx.Y = minScreen.ChunkGrindex.Y; screenIx.Y <= maxScreen.ChunkGrindex.Y; screenIx.Y++)
                {
                    for (screenIx.X = minScreen.ChunkGrindex.X; screenIx.X <= maxScreen.ChunkGrindex.X; screenIx.X++)
                    {
                        Map.World.ReloadChunkMesh(screenIx);
                        LfRef.worldOverView.ChangedChunk(screenIx);
                    }

                }


            }
            else
            {
                base.updateVoxelObj(updateArea);
                //if (voxelObj != null)
                //{
                //    voxelObj.DeleteMe();
                //}
                // voxelObj = VoxelObjBuilder.BuildFromVoxels(drawLimits, voxels.Voxels, Vector3.Zero);
            }

        }
        //protected override void updateSelectionObj()
        //{
        //    List<Voxel> voxels = selectedVoxels.Voxels;
        //    const int MaxVoxels = 8192;
        //    if (voxels.Count > MaxVoxels)
        //    {
        //        voxels = new List<Voxel>();
        //        voxels.AddRange(selectedVoxels.Voxels);
        //        voxels.RemoveRange(MaxVoxels, selectedVoxels.Voxels.Count - MaxVoxels);
        //    }
        //    Graphics.VoxelObj obj = VoxelObjBuilder.BuildFromVoxels(drawLimits, voxels, Vector3.Zero);
        //    obj.Position = offSet;
        //    selectionVoxelObj = obj;

        //}

        protected override int pencilSize
        {
            get
            {
                return settings.PencilSize;
            }
        }
        //protected override bool roundPencil
        //{
        //    get
        //    {
        //        return settings.RoundPencil;
        //    }
        //}
        protected override bool roundPencil
        {
            get
            {
                return settings.RoundPencil;
            }
        }
        protected override int roadAboveClear { get { return settings.RoadUpwardClear; } }
        protected override int roadBelowFill { get { return settings.RoadBelowFill; } }
        protected override int roadEdge { get { return settings.RoadEdgeSize; } }
        protected override int roadPercentFill { get { return settings.RoadPercentFill; } }


        public override void DeleteMe()
        {
            //if (HaveSelection)
            //    makeStamp();
            base.DeleteMe();
            menu.DeleteMe();
            infoText.DeleteMe();
            crossHair.DeleteMe();
            clearDoor();
        }
        void clearDoor()
        {
            if (doorOutline != null)
            {
                door = null;
                doorOutline.DeleteMe();
                doorOutline = null;
            }
        }

        enum SaveCategory
        {
            non,
            dontKnow,
            temporary,
            art,
            terrain,
            house,
            castle,
            space,
            roadSign,
            animals,
            squares,
            veihcle,
            smiley,
            character,
            furniture,
            tools,
            NUM
        }
        enum MaterialCathegory
        {
            Nature,
            Animal,
            Fabricated,
            Color,
            Flat,
            Metal,
            Special,
            NUM
        }
        enum DrawLimits
        {
            W16H24,
            W24H24,
            W24H32,
            W32H32,
            W32H48,
            NUM,
        }

    }

}
