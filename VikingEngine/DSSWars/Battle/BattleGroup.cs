using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Battle
{
    class BattleGroup
    {
        int index;
        List<AbsMapObject> members;
        Vector2 center;
        Rotation1D rotation;

        public BattleGroup(AbsMapObject m1, AbsMapObject m2) 
        {
            members = new List<AbsMapObject> 
            { 
                m1, m2
            };

            m2.battleGroup = this;

            index = DssRef.state.battles.Add(this);

            center = VectorExt.V3XZtoV2(m2.position + m1.position) / 2f;
            
            Vector2 diff = VectorExt.V3XZtoV2(m2.position - m1.position);
            rotation = Rotation1D.FromDirection(diff);
            Ref.update.AddSyncAction(new SyncAction(debugVisuals));
        }

        void debugVisuals()
        {
            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(IntVector2.Zero, 5);
            ForXYLoop loop = new ForXYLoop(area);

            while (loop.Next())
            {
                Vector3 pos = gridPosToWp(loop.Position);

                Graphics.Mesh dot = new Graphics.Mesh(LoadedMesh.cube_repeating,
                        pos,
                        new Vector3(0.05f), Graphics.TextureEffectType.Flat,
                        SpriteName.WhiteArea, loop.Position == IntVector2.Zero? Color.Blue : Color.Red, false);
               // dot.Opacity = opacity;
                dot.AddToRender(DrawGame.UnitDetailLayer);
            }
        }

        Vector3 gridPosToWp(IntVector2 gridPos)
        {
            Vector2 offset = VectorExt.RotateVector(
                new Vector2(
                    gridPos.X * SoldierGroup.GroupSpacing, 
                    gridPos.Y * SoldierGroup.GroupSpacing), 
                rotation.radians);

            Vector3 result = new Vector3(
                center.X + offset.X,
                0,
                center.Y + offset.Y);

            //IntVector2 tilePos = new IntVector2(result.X, result.Z);
            result.Y = DssRef.world.SubTileHeight(result);

            return result;  
        }
    }
}
