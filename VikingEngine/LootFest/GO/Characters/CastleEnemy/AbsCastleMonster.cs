using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters.CastleEnemy
{
    abstract class AbsCastleMonster : AbsMonster
    {
        protected VectorRect roomSize; //used for collision check inside the labyrinth
        //public const float CastleFloorY = Map.WorldPosition.ChunkStandardHeight + 1;

        public AbsCastleMonster(GoArgs args)
            : base(args)
        {
            if (args.LocalMember)
            {
                roomSize = CreateRoomSize(args.startWp.ChunkGrindex);
                //image.Position.Y = CastleFloorY;
            }
        }

        //public AbsCastleMonster(System.IO.BinaryReader r)
        //    : base(r)
        //{  }

        public static VectorRect CreateRoomSize(IntVector2 screen)
        {
            //går in i ner/höger vägg
            const float Edge = 4;
            const float SouthEastAdj = 0;
            VectorRect result = new VectorRect(VectorExt.V3XZtoV2(new Map.WorldPosition(screen).PositionV3) + new Vector2(SouthEastAdj) , Map.WorldPosition.ChunkPlaneSize);
            result.AddRadius(-Edge);
            return result;
        }

        public static void CheckCastleRoomBounds(AbsCharacter obj, VectorRect roomSize)
        {
            float radius = obj.CollisionAndDefaultBound.MainBound.halfSize.X * PublicConstants.Half;
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
                myBound.SetRight(roomSize.Right, true);
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
                obj.Position = VectorExt.V2toV3XZ(myBound.Center, obj.Y);
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
        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            //base.AIupdate(args);
            //if (checkOutsideUpdateArea_ActiveChunk())
            //    UnthreadedDeleteMe();
        }
    }
}
