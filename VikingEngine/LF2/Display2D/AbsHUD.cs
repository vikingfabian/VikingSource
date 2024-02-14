using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2
{
    /*
     * hudden ska vara dynamisk så man kan lägga till och dra ifrån, och bitarna lägger sig rätt
     * när man lägger till nya ska dom gamla bitarna röra sig mjukt
     * i splitscreen eller i låg res (eller vid val), ska dom gå undan när dom inte är aktuella
     */
    abstract class AbsHUD : LazyUpdate
    {
        protected const ImageLayers HUDLayer = ImageLayers.Lay7;
        protected const float HUDStandardHeight = 32;
        public const float HUDRowHeight = HUDStandardHeight + 8;

        protected Graphics.Image icon;

        //public AbsHUD(VoxelModelName iconName)
        //    :base(false)
        //{
        //    icon = LootfestLib.Images.GetVoxelIcon(iconName, Vector2.Zero, new Vector2(HUDStandardHeight), HUDLayer);
        //}
        public AbsHUD(SpriteName iconTile)
            : base(false)
        {
            icon = new Image(iconTile, Vector2.Zero, new Vector2(HUDStandardHeight), HUDLayer, false);
            //icon = LootfestLib.Images.GetVoxelIcon(iconName, position, , ImageLayers.Lay4);
        }
        virtual public Vector2 Position
        {
            set
            {
                icon.Position = value;
            }
        }
        virtual public bool Visible
        {
            set { icon.Visible = value; }
        }
        abstract public float Width
        {
            get;
        }
        public float Right
        {
            get { return icon.Xpos + Width; }
        }

        override public void DeleteMe()
        { icon.DeleteMe(); }
        override public bool IsDeleted
        {
            get { return icon.IsDeleted; }
        }
        public override void Time_Update(float time)
        {
            throw new NotImplementedException();
        }
    }
    
}
