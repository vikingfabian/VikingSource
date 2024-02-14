using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    class DiceModel
    {
        const LoadedTexture Texture = LoadedTexture.SpriteSheet;
        public static Vector2 Size;
        public static int SpacingX;
        BattleDice dice;

        public static void Init()
        {
            Size = new Vector2((int)(Engine.Screen.IconSize * 1.4f));
            SpacingX = (int)(Size.X * -0.1f);
        }

        public Graphics.RenderTargetDraw3dContainer drawcontainer;
        Graphics.TopViewCamera camera;
        Graphics.VoxelModel model;

        public Graphics.PolygonNormal side1, side2;
        public Graphics.PolygonNormal[] dieFaces;

        Graphics.EffectBasicNormal effect;

        public Rotation1D rotation = Rotation1D.D0;
        
        public DiceModel(Vector2 position)
        {
            setupEffect();

            var faces = Graphics.PolygonLib.createFaces(1f);
            side1 = new Graphics.PolygonNormal(faces[(int)CubeFace.Xnegative]);
            side2 = new Graphics.PolygonNormal(faces[(int)CubeFace.Xpositive]);

            dieFaces = new Graphics.PolygonNormal[]
                {
                    new Graphics.PolygonNormal(faces[(int)CubeFace.Zpositive]),
                    new Graphics.PolygonNormal(faces[(int)CubeFace.Ypositive]),
                    new Graphics.PolygonNormal(faces[(int)CubeFace.Znegative]),
                    new Graphics.PolygonNormal(faces[(int)CubeFace.Ynegative]),
                };
            
            model = new Graphics.VoxelModel(false);
            
            drawcontainer = new Graphics.RenderTargetDraw3dContainer(position, Engine.Screen.IconSizeV2 * 1.4f, HudLib.AttackWheelLayer);
            ((ToGG.Draw)Ref.draw).drawContainers.Add(drawcontainer);
            camera = new Graphics.TopViewCamera(10, new Vector2(MathHelper.PiOver2 + 0.2f, MathHelper.PiOver2 - 0.2f));
            camera.FieldOfView = 10;
            camera.instantMoveToTarget();

            drawcontainer.renderList.Add(this.model);
            drawcontainer.setCamera(camera);
        }

        public void refreshModel()
        {
            model.Effect = effect;
            List<Graphics.PolygonNormal> polygons = new List<PolygonNormal>(6);
            polygons.AddRange(dieFaces);
            polygons.Add(side1); polygons.Add(side2);
            model.BuildFromPolygons(new Graphics.PolygonsAndTrianglesNormal(polygons, null), new List<int> { polygons.Count }, Texture);
        }

        public void setFaceTexture(int face, SpriteName sprite)
        {
            var poly = dieFaces[face];

            Dir4 rotation;
            if (face <= 1)
            {
                rotation = Dir4.N;
            }
            else
            {
                rotation = Dir4.S;
            }

            poly.setSprite(sprite, rotation);
            dieFaces[face] = poly;
        }

        void setupEffect()
        {
            effect = new EffectBasicNormal(Texture);
            effect.basicEffect.AmbientLightColor = new Vector3(0.4f);

            effect.basicEffect.DirectionalLight0.Direction = -Vector3.UnitZ;
            effect.basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.7f);
            effect.basicEffect.DirectionalLight0.SpecularColor = new Vector3(0.05f);
            
            Vector3 blueLight = new Vector3(0.2f, 0.2f, 1f) * 0.2f;
            effect.basicEffect.DirectionalLight1.DiffuseColor = blueLight;
            effect.basicEffect.DirectionalLight1.Direction = Vector3.UnitX;

            effect.basicEffect.DirectionalLight2.Enabled = false;

        }       

        public void update()
        {
            model.Rotation = RotationQuarterion.Identity;
            model.Rotation.RotateWorldY(rotation.radians);

            camera.Time_Update(Ref.DeltaTimeMs);
            camera.RecalculateMatrices();
        }

        public void DeleteMe()
        {
            drawcontainer.DeleteMe();
        }
    }
}
