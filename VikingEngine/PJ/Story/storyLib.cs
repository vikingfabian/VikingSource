using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Story
{
    static class storyLib
    {
        public static Vector3 ToV3(Vector2 pos)
        {
            Vector3 ret = Vector3.Zero;
            ret.X = pos.X;
            ret.Y = pos.Y;
            return ret;
        }

        public static Vector2 ToV2(Vector3 pos)
        {
            Vector2 ret = Vector2.Zero;
            ret.X = pos.X;
            ret.Y = pos.Y;
            return ret;
        }

        public static IntVector2 toGridPos(Vector3 position)
        {
            return new IntVector2(position.X, position.Y);
        }

        public static Vector3 toWorldPos(IntVector2 gridPos)
        {
            Vector3 result = Vector3.Zero;
            result.X = gridPos.X;
            result.Y = gridPos.Y;
            return result;
        }
    }
}
