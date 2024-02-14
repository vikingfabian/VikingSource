using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.CastleEnemy
{
    abstract class AbsCastleMonster : AbsMonster2
    {
        protected VectorRect roomSize; //used for collision check inside the labyrinth
        public const float CastleFloorY = Map.WorldPosition.ChunkStandardHeight + 1;

        public AbsCastleMonster(Map.WorldPosition startPos, int areaLevel)
            :base(startPos, areaLevel)
        {
            roomSize = CreateRoomSize(startPos.ChunkGrindex);
            image.position.Y = CastleFloorY;
        }

        public AbsCastleMonster(System.IO.BinaryReader r)
            : base(r)
        {  }

        public static VectorRect CreateRoomSize(IntVector2 screen)
        {
            //går in i ner/höger vägg
            const float Edge = 4;
            const float SouthEastAdj = 0;
            VectorRect result = new VectorRect(Map.WorldPosition.V3toV2(new Map.WorldPosition(screen).ToV3()) + new Vector2(SouthEastAdj) , Map.WorldPosition.ChunkPlaneSize);
            result.AddRadius(-Edge);
            return result;
        }

        public static void CheckCastleRoomBounds(AbsCharacter obj, VectorRect roomSize)
        {
            float radius = obj.CollisionBound.MainBound.Bound.OuterBound.HalfSizeX * PublicConstants.Half;
            VectorRect myBound = new VectorRect(obj.PlanePos - Vector2.One * radius, radius* PublicConstants.Twice * Vector2.One);
            bool collision = false;

            if (myBound.X < roomSize.X)
            {
                collision = true;
                myBound.X = roomSize.X;
            }
            else if (myBound.Right > roomSize.Right)
            {
                collision = true;
                myBound.Right = roomSize.Right;
            }
            else if (myBound.Y < roomSize.Y)
            {
                collision = true;
                myBound.Y = roomSize.Y;
            }
            else if (myBound.Bottom > roomSize.Bottom)
            {
                collision = true;
                myBound.Bottom = roomSize.Bottom;
            }

            if (collision)
            {
                obj.Position = Map.WorldPosition.V2toV3(myBound.Center, obj.Y);
                obj.HandleCastleRoomCollision();
            }
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.NO_PHYSICS;
            }
        }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
        }
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            //base.AIupdate(args);
            //if (checkOutsideUpdateArea_ActiveChunk())
            //    UnthreadedDeleteMe();
        }
    }
}
