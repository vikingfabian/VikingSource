using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using VikingEngine.PJ.PjEngine;

namespace VikingEngine.PJ
{
    class GamerData
    {
        static readonly Sound.SoundSettings JoinSound = new Sound.SoundSettings(LoadedSound.flap, 0.5f, 0, 0, 0f);
        static readonly Sound.SoundSettings NextCharacterSound = new Sound.SoundSettings(LoadedSound.flap, 0.4f, 0, 0, 0.2f);
        static readonly Sound.SoundSettings NextHatSound = new Sound.SoundSettings(LoadedSound.flap, 0.35f, 0, 0, 0.4f);

        public const float ChangeHatTimeMs = 600;
        Timer.Basic chageHatTimer = new Timer.Basic(ChangeHatTimeMs, true);
        float buttonDownDate;

        public Hat hat = Hat.NoHat;
        public IButtonMap button;

        public JoustAnimal joustAnimal;
        public CarAnimal carAnimal;

        public int Victories = 0;
        public int coins = 0;

        public int GamerIndex;
        public int team;

        public LobbyAvatar lobbyAvatar = null;
        public Joust.Gamer gamer = null;
        public Network.AbsNetworkPeer networkPeer = null;

        public void write(System.IO.BinaryWriter w)
        {
            button.write(w);
            w.Write((byte)joustAnimal);
            w.Write((byte)carAnimal);
            w.Write((byte)hat);
        }

        void read(System.IO.BinaryReader r, int version)
        {
            button = MapRead.Button(r);
            joustAnimal = (JoustAnimal)r.ReadByte();

            if (version >= 2)
            {
                carAnimal = (CarAnimal)r.ReadByte();
            }

            hat = (Hat)r.ReadByte();

        }

        public void netwrite(System.IO.BinaryWriter w)
        {
            w.Write((byte)joustAnimal);
            w.Write((byte)hat);
        }
        public void netread(Network.ReceivedPacket packet)
        {
            networkPeer = packet.sender;

            joustAnimal = (JoustAnimal)packet.r.ReadByte();
            hat = (Hat)packet.r.ReadByte();
            refreshHat();
        }

        public GamerData()
        {
            joustAnimal = JoustAnimal.NUM_NON;
            hat = Hat.NUM_NON;
        }

        public GamerData(IButtonMap button, JoustAnimal animal)
        {
            this.button = button;
            this.joustAnimal = animal;
        }

        public GamerData(Network.ReceivedPacket packet)
        {
            netread(packet);
        }

        public GamerData(System.IO.BinaryReader r, int version)
        {
            read(r, version);
        }

        public GamerData(IButtonMap button, LobbyState lobby)
        {
            var allGamers = arraylib.MergeArrays(lobby.joinedLocalGamers, lobby.joinedRemoteGamerData);

            JoinSound.PlayFlat();
            GamerIndex = lobby.joinedLocalGamers.Count;
            this.button = button;


            joustAnimal = arraylib.RandomListMember(lobby.avatarSetup.availableJoustAnimals);
            carAnimal = arraylib.RandomListMember(lobby.avatarSetup.availableCarAnimals);
            
            if (PjRef.storage.joinedGamersSetup != null)
            {
                foreach (var g in PjRef.storage.joinedGamersSetup)
                {
                    if (InputLib.Equals(g.button, this.button))
                    {
                        
                        this.joustAnimal = g.joustAnimal;
                        this.carAnimal = g.carAnimal;
                        this.hat = g.hat;
                        break;
                    }
                }

            }

            nextAvailableAppearance(lobby, false);
            
        }

        bool availableAppearance(int avatar, ModeAvatarSetup avatarSetup, List<GamerData> allGamers)
        {
            foreach (var g in allGamers)
            {
                if (g != this)
                {
                    if (g.usingAvatar(avatar, PjRef.storage.modeSettings.avatarType))
                    { return false; }
                }
            }

            return true;
        }

        bool usingAvatar(int avatar, ModeAvatarType type)
        {
            return avatar == getAvatar(type);
        }

        int getAvatar(ModeAvatarType type)
        {
            int selectedAvatar;

            if (type == PjEngine.ModeAvatarType.Car)
            {
                selectedAvatar = (int)carAnimal;
            }
            else
            {
                selectedAvatar = (int)joustAnimal;
            }

            return selectedAvatar;
        }
        
