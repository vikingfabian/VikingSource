using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Net;

namespace VikingEngine.LootFest.Creation.PvP
{
    #if CMODE
    //class FatKing
    //{
    //    bool hosting = false;
    //    public bool Hosting
    //    {
    //        get { return hosting; }
    //    }
    //    Team red, blue;
    //    IntVector2 startPos;
    //    //IntVector2 redSpawn;
    //    //IntVector2 redSpawn;
    //    public FatKing(PacketReader r)
    //    {
    //        hosting = false;

    //        basicInit(Map.WorldPosition.ReadChunkGrindex(r));

            
    //    }
    //    public void SetClientKing(King king, bool blue)
    //    {
    //        GetTeam(blue).king = king;
    //    }
    //    public FatKing(IntVector2 startPos)
    //    {
    //        hosting = true;

    //        basicInit(startPos);
    //    }

    //    void basicInit(IntVector2 center)
    //    {
    //        red = new Team(center, -1, false, hosting);
    //        blue = new Team(center, 1, true, hosting);
    //        this.startPos = center;
    //    }

    //    public void DeleteMe()
    //    {
    //        if (hosting)
    //        {
    //            if (red != null)
    //                red.DeleteMe();
    //            if (blue != null)
    //                blue.DeleteMe();
    //        }
    //    }
    //    public IntVector2 GetSpawn(bool blue)
    //    {
    //        return GetTeam(blue).Spawn;
    //    }

    //    public King GetKing(bool blue)
    //    {
    //        return GetTeam(blue).king;
    //    }
    //    Team GetTeam(bool blueColor)
    //    {
    //        return (blueColor ? blue : red);
    //    }
    //    public static Data.MaterialType TeamColor(bool blue)
    //    {
    //        return blue ? Data.MaterialType.Blue : Data.MaterialType.RedBrown;
    //    }
    //}
    //enum Proffesion
    //{
    //    Warrior,
    //    Miner,
    //}
    //class Team
    //{
    //    public IntVector2 Spawn;
    //    public King king;

    //    public Team(IntVector2 center, int dir , bool blue, bool hosted)
    //    {
    //        const int AreaRadius = 2;
    //        this.Spawn = center;
    //        Spawn.X = (short)(Spawn.X +dir * AreaRadius);

    //        if (hosted)
    //        {
    //            Map.WorldPosition wp = new Map.WorldPosition();
    //            wp.ScreenIndex = Spawn;
    //            king = new King(wp, blue);
    //        }
            

    //        //will spawn behind the king
    //        Spawn.X = (short)(Spawn.X + dir);
    //    }
    //    public void DeleteMe()
    //    {
    //        king.DeleteMe();
    //    }
    //}
#endif
}
