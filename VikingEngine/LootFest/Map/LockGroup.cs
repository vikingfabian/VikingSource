using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Map
{
    //class LockGroup
    //{
    //    public int connectedGameObjects = 0;
    //    public int collectedGameObjects = 0;
    //    GO.EnvironmentObj.AreaLock arealock = null;

    //    public void AddConnectedGo(GO.AbsUpdateObj go)
    //    {
    //        connectedGameObjects++;
    //        new LockConnectedEnemy(go, this);
    //        refreshPlayersHud(go.Position);
    //    }

    //    public void AddAreaLock(GO.EnvironmentObj.AreaLock arealock)
    //    {
    //        this.arealock = arealock;
    //        unlockCheck();
    //        //if (connectedGameObjects <= 0)
    //        //{
    //        //    arealock.openLock(); 
    //        //}
    //        refreshPlayersHud(arealock.Position);
    //    }

    //    public void ConnectedGoCollectedByPlayer(GO.AbsUpdateObj enemy)
    //    {
    //        collectedGameObjects++;
    //        unlockCheck();
    //        //if (++collectedGameObjects >= connectedGameObjects)
    //        //{
    //        //    if (arealock != null)
    //        //    {
    //        //        arealock.openLock();
    //        //    }
    //        //}
    //        refreshPlayersHud(enemy.Position);
    //    }

    //    void unlockCheck()
    //    {
    //        if (arealock != null)
    //        {
    //            arealock.isUnlocked = collectedGameObjects >= connectedGameObjects && connectedGameObjects > 0;
    //        }
    //    }

    //    void refreshPlayersHud(Vector3 actionCenter)
    //    {
    //        for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
    //        {
    //            if (lib.VectorSideLength(LfRef.LocalHeroes[i].Position, actionCenter) < 48)
    //            {
    //                HeroHUDRefresh(LfRef.LocalHeroes[i]);
    //            }
    //        }
    //    }

    //    public void HeroHUDRefresh(VikingEngine.LootFest.GO.PlayerCharacter.AbsHero h)
    //    {
    //        if (h.Player.interactDisplay == null || !(h.Player.interactDisplay is Display.LockGroupHUD))
    //        {
    //            h.Player.deleteInteractDisplay();
    //            h.Player.interactDisplay = new Display.LockGroupHUD(this);
    //        }
    //        h.Player.interactDisplay.refresh(h.Player, this);
    //    }
    //}

    ///// <summary>
    ///// Reports to a lock if the enemy is killed
    ///// </summary>
    //class LockConnectedEnemy : AbsUpdateable
    //{
    //    GO.AbsUpdateObj enemy;
    //    LockGroup lockGroup;
    //    Graphics.Mesh connectionIcon = null;
        
    //    public LockConnectedEnemy(GO.AbsUpdateObj enemy, LockGroup lockGroup)
    //        :base(true)
    //    {
    //        this.enemy = enemy;
    //        this.lockGroup = lockGroup;

    //        connectionIcon = new Graphics.Mesh( LoadedMesh.cube_repeating, Vector3.Zero, 
    //            new Graphics.TextureEffect( Graphics.TextureEffectType.LambertFixed, SpriteName.WhiteArea, Color.DarkGray),
    //            0.4f);
    //        Time_Update(0);
    //    }

    //    public override void Time_Update(float time)
    //    {
    //        if (enemy.IsDeleted)
    //        {
    //            lockGroup.ConnectedGoCollectedByPlayer(enemy);
    //            this.DeleteMe();
    //        }
    //        else
    //        {
    //            connectionIcon.Position = enemy.HeadPosition;
    //            connectionIcon.Y += 2f;

    //            connectionIcon.Rotation.RotateWorldX(0.002f * time);
    //        }
    //    }

    //    public override void DeleteMe()
    //    {
    //        base.DeleteMe();
    //        connectionIcon.DeleteMe();
    //    }
    //}
}
