using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    abstract class AbsUpdateableModel : Mesh, IUpdateable
    {
        public UpdateType updateType = UpdateType.Full;
        public bool runDuringPause = true;

        public AbsUpdateableModel(LoadedMesh mesh, Vector3 pos, Vector3 scale,
            TextureEffectType effectType, SpriteName texture, Color col, bool addToRender, bool addToUpdate)
            : base(mesh, pos, scale, effectType, texture, col, addToRender)
        {
            if (addToUpdate)
            {
                AddToOrRemoveFromUpdateList(true);
            }
        }

        public void AddToOrRemoveFromUpdateList(bool add)
        {
            Ref.update.AddToOrRemoveFromUpdate(this, add);
            inUpdateList = add;
        }

        public bool inUpdateList = false;

        public override void DeleteMe()
        {
            base.DeleteMe();
            if (inUpdateList)
            {
                AddToOrRemoveFromUpdateList(false);
            }
        }

        public UpdateType UpdateType { get { return updateType; } }
        public bool RunDuringPause { get { return runDuringPause; } }
        abstract public void Time_Update(float time_ms);
    }
}

