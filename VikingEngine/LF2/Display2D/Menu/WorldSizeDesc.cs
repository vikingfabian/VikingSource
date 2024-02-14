using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.LF2
{
    struct WorldSizeDescData //: HUD.IMemberData
    {
        string folder;

        public WorldSizeDescData(string folder)
        {
            this.folder = folder;
        }
        //public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
        //{
        //    return new WorldSizeDesc(args, folder);
        //}
        public string LinkCaption { get { return null; } }
    }

    //class WorldSizeDesc : HUD.AbsBlockMenuMember, IQuedObject, IUpdateable
    //{
    //    string folder;
    //    bool loading = true;
    //    Timer.Basic dotTimer = new Timer.Basic(500);
    //    string szLabel;
    //    TextBoxSimple text;
    //    const float Edge = 10;

    //    public WorldSizeDesc(MemberDataArgs args, string folder)
    //        : base(args, null, null)
    //    {
    //        this.folder = folder;
    //        Engine.Storage.AddToSaveQue(StartQuedProcess, false);

    //        this.text = new TextBoxSimple(args.textformat.Font, background.Position, args.textformat.Scale, Align.Zero,
    //            "Loading info", args.textformat.Color, args.layer, args.width - Edge);
    //        background.Height = text.MesureText().Y;
    //        this.text.Xpos += Edge;

    //        Ref.update.AddToUpdate(this, true);
    //    }

    //    override public UpdateType UpdateType { get { return UpdateType.Full; } }

    //    public override void Time_Update(float time)
    //    {
    //        if (loading)
    //        {
    //            if (dotTimer.Update(time))
    //            {
    //                dotTimer.Reset();
    //                text.TextString += ".";
    //            }
    //        }
    //        else
    //        {
    //            text.TextString = szLabel;
    //        }
    //    }
    //    public void Time_LasyUpdate(float time) { }

    //    public void StartQuedProcess(bool saveThread)
    //    {
    //        szLabel = DataLib.SaveLoad.DirectorySizeLabel(folder);
    //        loading = false;
    //    }

    //    public override float Height
    //    {
    //        get
    //        {
    //            return background.Height - 2;
    //        }
    //    }
    //    public override void GoalY(float y, bool set)
    //    {
    //        base.GoalY(y, set);
    //        text.Ypos = y + 5;
    //    }

    //    public override void DeleteMe()
    //    {
    //        base.DeleteMe();
    //        text.DeleteMe();
    //    }
    //    public override bool Visible
    //    {
    //        set
    //        {
    //            base.Visible = value;
    //            text.Visible = value;
    //        }
    //    }
    //    //public override float Transparentsy
    //    //{
    //    //    get
    //    //    {
    //    //        return base.Transparentsy;
    //    //    }
    //    //    set
    //    //    {
    //    //        base.Transparentsy = value;
    //    //        text.Transparentsy = value;

    //    //    }
    //    //}
    //    public override HUD.ClickFunction Function
    //    {
    //        get { return HUD.ClickFunction.NotSelectable; }
    //    }
    //    public override bool SavingThread { get { return false; } }
    //}
}
