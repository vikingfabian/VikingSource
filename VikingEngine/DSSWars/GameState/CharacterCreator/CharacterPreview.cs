using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public CharacterPreview(VectorRect screenArea, CharacterPreviewType previewType) 
        {
            this.previewType = previewType;
            screenArea.Round();
            characterModelBuilder = new CharacterModelBuilder();
            var model = buildModel(out float zoom);
            target = new RenderTargetImage(screenArea.Position, screenArea.Size, ImageLayers.Background0, true);
            camera = new TopViewCamera(zoom, new Vector2(MathHelper.PiOver2 - 0.6f, MathHelper.PiOver4 + 0.3f),
                    screenArea.Size.X, screenArea.Size.Y);
            camera.FieldOfView = 20f;
            

            //camera.LookTarget = model.GridSize.Vec * 0.5f;
            camera.LookTarget = new Vector3(0, 14f, 0) + model.scale *0.5f;


            camera.instantMoveToTarget();
            camera.Time_Update(0);
            camera.RecalculateMatrices();

            target.Camera = camera;
            drawList = new List<AbsDraw> { model };

            Graphics.RectangleLines rectangle = new RectangleLines(screenArea, 2f, 1f, ImageLayers.Lay9);
        }

        AbsVoxelObj buildModel(out float zoom)
        {
            switch (previewType)
            {
                case CharacterPreviewType.Soldier:
                    zoom = 140;
                    return characterModelBuilder.buildModel(DssRef.storage.HostProfile(),
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

        public void update()
        {
            camera.Time_Update(Ref.DeltaTimeMs);
            target.DrawImagesToTarget(null, drawList, true, 0);
        }
    }

    enum CharacterPreviewType
    { 
        Soldier,
        RideAnimal,
    }
}
