using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    delegate T ResizeGrid3DL_GetDefault<T>(int x, int y, int z);
    delegate void ResizeGrid3DL_Removing<T>(T item, int x, int y, int z);

    /// <summary>
    /// Extended functions for a 1D array representing a 3D grid/volume
    /// </summary>
    class Grid3D_L<T>
    {
        IntVector3 size;

        public T[] array;
        public T sel;

        public Grid3D_L()
        { }

        public Grid3D_L(int size)
            : this(new IntVector3(size, size, size))
        {
        }

        public Grid3D_L(T[] array, IntVector3 size)
        {
            this.array = array;
            this.size = size;
        }

        public Grid3D_L(IntVector3 size)
        {
            this.size = size;
            array = new T[size.Volume()];
        }

        public void initGrid(IntVector3 size)
        {
            this.size = size;
            array = new T[size.Volume()];
        }

        /// <summary>
        /// Maps 3D coordinates to the 1D array index
        /// </summary>
        private int GetIndex(int x, int y, int z)
        {
            return x + (y * size.X) + (z * size.X * size.Y);
        }

        public void ReSize(IntVector3 newSize, ResizeGrid3DL_GetDefault<T> getDefaultItem, ResizeGrid3DL_Removing<T> removeItem)
        {
            T[] newArray = new T[newSize.Volume()];
            int lengthX = lib.LargestValue(size.X, newSize.X);
            int lengthY = lib.LargestValue(size.Y, newSize.Y);
            int lengthZ = lib.LargestValue(size.Z, newSize.Z);

            for (int z = 0; z < lengthZ; ++z)
            {
                bool inOldZRange = z < size.Z;
                bool inNewZRange = z < newSize.Z;
                bool overlapZ = inOldZRange && inNewZRange;

                for (int y = 0; y < lengthY; ++y)
                {
                    bool inOldYRange = y < size.Y;
                    bool inNewYRange = y < newSize.Y;
                    bool overlapY = inOldYRange && inNewYRange;

                    for (int x = 0; x < lengthX; ++x)
                    {
                        bool inOldXRange = x < size.X;
                        bool inNewXRange = x < newSize.X;
                        bool overlapX = inOldXRange && inNewXRange;

                        if (overlapX && overlapY && overlapZ)
                        {
                            newArray[x + (y * newSize.X) + (z * newSize.X * newSize.Y)] = array[GetIndex(x, y, z)];
                        }
                        else if (inNewXRange && inNewYRange && inNewZRange)
                        { //Expanding
                            if (getDefaultItem != null)
                                newArray[x + (y * newSize.X) + (z * newSize.X * newSize.Y)] = getDefaultItem(x, y, z);
                        }
                        else
                        { //Cutting
                            if (removeItem != null)
                            {
                                if (x < size.X && y < size.Y && z < size.Z)
                                {
                                    removeItem(array[GetIndex(x, y, z)], x, y, z);
                                }
                            }
                        }
                    }
                }
            }

            array = newArray;
            size = newSize;
        }

        public void ExpandSize(IntVector3 add, IntVector3 move)
        {
            IntVector3 newSize = size + add;
            T[] newArray = new T[newSize.Volume()];

            for (int z = 0; z < size.Z; ++z)
            {
                int newZ = z + move.Z;
                for (int y = 0; y < size.Y; ++y)
                {
                    int newY = y + move.Y;
                    for (int x = 0; x < size.X; ++x)
                    {
                        newArray[(x + move.X) + (newY * newSize.X) + (newZ * newSize.X * newSize.Y)] = array[GetIndex(x, y, z)];
                    }
                }
            }

            array = newArray;
            size = newSize;
        }

        /// <summary>
        /// Move all members in a scroll-like way
        /// </summary>
        public void ShiftData(IntVector3 length)
        {
            var clone = (T[])array.Clone();
            for (int z = 0; z < size.Z; ++z)
            {
                int newZ = Bound.SetRollover(z + length.Z, 0, size.Z - 1);
                for (int y = 0; y < size.Y; ++y)
                {
                    int newY = Bound.SetRollover(y + length.Y, 0, size.Y - 1);
                    for (int x = 0; x < size.X; ++x)
                    {
                        int newX = Bound.SetRollover(x + length.X, 0, size.X - 1);
                        array[newX + (newY * size.X) + (newZ * size.X * size.Y)] = clone[GetIndex(x, y, z)];
                    }
                }
            }
        }

        public T Get(IntVector3 position)
        {
            return array[GetIndex(position.X, position.Y, position.Z)];
        }

        public ref T GetRef(int x, int y, int z)
        {
            return ref array[GetIndex(x, y, z)];
        }

        public ref T GetRef(IntVector3 position)
        {
            return ref array[GetIndex(position.X, position.Y, position.Z)];
        }

        public T Get(int x, int y, int z)
        {
            return array[GetIndex(x, y, z)];
        }

        public void Set(IntVector3 position, T value)
        {
            array[GetIndex(position.X, position.Y, position.Z)] = value;
        }

        public void SetRef(ref IntVector3 position, ref T value)
        {
            array[GetIndex(position.X, position.Y, position.Z)] = value;
        }

        public void Set(int x, int y, int z, T value)
        {
            array[GetIndex(x, y, z)] = value;
        }

        public bool TryGet(IntVector3 position, out T value)
        {
            if (InBounds(position))
            {
                value = array[GetIndex(position.X, position.Y, position.Z)];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public bool TryGet(int x, int y, int z, out T value)
        {
            if (InBounds(x, y, z))
            {
                value = array[GetIndex(x, y, z)];
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public bool TrySet(IntVector3 position, T value)
        {
            if (InBounds(position))
            {
                array[GetIndex(position.X, position.Y, position.Z)] = value;
                return true;
            }
            return false;
        }

        public bool InBounds(IntVector3 position)
        {
            return position.X >= 0 && position.X < size.X &&
                   position.Y >= 0 && position.Y < size.Y &&
                   position.Z >= 0 && position.Z < size.Z;
        }

        public bool InBounds(int x, int y, int z)
        {
            return x >= 0 && x < size.X &&
                   y >= 0 && y < size.Y &&
                   z >= 0 && z < size.Z;
        }

        #region LOOP

        ForXYZLoop loop;

        public void LoopBegin()
        {
            loop = new ForXYZLoop(size);
        }

        public ForXYZLoop LoopInstance()
        {
            return new ForXYZLoop(size);
        }

        public bool LoopNext()
        {
            return loop.Next();
        }

        public void LoopUndoToPrev()
        {
            loop.UndoToPrevious();
        }

        public bool LoopNextSel()
        {
            bool next = loop.Next();
            if (next)
            {
                sel = array[GetIndex(loop.Position.X, loop.Position.Y, loop.Position.Z)];
            }
            else
            {
                sel = default(T);
            }
            return next;
        }

        public T LoopValueGet()
        {
            return array[GetIndex(loop.Position.X, loop.Position.Y, loop.Position.Z)];
        }

        public void LoopValueSet(T value)
        {
            array[GetIndex(loop.Position.X, loop.Position.Y, loop.Position.Z)] = value;
        }

        public IntVector3 LoopPosition
        {
            get
            {
                return loop.Position;
            }
        }
        #endregion

        public void Clear()
        {
            Array.Clear(array, 0, array.Length);
        }

        public void SetAll(T value)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = value;
            }
        }

        public void MoveEveryThing(IntVector3 dir)
        {
            T[] copy = new T[size.Volume()];

            int cx, cy, cz;
            for (int z = 0; z < size.Z; ++z)
            {
                cz = z + dir.Z;
                if (cz < 0) { cz += size.Z; }
                else if (cz >= size.Z) { cz -= size.Z; }

                for (int y = 0; y < size.Y; ++y)
                {
                    cy = y + dir.Y;
                    if (cy < 0) { cy += size.Y; }
                    else if (cy >= size.Y) { cy -= size.Y; }

                    for (int x = 0; x < size.X; ++x)
                    {
                        cx = x + dir.X;
                        if (cx < 0) { cx += size.X; }
                        else if (cx >= size.X) { cx -= size.X; }

                        copy[cx + (cy * size.X) + (cz * size.X * size.Y)] = array[GetIndex(x, y, z)];
                    }
                }
            }

            array = copy;
        }

        /// <summary>
        /// Rotates the 3D grid 90 degrees clockwise on a specific axis.
        /// To rotate 180 or 270, call multiple times or extend logic.
        /// </summary>
        public Grid3D_L<T> Rotate(Dimensions axis, int clockWiseSteps = 1)
        {
            clockWiseSteps = clockWiseSteps % 4;
            if (clockWiseSteps < 0) clockWiseSteps += 4;
            if (clockWiseSteps == 0) return this;

            Grid3D_L<T> result;
            IntVector3 max = size - 1;

            if (axis == Dimensions.Y) // Rotate on X/Z plane
            {
                result = new Grid3D_L<T>(new IntVector3(size.Z, size.Y, size.X));
                for (int z = 0; z < size.Z; ++z)
                {
                    for (int y = 0; y < size.Y; ++y)
                    {
                        for (int x = 0; x < size.X; ++x)
                        {
                            if (clockWiseSteps == 1)
                                result.array[(max.Z - z) + (y * result.size.X) + (x * result.size.X * result.size.Y)] = this.array[GetIndex(x, y, z)];
                            else if (clockWiseSteps == 2)
                                result.array[(max.X - x) + (y * result.size.X) + ((max.Z - z) * result.size.X * result.size.Y)] = this.array[GetIndex(x, y, z)];
                            else // 3
                                result.array[z + (y * result.size.X) + ((max.X - x) * result.size.X * result.size.Y)] = this.array[GetIndex(x, y, z)];
                        }
                    }
                }
            }
            else if (axis == Dimensions.Z) // Rotate on X/Y plane
            {
                result = new Grid3D_L<T>(new IntVector3(size.Y, size.X, size.Z));
                for (int z = 0; z < size.Z; ++z)
                {
                    for (int y = 0; y < size.Y; ++y)
                    {
                        for (int x = 0; x < size.X; ++x)
                        {
                            if (clockWiseSteps == 1)
                                result.array[(max.Y - y) + (x * result.size.X) + (z * result.size.X * result.size.Y)] = this.array[GetIndex(x, y, z)];
                            else if (clockWiseSteps == 2)
                                result.array[(max.X - x) + ((max.Y - y) * result.size.X) + (z * result.size.X * result.size.Y)] = this.array[GetIndex(x, y, z)];
                            else // 3
                                result.array[y + ((max.X - x) * result.size.X) + (z * result.size.X * result.size.Y)] = this.array[GetIndex(x, y, z)];
                        }
                    }
                }
            }
            else // Axis.X - Rotate on Y/Z plane
            {
                result = new Grid3D_L<T>(new IntVector3(size.X, size.Z, size.Y));
                for (int z = 0; z < size.Z; ++z)
                {
                    for (int y = 0; y < size.Y; ++y)
                    {
                        for (int x = 0; x < size.X; ++x)
                        {
                            if (clockWiseSteps == 1)
                                result.array[x + ((max.Z - z) * result.size.X) + (y * result.size.X * result.size.Y)] = this.array[GetIndex(x, y, z)];
                            else if (clockWiseSteps == 2)
                                result.array[x + ((max.Y - y) * result.size.X) + ((max.Z - z) * result.size.X * result.size.Y)] = this.array[GetIndex(x, y, z)];
                            else // 3
                                result.array[x + (z * result.size.X) + ((max.Y - y) * result.size.X * result.size.Y)] = this.array[GetIndex(x, y, z)];
                        }
                    }
                }
            }
            return result;
        }

        public Grid3D_L<T> Flip(bool flipX, bool flipY, bool flipZ)
        {
            var result = new Grid3D_L<T>(size);
            IntVector3 max = size - 1;

            for (int z = 0; z < size.Z; ++z)
            {
                int zTarget = flipZ ? max.Z - z : z;
                for (int y = 0; y < size.Y; ++y)
                {
                    int yTarget = flipY ? max.Y - y : y;
                    for (int x = 0; x < size.X; ++x)
                    {
                        int xTarget = flipX ? max.X - x : x;
                        result.array[xTarget + (yTarget * size.X) + (zTarget * size.X * size.Y)] = this.array[GetIndex(x, y, z)];
                    }
                }
            }

            return result;
        }

        public Grid3D_L<T> Clone()
        {
            return new Grid3D_L<T>((T[])array.Clone(), size);
        }

        public bool EqualData(Grid3D_L<T> other)
        {
            if (other.size == size)
            {
                for (int i = 0; i < array.Length; ++i)
                {
                    if (!EqualityComparer<T>.Default.Equals(this.array[i], other.array[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public T RandomMember()
        {
            return array[Ref.rnd.Int(array.Length)];
        }

        public T RandomMember(out IntVector3 position)
        {
            int index = Ref.rnd.Int(array.Length);

            int z = index / (size.X * size.Y);
            int rem = index % (size.X * size.Y);
            int y = rem / size.X;
            int x = rem % size.X;

            position = new IntVector3(x, y, z);
            return array[index];
        }

        public IntVector3 Size
        {
            get { return size; }
        }

        //public Rectangle3 Volume
        //{
        //    get { return new Rectangle3(IntVector3.Zero, size); }
        //}

        //public Rectangle3 VoxelBound()
        //{
        //    return new Rectangle3(IntVector3.Zero, size - 1);
        //}

        public void Print()
        {
            Debug.Log("GRID PRINT " + size.ToString());
            for (int z = 0; z < size.Z; ++z)
            {
                Debug.Log($"--- LAYER Z: {z} ---");
                for (int y = 0; y < size.Y; ++y)
                {
                    StringBuilder row = new StringBuilder();
                    for (int x = 0; x < size.X; ++x)
                    {
                        row.Append(array[GetIndex(x, y, z)].ToString());
                        row.Append(", ");
                    }

                    Debug.Log(row.ToString());
                }
            }
        }

        public int Width { get { return size.X; } }
        public int Height { get { return size.Y; } }
        public int Depth { get { return size.Z; } }

        public override string ToString()
        {
            return "Grid 1D<" + typeof(T).ToString() + ">[" + size.X.ToString() + ", " + size.Y.ToString() + ", " + size.Z.ToString() + "]";
        }
    }
}
