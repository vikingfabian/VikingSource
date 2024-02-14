using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    class SplitScreenModel
    {
        OneCameraModel[] models;
        public static int ActivePlayerCount = 1;

        public SplitScreenModel(LoadedMesh mesh, Vector3 pos, Vector3 scale,
            TextureEffectType effectType, SpriteName texture, Color col,
            bool addToRender = true)
        {
            models = new Graphics.OneCameraModel[ActivePlayerCount];
            for (int i = 0; i < models.Length; ++i)
            {
                models[i] = new OneCameraModel(mesh, pos, scale, effectType, texture, col, 0, addToRender);
                models[i].cameraIndex = i;
            }
        }

        public Mesh model(int playerIx)
        {
            return models[Engine.XGuide.GetPlayer(playerIx).view.ScreenIndex];
        }
    } 

    /// <summary>
    /// Mesh that only will render in one player view of choice
    /// </summary>
    class OneCameraModel : Mesh
    {
        public int cameraIndex;

        public OneCameraModel(LoadedMesh mesh, Vector3 pos, Vector3 scale,
            TextureEffectType effectType, SpriteName texture, Color col,
             int playerIx, bool addToRender = true)
            : base(mesh, pos, scale, effectType, texture, col, addToRender)
        {
            cameraIndex = Engine.XGuide.GetPlayer(playerIx).view.ScreenIndex;
        }

        public override void Draw(int cameraIndex)
        {
            if (cameraIndex == this.cameraIndex)
            {
                base.Draw(cameraIndex);
            }
        }
    }
}
