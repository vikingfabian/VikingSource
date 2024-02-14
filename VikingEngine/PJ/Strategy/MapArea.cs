using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Strategy
{
    class MapArea
    {
        public const int MaxStartPrio = 3;
        public static int NextIndex = 0;
        public int index;

        public Graphics.Image image;
        public Vector2 center;
        public List<AreaConnection> adjacentAreas = new List<AreaConnection>(4);
        public Gamer owner = null;
        public int ArmyCount = 0;
        public AreaType type = AreaType.Standard;
        public int startAreaPrio = -1;

        Graphics.Image army = null;
        Display.SpriteText armyCountText = null;
        public Graphics.ImageGroup resourceIcons = new Graphics.ImageGroup();
        public int areaLevel = 1;

        public MapArea()
        {
            index = NextIndex++;
        }

        public MapArea(Vector2 center)
        {
            this.center = center;
            createVisuals(null);
        }

        public void createVisuals(MapArea selectedArea)
        {
            image = new Graphics.Image(SpriteName.WhiteArea, center, new Vector2(32), ImageLayers.Background2, true);
            setInfoColor();

            foreach (var m in adjacentAreas)
            {
                m.createVisuals(selectedArea);
            }
        }

        public void createResourceIcons()
        {
            StrategyLib.SetMapLayer();
            resourceIcons.DeleteAll();

            image.Size1D = Engine.Screen.IconSize * 0.4f;
            SpriteName iconTile = SpriteName.NO_IMAGE;
            float iconSize = 0.65f;
            int resourceCount = areaLevel;

            switch (type)
            {
                case AreaType.Standard:
                    iconTile = SpriteName.birdCoin1;
                    break;
                case AreaType.VictoryPoint:
                    iconTile = SpriteName.winnerParticle;
                    iconSize = 1.0f;
                    break;
                case AreaType.Castle:
                    if (isStartArea)
                    {
                        iconTile = SpriteName.birdCoin1;
                        resourceCount = StrategyLib.StartAreaIncome;
                    }
                    image.Color = Color.Gray;
                    image.Size1D = Engine.Screen.IconSize * 1.1f;
                    break;
            }

            if (iconTile != SpriteName.NO_IMAGE)
            {
                for (int i = 0; i < resourceCount; ++i)
                {
                    var icon = new Graphics.Image(iconTile,
                        center + new Vector2(-Engine.Screen.IconSize * 0.1f, -Engine.Screen.IconSize * 0.5f),
                        new Vector2(Engine.Screen.IconSize * iconSize), 
                        ImageLayers.AbsoluteBottomLayer, true);
                    icon.LayerAbove(image);

                    icon.PaintLayer -= i * PublicConstants.LayerMinDiff;
                    icon.Xpos += i * Engine.Screen.IconSize * 0.26f;
                    resourceIcons.Add(icon);
                }
            }

        }

        public bool isStartArea
        {
            get {
                if (owner != null)
                {
                    return owner.startArea == this;
                }
                return false;
            }
        }

        void setInfoColor()
        {
            Color c = Color.Yellow;

            if (startAreaPrio >= 0)
            {
                c.G = (byte)(160 + startAreaPrio * 15);
            }

            image.Color = c;
        }

        public void fineAdjust(IntVector2 dir, MapArea selectedArea)
        {

            center += dir.Vec;
            image.Position = center;

            refreshVisuals(true, selectedArea);
        }

        public void connectToArea(MapArea adjacent, MapArea selectedArea)
        {
            if (!hasConnection(adjacent))
            {
                adjacentAreas.Add(new AreaConnection(selectedArea, this, adjacent));
                refreshVisuals(true, selectedArea);
                adjacent.connectToArea(this, selectedArea);
            }
        }

        bool hasConnection(MapArea toArea)
        {
            foreach (var m in adjacentAreas)
            {
                if (m.toArea == toArea)
                    return true;
            }
            return false;
        }

        void arrangeAreaConnections()
        {
            const float FirstAngle = MathHelper.Pi / 6f;

            //arrange them to follow clockwise rotation
            if (adjacentAreas.Count > 1)
            {
                List<AreaConnection> adjacentAreasArranged = new List<AreaConnection>(adjacentAreas.Count);
                while (adjacentAreas.Count > 1)
                {
                    FindMinValuePointer<AreaConnection> lowestAngle = new FindMinValuePointer<AreaConnection>();

                    foreach (var m in adjacentAreas)
                    {
                        Rotation1D angle = Rotation1D.FromDirection(m.arrowPos - center);
                        angle.Add(FirstAngle);
                        lowestAngle.Next(angle.Radians, m);
                    }

                    adjacentAreasArranged.Add(lowestAngle.minMember);
                    adjacentAreas.Remove(lowestAngle.minMember);
                }

                adjacentAreasArranged.Add(adjacentAreas[0]);
                adjacentAreas = adjacentAreasArranged;
            }
        }

        public AreaConnection getConnection(MapArea toArea)
        {
            foreach (var m in adjacentAreas)
            {
                if (m.toArea == toArea)
                    return m;
            }
            return null;
        }

        public void refreshVisuals(bool inEditor, MapArea selectedArea)
        {
            foreach (var m in adjacentAreas)
            {
                m.refreshVisuals(selectedArea);
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            arrangeAreaConnections();

            SaveLib.WriteVector(w, center / Map.MapScale);
            w.Write(startAreaPrio);

            w.Write(adjacentAreas.Count);
            foreach (var m in adjacentAreas)
            {
                m.write(w);
            }

        }

        public void read(System.IO.BinaryReader r, int version, List<MapArea> areas)
        {
            center = SaveLib.ReadVector2(r) * Map.MapScale;
            startAreaPrio = r.ReadInt32();

            int adjacentCount = r.ReadInt32();
            for (int i = 0; i < adjacentCount; ++i)
            {
                adjacentAreas.Add(new AreaConnection(r, version, this, areas));
            }
        }

        public void clearConnections(MapArea selectedArea)
        {
            foreach (var m in adjacentAreas)
            {
                m.toArea.removedConnection(this, selectedArea);
                m.deleteVisuals();
            }
            adjacentAreas.Clear();
            refreshVisuals(true, null);
        }

        public void removedConnection(MapArea area, MapArea selectedArea)
        {
            AreaConnection conn = getConnection(area);
            conn.deleteVisuals();
            adjacentAreas.Remove(conn);
            refreshVisuals(true, selectedArea);
        }

        public void DeleteMe()
        {
            clearConnections(null);
            image.DeleteMe();
        }

        public void nextStartPrio()
        {
            startAreaPrio++;
            if (startAreaPrio > MaxStartPrio)
            {
                startAreaPrio = -1;
            }
            setInfoColor();
        }

        public void placeArmy(Gamer newOwner, int count)
        {
            if (this.owner != newOwner)
            {
                if (this.owner != null)
                {
                    this.owner.areas.Remove(this);
                }
                this.owner = newOwner;
                arraylib.ListAddIfNotExist<MapArea>(newOwner.areas, this);

                if (resourceIcons != null)
                {
                    resourceIcons.SetVisible(isStartArea);
                }
            }
            ArmyCount = count;
            refreshArmyVisuals();


        }

        public void refreshArmyVisuals()
        {
            Debug.CrashIfThreaded();
            StrategyLib.SetMapLayer();

            if (owner != null)
            {
                if (army == null)
                {
                    Vector2 iconSz = new Vector2(Engine.Screen.IconSize * 1.2f);
                    army = new Graphics.Image(SpriteName.pigP1WingDown, center, iconSz, ImageLayers.Lay3, true);
                    army.Xpos -= army.Width * 0.4f;

                    armyCountText = new Display.SpriteText(ArmyCount.ToString(),
                        army.Position + new Vector2(army.Width * 0.4f, 0f), Engine.Screen.IconSize, ImageLayers.Lay2,
                        new Vector2(0, 0.5f), Color.White, true);
                }
                else
                {
                    armyCountText.Text(ArmyCount.ToString());
                }

                army.SetSpriteName(owner.animalSetup.wingUpSprite);
            }
        }

        public void gainResource(GamerDisplay display, Action winnerEvent)
        {
            if (isStartArea)
            {
                owner.Coins += StrategyLib.StartAreaIncome;
                new GainResourceAnimationEmitter(resourceIcons.images[0] as Graphics.Image, StrategyLib.StartAreaIncome);
                 display.refreshGamer(owner);
            }
            switch (type)
            {
                case AreaType.Standard:
                    owner.Coins += areaLevel;
                    new GainResourceAnimationEmitter(resourceIcons.images[0] as Graphics.Image, areaLevel);
                    display.refreshGamer(owner);
                    break;
                case AreaType.VictoryPoint:
                    owner.VictoryPoints += areaLevel;
                    new GainResourceAnimationEmitter(resourceIcons.images[0] as Graphics.Image, areaLevel);
                    display.refreshGamer(owner);

                    if (owner.VictoryPoints >= StrategyLib.WinnerVP)
                    {
                        winnerEvent();
                        return;
                    }
                    break;
            }
        }

        public AreaActionType canAffordAreaAction()
        {
            AreaActionType action = hasAreaAction();
            if (action != AreaActionType.NoAction)
            {
                if (actionCost() <= owner.Coins)
                {
                    return action;
                }
            }

            return AreaActionType.NoAction;
        }

        public AreaActionType hasAreaAction()
        {
            //can click on area to purchase upgrades and units
            if (type == AreaType.Castle)
            {    
                return AreaActionType.BuySoldier;
            }
            else if (areaLevel == 1)
            {
                if (type == AreaType.Standard)
                    return AreaActionType.UpgradeIncome;
                if (type == AreaType.VictoryPoint)
                    return AreaActionType.UpgradeVp;
            }

            return AreaActionType.NoAction;
        }

        public int actionCost()
        {
            switch (hasAreaAction())
            {
                case AreaActionType.BuySoldier:
                    return StrategyLib.SoldierCost;
                case AreaActionType.UpgradeIncome:
                    return StrategyLib.UpgradeIncomeCost;
                case AreaActionType.UpgradeVp:
                    return StrategyLib.UpgradeVpCost;
            }

            return int.MinValue;
        }

        public VectorRect selectionArea
        {
            get { return VectorRect.FromCenterSize(center, image.Size * 1.2f); }
        }

        public override string ToString()
        {
            return "Area" + index.ToString() + ", start prio:" + startAreaPrio.ToString();
        }
    }
}
