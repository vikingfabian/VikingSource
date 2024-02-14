using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.SpaceWar
{
    static class SpaceLib
    {
        public const float DriftSpeed = 0.0025f;
        public const float DeathRubbleSpeed = DriftSpeed * 0.2f;

        public static void Rotation1DToQuaterion(Graphics.Mesh mesh, float rotation)
        {
            mesh.Rotation.QuadRotation = Quaternion.Identity;
            Vector3 rot = Vector3.Zero;
            rot.X = MathHelper.TwoPi - rotation;
            mesh.Rotation.RotateWorld(rot);
        }
    }

    
}
