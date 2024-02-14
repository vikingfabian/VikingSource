using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.BlockMap
{
    class LockGroup
    {
        public int connectedGameObjects = 0;
        public int collectedGameObjects = 0;
        GO.EnvironmentObj.AreaLock arealock = null;

        public void AddConnectedGo(GO.AbsUpdateObj go)
        {
            connectedGameObjects++;
            new LockConnectedEnemy(go, this);
            refreshPlayersHud(go.Position);
        }

        public void AddAreaLock(GO.EnvironmentObj.AreaLock arealock)
        {
            this.arealock = arealock;
            unlockCheck();
            //if (connectedGameObjects <= 0)
            //{
            //    arealock.openLock(); 
            //}
            refreshPlayersHud(arealock.Position);
        }

        public void ConnectedGoCollectedByPlayer(GO.AbsUpdateObj enemy)
        {
            collectedGameObjects++;
            unlockCheck();
            //if (++collectedGameObjects >= connectedGameObjects)
            //{
            //    if (arealock != null)
            //    {
            //        arealock.openLock();
            //    }
            //}
            refreshPlayersHud(enemy.Position);
        }

        void unlockCheck()
        {
            if (arealock != null)
            {
                arealock.isUnlocked = collectedGameObjects >= connectedGameObjects && connectedGameObjects > 0;
            }
        }

        void refreshPlayersHud(Vector3 actionCenter)
        {
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
            {
                if (VectorExt.SideLength(LfRef.LocalHeroes[i].Position, actionCenter) < 48)
                {
                    HeroHUDRefresh(LfRef.LocalHeroes[i]);
                }
            }
        }

        public void HeroHUDRefresh(VikingEngine.LootFest.GO.PlayerCharacter.AbsHero h)
        {
            if (h.player.interactDisplay == null || !(h.player.interactDisplay is Display.LockGroupHUD))
            {
                h.player.deleteInteractDisplay();
                h.player.interactDisplay = new Display.LockGroupHUD(this);
            }
            h.player.interactDisplay.refresh(h.player, this);
        }
    }

    /// <summary>
    /// Reports to a lock if the enemy is killed
    /// </summary>
    class LockConnectedEnemy : AbsUpdateable
    {
        GO.AbsUpdateObj enemy;
        LockGroup lockGroup;
        Graphics.Mesh connectionIcon = null;

        public LockConnectedEnemy(GO.AbsUpdateObj enemy, LockGroup lockGroup)
            : base(true)
        {
            this.enemy = enemy;
            this.lockGroup = lockGroup;

            connectionIcon = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, new Vector3(0.4f),
                Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea, Color.DarkGray);
                //new Graphics.TextureEffect(Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea, Color.DarkGray),
                //0.4f);
            Time_Update(0);
        }

        public override void Time_Update(float time)
        {
            if (enemy.IsDeleted)
            {
                lockGroup.ConnectedGoCollectedByPlayer(enemy);
                this.DeleteMe();
            }
            else
            {
                //connectionIcon.Position = enemy.HeadPosition;
                //connectionIcon.Y += 2f;

                connectionIcon.Position = enemy.expressionEffectPos();
                connectionIcon.Y += 1f;

                connectionIcon.Rotation.RotateWorldX(0.002f * time);
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            connectionIcon.DeleteMe();
        }
    }
}
