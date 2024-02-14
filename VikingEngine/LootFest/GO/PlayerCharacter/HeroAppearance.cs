using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Process;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Map.HDvoxel;

namespace VikingEngine.LootFest.GO.PlayerCharacter
{
    class HeroAppearance : ModifiedImage
    {
        Action<Graphics.VoxelModel, VoxelModelName> callBackEvent;

        public HeroAppearance(VoxelModelName modelName, bool faceExpressionFromStorage, Vector3 posAdj, Players.PlayerStorage storage, 
            Players.SuitAppearance suitAppearance,
            Action<Graphics.VoxelModel, VoxelModelName> callBackEvent, 
            GO.GameObjectType heroType)
         :base()
        {
            this.posAdj = posAdj;
            this.callBackEvent = callBackEvent;
            
            this.baseImage = modelName;

            if (heroType == GameObjectType.Hero)
            {
                this.addItems = ImageAddOns(true, suitAppearance, storage);
                this.findReplace = new List<TwoAppearanceMaterials>
                {
                    new TwoAppearanceMaterials(AppearanceMaterial.Material1, new AppearanceMaterial(storage.SkinColor)), //skinn 
                    new TwoAppearanceMaterials(AppearanceMaterial.Material2, new AppearanceMaterial(storage.ClothColor)), //tunic
                    new TwoAppearanceMaterials(AppearanceMaterial.Material3, new AppearanceMaterial(storage.PantsColor)), 
                    new TwoAppearanceMaterials(AppearanceMaterial.Material4, new AppearanceMaterial(storage.ShoeColor)), 
                    
                };
            }
            //else if (heroType == GameObjectType.WolfHero)
            //{
            //    this.findReplace = new List<ByteVector2>
            //    {
            //        new ByteVector2((byte)Data.MaterialType.RGB_red, suitAppearance.HatMainColor), //main col
            //        new ByteVector2((byte)Data.MaterialType.pale_skin, suitAppearance.HatDetailColor), //sec col
            //    };
            //}
            //else if (heroType == GameObjectType.HorseRidingHero)
            //{
            //    this.findReplace = new List<ByteVector2>
            //    {
            //        new ByteVector2((byte)Data.MaterialType.RGB_red, storage.HorseMainColor), //tunic
            //        new ByteVector2((byte)Data.MaterialType.pale_skin, storage.HorseNoseColor), //skinn 
            //        new ByteVector2((byte)Data.MaterialType.RGB_Cyan, storage.HorseHairColor), 
            //        new ByteVector2((byte)Data.MaterialType.RGB_green, storage.HorseHoofColor), 
            //    };
            //}
            //Engine.Storage.AddToSaveQue(StartQuedProcess, false);
            addTaskToQue(MultiThreadType.Storage);
        }
        public override void Time_Update(float time)
        {
            callBackEvent(originalMesh, baseImage);
            //base.Time_Update(time);
        }

