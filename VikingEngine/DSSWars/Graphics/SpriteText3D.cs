using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.PJ.Display;
using VikingEngine.ToGG;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars
{
    class SpriteText3D : AbsUpdateable
    {
        const float MoveTime = 600;
        const float ViewTime = 600;

        public static SpriteText3D GetOrCreate()
        {
            SpriteText3D spriteText3D;
            if (!DssRef.state.Text3DPool.TryPop(out spriteText3D))
            {
                spriteText3D = new SpriteText3D();
            }

            return spriteText3D;
        }

        Time beginFade;
        float fadeSpeed;
        List<Mesh> letters;
        StringBuilder stringBuilder = new StringBuilder(3);

        float stateTime;
        bool moveState;

        public SpriteText3D()
            :base(false)
        {
            letters = new List<Mesh>(4);
        }

        public void init(ItemResourceType item, int add, Vector3 center, ResourceEffectType type)//string text, Vector3 center, float height, Color color)
        {
            stateTime = MoveTime;
            moveState = true;

            float height = DssConst.Men_StandardModelScale * 0.4f;

            //string text;
            Color textCol;
            
            if (type == ResourceEffectType.Add)
            {
                if (add < 0)
                {
                    //stringBuilder.Append('-');
                    textCol = HudLib.NotAvailableColor;
                }
                else
                {
                    stringBuilder.Append('+');
                    textCol = HudLib.AvailableColor;
                }
                //text = TextLib.PlusMinus(add);
                //textCol = add > 0 ? HudLib.AvailableColor : HudLib.NotAvailableColor;

            }
            else
            {
                //text = add.ToString();
                textCol = Color.White;
            }
            stringBuilder.Append(add);

            
            float letterSpacing = SpriteText.LetterWidthScale * height;
            float totalWidth = Table.TotalWidth(stringBuilder.Length +2, letterSpacing, 0f);
            center.X -= (totalWidth - letterSpacing) * 0.5f;

            Vector3 sz = new Vector3(height);
            int letterIndex = 0;

            for(int i = 0; i < stringBuilder.Length; ++i)
            {
                var sprite = SpriteText.CharTile(stringBuilder[i]);
                nextMesh(sprite);

                center.X += letterSpacing;
            }

            //foreach (var c in text)
            //{
            //    //Mesh l = new Mesh(LoadedMesh.plane, center, sz, TextureEffectType.Flat,
            //    //    SpriteText.CharTile(c), textCol);
            //    //letters.Add(l);
            //    nextMesh(SpriteText.CharTile(c));

            //    center.X += letterSpacing;
            //}

            textCol = Color.White;

            center.X += letterSpacing * 0.5f;
            sz *= 1.7f;
            IconName.Item(item, out var itemIcon, out _);
            nextMesh(itemIcon);

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
                    l.Rotation = toggLib.PlaneTowardsCam;
                    letters.Add(l);
                }
                letterIndex++;
            }
        }

        //public SpriteText3D(string text, Vector3 center, float height, Color color)
        //    : base(false)
        //{
        //    letters = new List<Mesh>(text.Length);
        //    float letterSpacing = SpriteText.LetterWidthScale * height;
        //    float totalWidth = Table.TotalWidth(text.Length, SpriteText.LetterWidthScale * height, 0f);
        //    center.X -= (totalWidth - letterSpacing) * 0.5f;

        //    Vector3 sz = new Vector3(height);
        //    foreach (var c in text)
        //    {
        //        Mesh l = new Mesh(LoadedMesh.plane, center, sz, TextureEffectType.Flat,
        //            SpriteText.CharTile(c), color);
        //        letters.Add(l);

        //        center.X += letterSpacing;
        //    }
        //}

        //public override void Time_Update(float time_ms)
        //{
        //    if (beginFade.CountDown())
        //    {
        //        letters[0].Opacity -= fadeSpeed * time_ms;

        //        if (letters[0].Opacity <= 0)
        //        {
        //            AddToOrRemoveFromUpdateList(false);
        //            recycle();
        //        }
        //        else
        //        {
        //            for (int i = 1; i < letters.Count; ++i)
        //            {
        //                letters[i].Opacity = letters[0].Opacity;
        //            }
        //        }
        //    }
        //}
        public override void Time_Update(float time_ms)
        {
            stateTime -= time_ms;

            if (moveState)
            {
                letters[0].Y += time_ms * 0.0001f;
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
            DssRef.state.Text3DPool.Push(this);
        }
    }
}
