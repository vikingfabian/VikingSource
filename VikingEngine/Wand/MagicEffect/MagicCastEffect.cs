using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Display.GameDisplay;
using VikingEngine.Core.Wand.Equipment;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.MagicEffect
{
    class MagicCastEffect
    {
        Mesh model;
        Vector3 fromPosV3;
        Vector3 toPosV3;

        public MapPlacement currentPos, nextPos;
        public MagicWand magic;

        public bool activated = true;

        public MagicCastEffect(MagicWand magic, MapPlacement placementData)
        {
            if (magic.magicEffects.Array[0].effect == Data.MagicEffect.Activate)
            {
                activated = false;
            }
            for (int effectIx = 0; effectIx < magic.magicEffects.Count; effectIx++)
            {
                magic.magicEffects.Array[effectIx].Recharge();
            }
            this.magic = magic;
            currentPos = placementData;
            nextPos = placementData;
            model = new Mesh(LoadedMesh.sphere, Vector3.Zero,
                new Vector3(0.4f), TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles, Color.LightBlue);
            refreshPos();
        }

        void refreshPos()
        {
            fromPosV3 = VectorExt.AddY(WP.TileToWp(currentPos.tilePos), 0.5f);
            model.position = fromPosV3;
        }

        public void Next(MapPlacement next)
        {
            fromPosV3 = model.position;
            nextPos = next;
            toPosV3 = VectorExt.AddY(WP.TileToWp(nextPos.tilePos), 0.5f);
        }

        public void TweenUpdate(float tween)
        {
            model.position = fromPosV3 * (1 - tween) + toPosV3 * tween;
        }

        public Vector3 WorldPosition()
        {
            return model.position;
        }

        public void DeleteMe()
        {
            model.DeleteMe();
        }
    }
}
