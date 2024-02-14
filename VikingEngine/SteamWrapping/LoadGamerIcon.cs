using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.SteamWrapping
{
    class LoadGamerIcon : LazyUpdate
    {
        static Texture2D storedNullGamerIcon = null;

#if PCGAME
        SteamImageLoadData steamImage;
#endif
        Graphics.ImageAdvanced image;
        Network.AbsNetworkPeer gamer;
        bool local;
        Texture2D tex;


        int trials = 0;
        Timer.Basic trialTimer = new Timer.Basic(500, true);


        public LoadGamerIcon(Graphics.ImageAdvanced image, Engine.AbsPlayerData pData)
           : this(image, pData.netPeer(), pData.Type != Engine.PlayerType.Remote)
        { }

        public LoadGamerIcon(Graphics.ImageAdvanced image, Network.AbsNetworkPeer gamer, bool local)
            :base(false)
        {
            this.local = local;
            image.SetSpriteName(SpriteName.defaultGamerIcon);

            this.image = image;
            this.gamer = gamer;

            if (gamer == null && storedNullGamerIcon != null)
            {
                tex = storedNullGamerIcon;
                setTexture();
            }
            else if (gamer != null && gamer.storedGamerIcon != null)
            {
                tex = gamer.storedGamerIcon;
                setTexture();
            }
            else if (TryLoadSteamImage() == false)
            {
                AddToUpdateList();
            }
        }

        public override void Time_Update(float time_ms)
        {
            if (image.IsDeleted)
            {
                DeleteMe();
            }

            if (trialTimer.Update(time_ms))
            {
                if (TryLoadSteamImage() || ++trials >= 6)
                {
                    DeleteMe();
                }
            }
        }

        bool TryLoadSteamImage()
        {
#if PCGAME
            var steamPeer = gamer as SteamNetworkPeer;

            if (local)
            {
                if (Ref.steam.isInitialized)
                {
                    steamImage = SteamNetworkPeer.GetLocalGamerIcon184x184();
                }
            }
            else
            {
                if (steamPeer != null)
                {
                    steamImage = steamPeer.GetGamerIcon184x184();
                }
            }

            if (steamImage.state == SteamImageLoadState.ImageLoadedCorrectly)
            {
                tex = steamImage.texture;
                if (steamPeer == null)
                {
                    storedNullGamerIcon = steamImage.texture;
                }
                else
                {
                    steamPeer.storedGamerIcon = steamImage.texture;
                }
                setTexture();

                return true;
            }
#endif
            return false;
        }

        void setTexture()
        {
            image.Texture = tex;
            image.SetFullTextureSource();
        }
    }
}