        List<Process.AddImageToCustom> ImageAddOns(bool faceExpressionFromStorage, Players.SuitAppearance suitAppearance,
            Players.PlayerStorage storage)
        {
            
            VoxelModelName beardObjName = VoxelModelName.NUM_NON;
            VoxelModelName helmetObjName = VoxelModelName.NUM_NON;
            VoxelModelName hairObjName = VoxelModelName.NUM_NON;
            List<Process.AddImageToCustom> addItems = new List<Process.AddImageToCustom>();
            if (suitAppearance.beard != Players.BeardType.Shaved)
            {
                switch (suitAppearance.beard)
                {
                    case Players.BeardType.BeardSmall:
                        beardObjName = VoxelModelName.BeardSmall;
                        break;
                    case Players.BeardType.BeardLarge:
                        beardObjName = VoxelModelName.BeardLarge;
                        break;
                    case Players.BeardType.MustacheBiker:
                        beardObjName = VoxelModelName.MustacheBikers;
                        break;
                    case Players.BeardType.MustachePlummer:
                        beardObjName = VoxelModelName.Mustache;
                        break;
                    case Players.BeardType.Barbarian1:
                        beardObjName = VoxelModelName.barbarian_beard1;
                        break;
                    case Players.BeardType.Barbarian2:
                        beardObjName = VoxelModelName.barbarian_beard2;
                        break;
                    case Players.BeardType.Barbarian3:
                        beardObjName = VoxelModelName.barbarian_beard3;
                        break;
                    case Players.BeardType.Barbarian4:
                        beardObjName = VoxelModelName.barbarian_beard4;
                        break;
                    case Players.BeardType.Barbarian5:
                        beardObjName = VoxelModelName.barbarian_beard5;
                        break;

                    case Players.BeardType.Gentle1:
                        beardObjName = VoxelModelName.beard_gentle1;
                        break;
                    case Players.BeardType.Gentle2:
                        beardObjName = VoxelModelName.beard_gentle2;
                        break;
                    case Players.BeardType.Gentle3:
                        beardObjName = VoxelModelName.beard_gentle3;
                        break;
                    case Players.BeardType.Gentle4:
                        beardObjName = VoxelModelName.beard_gentle4;
                        break;
                    case Players.BeardType.Robin:
                        beardObjName = VoxelModelName.beard_robin;
                        break;

                    default: //small beard
                        throw new Exception("Missing beard type: " + suitAppearance.beard.ToString());
                }
                addItems.Add(new Process.AddImageToCustom(beardObjName, AppearanceMaterial.Material1, new AppearanceMaterial(storage.BeardColor), false));
            }

            faceExpression(faceExpressionFromStorage, storage, addItems);
            

            if (suitAppearance.hair != Players.HairType.NoHair)//storage.hairType != Players.HairType.NoHair)
            { //HAIR
                switch (suitAppearance.hair)//storage.hairType)
                {
                    case Players.HairType.Normal:
                        hairObjName = VoxelModelName.hair_normal;
                        break;
                    case Players.HairType.Spiky1:
                        hairObjName = VoxelModelName.hair_spiky1;
                        break;
                    case Players.HairType.Spiky2:
                        hairObjName = VoxelModelName.hair_spiky2;
                        break;
                    case Players.HairType.Rag1:
                        hairObjName = VoxelModelName.hair_rag1;
                        break;
                    case Players.HairType.Rag2:
                        hairObjName = VoxelModelName.hair_rag2;
                        break;
                    case Players.HairType.Bald1:
                        hairObjName = VoxelModelName.hair_bald1;
                        break;
                    case Players.HairType.Bald2:
                        hairObjName = VoxelModelName.hair_bald2;
                        break;
                    case Players.HairType.GirlyShort1:
                        hairObjName = VoxelModelName.HairGirly1;
                        break;
                    case Players.HairType.GirlyShort2:
                        hairObjName = VoxelModelName.HairGirly2;
                        break;
                    case Players.HairType.GirlyLong1:
                        hairObjName = VoxelModelName.hair_female_long1;
                        break;
                    case Players.HairType.GirlyLong2:
                        hairObjName = VoxelModelName.hair_female_long2;
                        break;
                    case Players.HairType.Emo1:
                        hairObjName = VoxelModelName.hair_emo1;
                        break;
                    case Players.HairType.Emo2:
                        hairObjName = VoxelModelName.hair_emo2;
                        break;
                    case Players.HairType.Emo3:
                        hairObjName = VoxelModelName.hair_emo3;
                        break;

                }
                if (hairObjName != VoxelModelName.NUM_NON)
                {
                    addItems.Add(new Process.AddImageToCustom(hairObjName, AppearanceMaterial.Material1, new AppearanceMaterial(storage.hairColor), false));
                }
            }
            if (suitAppearance.hat != Players.HatType.None)
            {
                switch (suitAppearance.hat)
                {
                    default: //vendel
                        helmetObjName = VoxelModelName.HelmetVendel;
                        break;
                    case Players.HatType.Viking1:
                        helmetObjName = VoxelModelName.HelmetHorned1;
                        break;
                    case Players.HatType.Viking2:
                        helmetObjName = VoxelModelName.HelmetHorned2;
                        break;
                    case Players.HatType.Viking3:
                        helmetObjName = VoxelModelName.HelmetHorned3;
                        break;
                    case Players.HatType.Viking4:
                        helmetObjName = VoxelModelName.HelmetHorned4;
                        break;
                    case Players.HatType.Knight:
                        helmetObjName = VoxelModelName.HelmetKnight;
                        break;
                    case Players.HatType.Cap:
                        helmetObjName = VoxelModelName.HatCap;
                        break;
                    case Players.HatType.Football:
                        helmetObjName = VoxelModelName.HatFootball;
                        break;
                    case Players.HatType.Spartan:
                        helmetObjName = VoxelModelName.HatSpartan;
                        break;
                    case Players.HatType.Witch:
                        helmetObjName = VoxelModelName.HatWitch;
                        break;
                    case Players.HatType.Pirate1:
                        helmetObjName = VoxelModelName.HatPirate1;
                        break;
                    case Players.HatType.Pirate2:
                        helmetObjName = VoxelModelName.HatPirate2;
                        break;
                    case Players.HatType.Pirate3:
                        helmetObjName = VoxelModelName.HatPirate3;
                        break;
                    case Players.HatType.Girly1:
                        helmetObjName = VoxelModelName.HairGirly1;
                        break;
                    case Players.HatType.Girly2:
                        helmetObjName = VoxelModelName.HairGirly2;
                        break;
                    case Players.HatType.Archer:
                        helmetObjName = VoxelModelName.hat_archer;
                        break;
                    case Players.HatType.WolfHead:
                        helmetObjName = VoxelModelName.hat_wolf;
                        break;
                    case Players.HatType.BearHead:
                        helmetObjName = VoxelModelName.hat_bear;
                        break;
                    case Players.HatType.PoodleHead:
                        helmetObjName = VoxelModelName.hat_poodle;
                        break;

                    case Players.HatType.Future1:
                        helmetObjName = VoxelModelName.hat_future1;
                        break;
                    case Players.HatType.FutureMask1:
                        helmetObjName = VoxelModelName.hat_futuremask1;
                        break;
                    case Players.HatType.FutureMask2:
                        helmetObjName = VoxelModelName.hat_futuremask2;
                        break;
                    case Players.HatType.GasMask:
                        helmetObjName = VoxelModelName.hat_gasmask;
                        break;

                    case Players.HatType.Santa1:
                        helmetObjName = VoxelModelName.hat_santa1;
                        break;
                    case Players.HatType.Santa2:
                        helmetObjName = VoxelModelName.hat_santa2;
                        break;
                    case Players.HatType.Santa3:
                        helmetObjName = VoxelModelName.hat_santa3;
                        break;
                    case Players.HatType.Baby1:
                        helmetObjName = VoxelModelName.hat_baby1;
                        break;
                    case Players.HatType.Baby2:
                        helmetObjName = VoxelModelName.hat_baby2;
                        break;


                    case Players.HatType.Arrow:
                        helmetObjName = VoxelModelName.hat_arrow;
                        break;
                    case Players.HatType.Coif1:
                        helmetObjName = VoxelModelName.hat_coif1;
                        break;
                    case Players.HatType.Coif2:
                        helmetObjName = VoxelModelName.hat_coif2;
                        break;
                    case Players.HatType.High1:
                        helmetObjName = VoxelModelName.hat_high1;
                        break;
                    case Players.HatType.High2:
                        helmetObjName = VoxelModelName.hat_high2;
                        break;
                    case Players.HatType.Hunter1:
                        helmetObjName = VoxelModelName.hat_hunter1;
                        break;
                    case Players.HatType.Hunter2:
                        helmetObjName = VoxelModelName.hat_hunter2;
                        break;
                    case Players.HatType.Hunter3:
                        helmetObjName = VoxelModelName.hat_hunter3;
                        break;
                    case Players.HatType.Low1:
                        helmetObjName = VoxelModelName.hat_low1;
                        break;
                    case Players.HatType.Low2:
                        helmetObjName = VoxelModelName.hat_low2;
                        break;
                    case Players.HatType.Mini1:
                        helmetObjName = VoxelModelName.hat_mini1;
                        break;
                    case Players.HatType.Mini2:
                        helmetObjName = VoxelModelName.hat_mini2;
                        break;
                    case Players.HatType.Mini3:
                        helmetObjName = VoxelModelName.hat_mini3;
                        break;
                    case Players.HatType.Turban1:
                        helmetObjName = VoxelModelName.hat_turban1;
                        break;
                    case Players.HatType.Turban2:
                        helmetObjName = VoxelModelName.hat_turban2;
                        break;
                    case Players.HatType.Headband1:
                        helmetObjName = VoxelModelName.headband1;
                        break;
                    case Players.HatType.Headband2:
                        helmetObjName = VoxelModelName.headband2;
                        break;
                    case Players.HatType.Headband3:
                        helmetObjName = VoxelModelName.headband3;
                        break;
                    case Players.HatType.MaskTurtle1:
                        helmetObjName = VoxelModelName.mask_turtle;
                        break;
                    case Players.HatType.MaskZorro1:
                        helmetObjName = VoxelModelName.mask_zorro;
                        break;
                    case Players.HatType.MaskZorro2:
                        helmetObjName = VoxelModelName.mask_zorro2;
                        break;
                    case Players.HatType.Zelda:
                        helmetObjName = VoxelModelName.hat_zelda;
                        break;

                    case Players.HatType.Cone1:
                        helmetObjName = VoxelModelName.hat_zelda;
                        break;
                    case Players.HatType.Cone2:
                        helmetObjName = VoxelModelName.hat_zelda;
                        break;
                    case Players.HatType.Cone3:
                        helmetObjName = VoxelModelName.hat_zelda;
                        break;
                    case Players.HatType.Cone4:
                        helmetObjName = VoxelModelName.hat_zelda;
                        break;


                    case Players.HatType.crown1:
                        helmetObjName = VoxelModelName.hat_crown1;
                        break;
                    case Players.HatType.crown2:
                        helmetObjName = VoxelModelName.hat_crown2;
                        break;
                    case Players.HatType.crown3:
                        helmetObjName = VoxelModelName.hat_crown3;
                        break;
                    case Players.HatType.princess1:
                        helmetObjName = VoxelModelName.hat_princess1;
                        break;
                    case Players.HatType.princess2:
                        helmetObjName = VoxelModelName.hat_princess2;
                        break;

                    case Players.HatType.Bucket:
                        helmetObjName = VoxelModelName.hat_bucket;
                        break;
                }
                addItems.Add(new Process.AddImageToCustom(helmetObjName, new List<TwoAppearanceMaterials>
                {
                    new TwoAppearanceMaterials(AppearanceMaterial.Material1, new AppearanceMaterial(suitAppearance.HatMainColor)),
                    new TwoAppearanceMaterials(AppearanceMaterial.Material2, new AppearanceMaterial(suitAppearance.HatDetailColor)),
                }, false));
            }

            if (storage.BeltType != Players.BeltType.No_belt)
            {
                VoxelModelName beltName = VoxelModelName.NUM_NON;
                switch (storage.BeltType)
                {
                    default://case Players.BeltType.Slim:
                        beltName = VoxelModelName.belt_slim;
                        break;
                    case Players.BeltType.Thick:
                        beltName = VoxelModelName.belt_thick;
                        break;
                }
                //addItems.Add(new Process.AddImageToCustom(beltName, new List<ByteVector2>
                //{
                //   new ByteVector2( (byte)Data.MaterialType.RGB_red, storage.BeltColor),
                //    new ByteVector2((byte)Data.MaterialType.RGB_green, storage.BeltBuckleColor),
                //}, false));

                addItems.Add(new Process.AddImageToCustom(beltName, new List<TwoAppearanceMaterials>
                {
                    new TwoAppearanceMaterials(AppearanceMaterial.Material1, new AppearanceMaterial(storage.BeltColor)),
                    new TwoAppearanceMaterials(AppearanceMaterial.Material2, new AppearanceMaterial(storage.BeltBuckleColor)),
                }, false));
            }

            if (storage.UseCape)
            {
                //addItems.Add(new Process.AddImageToCustom(VoxelModelName.cape, new List<ByteVector2>
                //{
                //   new ByteVector2((byte)Data.MaterialType.RGB_red, storage.CapeColor),
                //}, true));
            }

            return addItems;
        }

