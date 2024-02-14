using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Graphics
{
    class SetFrame : AbsUpdateable
    {
        Image image; SpriteName frame;
        float time;

        public SetFrame(Image image, SpriteName frame, float time, bool addToUpdate = true)
            :base(addToUpdate)
        {
            this.image = image;
            this.frame = frame;
            this.time = time;
        }

        public override void Time_Update(float time_ms)
        {
            time -= time_ms;
            if (time <= 0)
            {
                image.SetSpriteName(frame);
                DeleteMe();
            }
        }
    }
}
