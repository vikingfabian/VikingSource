using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    class BlockHitEffect
    {
        Graphics.Image image;

        public BlockHitEffect(SlotMachineWheel wheel, bool block_notIgnored)
        {
            image = new Graphics.Image(SpriteName.cmdArmorResult, wheel.Center, SlotMachineWheel.Size * 0.5f, ImageLayers.Lay1, true);
            new Graphics.Motion2d(Graphics.MotionType.SCALE, image, image.Size * 0.5f, Graphics.MotionRepeate.BackNForwardOnce, 200, true);
        }

        public void DeleteMe()
        {
            image.DeleteMe();
        }
    }
}
