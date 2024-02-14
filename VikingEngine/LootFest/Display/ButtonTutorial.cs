//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest
//{
//    struct ButtonTutorialArgs
//    {
        
//        public static ButtonTutorialArgs EndTutuorial = new ButtonTutorialArgs();

//        public bool begin;
//        public SpriteName tile;
//        public bool buttonNotStick
//        {
//            get { return button != null; }
//        }
//        public numBUTTON? button; public Stick? stick; 
//        public string message; public VectorRect screenarea;
//        public ButtonTutorialArgs(SpriteName tile, numBUTTON? button, Stick? stick, string message, VectorRect screenarea)
//        {
//            this.tile = tile; this.button = button; 
//            this.stick = stick; this.message = message; this.screenarea = screenarea;
//            begin = true;
//        }
//    }

//    class ButtonTutorial
//    {
//        List<Graphics.AbsDraw2D> images;

//        public ButtonTutorialArgs args;

//        public ButtonTutorial(ButtonTutorialArgs args)
//        {
//            this.args = args;
//            //this.stick = stick;
//            Graphics.Image backg;
//            Graphics.Image backgBorder;
//            Graphics.Image icon;
//            Graphics.TextBoxSimple text;

//            const float IconSz = 48;
//            float Width = args.screenarea.Width * 0.8f;

//            text = new Graphics.TextBoxSimple(LoadedFont.PhoneText, Vector2.Zero, Vector2.One * 0.8f, Graphics.Align.Zero,
//                args.message, Color.White, ImageLayers.Foreground2, Width * 0.8f - IconSz);

//            float height = text.MeasureText().Y * 1.2f;
//            Vector2 sz = new Vector2(Width, height);

//            backg = new Graphics.Image(SpriteName.WhiteArea, args.screenarea.Center, sz, ImageLayers.Foreground3, true);
//            text.Position = backg.Position + new Vector2(IconSz - backg.Width * 0.4f, -backg.Height * 0.4f);
//            backg.Color = new Color(33, 72, 148);

//            icon = new Graphics.Image(args.tile, args.screenarea.Center - sz * PublicConstants.Half + Vector2.One * 10, Vector2.One * IconSz, ImageLayers.Foreground2);


//            const float Border = 3;
//            backgBorder = new Graphics.Image(SpriteName.WhiteArea, backg.Position, backg.Size + Vector2.One * Border * PublicConstants.Twice,
//                ImageLayers.Foreground4, true);
//            //backgBorder.Color = backg.Color * 0.6f;

//            images = new List<Graphics.AbsDraw2D> { backg, icon, text, backgBorder };
            
//        }
//        public void DeleteMe()
//        {
//            List<IDeleteable> del = new List<IDeleteable>();
//            const float FadeTime = 200;
//            foreach (Graphics.AbsDraw2D img in images)
//            {
//                new Graphics.Motion2d(Graphics.MotionType.TRANSPARENSY, img, 
//                    VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);
//                del.Add(img);
//            }
//            new Timer.TerminateCollection(FadeTime, del);
//        }
//    }

//    class PostPhonedTutorial : Timer.AbsRepeatingTrigger
//    {
//        ButtonTutorialArgs tutorial;
//        Players.Player player;

//        public PostPhonedTutorial(Players.Player player, ButtonTutorialArgs tutorial)
//            : base(1000, UpdateType.Lasy)
//        {
//            this.tutorial = tutorial;
//            this.player = player;
//        }
//        protected override void timeTrigger()
//        {
//            if (player.ReadyForButtonTutorial)
//            {
//                player.beginButtonTutorial(tutorial);
//                DeleteMe();
//            }
//        }
//    }
//}
