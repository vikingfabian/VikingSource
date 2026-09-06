using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.Map2;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{
    
    class MapEditHistory
    {
        const int RestoreLength = 32;
        List2<AbsRestorePoint> history = new List2<AbsRestorePoint>(RestoreLength);

        public void AddStorePoint(MapEditor2_Scene scene)
        {
            AbsRestorePoint restorePoint;
            if (scene.generator.currentPass >= Map.Map2.Map2Pass.Icon)
            {
                restorePoint = new IconRestorePoint(scene);
            }
            else if (scene.generator.currentPass == Map.Map2.Map2Pass.NodeGrid)
            {
                restorePoint = new NodeRestorePoint(scene);
            }
            else
            {
                return;
            }

            history.Add(restorePoint);
        }

        public void undo(MapEditor2_Scene scene)
        {
            if (history.Count > 0)
            {
                var restorePoint = history.RemoveLast();
                restorePoint.restore(scene);
            }
        }
    }

    abstract class AbsRestorePoint
    {
        protected Map.Map2.Map2Pass pass;

        public AbsRestorePoint(MapEditor2_Scene scene)
        { 
            pass = scene.generator.currentPass;
        }
        virtual public void restore(MapEditor2_Scene scene)
        { 
            scene.generator.currentPass = pass;
        }
    }

    class NodeRestorePoint: AbsRestorePoint
    {
        public NodeMap nodeMap;

        public NodeRestorePoint(MapEditor2_Scene scene)
            :base(scene)
        {
            nodeMap = scene.generator.nodeMap.CloneMe();
        }

        public override void restore(MapEditor2_Scene scene)
        {
            base.restore(scene);
            scene.generator.nodeMap = nodeMap;
        }
    }
    class IconRestorePoint: AbsRestorePoint
    {
        public IconWorldData iconWorldData;
        public IconRestorePoint(MapEditor2_Scene scene)
            : base(scene)
        {
            iconWorldData = scene.generator.iconWorld.CloneMe();
        }
        public override void restore(MapEditor2_Scene scene)
        {
            base.restore(scene);
            scene.generator.iconWorld = iconWorldData;
        }
    }
}
