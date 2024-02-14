using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Players
{
    partial class Player
    {
        MiniMapSettings mapSettings;
        MiniMap miniMap = null;
        IntVector2 newMapLocationPos;
        Time viewNewMapLocationTime = 0;

        public void NewMapLocation(IntVector2 pos)
        {
            newMapLocationPos = pos;
            viewNewMapLocationTime.Minutes = 2;
        }

        public void SelectTravelDestination()
        {
            CloseMenu();
            openMiniMap(true, true);
        }

        public void openMiniMap(bool open, bool travel = false)
        {
            removeExitModeButton();
            if (open && !travel)
                exitModeButton = new ExitModeButton(SpriteName.DpadDown, safeScreenArea);
            
#if !CMODE
            Music.SoundManager.PlayFlatSound(open? LoadedSound.open_map : LoadedSound.MenuBack);
            if (open)
            {
                mode = PlayerMode.Map;
                miniMap = new MiniMap(screenArea, mapSettings, hero, travel, localPData.view.Camera, progress.mapFlag,
                    viewNewMapLocationTime.MilliSeconds > 0, newMapLocationPos);
                viewNewMapLocationTime.MilliSeconds = 0;
            }
            else
            {
                mode = PlayerMode.Play;
                if (miniMap != null)
                {
                    mapSettings = miniMap.DeleteMap();
                    miniMap = null;
                }
                updateExitFPSHUD();
            }
#endif
        }

        
        
        

    }
}