        void faceExpression(bool useStorageSetup, Players.PlayerStorage storage, List<Process.AddImageToCustom> addItems)
        {
            VoxelModelName mouthObjName = VoxelModelName.NUM_NON;
            VoxelModelName eyesObjName = VoxelModelName.NUM_NON;

            if (useStorageSetup)
            {
                if (storage.mouth != Players.MouthType.NoMouth)
                {
                    switch (storage.mouth)
                    {
                        case Players.MouthType.BigSmile:
                            mouthObjName = VoxelModelName.MouthBigSmile;
                            break;
                        case Players.MouthType.Hmm:
                            mouthObjName = VoxelModelName.MouthHmm;
                            break;
                        case Players.MouthType.Loony:
                            mouthObjName = VoxelModelName.MouthLoony;
                            break;
                        case Players.MouthType.OMG:
                            mouthObjName = VoxelModelName.MouthOMG;
                            break;
                        case Players.MouthType.Orc:
                            mouthObjName = VoxelModelName.MouthOrch;
                            break;
                        case Players.MouthType.Smile:
                            mouthObjName = VoxelModelName.MouthSmile;
                            break;
                        case Players.MouthType.Sour:
                            mouthObjName = VoxelModelName.MouthSour;
                            break;
                        case Players.MouthType.Straight:
                            mouthObjName = VoxelModelName.MouthStraight;
                            break;
                        case Players.MouthType.WideSmile:
                            mouthObjName = VoxelModelName.MouthWideSmile;
                            break;
                        case Players.MouthType.Laugh:
                            mouthObjName = VoxelModelName.mouth_laugh;
                            break;
                        case Players.MouthType.Girly1:
                            mouthObjName = VoxelModelName.MouthGirly1;
                            break;
                        case Players.MouthType.Girly2:
                            mouthObjName = VoxelModelName.MouthGirly2;
                            break;
                        case Players.MouthType.Gasp:
                            mouthObjName = VoxelModelName.MouthGasp;
                            break;
                        case Players.MouthType.Pirate:
                            mouthObjName = VoxelModelName.MouthPirate;
                            break;
                        case Players.MouthType.Baby1:
                            mouthObjName = VoxelModelName.mouth_baby1;
                            break;
                        case Players.MouthType.Baby2:
                            mouthObjName = VoxelModelName.mouth_baby2;
                            break;
                        case Players.MouthType.open_smile:
                            mouthObjName = VoxelModelName.mouth_open_smile;
                            break;
                        case Players.MouthType.souropen:
                            mouthObjName = VoxelModelName.mouth_souropen;
                            break;
                        case Players.MouthType.teeth1:
                            mouthObjName = VoxelModelName.mouth_teeth1;
                            break;
                        case Players.MouthType.teeth2:
                            mouthObjName = VoxelModelName.mouth_teeth2;
                            break;
                        case Players.MouthType.teeth3:
                            mouthObjName = VoxelModelName.mouth_teeth3;
                            break;
                        case Players.MouthType.vampire:
                            mouthObjName = VoxelModelName.mouth_vampire;
                            break;
                        case Players.MouthType.SideSmile1:
                            mouthObjName = VoxelModelName.MouthSideSmile1;
                            break;
                        case Players.MouthType.SideSmile2:
                            mouthObjName = VoxelModelName.MouthSideSmile2;
                            break;


                    }
                    //addItems.Add(new Process.AddImageToCustom(mouthObjName, (byte)Data.MaterialType.pale_skin, storage.SkinColor, false));
                }

                if (storage.eyes != Players.EyeType.NoEyes)
                {
                    switch (storage.eyes)
                    {
                        case Players.EyeType.Cross:
                            eyesObjName = VoxelModelName.EyeCross;
                            break;
                        case Players.EyeType.Evil:
                            eyesObjName = VoxelModelName.EyeEvil;
                            break;
                        case Players.EyeType.Frown:
                            eyesObjName = VoxelModelName.EyeFrown;
                            break;
                        case Players.EyeType.Loony:
                            eyesObjName = VoxelModelName.EyeLoony;
                            break;
                        case Players.EyeType.Normal:
                            eyesObjName = VoxelModelName.EyeNormal;
                            break;
                        case Players.EyeType.Red:
                            eyesObjName = VoxelModelName.EyeRed;
                            break;
                        case Players.EyeType.Sleepy:
                            eyesObjName = VoxelModelName.EyeSleepy;
                            break;
                        case Players.EyeType.Slim:
                            eyesObjName = VoxelModelName.EyeSlim;
                            break;
                        case Players.EyeType.Sunshine:
                            eyesObjName = VoxelModelName.EyeSunshine;
                            break;
                        case Players.EyeType.Crossed1:
                            eyesObjName = VoxelModelName.EyesCrossed1;
                            break;
                        case Players.EyeType.Crossed2:
                            eyesObjName = VoxelModelName.EyesCrossed2;
                            break;
                        case Players.EyeType.Cyclops:
                            eyesObjName = VoxelModelName.EyesCyclops;
                            break;

                        case Players.EyeType.Girly1:
                            eyesObjName = VoxelModelName.EyesGirly1;
                            break;
                        case Players.EyeType.Girly2:
                            eyesObjName = VoxelModelName.EyesGirly2;
                            break;
                        case Players.EyeType.Girly3:
                            eyesObjName = VoxelModelName.EyesGirly3;
                            break;
                        case Players.EyeType.Pirate:
                            eyesObjName = VoxelModelName.EyesPirate;
                            break;
                        case Players.EyeType.SunGlasses1:
                            eyesObjName = VoxelModelName.eyes_sunglass1;
                            break;
                        case Players.EyeType.SunGlasses2:
                            eyesObjName = VoxelModelName.eyes_sunglass2;
                            break;
                        case Players.EyeType.Emo1:
                            eyesObjName = VoxelModelName.eyes_emo1;
                            break;
                        case Players.EyeType.Emo2:
                            eyesObjName = VoxelModelName.eyes_emo2;
                            break;

                        case Players.EyeType.Crossed3:
                            eyesObjName = VoxelModelName.EyeCrossed3;
                            break;
                        case Players.EyeType.Crossed4:
                            eyesObjName = VoxelModelName.EyeCrossed4;
                            break;
                        case Players.EyeType.Crossed5:
                            eyesObjName = VoxelModelName.EyeCrossed5;
                            break;
                        case Players.EyeType.HardShut:
                            eyesObjName = VoxelModelName.EyeHardShut;
                            break;
                        case Players.EyeType.Sad1:
                            eyesObjName = VoxelModelName.EyeSad1;
                            break;
                        case Players.EyeType.Sad2:
                            eyesObjName = VoxelModelName.EyeSad2;
                            break;
                        case Players.EyeType.Sad3:
                            eyesObjName = VoxelModelName.EyeSad3;
                            break;
                        case Players.EyeType.Sad4:
                            eyesObjName = VoxelModelName.EyeSad4;
                            break;
                        case Players.EyeType.Sad5:
                            eyesObjName = VoxelModelName.EyeSad5;
                            break;
                        case Players.EyeType.SleepyCross:
                            eyesObjName = VoxelModelName.EyeSleepyCross;
                            break;
                        case Players.EyeType.Vertical:
                            eyesObjName = VoxelModelName.EyeVertical;
                            break;
                        case Players.EyeType.Wink:
                            eyesObjName = VoxelModelName.EyeWink;
                            break;
                    }


                    //addItems.Add(new Process.AddImageToCustom(eyesObjName, (byte)Data.MaterialType.pale_skin, storage.SkinColor, false));
                }
                
               
            }
            //else
            //{
            //    //Gör en exression
                

            //    //addItems.Add(new Process.AddImageToCustom(eyesObjName, (byte)Data.MaterialType.pale_skin, storage.SkinColor, false));
            //    //addItems.Add(new Process.AddImageToCustom(mouthObjName, (byte)Data.MaterialType.pale_skin, storage.SkinColor, false));

            //}

            
            //EXPRESSION override
            switch (baseImage)
            {
                case VoxelModelName.express_anger:
                    eyesObjName = VoxelModelName.EyeFrown;
                    mouthObjName = VoxelModelName.MouthSour;
                    break;
                case VoxelModelName.express_hi:
                    eyesObjName = VoxelModelName.EyeSunshine;
                    mouthObjName = VoxelModelName.mouth_open_smile;
                    break;
                case VoxelModelName.express_laugh:
                    eyesObjName = VoxelModelName.EyeSunshine;
                    mouthObjName = VoxelModelName.mouth_laugh;
                    break;
                case VoxelModelName.express_loot:
                    eyesObjName = VoxelModelName.NUM_NON;
                    mouthObjName = VoxelModelName.NUM_NON;
                    break;
                case VoxelModelName.express_sad1:
                    eyesObjName = VoxelModelName.EyeSad5;
                    mouthObjName = VoxelModelName.MouthSour;
                    break;
                case VoxelModelName.express_teasing:
                    eyesObjName = VoxelModelName.EyeSlim;
                    mouthObjName = VoxelModelName.MouthLoony;
                    break;
                case VoxelModelName.express_thumbup:
                    eyesObjName = VoxelModelName.EyeSunshine;
                    mouthObjName = VoxelModelName.MouthSideSmile2;
                    break;
            }

            if (eyesObjName != VoxelModelName.NUM_NON)
            {
                addItems.Add(new Process.AddImageToCustom(eyesObjName, AppearanceMaterial.Material1, new AppearanceMaterial(storage.SkinColor), false));
            }
            if (mouthObjName != VoxelModelName.NUM_NON)
            {
                addItems.Add(new Process.AddImageToCustom(mouthObjName, AppearanceMaterial.Material1, new AppearanceMaterial(storage.SkinColor), false));
            }
        }
    }
}
