using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;
using VikingEngine.Timer;

namespace VikingEngine.DSSWars.Players
{
    partial class LocalPlayer
    {
        public bool inTutorialMode;
        public bool tutorialMission_BuySoldier = false;
        public bool tutorialMission_MoveArmy = false;

        public void InitTutorial() 
        {
            //inTutorialMode = DssRef.storage.runTutorial;
            inTutorialMode = false;
            mapControls.setZoomRange(inTutorialMode);
        }

        public void tutorial_writeGameState(BinaryWriter w)
        {
            w.Write(inTutorialMode);
            w.Write(tutorialMission_BuySoldier);
            w.Write(tutorialMission_MoveArmy);
        }

        public void tutorial_readGameState(BinaryReader r, int subversion)
        {
            if (subversion >= 7)
            {
                inTutorialMode = r.ReadBoolean();
                tutorialMission_BuySoldier = r.ReadBoolean();
                tutorialMission_MoveArmy = r.ReadBoolean();
            }
        }

        public void tutorial_ToHud(RichBoxContent content)
        {
            //if ( inTutorialMode)
            {
                content.Add(new RichBoxSeperationLine());

                content.h1(DssRef.lang.Tutorial_MissionsTitle);
                content.icontext(HudLib.CheckImage(tutorialMission_BuySoldier), DssRef.lang.Tutorial_Mission_BuySoldier);
                content.icontext(HudLib.CheckImage(tutorialMission_MoveArmy), DssRef.lang.Tutorial_Mission_MoveArmy);

                content.newParagraph();
                content.icontext(input.Select.Icon, DssRef.lang.Tutorial_SelectInput);
                content.icontext(input.Execute.Icon, DssRef.lang.Tutorial_MoveInput);
            }
        }

        public void onBuySoldier()
        {
            if (inTutorialMode && !tutorialMission_BuySoldier)
            {
                tutorialMission_BuySoldier = true;
                hud.needRefresh = true;
                SoundLib.trophy.Play();
                checkTutorialComplete();
            }
        }

        public void onMoveArmy()
        {
            if (inTutorialMode && !tutorialMission_MoveArmy)
            {
                tutorialMission_MoveArmy = true;
                hud.needRefresh = true;
                SoundLib.trophy.Play();
                checkTutorialComplete();
            }
        }

        void checkTutorialComplete()
        {
            if (tutorialMission_BuySoldier && tutorialMission_MoveArmy)
            {
                new TimedAction0ArgTrigger(completeTutorial, 1000);        
            }
        }

        void completeTutorial()
        {
            inTutorialMode = false;
            mapControls.setZoomRange(false);
            hud.messages.Add(DssRef.lang.Tutorial_CompleteTitle, DssRef.lang.Tutorial_CompleteMessage);

            DssRef.storage.runTutorial = false;
            DssRef.storage.Save(null);
        }
    }
}
