using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG
{
    class UnitModel : Graphics.Point3D
    {
        const LoadedTexture Texture = LoadedTexture.SpriteSheet;
        static readonly CustomEffect ShadowEffect = new CustomEffect("Shadow", false);

        const float ShadowHeight = 0.45f;
        const float ShadowXTilt = 0.25f;
        const byte ShadowBottomAlfa = 128;
        const byte ShadowTopAlfa = 20;


        const float SoldierPlacementsRndOffsetX = 0.03f;
        const float SoldierPlacementsRndOffsetZ = 0.02f;

        const float MaxPlaceRange = 0.3f;
        const float PlaceRangeCenterZ = 0.18f;
        static readonly Vector3[] SoldierPlacements = new Vector3[]
        {
            new Vector3(-0.5f * MaxPlaceRange, 0f, 0.8f * MaxPlaceRange + PlaceRangeCenterZ),
            new Vector3(0.5f * MaxPlaceRange, 0f, -0.8f * MaxPlaceRange + PlaceRangeCenterZ),
            new Vector3(-0.7f * MaxPlaceRange, 0f, -0.4f * MaxPlaceRange + PlaceRangeCenterZ),
            new Vector3(0.7f * MaxPlaceRange, 0f, 0.4f * MaxPlaceRange + PlaceRangeCenterZ),
            new Vector3(0f, 0f, -0.1f * MaxPlaceRange + PlaceRangeCenterZ),
        };
        static readonly IntervalF DepthToBrightnessRange = new IntervalF(-0.3f, 0.1f);

        public Graphics.Mesh restIcon;
        public Graphics.VoxelModel model, shadow;
        SoldierModelSetup[] soldiers;
        PolygonColor shield;
        PolygonColor[] levelup = null;

        Graphics.ImageGroupParent3D childModels;
        float offsetZ;

        public UnitModel()
        { }

        public void itemSetup(SpriteName image, Vector2 size, float shadowOffset, bool invertX)
        {
            setup(image, size, Vector3.Zero, shadowOffset, 1, invertX, Color.Black);
        }

        public void standardSetup(ToggEngine.Data.UnitModelSettings modelSettings, int soldierCount, PlayerRelationVisuals relation)
        {
            var sprite = modelSettings.Sprite;
                       
            var sz = DataLib.SpriteCollection.RatioFromLargestSide(sprite) * modelSettings.modelScale;
            bool invX = relation.faceRight != modelSettings.facingRight;
            
            setup(sprite, sz, modelSettings.centerOffset, modelSettings.shadowOffset, 
                soldierCount, invX, relation.shadowColor);
        }

        void setup(SpriteName image, Vector2 scale, Vector3 centerOffset, float shadowOffset, int soldierCount, bool invertX, Color shadowCol)
        {
            offsetZ = centerOffset.Z;
            soldiers = new SoldierModelSetup[soldierCount];
            Sprite imageFile = DataLib.SpriteCollection.Get(image);

            if (invertX)
            {
                imageFile.FlipX();
            }

            for (int i = 0; i < soldierCount; ++i)
            {
                SoldierModelSetup soldier = new SoldierModelSetup();

                Vector3 place = Vector3.Zero;
                
                float brightness = 1f;

                if (soldierCount > 1)
                {
                    place = SoldierPlacements[i];
                    if (place.Z < DepthToBrightnessRange.Max)
                    {
                        brightness -= 0.5f * (1f - DepthToBrightnessRange.GetValuePercentPos(place.Z));
                    }
                    if (invertX)
                    {
                        place.X = -place.X;
                    }
                }
                else
                {
                    place.Z = 0.3f;
                }

                place += centerOffset;

                place.X += Ref.rnd.Plus_MinusF(SoldierPlacementsRndOffsetX);
                place.Y = ToggEngine.Map.SquareModelLib.TerrainY_UnitShadow;
                place.Z += Ref.rnd.Plus_MinusF(SoldierPlacementsRndOffsetZ);
                
                //Figure Poly
                soldier.figure = toggLib.CamFacingPolygon(place, scale, imageFile, ColorExt.GrayScale(brightness));


                {//Shadow Poly
                    place.X -= 0.02f;
                    place.Z += shadowOffset * scale.Y;
                    place.Z -= 0.04f;

                    Vector3 sw = place;
                    sw.X -= scale.X * 0.5f;
                    Vector3 se = sw;
                    se.X += scale.X;

                    Vector3 nw = sw;
                    nw.Z -= scale.Y * ShadowHeight;
                    nw.X += scale.Y * ShadowXTilt;

                    Vector3 ne = nw;
                    ne.X += scale.X;

                    soldier.shadow = new Graphics.PolygonColor(nw, ne, sw, se,
                         imageFile, shadowCol);

                    soldier.shadow.V0sw.Color.A = ShadowBottomAlfa;
                    soldier.shadow.V2se.Color.A = ShadowBottomAlfa;
                    soldier.shadow.V1nw.Color.A = ShadowTopAlfa;
                    soldier.shadow.V3ne.Color.A = ShadowTopAlfa;

                }

                soldiers[i] = soldier;
            }

            refreshModel(soldierCount);
        }
        
        void createShield(SpriteName image, bool invertX)
        {
            const float ShieldScale = 0.3f;

            shield = toggLib.CamFacingPolygon(
                new Vector3(0.31f * lib.BoolToLeftRight(invertX), 0.1f, 0.5f),
                new Vector2(ShieldScale),
                DataLib.SpriteCollection.Get(image),
                Color.White);
        }

        void createLevelStars(int level, bool invertX)
        {
            if (level > 0)
            {
                const float Scale = 0.2f;

                float y = 0.1f;

                if (shield.hasValue())
                {
                    y += 0.25f;
                }

                levelup = new PolygonColor[level];

                for (int i = 0; i < level; ++i)
                {
                    var poly = toggLib.CamFacingPolygon(
                        new Vector3(0.31f * lib.BoolToLeftRight(invertX), y, 0.5f),
                        new Vector2(Scale),
                        DataLib.SpriteCollection.Get(SpriteName.winnerParticle),
                        Color.White);
                    levelup[i] = poly;

                    y += Scale;
                }
            }
        }        

        public void createRestIcon(Color col)
        {
            if (restIcon == null)
            {
                restIcon = new Graphics.Mesh(LoadedMesh.plane, new Vector3(0f, 0.35f, 0.3f), new Vector3(0.2f),
                    Graphics.TextureEffectType.Flat, SpriteName.cmdIconTimeOut, col);

                restIcon.Visible = false;
                restIcon.Rotation = toggLib.PlaneTowardsCam;

                getOrCreateChildModels().AddAndUpdate(restIcon);
            }
        }

        public bool RestVisibleGet()
        {
           return restIcon != null && restIcon.Visible;
        }
        public void RestVisibleSet(bool value, PlayerRelationVisuals relation)
        {
            if (value)
            {
                createRestIcon(relation.restIconColor);
            }
            else if (restIcon == null)
            {
                return;
            }

            restIcon.Visible = value;
        }

        public void refreshModel(int soldierCount, int frame = 0)
        {
            Vector3 storedPos = Vector3.Zero;
            if (model != null)
            {
                storedPos = model.position;
                deleteSoldierModels();
            }

            List<PolygonColor> polygons = new List<PolygonColor>(soldierCount * 2 + 2);
            List<PolygonColor> shadowPolygons = new List<PolygonColor>(soldierCount * 2);
            
            for (int i = 0; i < soldierCount; ++i)
            {
                addWithZorder(soldiers[i].figure, polygons);
                addWithZorder(soldiers[i].shadow, shadowPolygons);
            }

            if (shield.hasValue())
            {
                polygons.Add(shield);
            }
            if (levelup != null)
            {
                polygons.AddRange(levelup);
            }

            shadow = new Graphics.VoxelModel(true);
            shadow.Effect = Ref.draw.getEffect(TextureEffectType.Shadow);
            shadow.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(shadowPolygons, null),
                new List<int> { shadowPolygons.Count }, Texture);

            model = new Graphics.VoxelModel(true);
            model.Effect = toggLib.ModelEffect;
            model.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(polygons, null), 
                new List<int> { polygons.Count }, Texture);

            Position = storedPos;
        }

        void addWithZorder(PolygonColor add, List<PolygonColor> polygons)
        {
            float z = add.CenterZ();

            for (int i = 0; i < polygons.Count; ++i)
            {
                if (z < polygons[i].CenterZ())
                {
                    polygons.Insert(i, add);
                    return;
                }
            }

            polygons.Add(add);
        }

        public override bool Visible
        {
            get
            {
                return model.Visible;
            }

            set
            {
                model.Visible = value;
                shadow.Visible = value;

            }
        }

        override public void DeleteMe()
        {
            deleteSoldierModels();
            lib.SafeDelete(restIcon);
            if (childModels != null)
            {
                childModels.DeleteAll();
            }
        }

        void deleteSoldierModels()
        {
            if (model != null)
            {
                model.DeleteMe();
                shadow.DeleteMe();
            }
        }

        override public Vector3 Position
        {
            get
            {
                return model.position;
            }
            set
            {
                model.position = value;
                updateChildren();
            }
        }

        public Vector3 GetCharacterPosition()
        {
            Vector3 result = model.position;
            result.Z += offsetZ;
            return result;
        }

        public override bool InRenderList
        {
            get
            {
                return model.InRenderList;
            }
        }

        public void SetPositionXZ(float x, float z)
        {
            model.position.X = x;
            model.position.Z = z;
            updateChildren();
        }
        void updateChildren()
        {
            shadow.position = model.position;
            if (childModels != null)
            {
                childModels.ParentPosition = model.position;
            }
        }

        public Graphics.ImageGroupParent3D getOrCreateChildModels()
        {
            if (childModels == null)
            {
                childModels = new ImageGroupParent3D(2);
                childModels.ParentPosition = model.position;
            }
            return childModels;
        }


        public IntVector2 SquarePos
        {
            get
            {
                return new IntVector2(model.position.X, model.position.Z);
            }
        }
        class SoldierModelSetup
        {
            public PolygonColor figure, shadow;
        }

    }
}
