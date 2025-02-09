using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.PjEngine
{
    class ModeAvatarSetup
    {
        public List<CarAnimal> availableCarAnimals = null;
        public List<JoustAnimal> availableJoustAnimals = null;
        
        public List<Hat> availableHats = null;
        public List2<int> availableAvatars = null;

        public ModeAvatarSetup()
        {
            allAvatarsSetup();

            onModeChanged();
        }

        public void onModeChanged()
        {
            switch (PjRef.storage.modeSettings.avatarType)
            {
                case ModeAvatarType.Joust:
                    availableAvatars = arraylib.CastValueL2<JoustAnimal, int>(availableJoustAnimals);
                    break;
                case ModeAvatarType.Car:
                    availableAvatars = arraylib.CastValueL2<CarAnimal, int>(availableCarAnimals);
                break;
            }            
        }

        public void add(JoustAnimal animal)
        {
            availableJoustAnimals.Add(animal);
            onModeChanged();
        }

        public void replace(JoustAnimal from, JoustAnimal to)
        {
            arraylib.ReplaceValue(availableJoustAnimals, from, to);
            
            onModeChanged();
        }

        void allAvatarsSetup()
        {
            bool Dlc1Characters = false;
            bool Dlc2Bling = false;
            bool DlcZombie = false;


            availableCarAnimals = new List<CarAnimal> {
                CarAnimal.ChickenYellow,
                CarAnimal.FishOrange,
                CarAnimal.PigPink,
                CarAnimal.SheepWhite,

                CarAnimal.ChickenPink,
                CarAnimal.FishGreen,
                CarAnimal.PigOrange,
                CarAnimal.SheepBlack,
            };

#if PCGAME
            if (Ref.steam.isInitialized)
            {
                var dlc = Ref.steam.DLC;
                if (dlc != null)
                {
                    Dlc1Characters = dlc.JoustingCharacterPack;
                    Dlc2Bling = dlc.JoustingBlingPack;
                    DlcZombie = dlc.JoustingZombiePack;

                    if (PlatformSettings.DevBuild)
                    {
                        Debug.Log("===DLC UNLOCKED (" + dlc.DlcCount_FromApi.ToString() + "): " + Dlc1Characters.ToString() + ", " + Dlc2Bling.ToString() + " ===");
                    }
                }
            }
#elif XBOX
            Dlc1Characters = true;
            Dlc2Bling = false;
            DlcZombie = false;
#else
             throw new NotImplementedException();
#endif

            availableHats = new List<Hat> { Hat.NoHat };

            //if (PlatformSettings.DebugLevel < BuildDebugLevel.Release)
            //{
                Dlc1Characters = true;
            //}
                        
            if (Dlc1Characters || Dlc2Bling)
            {
                availableJoustAnimals = new List<JoustAnimal>
                {
                    JoustAnimal.Bird1,
                    JoustAnimal.Pig1,
                    JoustAnimal.Fish1,
                    JoustAnimal.SheepWhite,

                    JoustAnimal.Bird2,
                    JoustAnimal.Pig2,
                    JoustAnimal.Fish2,
                    JoustAnimal.Sheep2,

                    JoustAnimal.Bird3,
                    JoustAnimal.Pig3,
                    JoustAnimal.Fish3,
                    JoustAnimal.SheepPink,

                    JoustAnimal.Bird4,
                    JoustAnimal.Pig4,
                    JoustAnimal.Fish4,
                    JoustAnimal.Sheep4,

                    //--
                    JoustAnimal.CatRed,
                    JoustAnimal.Goat,
                    JoustAnimal.DogBrown,
                    JoustAnimal.CowSpotty,

                    JoustAnimal.CatBlue,
                    JoustAnimal.HamsterOrange,
                    JoustAnimal.Pug,
                    JoustAnimal.CowBrown,                    

                    JoustAnimal.PigVoid,
                    JoustAnimal.PugVoid,

                    JoustAnimal.CatRainbow,
                    JoustAnimal.SheepRainbow,
                    JoustAnimal.HamsterPink,
                };

                availableHats.AddRange(
                    new List<Hat>
                    {
                        Hat.High,
                        Hat.Viking,
                        Hat.Fez,
                        Hat.Pirate,
                        Hat.Cowboy,
                        Hat.Indian,
                        Hat.Vlc,
                        Hat.Halo,
                        Hat.RobinHood,
                        Hat.ChildCap,
                        Hat.English,
                        Hat.GooglieEyes,
                        Hat.SkyMask,
                        Hat.Bow,
                        Hat.Bunny,
                        Hat.Butterix,
                        Hat.Scotish,
                        Hat.Frank,
                        Hat.Riddler,
                        Hat.Shades,
                        Hat.HeartEyes,
                        Hat.UniHorn,
                        Hat.HighRainbow,
                    });

                if (Dlc2Bling)
                {
                    availableJoustAnimals.AddRange(new List<JoustAnimal>
                    {
                        JoustAnimal.PigBling,
                        JoustAnimal.FishBling,
                    });

                    availableHats.AddRange(
                    new List<Hat>
                    {
                        Hat.BlingCap,
                        Hat.King,
                        Hat.Princess,
                        
                    });
                }
            }
            else
            { //No DLC
                availableJoustAnimals = new List<JoustAnimal>
                {
                    JoustAnimal.Bird1,
                    JoustAnimal.Pig1,
                    JoustAnimal.Fish1,
                    JoustAnimal.SheepWhite,

                    JoustAnimal.Bird2,
                    JoustAnimal.Pig2,
                    JoustAnimal.Fish2,
                    JoustAnimal.Sheep2,

                    JoustAnimal.Bird3,
                    JoustAnimal.Pig3,
                    JoustAnimal.Fish3,
                    JoustAnimal.SheepPink,

                    JoustAnimal.Bird4,
                    JoustAnimal.Pig4,
                    JoustAnimal.Fish4,
                    JoustAnimal.Sheep4,
                };
            }

            if (DlcZombie)
            {
                availableJoustAnimals.AddRange(new List<JoustAnimal>
                   {
                        JoustAnimal.BirdZombie,
                        JoustAnimal.PigZombie,
                        JoustAnimal.FishZombie,
                        JoustAnimal.SheepZombie,
                   });
            }
        }
    }

    enum ModeAvatarType
    {
        Joust,
        Car,
        NUM_NON
    }
}
