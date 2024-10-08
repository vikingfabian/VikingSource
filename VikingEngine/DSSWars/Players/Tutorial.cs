using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.Timer;
using VikingEngine.ToGG.Commander.UnitsData;
using VikingEngine.ToGG.ToggEngine.QueAction;

namespace VikingEngine.DSSWars.Players
{
    partial class LocalPlayer
    {
        //TutorialMission tutorialMission = 0;
        //public bool inTutorialMode;
        //public bool tutorialMission_BuySoldier = false;
        //public bool tutorialMission_MoveArmy = false;

        public PlayerControls.Tutorial tutorial = null;

        public void InitTutorial(bool newGame) 
        {
            if (newGame && DssRef.storage.runTutorial)
            { 
                tutorial = new PlayerControls.Tutorial(this);
            }
            //inTutorialMode = false;
            //mapControls.setZoomRange(inTutorialMode);
        }

        public void tutorial_writeGameState(BinaryWriter w)
        {
            //w.Write(inTutorialMode);
            //w.Write((int)tutorialMission);
            //w.Write(tutorialMission_BuySoldier);
            //w.Write(tutorialMission_MoveArmy);
            if (tutorial != null)
            {
                w.Write(true);
                tutorial.tutorial_writeGameState(w);
            }
            else
            { w.Write(false); }
        }

        public void tutorial_readGameState(BinaryReader r, int subversion)
        {
            if (subversion >= 7)
            {
                bool inTutorialMode = r.ReadBoolean();
                if (subversion < 15)
                {
                    bool non1 = r.ReadBoolean();
                    bool non2 = r.ReadBoolean();
                }

                if (inTutorialMode)
                {
                    tutorial = new PlayerControls.Tutorial(this);
                    tutorial.tutorial_readGameState(r, subversion);
                }
            }
        }

        

        //public void onBuySoldier()
        //{
        //    if (inTutorialMode && !tutorialMission_BuySoldier)
        //    {
        //        tutorialMission_BuySoldier = true;
        //        hud.needRefresh = true;
        //        SoundLib.trophy.Play();
        //        checkTutorialComplete();
        //    }
        //}

        public void onMoveArmy()
        {
            //if (inTutorialMode && !tutorialMission_MoveArmy)
            //{
            //    tutorialMission_MoveArmy = true;
            //    hud.needRefresh = true;
            //    SoundLib.trophy.Play();
            //    checkTutorialComplete();
            //}
        }

        void checkTutorialComplete()
        {
            //if (tutorialMission_BuySoldier && tutorialMission_MoveArmy)
            //{
            //    new TimedAction0ArgTrigger(completeTutorial, 1000);        
            //}
        }

        void completeTutorial()
        {
            //inTutorialMode = false;
            mapControls.setZoomRange(false);
            hud.messages.Add(DssRef.lang.Tutorial_CompleteTitle, DssRef.lang.Tutorial_CompleteMessage);

            DssRef.storage.runTutorial = false;
            DssRef.storage.Save(null);

            tutorial = null;
        }

        

    }

    


}