        public void lobbyUpdate(LobbyState lobby)
        {
            lobbyAvatar.removeIconHighlight.Visible = false;

            lobbyAvatar.highlight.Visible = false;

            if (lobbyAvatar.lifeTime++ > 2)
            {
                if (lobbyAvatar.removeArea.IntersectPoint(Input.Mouse.Position))
                {
                    lobbyAvatar.removeIconHighlight.Visible = true;
                    if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                    {
                        //Remove this player
                        lobby.removeFromLobby(this);
                        return;
                    }
                }
                else
                {
                    if (lobbyAvatar.area.IntersectPoint(Input.Mouse.Position))
                    {
                        lobbyAvatar.highlight.Visible = true;
                        if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                        {
                            appearanceDownEvent();
                        }
                        else if (Input.Mouse.ButtonUpEvent(MouseButton.Left))
                        {
                            appearanceUpEvent(lobby);
                        }
                        else if (Input.Mouse.IsButtonDown(MouseButton.Left))
                        {
                            appearanceButtonHold(lobby.avatarSetup.availableHats);
                        }
                    }
                }

                if (!button.IsMouse)
                {
                    if (button.DownEvent)
                    {
                        appearanceDownEvent();
                    }
                    else if (button.UpEvent)
                    {
                        appearanceUpEvent(lobby);
                    }
                    else if (button.IsDown)
                    {
                        appearanceButtonHold(lobby.avatarSetup.availableHats);
                    }
                }
            }

            lobbyAvatar.updateButtonSize(button);
            if (lobbyAvatar.hatImage != null)
            {
                lobbyAvatar.hatImage.update();
            }
        }



        void appearanceDownEvent()
        {
            chageHatTimer.Reset();
            buttonDownDate = Ref.TotalTimeSec;
        }
        void appearanceUpEvent(LobbyState lobby)
        {

            if ((Ref.TotalTimeSec - buttonDownDate) < 0.5f)
            {
                nextAvailableAppearance(lobby, true);
            }
        }
        void appearanceButtonHold(List<Hat> availableHats)
        {
            if (chageHatTimer.Update())
            {
                //next hat
                if (availableHats.Count > 1)
                {
                    int hatIndex = 0;
                    for (int i = 0; i < availableHats.Count; ++i)
                    {
                        if (availableHats[i] == hat)
                        {
                            hatIndex = i;
                            break;
                        }
                    }

                    hatIndex++;
                    if (hatIndex >= availableHats.Count)
                    { hatIndex = 0; }

                    hat = availableHats[hatIndex];
                    refreshHat();

                    NextHatSound.PlayFlat();

                }
            }
        }

        void nextAvailableAppearance(LobbyState lobby, bool moveToNext)
        {
            int avatar = getAvatar(PjRef.storage.modeSettings.avatarType);
            //if (avatar >= lobby.avatarSetup.availableAvatars.Count)
            //{
            //    getAvatar(PjRef.storage.modeSettings.avatarType);
            //}

            lobby.avatarSetup.availableAvatars.SelectItem(avatar);
            
            if (moveToNext)
            {
                lobby.avatarSetup.availableAvatars.selectNext_Rollover();
            }

            while (availableAppearance(lobby.avatarSetup.availableAvatars.sel, lobby.avatarSetup, lobby.joinedLocalGamers) == false)
            {
                lobby.avatarSetup.availableAvatars.selectNext_Rollover();
            }

            if (PjRef.storage.modeSettings.avatarType == PjEngine.ModeAvatarType.Car)
            {
                carAnimal = (CarAnimal)lobby.avatarSetup.availableAvatars.sel;
            }
            else
            {
                joustAnimal = (JoustAnimal)lobby.avatarSetup.availableAvatars.sel;
            }
            
            if (lobbyAvatar == null)
            {
                lobbyAvatar = new LobbyAvatar(this, true);
            }
            else
            {
                lobbyAvatar.refreshVisuals();
            }
            refreshHat();

            if (moveToNext)
            {
                lobbyAvatar.bumpMotion();
                NextCharacterSound.PlayFlat();
            }
        }

        public void refreshLobbyPositon(int index, int gamersCount, PjEngine.ModeAvatarSetup avatarSetup)
        {
            GamerIndex = index;
            lobbyAvatar.DeleteMe();
            lobbyAvatar = new LobbyAvatar(this, index == gamersCount-1);
            refreshHat();
        }

        public void resetScore()
        {
            Victories = 0; coins = 0;
        }

        public int Score()
        {
            return Victories * 1000 + coins;
        }
        
        void refreshHat()
        {
            lobbyAvatar?.refreshVisuals();

            if (Ref.gamestate is LobbyState)
            {
                ((LobbyState)Ref.gamestate).onLocalJoinedGamerChange();
            }
        }

        public bool LeftTeam
        {
            get { return team == 0; }
        }

        public static List<GamerData> OrderPlayers(List<GamerData> joinedGamers)
        {
            List<GamerData> order = new List<GamerData> { joinedGamers[0] };

            for (int i = 1; i < joinedGamers.Count; ++i)
            {
                int score = joinedGamers[i].Score();
                int insertIx;
                for (insertIx = 0; insertIx < order.Count; ++insertIx)
                {
                    if (score > order[insertIx].Score())
                    {
                        break;
                    }
                }
                order.Insert(insertIx, joinedGamers[i]);
            }

            return order;
        }

        public static void SetLeftRightTeams(List<GamerData> joinedGamers)
        {
            int team = 0;

            for (int i = 0; i < joinedGamers.Count; ++i)
            {
                joinedGamers[i].team = team;
                team = lib.AlternateBetweenTwoValues(team, 0, 1);
            }
        }
    }

    enum AnimalType { Bird, Pig, Fish, Sheep, NUM, };
}
