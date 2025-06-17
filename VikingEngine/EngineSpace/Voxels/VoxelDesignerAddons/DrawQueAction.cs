using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.Voxels
{
    class DrawQueAction
    {
        public ushort material1, material2; 
        public PaintFillType fill; 
        //public PaintToolType shape; 
        public Dimensions toolDir;
        public IntVector3 mostRecentMoveXZ;
        public IntervalIntV3 volume;
        public IntVector3 keyDownDrawCoord;
        public int frame;
        public DrawQueType type;
        public bool dropSelection;
        public PaintSettings paintSettings;
        public bool allFrames;
        //public int pencilSize, roadEdge, roadPercentFill, roadBelowFill, roadAboveClear;
        //public bool roundPencil;

        public DrawQueAction(DrawQueType type)
        {
            this.type = type;
        }

        public DrawQueAction(bool dropSelection)
        {
            this.dropSelection = dropSelection;

            type = DrawQueType.StampSelection;
        }

        public DrawQueAction(ushort material, PaintFillType fill, PaintToolType tool, 
             IntVector3 keyDownDrawCoord, int frame)
        {
            this.frame = frame;
            this.material1 = material;
            this.fill = fill;
            this.paintSettings.drawTool = tool;
            
            this.keyDownDrawCoord = keyDownDrawCoord;
            

            this.type = DrawQueType.FillVolume;
        }

        public void endDraw(IntervalIntV3 volume, Dimensions toolDir, IntVector3 mostRecentMoveXZ)
        {
            this.volume = volume;
            this.toolDir = toolDir;
            this.mostRecentMoveXZ = mostRecentMoveXZ;
        }

        public DrawQueAction(ushort material1, ushort material2, PaintFillType fill, IntVector3 pos, PaintSettings paintSettings, bool allFrames)
        {
            this.fill = fill;
            this.material1 = material1;
            this.material2 = material2;
            keyDownDrawCoord = pos;
            this.paintSettings = paintSettings;
            this.allFrames = allFrames;

            this.type = paintSettings.drawTool == PaintToolType.Bucket? DrawQueType.Bucket : DrawQueType.Dot;
        }

        public void fillArea(AbsVoxelDesigner designer)
        {
            EditorDrawTools.FillArea(this, designer.selectedVoxels, designer.Settings.SelectionCut, designer); 
              
        }
    }

    enum DrawQueType
    {
        FillVolume,
        StampSelection,
        Dot,
        EndDot,
        Bucket,
    }
}
