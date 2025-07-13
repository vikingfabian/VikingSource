using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars.GameState.CharacterCreator
{
    class CharacterPreview
    {
        CharacterModelBuilder characterModelBuilder;
        TopViewCamera camera = null;
        RenderTargetImage target;
        List<AbsDraw> drawList;

        CharacterPreviewType previewType;
        VectorRect area;

        int flagIndex;
        int characterIndex;
        bool mouseDown = false;
        public CharacterPreview(VectorRect screenArea, CharacterPreviewType previewType) 
        {
            characterIndex = DssRef.storage.characterStorage.selected;
            this.previewType = previewType;
            screenArea.Round();
            this.area = screenArea;

            Init(screenArea.Size, true);
            target.position = screenArea.Position;
            Graphics.RectangleLines rectangle = new RectangleLines(screenArea, 2f, 1f, ImageLayers.Lay9);
        }

        public CharacterPreview(int characterIndex, int flagIndex, Vector2 size)
        {
            this.characterIndex = characterIndex;
            this.previewType = CharacterPreviewType.Soldier;
            size.Round();
            Init(size, false);
            target.ClearColor = Color.Black;
            target.ClearColor.A =10;
            camera.CurrentZoom *= 0.4f;
        }

        public void Init(Vector2 size, bool toRender)
        {
            characterModelBuilder = new CharacterModelBuilder();
            var model = buildModel(out float zoom);
            target = new RenderTargetImage(Vector2.Zero, size, ImageLayers.Background0, toRender);
            camera = new TopViewCamera(zoom, new Vector2(MathHelper.PiOver2 - 0.6f, MathHelper.PiOver4 + 0.3f),
                    size.X, size.Y);
            camera.FieldOfView = 20f;
            camera.FarPlane = 400;
            camera.NearPlane = 0.01f;

            camera.LookTarget = new Vector3(0, 14f, 0) + model.scale * 0.5f;

            camera.instantMoveToTarget();
            camera.Time_Update(0);
            camera.RecalculateMatrices();

            target.Camera = camera;
            drawList = new List<AbsDraw> { model };
        }

        AbsVoxelObj buildModel(out float zoom)
        {
            switch (previewType)
            {
                case CharacterPreviewType.Soldier:
                    zoom = 140;
                    return characterModelBuilder.buildModel(new Players.Profile.PlayerProfile(characterIndex, flagIndex),
                    new SoldierModelData(ArmorLevel.None, Resource.ItemResourceType.Sword, Conscript.SpecializationType.None, VisualExperience.Experienced, 0, 0));

                case CharacterPreviewType.RideAnimal:
                    zoom = 80;
                    return  new Graphics.VoxelModelInstance( DssRef.models.voxelModels[LootFest.VoxelModelName.horse_brown], false);
            }

            zoom = 0;
            return null;
        }

        public void refresh()
        {
            var model = buildModel(out float zoom);
            drawList = new List<AbsDraw> { model };
        }

        public void rotationUpdate()
        {
            camera.TiltX += 1f * Ref.DeltaGameTimeSec;
            camera.RecalculateMatrices();
            drawUpdate();
        }

        public void update()
        {
            drawUpdate();

            if (mouseDown)
            {
                camera.TiltX += Input.Mouse.MoveDistance.X * 0.01f;
                camera.RecalculateMatrices();

                if (!Input.Mouse.IsButtonDown(MouseButton.Left))
                {
                    mouseDown = false;
                }
            }
            else if (Input.Mouse.ButtonDownEvent(MouseButton.Left) && 
                area.IntersectPoint(Input.Mouse.Position))
            {
                mouseDown = true;
            }
        }

        void drawUpdate()
        {
            camera.Time_Update(Ref.DeltaTimeMs);
            target.DrawImagesToTarget(null, drawList, true, 0);
        }

        public Texture2D Texture()
        { 
            return target.renderTarget;
        }
    }

    enum CharacterPreviewType
    { 
        Soldier,
        RideAnimal,
    }
}
