using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Map.Terrain.AlgoObjects
{
    class Labyrinth
    {
        IntVector2 size;
        LabyrinthSquare[,] squares;
        // Unused var
        //IntVector2[] openingsDir;
        Rectangle2 openingsCheckRange; 
        Rectangle2 checkRange;
        
        public Labyrinth(IntVector2 size)
        {
            this.size = size;
            squares = new LabyrinthSquare[size.X, size.Y];

            int bottom = size.Y -1;
            LabyrinthSquare southEdge = new LabyrinthSquare();
            southEdge.SouthSide = OpeningType.OutsideArea;
            for (int x = 0; x < size.X; x++)
            {
                squares[x, bottom] = southEdge;
            }

            int right = size.X - 1;
            LabyrinthSquare eastEdge = new LabyrinthSquare();
            eastEdge.EastSide = OpeningType.OutsideArea;
            for (int y = 0; y < size.Y; y++)
            {
                squares[right, y] = squares[right, y].SetEdge(true, OpeningType.OutsideArea);
            }

            openingsCheckRange = new Rectangle2(IntVector2.One, size - 2);

            currentWall = new Wall();
            currentWall.Position = new IntVector2(0, -1);
            currentWall.Dir = Dir4.NUM_NON - 1;
        }

        public bool UnLockedSquare(IntVector2 pos)
        {
            return squares[pos.X, pos.Y].Status == SquareStatus.Connected;
        }

        public LabyrinthSquare GetSquare(IntVector2 pos)
        {
            if (checkRange.IntersectPoint(pos))
            {
                return squares[pos.X, pos.Y];
            }
            else
            {
                return new LabyrinthSquare(SquareStatus.Locked);
            }
        }

        public OpeningType GetOpeningType(IntVector2 pos, Dir4 dir)
        {
            bool east = false;
            switch (dir)
            {
                case Dir4.N:
                    pos.Y--;
                    break;
                case Dir4.E:
                    east = true;
                    break;
                case Dir4.W:
                    east = true;
                    pos.X--;
                    break;
            }

            if (checkRange.IntersectPoint(pos))
            {
                LabyrinthSquare ls = squares[pos.X, pos.Y];
                if (ls.Status == SquareStatus.Locked)
                    return OpeningType.BorderToLocked;
                if (east)
                    return ls.EastSide;
                return ls.SouthSide;
            }
            return OpeningType.OutsideArea;
        }
        public void SetOpening(IntVector2 pos, Dir4 dir)
        {
            //pos += IntVector2.NegativeOne + IntVector2.FromFacing4Dir(dir);

            bool east = false;
            switch (dir)
            {
                case Dir4.N:
                    pos.Y--;
                    break;
                case Dir4.E:
                    east = true;
                    break;
                case Dir4.W:
                    east = true;
                    pos.X--;
                    break;
            }
            if (checkRange.IntersectPoint(pos))
            {
                squares[pos.X, pos.Y] = squares[pos.X, pos.Y].OpenSide(east);
            }
            //if (east)
            //    squares[pos.X, pos.Y].EastSide = OpeningType.Open;
            //return squares[pos.X, pos.Y].SouthSide;
            //openings[pos.X, pos.Y] = true;
        }

        public void LockArea(Rectangle2 area)
        {
            ForXYLoop loop = new ForXYLoop(area.pos, area.BottomRight - 1);
            while (!loop.Done)
            {
                IntVector2 pos = loop.Next_Old();
                squares[pos.X, pos.Y] = LabyrinthSquare.Locked;
            }

            int above = area.Y - 1;
            if (above >= 0)
            {
                for (int x = area.X; x < area.Right; x++)
                {
                    squares[x, above] = squares[x, above].SetEdge(false, OpeningType.BorderToLocked);
                }
            }

            int toLeft = area.X - 1;
            if (toLeft >= 0)
            {
                for (int y = area.Y; y < area.Bottom; y++)
                {
                    squares[toLeft, y] = squares[toLeft, y].SetEdge(true, OpeningType.BorderToLocked);
                }
            }
        }

        

        public void Generate(IntVector2 startPos, Random rnd)
        {
            checkRange = new Rectangle2(IntVector2.Zero, size - 1);


            squares[startPos.X, startPos.Y] = new LabyrinthSquare(SquareStatus.Connected);
            //-lista alla positioner, en är öppen
            //-plock rnd closed
            //-checka om den angränsar öppen
            //-öppna mot öppen
            //-ta bort från rnd listan
            //-loopa
            
            IntVector2 startCheck; IntVector2 checkPos;
            startCheck = rndStartCheck(rnd);
            checkPos = nextPos(startCheck);


            bool done = false;
            while (!done)//tills den gått hela varvet runt och ingen är disconnected
            {
                if (squares[checkPos.X, checkPos.Y].Status == SquareStatus.NotConnected)
                {
                    Dir4 dir = (Dir4)rnd.Next((int)Dir4.NUM_NON);
                    for (int i = 0; i < 4; i++)
                    {
                        dir++;
                        if (dir == Dir4.NUM_NON)
                            dir = (Dir4)0;
                        IntVector2 check = checkPos + IntVector2.FromDir4(dir);
                        if (checkRange.IntersectPoint(check))
                        {
                            if (squares[check.X, check.Y].Status == SquareStatus.Connected)
                            {
                                squares[checkPos.X, checkPos.Y] = squares[checkPos.X, checkPos.Y].Connect();
                                SetOpening(checkPos, dir);
                                startCheck = rndStartCheck(rnd);
                                checkPos = startCheck;
                                break;
                            }
                        }
                    }
                }
                checkPos = nextPos(checkPos);
                done = startCheck == checkPos;
            }
        }

        public void Debug()
        {
            IntVector2 pos;
            const int RoomLetterWith = 5;
            IntVector2 charGridSz = size * RoomLetterWith;
            char[,] charGrid = new char[charGridSz.X, charGridSz.Y];

            ForXYLoop loopRooms = new ForXYLoop(IntVector2.Zero, size - 1);
            while (!loopRooms.Done)
            {
                pos = loopRooms.Next_Old();
                IntVector2 roomCoord = pos * RoomLetterWith;
                ForXYEdgeLoop roomWalls = new ForXYEdgeLoop(roomCoord, roomCoord + (RoomLetterWith - 1));
                while (!roomWalls.Next())
                {
                    //IntVector2 charPos = roomWalls.OldNext();

                    char c = '#';
                    Dir4 dir = roomWalls.AtEdgeDir;
                    if (dir != Dir4.NUM_NON)
                    {
                        switch (GetOpeningType(pos, dir))
                        {
                            case OpeningType.OutsideArea:
                                c = 'n';
                                break;
                            case OpeningType.Open:
                                c = 'o';
                                break;
                            case OpeningType.BorderToLocked:
                                c = 'L';
                                break;
                        }
                       
                    }
                    charGrid[roomWalls.Position.X, roomWalls.Position.Y] = c;
                }
            }

            for (pos.Y = 0; pos.Y < charGridSz.Y; pos.Y++)
            {
                const char EmptyChar = new char();
                string text = TextLib.EmptyString;
                for (pos.X = 0; pos.X < charGridSz.X; pos.X++)
                {
                    char c = charGrid[pos.X, pos.Y];
                    if (c == EmptyChar)
                        c = ' ';
                    text += c;
                }
                VikingEngine.Debug.Log(text);
            }
        }

        IntVector2 rndStartCheck(Random rnd)
        {
            return new IntVector2( rnd.Next(size.X), rnd.Next(size.Y));
        }
        IntVector2 nextPos(IntVector2 current)
        {
            current.X++;
            if (current.X >= size.X)
            {
                current.X = 0;
                current.Y++;
                if (current.Y >= size.Y)
                    current.Y = 0;
            }
            return current;
        }

        Wall currentWall;
        
        // Unused var
        //bool nextX = false;
        //bool resetDir = false;

        public Wall NextWall()
        {
            do
            {
                bool nextPos = false;
                
                currentWall.Dir++;
                if (currentWall.Dir >= Dir4.NUM_NON)
                {
                    nextPos = true;
                    currentWall.Dir = (Dir4)0;
                }
                
                if (nextPos)
                {
                    //nextX = false;
                    currentWall.Position.X++;
                    if (currentWall.Position.X >= size.X)
                    {
                        currentWall.Position.X = -1;
                        currentWall.Position.Y++;
                        if (currentWall.Position.Y >= size.Y)
                        {
                            Wall empty = new Wall();
                            empty.Empty = true;
                            return empty;
                        }
                    }
                }
                currentWall.Opening = GetOpeningType(currentWall.Position, currentWall.Dir);
                currentWall.Status = GetSquare(currentWall.Position).Status;
                currentWall.NeighborStatus = GetSquare(currentWall.Position + IntVector2.FromDir4(currentWall.Dir)).Status;

            } while (currentWall.Status == SquareStatus.Locked && currentWall.NeighborStatus == SquareStatus.Locked);

            return currentWall;

        }

        public List<Wall> GetWalls(IntVector2 pos)
        {
            List<Wall> result = new List<Wall>();
            if (GetSquare(pos).Status == SquareStatus.Locked)
                return result; //Empty

            currentWall.Position = pos;
            for (currentWall.Dir = (Dir4)0; currentWall.Dir < Dir4.NUM_NON; currentWall.Dir++)
            {
               // bool nextPos = false;

                //currentWall.Dir++;
                //if (currentWall.Dir >= Facing4Dir.NUM)
                //{
                //    nextPos = true;
                //    currentWall.Dir = (Facing4Dir)0;
                //}

                //if (nextPos)
                //{
                //    nextX = false;
                //    currentWall.Position.X++;
                //    if (currentWall.Position.X >= size.X)
                //    {
                //        currentWall.Position.X = -1;
                //        currentWall.Position.Y++;
                //        if (currentWall.Position.Y >= size.Y)
                //        {
                //            Wall empty = new Wall();
                //            empty.Empty = true;
                //            return empty;
                //        }
                //    }
                //}
                currentWall.Opening = GetOpeningType(currentWall.Position, currentWall.Dir);
                currentWall.Status = GetSquare(currentWall.Position).Status;
                currentWall.NeighborStatus = GetSquare(currentWall.Position + IntVector2.FromDir4(currentWall.Dir)).Status;
                if (currentWall.NeighborStatus != SquareStatus.Locked || currentWall.Status != SquareStatus.Locked)
                {
                    result.Add(currentWall);
                }

            } 
            //while (currentWall.Status == SquareStatus.Locked && currentWall.NeighborStatus == SquareStatus.Locked) ;
            return result;
        }
    }

    struct Wall
    {
        public bool Empty;
        public IntVector2 Position;
        public Dir4 Dir;
        public OpeningType Opening;
        public SquareStatus Status;
        public SquareStatus NeighborStatus;

        public override string ToString()
        {
            return "Wall: " + Position.ToString() + " dir:" + Dir.ToString() + 
                " open:" + Opening.ToString() + " me:" + Status.ToString() + " nb:" + NeighborStatus.ToString();
        }
    }

    struct LabyrinthSquare
    {
        public static readonly LabyrinthSquare Locked = new LabyrinthSquare(SquareStatus.Locked);

        public SquareStatus Status;
        public OpeningType EastSide;
        public OpeningType SouthSide;

        public LabyrinthSquare(SquareStatus Status)
        {
            this.Status = Status;
            EastSide = OpeningType.Wall;
            SouthSide = OpeningType.Wall;
        }

        public LabyrinthSquare OpenSide(bool east)
        {
            if (east)
                EastSide = OpeningType.Open;
            else
                SouthSide = OpeningType.Open;
            return this;
        }
        public LabyrinthSquare Connect()
        {
            Status = SquareStatus.Connected;
            return this;
        }

        public LabyrinthSquare SetEdge(bool east, OpeningType type)
        {
            if (east)
                EastSide = type;
            else
                SouthSide = type;
            return this;
        }
    }

    enum SquareStatus
    {
        NotConnected,
        Connected,
        Locked, //Cant be used
    }
    enum OpeningType
    {
        Wall,
        Open,
        OutsideArea,
        BorderToLocked,
    }
}
