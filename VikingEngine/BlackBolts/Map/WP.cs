

using Microsoft.Xna.Framework;

namespace VikingEngine.Core.BlackBolts.Map
{
    static class WP
    {
        public static Vector3 TileToWp(IntVector2 tile)
        {
            return new Vector3(tile.X, 0, tile.Y);
        }
        public static void Rotation1DToQuaterion(Graphics.AbsVoxelObj mesh, float rotation)
        {
            if (mesh != null)
            {
                mesh.Rotation.QuadRotation = Quaternion.Identity;
                mesh.Rotation.RotateWorldX(MathHelper.Pi - rotation);
            }
        }
        public static void DirToQuaterion(Graphics.AbsVoxelObj mesh, Dir4 dir)
        {
            if (mesh != null)
            {
                mesh.Rotation.QuadRotation = Quaternion.Identity;
                mesh.Rotation.RotateWorldX(MathHelper.Pi - DirToAngle(dir));
            }
        }

        public static float DirToAngle(Dir4 dir)
        {
            return MathExt.TauOver4 * (int)dir;
        }
    }
}
