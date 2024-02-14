using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VikingEngine.ToGG.MoonFall
{
    class MapArea : MapNode
    {
        public List<AreaConnection> connectedAreas = new List<AreaConnection>(4);

        public List<UnitPlacementNode> placementNodes;
        
        public MapArea(Vector2 center)
        {
            this.center = center;
            createVisuals();

            placementNodes = new List<UnitPlacementNode>(moonLib.MaxPlayerCount);
            
            Rotation1D placementDir = Rotation1D.D0;
            for (int i = 0; i < moonLib.MaxPlayerCount; ++i)
            {
                placementNodes.Add(new UnitPlacementNode(center + placementDir.Direction(110), i));
                placementDir.Add(MathExt.Tau / moonLib.MaxPlayerCount);
            }
        }

        public override void createVisuals()
        {
            base.createVisuals();
            if (image != null)
            {
                image.Color = Color.Yellow;

                foreach (var m in connectedAreas)
                {
                    m.createVisuals();
                }
            }
        }

        override public void refreshVisuals()
        {
            base.refreshVisuals();

            foreach (var m in connectedAreas)
            {
                m.refreshVisuals();
            }
        }

        public void connectToArea(MapArea to)
        {
            if (to != this && !hasConnection(to))
            {
                var connection = new AreaConnection(this, to);
                addConnection(connection);
                //connectedAreas.Add(connection);
                //to.connectedAreas.Add(connection);
                //refreshVisuals();//true, selectedArea);
                //adjacent.connectToArea(this, selectedArea);
            }
        }

        void addConnection(AreaConnection connection)
        {
            connectedAreas.Add(connection);
            connection.oppositeArea(this).connectedAreas.Add(connection);
            refreshVisuals();
        }

        bool hasConnection(MapArea toArea)
        {
            foreach (var m in connectedAreas)
            {
                if (m.contains(toArea))
                    return true;
            }
            return false;
        }

        bool hasConnection(AreaConnection connection)
        {
            return hasConnection(connection.oppositeArea(this));
        }

        public void clearConnections(MapArea selectedArea)
        {
            foreach (var m in connectedAreas)
            {
                m.toArea.removedConnection(this, selectedArea);
                m.deleteVisuals();
            }
            connectedAreas.Clear();
            refreshVisuals();
        }

        public void removedConnection(MapArea area, MapArea selectedArea)
        {
            AreaConnection conn = getConnection(area);
            conn.deleteVisuals();
            connectedAreas.Remove(conn);
            refreshVisuals();
        }

        public AreaConnection getConnection(MapArea toArea)
        {
            foreach (var m in connectedAreas)
            {
                if (m.toArea == toArea)
                    return m;
            }
            return null;
        }

        public override void move(Vector2 value)
        {
            base.move(value);
            foreach (var m in placementNodes)
            {
                m.move(value);
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            clearConnections(null);
            moonRef.map.areas.Remove(this);
        }

        public override void write(BinaryWriter w)
        {
            base.write(w);

            foreach (var m in placementNodes)
            {
                m.write(w);
            }
        }

        public override void read(BinaryReader r, int version)
        {
            base.read(r, version);

            foreach (var m in placementNodes)
            {
                m.read(r, version);
                m.refreshVisuals();
            }
        }

        public void writeConnections(System.IO.BinaryWriter w)
        {
            w.Write(connectedAreas.Count);
            foreach (var m in connectedAreas)
            {
                m.write(w);
            }
        }

        public void readConnections(BinaryReader r, int version)
        {
            int connectedAreasCount = r.ReadInt32();
            for (int i = 0; i < connectedAreasCount; ++i)
            {
                AreaConnection connection = new AreaConnection(r, version);
                if (!hasConnection(connection))
                {
                    addConnection(connection);
                }
            }
        }

        //public void write(System.IO.BinaryWriter w)
        //{
        //    //arrangeAreaConnections();

        //    //SaveLib.WriteVector(w, center / Map.MapScale);
        //    //w.Write(startAreaPrio);

        //    //w.Write(adjacentAreas.Count);
        //    //foreach (var m in adjacentAreas)
        //    //{
        //    //    m.WriteStream(w);
        //    //}

        //}

        //public void read(System.IO.BinaryReader r, int version)
        //{
        //    //center = SaveLib.ReadVector2(r) * Map.MapScale;
        //    //startAreaPrio = r.ReadInt32();

        //    //int adjacentCount = r.ReadInt32();
        //    //for (int i = 0; i < adjacentCount; ++i)
        //    //{
        //    //    adjacentAreas.Add(new AreaConnection(r, version, this, areas));
        //    //}
        //}

        public int Index()
        {
            for (int i = 0; i < moonRef.map.areas.Count; ++i)
            {
                if (moonRef.map.areas[i] == this)
                {
                    return i;
                }
            }

            return -1;
        }

        public  UnitPlacementNode houseNode(Players.House house)
        {
            return placementNodes[house.index];
        }
    }
}
