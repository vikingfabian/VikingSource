using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.GO
{
    abstract class AbsGameObject : AbsUpdateable
    {
        public Graphics.Mesh model;
        protected float groundY;

        public AbsGameObject(bool addToUpdate)
            : base(addToUpdate)
        {
        }

        protected void createModel(SpriteName sprite, Vector3 pos, Vector2 scale, bool flipX)
        {
            if (model != null)
            {
                model.DeleteMe();
            }

            groundY = scale.Y * 0.3f;
            pos.Y += groundY;

            model = new Mesh(LoadedMesh.plane, pos, VectorExt.V3FromXZ(scale, 1f), TextureEffectType.Flat, sprite,
                Color.White);
               // new TextureEffect(TextureEffectType.Flat, sprite), 
               //, Vector3.Zero);
            if (flipX)
            {
                model.TextureSource.FlipX();
            }
            model.Rotation = toggLib.PlaneTowardsCam;
        }

        virtual public void DeleteModel()
        {
            if (model != null)
            {
                model.DeleteMe();
                model = null;
            }
        }

        public void DeleteObject()
        {
            DeleteMe();
            DeleteModel();
        }
    }
}
