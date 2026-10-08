using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.Interface
{
    struct ImageAndOffset
    {
        public Graphics.Image image;
        public Vector2 offset;

        public void DeleteMe()
        {
            image.DeleteMe();
        }
        public void update(Vector2 center)
        {
            image.position = center + offset;
        }
    }
    struct IOInputDisplayMember
    {
        public List<ImageAndOffset> icons;

        public Vector3 tileCenter;

        public void update(AbsCamera camera)
        {
            Vector2 center =camera.From3DToScreenPos(tileCenter, Engine.Draw.defaultViewport);

            foreach (var ic in icons)
            {
                ic.update(center);
            }
        }

        public void DeleteMe()
        {
            foreach (var ic in icons)
            {
                ic.DeleteMe();
            }
        }
    }

    class IOdisplay
    {
        List<IOInputDisplayMember> inputs = new List<IOInputDisplayMember>(4);
        public AbsGameObject currentDisplayObj = null;

        public void update(AbsCamera camera)
        {
            foreach (var input in inputs)
            {
                input.update(camera);
            }
        }

        public void refresh(IntVector2 tilepos)
        {
            foreach (var input in inputs)
            {
                input.DeleteMe();
            }
            inputs.Clear();

            if (BlackRef.mapData.tileGrid.TryGet(tilepos, out var tile) &&
                tile.pMachine.hasValue)
            {
                var mach = tile.pMachine.GetMachine();
                if (mach.RefreshUiDisplay(this))
                {
                    currentDisplayObj = mach;
                }
                else
                {
                    currentDisplayObj = null;
                }
            }
        }

        public void AddInput(IO_port port)
        {
            SpriteName sprite;
            switch (port.resourceType)
            {
                default:
                    sprite = ResourceLib.Get(port.resourceType).icon;
                    break;
                case ResourceType.Any:
                    sprite = SpriteName.pjNumQuestion;
                    break;
                case ResourceType.Heat:
                    sprite = SpriteName.birdFireball;
                    break;
            }
            IOInputDisplayMember displayMember = new IOInputDisplayMember()
            {
                tileCenter = WP.TileToWp( port.ForwardPos()),
            };

            displayMember.icons = new List<ImageAndOffset>(4);
            switch (port.amount)
            {
                case 1:
                    displayMember.icons.Add(new ImageAndOffset()
                    {
                        image = new Image(sprite, Vector2.Zero, Engine.Screen.IconSizeV2 * 0.75f, ImageLayers.Foreground1, true),
                        offset = Vector2.Zero,
                    });
                    break;
                case 2:
                    float spacing = Engine.Screen.IconSize * 0.35f;
                    Vector2 offset = Vector2.Zero;
                    offset.X -= spacing * 0.5f;
                    for (int i = 0; i < port.amount; ++i)
                    {
                        displayMember.icons.Add(new ImageAndOffset()
                        {
                            image = new Image(sprite, Vector2.Zero, Engine.Screen.IconSizeV2 * 0.5f, ImageLayers.Foreground1 + i, true),
                            offset = offset,
                        });
                        offset.X += spacing;
                    }
                    break;
            }

            if (!port.isTabledispence)
            {
                var arrow = new ImageAndOffset()
                {
                    image = new Image(SpriteName.cmdAttackDirectionWhite, Vector2.Zero, Engine.Screen.IconSizeV2 * 1.5f, ImageLayers.Foreground5, true),
                    offset = Vector2.Zero,
                };
                arrow.image.Color = port.input ? Color.Pink : Color.LightGreen;
                arrow.image.Rotation = Rotation1D.FromDir4(port.mapPlacement.direction).radians /*+ MathExt.TauOver2*/;
                if (!port.input)
                {
                    arrow.image.Rotation += MathExt.TauOver2;
                }
                displayMember.icons.Add(arrow);
            }
            var shadow = new ImageAndOffset()
            {
                image = new Image(SpriteName.WhiteCirkle, Vector2.Zero, Engine.Screen.IconSizeV2 * 1f, ImageLayers.Foreground6, true),
                offset = Vector2.Zero,
            };
            shadow.image.ColorAndAlpha(Color.Black, 0.5f);
            displayMember.icons.Add(shadow);

            inputs.Add(displayMember);
        }
    }
}
