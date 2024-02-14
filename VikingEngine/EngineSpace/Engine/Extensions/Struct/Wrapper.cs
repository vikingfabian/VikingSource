using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine//WRAPPER
{
    public struct SpriteSize
    {
        public int spriteWidth, textureWidth;

        public SpriteSize(int spriteWidth, int textureWidth)
        {
            this.spriteWidth = spriteWidth;
            this.textureWidth = textureWidth;
        }

        public float toImageSize(float visualSize)
        {
            return visualSize / spriteWidth * textureWidth;
        }

        public float toVisualSize(float imageSize)
        {
            return imageSize * spriteWidth / textureWidth;
        }

        public float relativeSizeTo(SpriteSize other)
        {
            return this.SpritePercentSize / other.SpritePercentSize;
        }

        public float SpritePercentSize
        {
            get
            {
                return (float)spriteWidth / textureWidth;
            }
        }

        public static Vector2 FitInsideArea(Vector2 textureSize, Vector2 areaSize)
        {
            Vector2 fromHeight = new Vector2(areaSize.Y / textureSize.Y * textureSize.X, areaSize.Y);
            if (fromHeight.X <= areaSize.X)
            {
                return fromHeight;
            }
            else
            {
                Vector2 fromWidth = new Vector2(areaSize.X, areaSize.X / textureSize.X * textureSize.Y);
                return fromWidth;
            }
        }

        //public Vector2 SizeFromHeight(float height)
        //{
        //    return new Vector2(height * 
        //}
    }

    public struct ComparableKeys<TKey, TValue> : IComparable 
        where TKey : IComparable
    {
        public TKey compareKey; 
        public TValue value;

        public ComparableKeys(TKey compareKey, TValue value)
        {
            this.compareKey = compareKey;
            this.value = value;
        }

        public int CompareTo(object obj)
        {
            return ((ComparableKeys<TKey, TValue>)obj).compareKey.CompareTo(compareKey);
        }
    }

    struct IconAndText
    {
        public SpriteName Icon;
        public string Text;
        public IconAndText(SpriteName icon, string text)
        {
            this.Text = text;
            this.Icon = icon;
        }
    }

    struct TwoBools
    {
        public static readonly TwoBools False = new TwoBools(false, false);
        public bool Value1;
        public bool Value2;
        public TwoBools(bool v1, bool v2)
        { Value1 = v1; Value2 = v2; }
    }

    struct TwoSprites
    {
        public SpriteName image1, image2;

        public TwoSprites(SpriteName image1, SpriteName image2 = SpriteName.NO_IMAGE)
        { this.image1 = image1; this.image2 = image2; }

        public override string ToString()
        {
            return image1.ToString() + ", " + image2.ToString();
        }
    }

    // struct FindMaxValue
    //{
    //    bool absolute;
    //    int highestMemberIndex;
    //    float highestValue;
    //    public float Highest
    //    {
    //        get { return highestValue; }
    //    }
    //    public int HighestMemberIndex
    //    {
    //        get { return highestMemberIndex; }
    //    }

    //    public FindMaxValue(bool absolute)
    //    {
    //        highestMemberIndex = 0;
    //        this.absolute = absolute;
    //        highestValue = float.MinValue;
    //    }
    //    public FindMaxValue(bool absolute, float minValue)
    //    {
    //        highestMemberIndex = 0;
    //        this.absolute = absolute;
    //        highestValue = minValue;
    //    }
    //    public void Next(float value)
    //    {
    //        if (value > highestValue)
    //        {
    //            highestValue = value;
    //        }
    //    }
    //    public void Next(float value, int index)
    //    {
    //        if (value > highestValue)
    //        {
    //            highestValue = value;
    //            highestMemberIndex = index;
    //        }
    //    }
    //}
    // struct FindMinValue
    // {
    //     public bool hasValue;
    //     bool absolute;
    //     public int lowestMemberIndex;
    //     public float lowestValue;
    //    //public float Lowest
    //    //{
    //    //    get { return lowestValue; }
    //    //}
    //    public float AbsLowest
    //    {
    //        get { return Math.Abs(lowestValue); }
    //    }
    //    //public int lowestMemberIndex
    //    //{
    //    //    get { return lowestMemberIndex; }
    //    //}

    //    public FindMinValue(bool absolute)
    //         :this()
    //     {
    //         //hasValue = true;
    //         //lowestMemberIndex = 0;
    //         this.absolute = absolute;
    //         //lowestValue = float.MaxValue;
    //     }
    //     public void Next(float value, int index)
    //     {
    //         if (!hasValue)
    //         {
    //             hasValue = true;
    //             lowestValue = float.MaxValue;
    //         }
    //         if (value < lowestValue)
    //         {
    //             lowestValue = value;
    //             lowestMemberIndex = index;
    //         }
    //     }
    // }

    // struct FindMinValuePointer<T>
    // {
    //     public bool hasValue;
    //     bool absolute;
    //     public T lowestMember;
    //     float lowestValue;
    //     public float Lowest
    //     {
    //         get { return lowestValue; }
    //     }
    //     //public int LowestMemberIndex
    //     //{
    //     //    get { return lowestMember; }
    //     //}

    //     public FindMinValuePointer(bool absolute)
    //         : this()
    //     {
    //         //hasValue = true;
    //         //lowestMemberIndex = 0;
    //         this.absolute = absolute;
    //         //lowestValue = float.MaxValue;
    //     }
    //     public void Next(float value, T member)
    //     {
    //         if (!hasValue)
    //         {
    //             hasValue = true;
    //             lowestValue = float.MaxValue;
    //         }
    //         if (value < lowestValue)
    //         {
    //             lowestValue = value;
    //             lowestMember = member;
    //         }
    //     }
    // }
    // struct FindMaxValuePointer<T>
    // {
    //     bool absolute;
    //     T highestMember;
    //     float highestValue;
    //     public float Highest
    //     {
    //         get { return highestValue; }
    //     }
    //     public T HighestMember
    //     {
    //         get { return highestMember; }
    //     }

    //     public FindMaxValuePointer(bool absolute)
    //     {
    //         highestMember = default(T);
    //         this.absolute = absolute;
    //         highestValue = float.MinValue;
    //     }
    //     public FindMaxValuePointer(bool absolute, float minValue)
    //     {
    //         highestMember = default(T);
    //         this.absolute = absolute;
    //         highestValue = minValue;
    //     }
    //     public void Next(float value)
    //     {
    //         if (value > highestValue)
    //         {
    //             highestValue = value;
    //         }
    //     }
    //     public void Next(float value, T member)
    //     {
    //         if (value > highestValue)
    //         {
    //             highestValue = value;
    //             highestMember = member;
    //         }
    //     }
    // }

    struct ClosestV2
    {
        Vector2 center;
        int closestIx;
        public int Result
        {
            get { return closestIx; }
        }
        float clostestDist;

        public ClosestV2(Vector2 center)
        {
            this.center = center;
            closestIx = 0;
            clostestDist = float.MaxValue;
        }
        public void Next(Vector2 point, int index)
        {
            float l = (point - center).Length();
            if (l < clostestDist)
            {
                clostestDist = l;
                closestIx = index;
            }
        }
    }


    public struct ControlButtons
    {
        public bool Ctrl;
        public bool Shift;
        public bool Alt;

        public ControlButtons(bool ctrl, bool shift, bool alt)
        {
            Ctrl = ctrl;
            Shift = shift;
            Alt = alt;
        }
    }
    public struct JoyStickValue
    {
        public Vector2 Direction;
        public Vector2 DirAndTime;
        public IntVector2 Stepping;
        public ThumbStickType Stick;

        public bool KeyDownEvent;
        //public int PlayerIx;
        public int ContolIx;
        //{
        //    get { return (int)PlayerIx; }
        //    set { PlayerIx = (int)value; }
        //}
        public void Time()
        {
            DirAndTime.X = Direction.X * Ref.DeltaTimeMs;
            DirAndTime.Y = Direction.Y * Ref.DeltaTimeMs;
        }
        public void InverseDir(Vector2 dir)
        {
            Direction.X = dir.X;
            Direction.Y = -dir.Y;
        }

        public override string ToString()
        {
            return Stick.ToString() + Direction.ToString();
        }
    }
    public struct ButtonValue
    {
        public numBUTTON Button;
        public bool KeyDown;
        public int ContolIx;

        //public int ContolIx
        //{
        //    get { return (int)PlayerIx; }
        //    set { PlayerIx = value; }
        //}

        public override string ToString()
        {
            return Button.ToString() + (KeyDown? " down" : " up");
        }
    }

}
