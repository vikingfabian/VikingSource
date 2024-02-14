#define PULL_EDGE
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.Graphics
{
    static class GraphicsLib
    {
        public const float LayerDiff = 1f / (int)ImageLayers.NUM;
        public const int PolygonIndicesCount = 4;
        public const int PolygonDrawOrderCount = 6;

        public static SpriteName SumSpriteName(SpriteName tile, int add)
        {
            return (SpriteName)((int)tile + add);
        }

        public static float ToPaintLayer(ImageLayers layer)
        {
            return (float)layer * LayerDiff;
        }

        public static bool InRender(AbsDraw img)
        {
            return img != null && img.InRenderList;
        }
    }

    struct CollitionPlane
    {
        public int PlaneXYZ;
        public bool MinPos;
        public Vector3 Postion;
        public Vector3 Size;
        public float Dampening;


        public CollitionPlane(int planeDimention, bool min, Vector3 pos, Vector3 sz, float damp)
        {
            PlaneXYZ = planeDimention;
            MinPos = min;
            Postion = pos;
            Size = sz;
            Dampening = damp;
        }
    }

    struct Align
    {
        public Vector2 Center;
        public Align(Vector2 center)
        {
            this.Center = center;
        }
        public Align(Dir8 dir)
        {
            Vector2 adj = conv.Facing8ToVec2(dir, PublicConstants.Half);
            Center = VectorExt.V2Half;//.V2(PublicConstants.Half);
            Center.X += adj.X;
            Center.Y += adj.Y;
        }
        public static Align Zero = new Align(Dir8.NW);
        public static Align CenterAll = new Align(Dir8.NO_DIR);
        public static Align CenterWidth = new Align(new Vector2(PublicConstants.Half, 0));
        public static Align CenterHeight = new Align(new Vector2(0,  PublicConstants.Half));
        
    }

    enum MotionType
    {
        NON = 0,
        ANIMATE,
        MOVE,
        ACCELERATE,
        PARTICLE,
        ROTATE,
        ROTATE_AXIS,
        SCALE,
        OPACITY,
        COLOR,
        NUM
    }

    enum ParticleSystemType
    {   

        LightSparks,
        Poision,
        GoldenSparkle,
        BulletTrace,
        Fire,
        ExplosionFire,
        Smoke,
        RunningSmoke,
        Dust,
        WeaponSparks,
        CommanderDamage,
        DssDamage,
        TorchFire,

        //CCGPieceDamage,
        //CCGMagicTrace,
        NUM,

        Sparkle,
    }

    struct ParticleInitData
    {
        public Vector3 Position;
        public Vector3 StartSpeed;

        
        public ParticleInitData(Vector3 pos, Vector3 speed)
        {
            Position = pos;
            StartSpeed = speed;
        }
        public ParticleInitData(Vector3 pos)
            : this(pos, Vector3.Zero)
        { }
    }

    enum PixelShader
    {
        Default,
        Inverse,
        Gray,

        Num_Non
    }

    //enum GeneratedMeshType
    //{
    //    Cube,
    //    CylinderClosed,
    //    CylinderOpen,
    //    Donut,
    //    Triangle,
    //    Plane,
    //    Sphere,
    //    Tree,
    //    //Pipe,
    //    NUM
    //}

    enum MotionRepeate
    {
        NO_REPEAT = 0,
        BackNForwardLoop,
        BackNForwardOnce,
        Loop,
        NUM
    }

    enum TextureEffectType
    {
        
        //Specular,
        //Toon,
        Flat,
        FlatNoOpacity,
        //FlatVerticeColor,

        //FlatMask,
        //FlatLighted,
        //Bump,
        //BumpMask,
        //Water,
        Shadow,
        FixedLight,
        NUM_NON,
    }

    //public enum ColorMapType
    //{
    //    Color,
    //    Reflect,
    //    Normal,
    //    Mask,
    //}

    //public enum ObjType
    //{
    //    NotSet,
    //    Mesh,
    //    Parent3D,
    //    AmbientMesh,
    //    Generated,
    //}

    struct ChildRelation
    {
        public static ChildRelation None = new ChildRelation(false, false, false);
        public static ChildRelation PositionOnly = new ChildRelation(true, false, false);
        public static ChildRelation Full = new ChildRelation(true, true, true);
        public bool Position;
        public bool Size;
        public bool Rotation;
        public bool Transparensy;
        public bool Visibility;
        
        public ChildRelation(bool position, bool size, bool rotation)
        {
            Position = position;
            Size = size;
            Rotation = rotation;
            Transparensy = false;
            Visibility = true;
        }

        public ChildRelation(bool position, bool size, bool rotation, bool transparensy, bool visibility)
        {
            Position = position;
            Size = size;
            Rotation = rotation;
            Transparensy = transparensy;
            Visibility = visibility;
        }
    }

    struct TextureSourceLib
    {
        public const string ColorPos = "SourcePos";
        public const string ColorSz = "SourceSize";
        public const string ReflectPos = "SourcePosReflect";
        public const string ReflectSz = "SourceSizeReflect";
        public const string BumpPos = "SourcePosBump";
        public const string BumpSz = "SourceSizeBump";
        public const string MaskPos = TextLib.EmptyString;
        public const string MaskSz = TextLib.EmptyString;
        
        public const string ColorMap = "ColorMap";
        public const string ReflectMap = "ReflectionMap";
        public const string BumpMap = "BumpMap";
        public const string MaskMap = TextLib.EmptyString;
     
        public Vector2 Position;
        public Vector2 Size;
        public LoadedTexture Texture;
        
        public TextureSourceLib(Vector2 pos, Vector2 sz, LoadedTexture texture)
        { Position = pos; Size = sz; Texture = texture;  }

        public static TextureSourceLib Full
        {
            get { return new TextureSourceLib(Vector2.Zero, Vector2.One, (LoadedTexture)0); }
        }
       

        public void Empty()
        { Position = Vector2.Zero; Size = Vector2.One; }
        
        public void FlipX()
        {
            Size.X = -Size.X;
        }
    }
}
