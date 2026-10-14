using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.DSSWars;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine.Display2D;


namespace VikingEngine.Core.BlackBolts.Render
{
    class Icon3DEffect : AbsUpdateable
    {
        const float MoveTime = 100;
        const float ViewTime = 600;

        public static Icon3DEffect GetOrCreate()
        {
            Icon3DEffect spriteText3D;
            if (!BlackRef.playScene.Text3DPool.TryPop(out spriteText3D))
            {
                spriteText3D = new Icon3DEffect();
            }

            return spriteText3D;
        }

        Time beginFade;
        float fadeSpeed;
        List<Mesh> letters;
        StringBuilder stringBuilder = new StringBuilder(3);

        float stateTime;
        bool moveState;

        public Icon3DEffect()
            : base(false)
        {
            letters = new List<Mesh>(4);
        }

        public void init(ResourceType item, MapPlacement placement)
        {
            stateTime = MoveTime;
            moveState = true;

            const float height = 0.26f;

            //string text;
            Color textCol;

            //if (type == ResourceEffectType.Add)
            //{
            //    if (add < 0)
            //    {
            //        //stringBuilder.Append('-');
            //        textCol = HudLib.NotAvailableColor;
            //    }
            //    else
            //    {
                    stringBuilder.Append('+');
                    textCol = HudLib.AvailableColor;
            //}
            //text = TextLib.PlusMinus(add);
            //textCol = add > 0 ? HudLib.AvailableColor : HudLib.NotAvailableColor;

            //}
            //else
            //{
            //    //text = add.ToString();
            //    textCol = Color.White;
            //}
            //stringBuilder.Append(add);

            Vector3 center = BlackBolts.Map.WP.TileToWp(placement.tilePos);
            center = center * 0.5f + BlackBolts.Map.WP.TileToWp(placement.ForwardPos().tilePos) * 0.5f;
            center.Y = 0.2f;

            float letterSpacing = SpriteText.LetterWidthScale * height;
            float totalWidth = Table.TotalWidth(stringBuilder.Length + 2, letterSpacing, 0f);
            center.X -= (totalWidth - letterSpacing) * 0.5f;

            Vector3 sz = new Vector3(height);
            int letterIndex = 0;

            for (int i = 0; i < stringBuilder.Length; ++i)
            {
                var sprite = SpriteText.CharTile(stringBuilder[i]);
                nextMesh(sprite);

                center.X += letterSpacing;
            }



            textCol = Color.White;

            center.X += letterSpacing * 0.5f;
            sz *= 1.7f;
            //IconName.Item(item, out var itemIcon, out _);
            var resProp = ResourceLib.Get(item);
            nextMesh(resProp.icon);

            AddToOrRemoveFromUpdateList(true);

            void nextMesh(SpriteName spriteName)
            {
                Mesh l;

                if (letterIndex < letters.Count)
                {
                    l = letters[letterIndex];
                    l.position = center;
                    l.scale = sz;
                    l.SetSpriteName(spriteName);
                    l.Color = textCol;
                    l.Visible = true;
                }
                else
                {
                    l = new Mesh(LoadedMesh.plane, center, sz, TextureEffectType.Flat,
                        spriteName, textCol);
                    l.Rotation = ToGG.toggLib.PlaneTowardsCam;
                    letters.Add(l);
                }
                letterIndex++;
            }
        }

        public override void Time_Update(float time_ms)
        {
            stateTime -= time_ms;

            if (moveState)
            {
                letters[0].Y += time_ms * 0.006f;
                for (int i = 1; i < letters.Count; ++i)
                {
                    letters[i].Y = letters[0].Y;
                }

                if (stateTime <= 0)
                {
                    stateTime = ViewTime;
                    moveState = false;
                }
            }
            else if (stateTime <= 0)
            {
                //DeleteMe();
                recycle();
            }
        }

        public void fadeOut(float startTime, float fadeTime)
        {
            beginFade.MilliSeconds = startTime;
            fadeSpeed = letters[0].Opacity / fadeTime;
            this.AddToUpdateList();
        }

        void recycle()
        {
            AddToOrRemoveFromUpdateList(false);

            stringBuilder.Clear();
            foreach (var letter in letters)
            {
                letter.Visible = false;
                letter.Opacity = 1;
            }
            //arraylib.DeleteAndClearArray(letters);
            BlackRef.playScene.Text3DPool.Push(this);
        }
    }
}
