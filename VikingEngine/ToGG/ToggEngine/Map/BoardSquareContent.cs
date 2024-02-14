using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DataStream;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Map
{ 
    class BoardSquareContent
    {
        public SquareTag2 tag;
        public byte roomId = 0;

        public SquareType squareType = SquareType.Grass;
        public SquareVisualProperties visualProperties;

        public int playerPlacement = Commander.Players.LocalPlayer.PlayerNumber_NoPlayer;
        public AbsUnit unit = null;

        public TileObjectColl tileObjects = new TileObjectColl();
        public bool adjacentToFlag = false;
        public bool hidden = true;
        public bool monsterSpawnAvailable = false;
        

        public void AddToUnitCard(ToGG.ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        {
            if (unit != null)
            {
                card.settings = ToggEngine.Display2D.UnitDisplaySettings.All;

                unit.AddToUnitCard(card, ref position);
            }
                       

            tileObjects.AddToUnitCard(card, ref position);

            if (tag.tagType == SquareTagType.MapEnter)
            {
                card.portrait(ref position, SpriteName.cmdMapEntranceIcon, "Map entrance", false, 0.8f);
            }

            if (toggRef.mode == GameMode.Commander)
            {
                terrainToUnitCard(card, ref position);
            }
        }

        void terrainToUnitCard(ToGG.ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        {
            var data = toggRef.sq.Get(squareType);

            if (data.Terrain().properties.Count > 0)
            {
                card.startSegment(ref position);
                card.portrait(ref position, data.LabelImage(), data.Terrain().Name, false, 0.8f, 0);

                card.propertyList(ref position, arraylib.CastObject<TerrainProperty, AbsProperty>(data.Terrain().properties), 
                    SpriteName.toggPropertyTex);
            }
        }

        //public Graphics.ImageGroupParent2D Card(Graphics.ImageGroupParent2D images)
        //{
        //    Graphics.Image icon;
        //    var data = toggRef.sq.Get(squareType);
        //    images = HudLib.HudCardBasics(images, data.Terrain().Name, data.LabelImage(), 0.5f, out icon);

        //    Vector2 skillTextPos = new Vector2(HudLib.cardContentArea.Width * 0.5f,
        //        HudLib.cardContentArea.Y + HudLib.cardContentArea.Height * 0.1f);

        //    List<string> skillsText = new List<string>(data.Terrain().properties.Length);
        //    foreach (var p in data.Terrain().properties)
        //    {
        //        skillsText.Add(MainTerrainProperties.PropertyName(p));
        //    }
        //    HudLib.ListSkillsText(skillsText, ref skillTextPos, images);

        //    return images;
        //}

        public void ClearAll()
        {
            squareType = SquareType.Grass;
            tag = SquareTag2.None;

            if (unit != null)
            {
                unit.DeleteMe();
            }

            tileObjects.clear();
        }

        public void writeMemory(System.IO.BinaryWriter w)
        {
            Write(w);

            if (unit == null)
            {
                w.Write(false);
            }
            else
            {
                w.Write(true);
                unit.writeAllData(w);
            }
        }

        public void readMemory(System.IO.BinaryReader r, IntVector2 pos, bool isPaste)
        {
            ClearAll();

            Read(r, pos, FileVersion.Max, isPaste);

            if (r.ReadBoolean())
            {
                HeroQuest.Unit.ReadAllData(r, FileVersion.Max, false).SetPosition(pos);
            }
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((byte)squareType);
            tag.Write(w);
            w.Write((byte)roomId);
            tileObjects.Write(w);
            
            visualProperties.write(w);
        }
               
        public void Read(System.IO.BinaryReader r, IntVector2 square, FileVersion version, bool isPaste)
        { 
            squareType = (SquareType)r.ReadByte();
            if (version.release >= 2)
            {
                tag.Read(r, version);
                roomId = r.ReadByte();
            }
            tileObjects.Read(r, square, version, isPaste);
           
            visualProperties.read(r, version);
        }

        public void ReadOldVersion(System.IO.BinaryReader r, IntVector2 square, byte version)
        {
            squareType = (SquareType)r.ReadByte();

            if (version == 2)
            {
                int spec = r.ReadByte();

                TileObjectType special;
                switch (spec)
                {
                    case 0: special = TileObjectType.TacticalBanner; break;
                    case 1: special = TileObjectType.EscapePoint; break;
                    default: special = TileObjectType.NONE; break;
                }

                if (special != TileObjectType.NONE)
                {
                    //AbsTileObject.CreateObject(special, this, square);
                }
            }

            if (version >= 3)
            {
                int tileObjectsCount = r.ReadByte();
                if (tileObjectsCount > 0)
                {
                    //tileObjects = new List<AbsTileObject>(tileObjectsCount);
                    for (int i = 0; i < tileObjectsCount; ++i)
                    {
                        TileObjectType type = (TileObjectType)r.ReadByte();
                        TileObjLib.CreateObject(type, square, this, true).Read(r, new FileVersion());
                    }
                }
            }

            if (version >= 4)
            {
                visualProperties.read(r, new FileVersion());
            }
        }

        public AbsSquare Square { get { return toggRef.sq.Get(squareType); } }

        public bool HasProperty(TerrainPropertyType property)
        {
            return MainTerrain.HasProperty(property);
        }

        public bool TryGetProperty(TerrainPropertyType type, out TerrainProperty result)
        {
            result = MainTerrain.GetProperty(type);
            return result != null;
        }

        //public bool HasProperty(TerrainPropertyType property1, TerrainPropertyType property2)
        //{
        //    var props = MainTerrainProperties.Get(toggRef.sq.Get(squareType).TerrainType).properties;

        //    if (props != null)
        //    {
        //        foreach (var m in props)
        //        {
        //            if (m == property1 || m == property2)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        public MainTerrainProperties MainTerrain => MainTerrainProperties.Get(toggRef.sq.Get(squareType).TerrainType);

        public bool BlocksLOS()
        {
            var door = tileObjects.GetObject(TileObjectType.Door);
            if (door != null && ((Door)door).sett.openStatus != OpenStatus.Open)
            {
                return true;
            }

            return HasProperty(TerrainPropertyType.BlocksLOS);
        }

        public bool IsRoomDivider()
        {
            if (Square.TerrainType == MainTerrainType.Wall)
            {
                return true;
            }

            AbsTileObject doorObj = tileObjects.GetObject(TileObjectType.Door);
            if (doorObj != null)
            {
                return ((Door)doorObj).IsClosed; 
            }

            return false;
        }

        public bool IsWall => toggRef.sq.Get(squareType).TerrainType == MainTerrainType.Wall;

        public bool IsFloor => !MainTerrain.HasProperty(TerrainPropertyType.Impassable, TerrainPropertyType.FlyOverObsticle);
            
        public bool destroyTerrain()
        {
            if (Square.SubType != SquareType.NUM_NON)
            {
                this.squareType = Square.SubType;

                toggRef.board.refresh();
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "Tile(" + squareType.ToString() + ")" + tileObjects.ToString();
        }

        public void clear()
        {
            tileObjects.clear();
            adjacentToFlag = false;
        }

        public void newPosition(IntVector2 position)
        {
            unit?.SetPosition(position);

            tileObjects.newPosition(position);
        }

        public void onLoadComplete()
        {
            tileObjects.onLoadComplete();
        }

        public ForListLoop<AbsTileObject> objLoop()
        {
            return new ForListLoop<AbsTileObject>(tileObjects.members);
        }

        public bool Revealed => !hidden;
    }

    

    enum SquareType
    {
        Grass =0,
        GreenForest =1,
        GreenHill =2,

        GreenMountain =3,
        GreenWaterPuddle =4,
        GreenSwamp =5,
        GreenRubble =6,
        GreenTown =7,

        OpenWater =8,

        GreenPalisad =9,
        StoneTower =10,
        GreenRoad =11,
        GreenStoneWall = 12,
        GreenStoneGate = 13,

        MountainGround = 14,
        MountainWall = 15,
        MountBrickWall = 16,
        MountBrickGround = 17,

        RedBrickGround = 18,
        GrassMud=19,
        GrassObsticle=20,
        HouseGround=21,
        HouseWall=22,

        Fallpit,

        NUM_NON,
    }
    
    enum TerrainPropertyType
    {
        BlocksLOS,
        MustStop,
        Block1,
        ReducedTo1,
        ReducedTo2,

        HorseMustStop,
        Damageing,
        ArrowBlock,
        Impassable,
        SittingDuck,
        MoveBonus,
        Trample,
        Oversight,
        FlyOverObsticle,
        Pit,
        Num_Non
    }

    enum MovementRestrictionType
    {
        NoRestrictions,
        WalkThroughCantStop,
        MustStop,
        Impassable,
        CostStamina,
    }

   
}
