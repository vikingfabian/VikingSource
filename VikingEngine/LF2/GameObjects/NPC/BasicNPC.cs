using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.NPC
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
#if WINDOWS
            Debug.DebugLib.CrashIfMainThread();
#endif

            const int NumNPCImages = 6;
            preLoadedImages = new BasicNPCImage[NumNPCImages];
            List<Data.MaterialType> clothcolors = new List<Data.MaterialType>
                {
                    Data.MaterialType.blue, Data.MaterialType.dark_blue, Data.MaterialType.dark_gray, 
                    Data.MaterialType.mossy_green, Data.MaterialType.light_gray, Data.MaterialType.orange, 
                    Data.MaterialType.red, Data.MaterialType.red_brown, Data.MaterialType.red_orange, 
                    Data.MaterialType.yellow,
                };
            Vector3 posAdj = Vector3.Up * 1f;

            for (int i = 0; i < NumNPCImages; ++i)
            {
                BasicNPCImage img = new BasicNPCImage();
                bool male = i < (NumNPCImages / 2);
                img.Cloth = clothcolors[Ref.rnd.Int(clothcolors.Count)];
                img.Race = rndRace_hair_skin();

                List<Process.AddImageToCustom> addImage = null;
                Voxels.VoxelObjGridDataAnim voxels;
                if (male)
                {
                    voxels = Process.ModifiedImage.npcMale;
                }
                else
                {
                    voxels = Process.ModifiedImage.npcFemale;

                    const int NumFemaleHairTypes = 3;
                    addImage = new List<Process.AddImageToCustom>
                    {
                        new Process.AddImageToCustom(VoxelModelName.npc_female_hair1 + Ref.rnd.Int(NumFemaleHairTypes), 
                            new List<ByteVector2>{ new ByteVector2((byte)Data.MaterialType.iron, img.Race.X) }, false)
                    };
                    addImage[0].ReadData();
                }


                img.Mesh = Process.ModifiedImage.Generate(voxels.Clone().Frames,
                    addImage,
                    new List<ByteVector2>
                    {
                        new ByteVector2((byte)Data.MaterialType.blue_gray, (byte)img.Cloth), //tunic
                        new ByteVector2((byte)Data.MaterialType.skin, img.Race.Y), //skinn 
                        new ByteVector2((byte)Data.MaterialType.brown, img.Race.X), //hair
                    }, posAdj);

                preLoadedImages[i] = img;
            }
                  
        }


        int swordType;
        bool male;

        public BasicNPC(Map.WorldPosition startPos, Data.Characters.NPCdata chunkData)
            : base(startPos, chunkData)
        {
            socialLevel = (SocialLevel)Ref.rnd.Int((int)SocialLevel.NUM);
            aggresive = (Aggressive)Ref.rnd.Int((int)Aggressive.NUM);

            swordType = Ref.rnd.Int(3);
            male = lib.RandomBool();
            setSword();
            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write((byte)swordType);
            writer.Write(male);
        }

        public BasicNPC(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(r, npcType)
        {
            swordType = r.ReadByte();
            male = r.ReadBoolean();
            setSword();
            loadImage();
        }

        void setSword()
        {
            switch (swordType)
            {
                case 0:
                    _swordImage = VoxelModelName.stick;
                    break;
                case 1:
                    _swordImage = VoxelModelName.npc_fork;
                    break;
                case 2:
                    _swordImage = VoxelModelName.npc_spearaxe;
                    break;

            }
        }
        protected override void loadImage()
        {
            //List<Data.MaterialType> clothcolors = new List<Data.MaterialType>
            //{
            //    Data.MaterialType.blue, Data.MaterialType.dark_blue, Data.MaterialType.dark_gray, 
            //    Data.MaterialType.mossy_green, Data.MaterialType.light_gray, Data.MaterialType.orange, 
            //    Data.MaterialType.red, Data.MaterialType.red_brown, Data.MaterialType.red_orange, 
            //    Data.MaterialType.yellow,
            //};
            //Data.MaterialType cloth = clothcolors[Ref.rnd.Int(clothcolors.Count)];
            //ByteVector2 race = rndRace_hair_skin();

           
            //List<Process.AddImageToCustom> addImage = null;

            //if (!male)
            //{
            //    const int NumFemaleHairTypes = 3;
            //    addImage = new List<Process.AddImageToCustom>
            //    {
            //        new Process.AddImageToCustom(VoxelModelName.npc_female_hair1 + Ref.rnd.Int(NumFemaleHairTypes), 
            //            new List<ByteVector2>{ new ByteVector2((byte)Data.MaterialType.iron, race.X) }, false)
            //    };
            //}

            //new Process.ModifiedImage(this,
            //    male ? VoxelModelName.npc_male : VoxelModelName.npc_female,
            //    new List<ByteVector2>
            //        {
            //    new ByteVector2((byte)Data.MaterialType.blue_gray, (byte)cloth), //tunic
            //    new ByteVector2((byte)Data.MaterialType.skin, race.Y), //skinn 
            //    new ByteVector2((byte)Data.MaterialType.brown, race.X), //hair
            //        },
            //    addImage, Vector3.Up * -3.5f);

            BasicNPCImage img = arraylib.RandomListMemeber(preLoadedImages);

            setDamageColors(img.Cloth, img.Race);
            image.SetMaster(img.Mesh);
        }

        override protected float scale
        {
            get
            {
                IntervalF scaleRange = new IntervalF(0.09f, 0.13f);
                return scaleRange.GetRandom();
            }
        }

        protected override bool willTalk
        {
            get { return false; }
        }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.BasicNPC; }
        }
        protected override bool hasQuestExpression
        {
            get { return false; }
        }

        VoxelModelName _swordImage;
        override protected VoxelModelName swordImage
        {
            get { return _swordImage; }
        }
        public override SpriteName CompassIcon
        {
            get { return SpriteName.NO_IMAGE; }
        }
        protected override bool Immortal
        {
            get { return false; }
        }
    }
}
