using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class i : AbsUpdateable
    {
        Characters.AbsCharacter parent;
        Graphics.VoxelModelInstance image;

        public i(Characters.AbsCharacter parent)
            :base(true)
        {
#if !CMODE
            this.parent = parent;
            image = LootfestLib.Images.StandardObjInstance(VoxelModelName.i);
            image.scale = image.OneScale * 1.4f * Vector3.One;
            LfRef.gamestate.Progress.i = this;
#endif
        }

        public Vector2 PlanePos
        { get { return Map.WorldPosition.V3toV2(image.position); } }

        public override void Time_Update(float time)
        {
            image.position = parent.Position;
            image.position.Y += 4;

            image.Rotation.RotateWorld(Vector3.UnitX * 0.1f);

            if (parent.IsDeleted)
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
#if !CMODE
            base.DeleteMe();
            image.DeleteMe();
            if (LfRef.gamestate.Progress.i == this)
            {
                LfRef.gamestate.Progress.i = null;
            }
#endif
        }
    }
}
