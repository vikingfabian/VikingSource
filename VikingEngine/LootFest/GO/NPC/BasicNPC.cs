using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.NPC
{
    struct BasicNPCImage
    {
        public Graphics.VoxelModel Mesh;
        public Data.MaterialType Cloth;
        public ByteVector2 Race;
    }

    class BasicNPC : AbsNPC
    {

        static BasicNPCImage[] preLoadedImages;
        public static void LoadContent()
        {
//#if PCGAME
//            Debug.CrashIfMainThread();
//#endif

//            const int NumNPCImages = 6;
//            preLoadedImages = new BasicNPCImage[NumNPCImages];
//            List<Data.MaterialType> clothcolors = new List<Data.MaterialType>
//                {
//                    Data.MaterialType.pure_red_orange,
//                    Data.MaterialType.darker_cool_brown,
//                    Data.MaterialType.pure_cyan_blue,
//                    Data.MaterialType.pure_yellow_orange,
//                    Data.MaterialType.gray_70,
//                    Data.MaterialType.pure_blue,
//                    Data.MaterialType.light_violet_magenta,
//                };
//            Vector3 posAdj = Vector3.Up * 1f;

//            for (int i = 0; i < NumNPCImages; ++i)
//            {
//                BasicNPCImage img = new BasicNPCImage();
//                bool male = i < (NumNPCImages / 2);
//                img.Cloth = clothcolors[Ref.rnd.Int(clothcolors.Count)];
//                img.Race = rndRace_hair_skin();

//                List<Process.AddImageToCustom> addImage = null;
//                Voxels.VoxelObjGridDataAnimHD voxels;
//                if (male)
//                {
//                    voxels = Process.ModifiedImage.npcMale;
//                }
//                else
//                {
//                    voxels = Process.ModifiedImage.npcFemale;

//                    const int NumFemaleHairTypes = 3;
//                    //addImage = new List<Process.AddImageToCustom>
//                    //{
//                    //    new Process.AddImageToCustom(VoxelModelName.npc_female_hair1 + Ref.rnd.Int(NumFemaleHairTypes), 
//                    //        new List<ByteVector2>{ new ByteVector2((byte)Data.MaterialType.RGB_red, img.Race.X) }, false)
//                    //};
//                    //addImage[0].ReadData();
//                }


//                //img.Mesh = Process.ModifiedImage.Generate(voxels.Clone().Frames,
//                //    addImage,
//                //    new List<ByteVector2>
//                //    {
//                //        new ByteVector2((byte)Data.MaterialType.RGB_red, (byte)img.Cloth), //tunic
//                //        new ByteVector2((byte)Data.MaterialType.pale_skin, img.Race.Y), //skinn 
//                //        new ByteVector2((byte)Data.MaterialType.RGB_Blue, img.Race.X), //hair
//                //    }, posAdj);

//                preLoadedImages[i] = img;
//            }
                  
        }


        int swordType;
        bool male;

        public BasicNPC(GoArgs args)
            : base(args)
        {
            loadImage();
            postImageSetup();

            if (args.LocalMember)
            {
                socialLevel = (SocialLevel)Ref.rnd.Int((int)SocialLevel.NUM);
                aggresive = (Aggressive)Ref.rnd.Int((int)Aggressive.NUM);

                swordType = Ref.rnd.Int(3);
                male = Ref.rnd.Bool();
                //setSword();
                NetworkShareObject();
            }
            else
            {
                swordType = args.reader.ReadByte();
                male = args.reader.ReadBoolean();
                //setSword();
                //loadImage();
            }
        }

        public override void netWriteGameObject(System.IO.BinaryWriter writer)
        {
            base.netWriteGameObject(writer);
            writer.Write((byte)swordType);
            writer.Write(male);
        }

        
        protected override void loadImage()
        {
            image = new Graphics.VoxelModelInstance(
                LfRef.Images.StandardModel_Character);

            BasicNPCImage img = arraylib.RandomListMember(preLoadedImages);

            setDamageColors(img.Cloth, img.Race);
            image.SetMaster(img.Mesh);
        }

        override protected float scale
        {
            get
            {
                IntervalF scaleRange = new IntervalF(0.12f, 0.15f);
                return scaleRange.GetRandom();
            }
        }

        override protected VoxelModelName swordImage
        {
            get { return VoxelModelName.stick; }
        }
        
        protected override bool Immortal
        {
            get { return false; }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.BasicNPC; }
        }
    }
}
