using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    struct StoredCameraPos
    {
        public bool hasValue;
        public Vector3 Position;
        public Vector3 lookTarget;
        public Vector2 tilt;
        public float targetZoom;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(hasValue);
            if (hasValue)
            {
                StreamLib.WriteVector(w, Position);
                StreamLib.WriteVector(w, lookTarget);
                StreamLib.WriteVector(w, tilt);
                w.Write(targetZoom);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            hasValue = r.ReadBoolean();
            if (hasValue)
            {
                Position = StreamLib.ReadVector3(r);
                lookTarget = StreamLib.ReadVector3(r);
                tilt = StreamLib.ReadVector2(r);
                targetZoom = r.ReadSingle();
            }
        }
    }
}
