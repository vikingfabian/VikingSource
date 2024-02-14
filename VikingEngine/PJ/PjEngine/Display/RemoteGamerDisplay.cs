using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Display
{
    class RemoteGamerDisplay : IDeleteable
    {
        Graphics.Image bg, animal = null, animalBg = null;
        HatImage hat = null;
        Graphics.ImageAdvanced gamerIcon;
        Graphics.TextG name;

        public RemoteGamerDisplay(int placementIx, Network.AbsNetworkPeer gamer, int localIx, GamerData data)
        {
            var area = JoinedRemoteGamerIcon.GamerIconPlacement(placementIx);

            bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Background4);
            gamerIcon = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, 
                area.Position, 
                new Vector2(area.Height), 
                ImageLayers.AbsoluteBottomLayer,
                false);
            gamerIcon.LayerAbove(bg);
            new SteamWrapping.LoadGamerIcon(gamerIcon, gamer, false);

            name = new Graphics.TextG(
                LoadedFont.Regular, gamerIcon.RightCenter, 
                new Vector2(Engine.Screen.TextSize),
                Graphics.Align.CenterHeight,
                Engine.LoadContent.CheckCharsSafety(gamer.Gamertag, LoadedFont.Regular), 
                Color.Black, 
                ImageLayers.AbsoluteBottomLayer);
            name.LayerAbove(bg);

            if (localIx > 0)
            {
                name.TextString += "(" + TextLib.IndexToString(localIx) + ")";
            }

            if (data.joustAnimal != JoustAnimal.NUM_NON)
            {
                var animalSetup = AnimalSetup.Get(data.joustAnimal);

                animalBg = new Graphics.Image(SpriteName.WhiteArea, gamerIcon.Center, gamerIcon.Size * 0.8f, ImageLayers.AbsoluteBottomLayer, true);
                animalBg.Color = Color.Gray;
                animalBg.Opacity = 0.8f;
                animalBg.LayerAbove(gamerIcon);

                animal = new Graphics.Image(animalSetup.wingUpSprite, gamerIcon.Center, 
                    gamerIcon.Size, ImageLayers.AbsoluteBottomLayer, true);
                animal.LayerAbove(animalBg);

                if (data.hat != Hat.NoHat)
                {
                    hat = new HatImage(data.hat, animal, animalSetup);
                }
            }
        }

        public void DeleteMe()
        {
            bg.DeleteMe();
            gamerIcon.DeleteMe();
            name.DeleteMe();

            lib.SafeDelete(animal);
            lib.SafeDelete(animalBg);
            lib.SafeDelete(hat);

        }

        public bool IsDeleted
        {
            get {  return bg.IsDeleted; }
        }
    }
}
