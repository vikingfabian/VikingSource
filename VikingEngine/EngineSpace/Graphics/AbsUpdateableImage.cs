using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    abstract class AbsUpdateableImage : ImageAdvanced, IUpdateable
    {
        UpdateType updateType = UpdateType.Full;
        bool runDuringPause = true;

        public AbsUpdateableImage(bool addToRender, bool addToUpdate)
            : base(addToRender)
        {
            if (addToUpdate)
            {
                AddToOrRemoveFromUpdateList(true);
            }
        }

        public AbsUpdateableImage(SpriteName SpriteName, Vector2 pos, Vector2 sz, ImageLayers layer, 
            bool centerMidpoint, bool addToRender, bool addToUpdate)
            : base(SpriteName, pos, sz, layer, centerMidpoint, addToRender)
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
