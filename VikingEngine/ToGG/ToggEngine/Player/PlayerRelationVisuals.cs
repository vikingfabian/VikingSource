using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG
{
    struct PlayerRelationVisuals
    {
        public Color attackWheelInfoCol;
        public Color shadowColor;
        public Color restIconColor;
        public SpriteName shield;
        public bool faceRight;

        //public PlayerRelationVisuals(AbsLobbyMember m, int ix, int localPlayerCount)
        //{
        //    if (m is LocalLobbyMember)
        //    {
        //        restIconColor = Color.White;
        //        shadowColor = Color.Black;
        //        attackWheelInfoCol = Color.LightBlue;

        //        if (toggRef.mode == GameMode.HeroQuest)
        //        {
        //            faceRight = true;
        //        }
        //        else
        //        {
        //            faceRight = ix == 0;
        //        }

        //        if (localPlayerCount > 1)
        //        {
        //            shield = ix == 0 ? SpriteName.cmdShieldP1 : SpriteName.cmdShieldP2;
        //        }
        //        else
        //        {
        //            shield = SpriteName.NO_IMAGE;
        //        }
        //    }
        //    else
        //    {
        //        shadowColor = Color.DarkRed;
        //        attackWheelInfoCol = Color.Pink;
        //        restIconColor = new Color(1f, 0.7f, 0.7f);
        //        faceRight = false;
        //        shield = SpriteName.cmdShieldP2;
        //    }

        //    m.relationVisuals = this;
        //}

        public void setLocalUser()
        {
            restIconColor = Color.White;
            shadowColor = Color.Black;
            attackWheelInfoCol = Color.LightBlue;
            faceRight = true;
            shield = SpriteName.NO_IMAGE;
        }

        public void setAlly()
        {
            restIconColor = Color.White;
            shadowColor = Color.DarkBlue;
            attackWheelInfoCol = Color.LightBlue;
            faceRight = true;
            shield = SpriteName.cmdShieldP1;
        }

        public void setEnemy()
        {
            shadowColor = Color.DarkRed;
            attackWheelInfoCol = Color.Pink;
            restIconColor = new Color(1f, 0.7f, 0.7f);
            faceRight = false;
            shield = SpriteName.cmdShieldP2;
        }

        public static PlayerRelationVisuals Empty()
        {
            PlayerRelationVisuals result = new PlayerRelationVisuals();
            result.shield = SpriteName.MissingImage;
            result.attackWheelInfoCol = Color.Purple;
            result.shadowColor = Color.Purple;

            return result;
        }
    }

}
