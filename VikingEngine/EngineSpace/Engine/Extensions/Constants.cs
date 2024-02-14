using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine
{
    public struct SaveData
    {
        public const string Version = "ver";
        public const string Textfile = ".txt";
        public const char Dimension = '_';
        public const char LParen = '(';
        public const char RParen = ')';
        public const char Comment = '/';
        public const char HeadName = '#';
        public const char LineObject = '$';
     
        public const string StorageContainerDir = "#StorageContainer";
        public const string EmptyValue = "[EMPTY]";

        public const string GroupStart = "GS";
        public const string GroupEnd = "GE";
        public const string Position = "ps";
        public const string Scale = "sc";
        public const string Rotation = "rt";
        public const string Visible = "vi";
        public const string IsParent = "ip";
        public const string Color = "co";
        public const string EffectType = "xy";
        public const string Transparency = "tr";
        public const string Mesh = "mh";
        public const string Ambient = "am";
        public const string LightStrength = "st";
        public const string TextureMap = "Tx";
        public const string ReflectMap = "Tr";
        public const string BumpMap = "Tb";
        public const string MaskMap = "Tm";

        public const string EmptyDataArray = "E";
        public const char RepeateDataArray = 'R';
        public const char EndRepeateDataArray = ']';

    }

    public struct PublicConstants
    {   //Public static readonlyants
       

        public const float LayerMinDiff = 0.0001f;
        public const float LayerMinDiff3D = 0.001f;
        //public static readonly int MAX_PLAYERS = 6;
        public const float Milli = 0.001f;
        //public const int MaxControllers = 4;

       

        public const int PlayerKeyboard = 4;
        public const int PlayerNumpad = 5;
        public const int MaxLights = 1;
        //public static readonly float TEXT_FEED_SPEED = 0.080f;
        public const int ImageFeedTime = 100;
        public const int MaxColor = 255;
        public const int NoTile = -1;
        public const float Half = 0.5f;
        public const float Third = 0.33f;
        public const int Twice = 2;
        public const int Invert = -1;
        public const float Quarter = 0.25f;
        public const int DimensionX = 0;
        public const int DimensionY = 1;
        public const int DimensionZ = 2;
        public const int DimensionW = 3;
        public const int DirectionForward = 1;
        public const int DirectionBack = -1;

        public const byte Bit1 = 1;
        public const byte Bit2 = 2;
        public const byte Bit3 = 4;
        public const byte Bit4 = 8;

        public const int ShortBitLenght = 16;
        public const ushort UShortHalf = ushort.MaxValue / 2;
        public const int ByteBitLenght = 8;
        public const int ByteSize = byte.MaxValue + 1;
        public const float ByteToPercent = 1f / Byte.MaxValue;
        public const int UInt16Size = ushort.MaxValue + 1;
        public const float Diagonal = 1.4142135623731f;
    }

    public struct ControlSense
    {
        public const float AnalogBuffert = 0.14f;
        public const float MoveNormal = 0.08f;
        public const float CharacterYSpeed = 0.02f;
    }
    public struct ControlScheme
    {
        public static readonly numBUTTON BattleMenuSelect = numBUTTON.A;
        public static readonly numBUTTON SelectTarget_Select = numBUTTON.A;
        public static readonly numBUTTON SelectTarget_Cancel = numBUTTON.B;
    }

    public struct Placements
    {//Positions and sizes of images
        public static readonly int DOC_STARTSPACE = 24;
        public static readonly int DOC_ENDSPACE = 46;
        public static readonly int DOC_FADE_AREA = 18;
    }

}
