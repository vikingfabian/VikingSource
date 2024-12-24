﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest
{
    struct VoxelModelNameAndRotation
    {
        public VoxelModelName name;
        public int rotation;

        public VoxelModelNameAndRotation(VoxelModelName name, int rotation = 0)
        {
            this.name = name;
            this.rotation = rotation;
        }

        public static implicit operator VoxelModelNameAndRotation(VoxelModelName name)
        {
            return new VoxelModelNameAndRotation(name, 0);
        }

        public override int GetHashCode()
        {
            return (int)name * 100 + rotation;
        }
    }
    
    enum VoxelModelName
    {
        //--
        CATEGORY_CHARACTER_0,
#region CHARACTER
        magician, end_magician, end_magician_riding,
        orch, orc2, orch_leader, orc_leader2, grunt, grunt2,
        orc_soldier, orc_archer, orc_knight,
        goblin1, goblin_berserk, goblin_lineman,
        goblin_king,
        bigorc,
        troll_boss,
        elf_archer, elf_wardancer, elf_knight,
        elf_king, 
        evil_spider,
        squig_lvl1, squig_lvl2,
        squig_red, squig_red_baby,
        squig_green, squig_green_baby,
        squig_horned, squig_horned_baby,
        spitchick_lvl1,
        wolf_lvl1, wolf_lvl2,
        hog_baby, hog_lvl1, hog_lvl2, pumba, old_swine, old_swine_red,
        hog_v2_lvl1,
        pitbull_lvl1,
        crockodile1, crockodile2, frog1, frog2, fire_goblin, fire_goblin2,
        scorpion1, scorpion1v2, scorpion2,
        scorpionbot, scorpionbot_arm_blue, scorpionbot_arm_yellow, scorpionbot_arm_knife_l, scorpionbot_arm_knife_r,
        lizard1, lizard2,
        spider1, spider2, bull_spider1, mini_spider1, poison_spider1,
        spiderbot_mid, spiderbot_leg, spiderbot_goblin,
        spiderboss,
        miner_spider,
        beetle1, beetle2,
        ent, ent2, bee, bee2,
        fat_bird, fat_bird2,
        moth_old1, moth_old2,
        endbossmount,
        ogre_body, ogre_arm, ogre_leg,
        BlockTrap, shootingturret,
        mommy, ghost,
        greatwolf,
        green_slime, green_slime_small, yellow_slime, yellow_slime_small,
        bat1, bat2,
        statueboss,
        temp_hero,
        Character,
        CharacterHD,
        char_hd,
        riding_hero2,
        herowolf,
        horse,  horse_red,
        cg_baseminion1,
        cg_baseminion2,
        cg_baseminion3,
        cg_baseminion4,
        cg_baseminion5,
        cg_baseminion6,

        cg_attminion1,
        cg_attminion2,
        cg_attminion3,
        cg_attminion4,
        cg_attminion5,
        cg_attminion6,
        Grandpa,
        messenger,
        war_veteran,
        dummie,
        
        
        white_hen,
        chick,
        miner_pig, miner_cow,
        butterfly,
        sheep,
        pet_pig,
        pet_falcon,
        pet_dragon,
        pet_angel,
        pet_bird,
        angel_watch,
        zombie1, zombiebaby, harpy, harpy2, wizard, zombieleader, zombiemom, fatzombie, Skeleton,
        
        npc_male, npc_female, happy_npc_female,
        father, mother,
        im_error, im_glitch, im_bug,
        granpa2,
        Salesman, Lumberjack, Miner, blacksmith,
        housebuilder,
        cook,
        healer,
        wiselady, priest,
        weaponsmith, armorsmith, bowmaker,
        tailor,
        volcan,
        banker,
        Wolf,
        lobby_character,
        lobby_character_friend,
        checkpoint_npc,
        transport_wagon, wagondriver,
#endregion

        //--
        CATEGORY_WEAPON_1,
#region WEAPON
        goblin_king_shield,
        scorpion_bullet,
        bot_rocket,

        spider_web,
        fat_egg,
        moth,
        bat_sonar,

        goblin_sword, goblin_spear, goblin_handspear, goblin_shield,
        EnemyProjectile, enemy_projectile_green, ent_root,
        spitchick_bullet,
        endmagician_projectile,
        stick,
        sword_base, axe_base, axe2h_base,
        barbarian2haxe_base, barbariandualaxe_l_base, barbariandualaxe_r_base, knife_base, pickaxe_base, sickle_base, spear_base,
        emo_sword,
        handspear, shapeshifter_spear, buildhammer,
        Sword1, pickaxe,
        Sword2, axe, broom, magicstaff,
        staff_fire, staff_poision, staff_light, staff_evil,
        Sword3, doubleaxe, pimpstick,
        shortbow, longbow, bronzebow, ironbow, silverbow, goldbow, mithrilbow, firebow, poisionbow, lightningbow, evilbow,
        slingshot,
        warvet_sword,
        npc_fork, npc_spearaxe,
        orc_sword1, orc_sword2, bigorc_club, orcbow, orc_handspear, orc_steel_shield,
        elf_sword1, elf_long_sword1, elf_knight_shield,
        elf_arrow, elfbow, elf_king_shield,
        magician_sword,

        gunmodel_mashine, gunmodel_sidearm,
        gunbullet, handgranade,
         golden_arrow, 
        orc_arrow,
        Shield1,
        Shield2,
        Shield3,
        shield_round1, shield_round2, shield_round3, shield_round4,
        shield_spartan1, shield_spartan2, shield_spartan3,
        shield_keit1,
        ThrowingSpear,
        throw_axe,
        pet_projectile_angel,
        pet_projectile_pig,
        pet_bird_projectile,
        witch_magic,
        FireBall, fire_goblin_ball,
        babyaxe,
        thunder, lightning, light_spark,
        explosion,
        poision_mushroom,
#endregion

        //--
        CATEGORY_APPEARANCE_2,
#region APPEARANCE

        EyeNormal, EyeCross, EyeEvil, EyeFrown, EyeLoony, EyeRed, EyeSleepy, EyeSlim, EyeSunshine, EyesCrossed1, EyesCrossed2, EyesCyclops,
        EyesPirate,
        EyesGirly1, EyesGirly2, EyesGirly3,
        eyes_sunglass1, eyes_sunglass2,
        eyes_emo1, eyes_emo2,
        EyeCrossed3, EyeCrossed4, EyeCrossed5,
        EyeHardShut,
        EyeSad1, EyeSad2, EyeSad3, EyeSad4, EyeSad5,
        EyeSleepyCross,
        EyeVertical,
        EyeWink,
        
        MouthOrch, MouthBigSmile, MouthHmm, MouthLoony, MouthOMG, MouthSmile, MouthSour, MouthWideSmile, MouthStraight, MouthGasp, mouth_laugh,
        MouthPirate,
        MouthGirly1, MouthGirly2,
        mouth_baby1, mouth_baby2,
        mouth_open_smile,
        mouth_souropen,
        mouth_teeth1, mouth_teeth2, mouth_teeth3,
        mouth_vampire,
        MouthSideSmile1, MouthSideSmile2,


        hair_normal, hair_spiky1, hair_spiky2, hair_rag1, hair_rag2, hair_bald1, hair_bald2,
        HairGirly1, HairGirly2,
        hair_female_long1, hair_female_long2,
        hair_emo1, hair_emo2, hair_emo3,
        aManikin, a_hat_manikin,

        HelmetVendel,
        HelmetHorned1, HelmetHorned2, HelmetHorned3, HelmetHorned4,
        hat_wolf, hat_bear, hat_poodle,
        hat_future1, hat_futuremask1, hat_futuremask2,
        hat_gasmask,
        HelmetKnight,

        HatCap,
        HatFootball,
        HatSpartan,
        HatWitch,
        hat_archer,
        hat_baby1, hat_baby2,
        hat_santa1, hat_santa2, hat_santa3,
        HatPirate1, HatPirate2, HatPirate3,
        hat_arrow, hat_coif1, hat_coif2, hat_high1, hat_high2, hat_hunter1, hat_hunter2, hat_hunter3,
        hat_low1, hat_low2, hat_mini1, hat_mini2, hat_mini3,
        hat_turban1, hat_turban2, headband1, headband2, headband3,
        mask_turtle, mask_zorro, mask_zorro2,
        hat_zelda,
        helmet_cone1, helmet_cone2, helmet_cone3, helmet_cone4,
        hat_crown1, hat_crown2, hat_crown3,
        hat_princess1, hat_princess2,
        hat_bucket,

        belt_slim, belt_thick,

        BeardSmall,
        BeardLarge, barbarian_beard1, barbarian_beard2, barbarian_beard3, barbarian_beard4, barbarian_beard5,
        beard_gentle1, beard_gentle2, beard_gentle3, beard_gentle4, beard_robin,
        Mustache,
        MustacheBikers,

        cape,
        npc_female_hair1, npc_female_hair2, npc_female_hair3,
        guardHead, guardCaptainHead,
        horse_hat, workerhat,

        using_menu, using_guide, using_steam, using_build, using_rc,
        express_anger, express_hi, express_laugh, express_teasing, express_thumbup,
        express_sad1, express_loot, express_duck,
#endregion

        //--
        CATEGORY_TERRAIN_3,
#region TERRAIN
        goldsmith_station,
        bow_station,
        armour_station,
        cook_station,
        vulcan_station,
        sheild_station,
        salesman_station,
        lumberjack_station,
        miner_station,
        healer_station,
        weaponsmith_station,
        blacksmith_station,
        wiselady_station, wiselady_cabin,
        tailor_station,
        priest_station,
        end_tomb, end_pillar,
        Well,
        FenceYard,
        privatehouse,
        buildarea1, buildarea2, buildarea3,

        DoorToFirstLevel,
        DoorToLobby,
        DoorToLevelGoblins,
        DoorToLevelMount,
        DoorToLevelTroll,
        DoorToLevelWolf,
        DoorToLevelStatue,
        DoorToLevelBird,
        DoorToLevelSwine,
        DoorToLevelOrcs,
        DoorToLevelElf,
        DoorToLevelSkeletonDungeon,
        DoorToLevelSpider,
        DoorToFinalLevel,
        DoorToFinalLevel2,
        DoorToChallengeZombies,
        DoorToChallengeFuture,
        PinkTree,
        OakTree1,
        OakTree2,
        Cactus1,
        Cactus2,
        BirchTree1,
        BirchTree2,
        PineTree1,
        PineTree2,
        ForestTree1,
        ForestTree2,
        ForestTree3,
        ForestTree4,
        ForestTree5,
        ForestStone1,
        ForestStone2,
        ForestStone3,
        ForestBurnedTree1,
        ForestBurnedTree2,
        TutTargetHanger1,
        TutTargetHanger2,
        TutJumpToTeleport2,
        TutShootPlatform,
        
        DirectionalSign,
        DungeonTerrain1,
        DungeonTerrain2,
        DungeonTerrain3,
        DungeonTerrain4,
        Stone1, Stone2, Stone3,

        TestSpawn,

        CaveSpawn1,
        CaveSpawn2,
        CaveSpawn3,

        DoorSpawn1,
        DoorSpawn2,
        DoorSpawn3,

        GoblinGate1,
        GoblinGate2,
        GoblinFortress1,
        GoblinEndFortress1,
        GoblinPost1,
        GoblinPatrolWall1,
        GoblinKeyGuard1,
        GoblinKeyGuard2,
        GoblinTent,
        GoblinDogHouse1,
        OrcFort,
        TrollWarningSign,
        TrollRuin1,
        TrollRuin2,
        TrollRuin3,
        troll_damagedtower,
        Troll_SpiderCave,
        troll_stoneship,

        FpsTut,
        SalesmanWagon,
        JointTest,

        TownLamp,
        HomeHouse1,
        TownHouse2,
        TownHouse3,
        
        TownStatue,
        TownChurch,
        TownCastle,
        CritterPen,

        MinerHouse,
        MinerMine,
        MinerTree1,
        MinerTree2,
        minerFence,
        Palace,
        Bank,
        Theater,
        goblin_hut, orc_hut,
        scenery_test3,
        mining_pileground,

        uppath, downpath,
        farmhouse1,
        FarmTownHouse1,
        FenceYardWide,

        townwall_corner,
        townwall_gate,
        townwall_wall,
        townwall_walltower,

#endregion

#region BLOCK_PATTERN
        CATEGORY_BLOCKPATTERN_4,
        grassmaterial,
        sandmaterial,
        dirtmaterial,
#endregion

        //--
        CATEGORY_OTHER_5,
#region OTHER
        scenery_test, scenery_test2,
        boss_lock, area_lock,
        suit_archer, suit_soldier, suit_barbarian_dane, suit_barbarian_dual, suit_spearman, suit_shapeshifter, suit_emo,
        
        chest_open, chest_closed,
        discard_pile,
        Coin,
        key_lvl1, key_lvl2, locklvl1, locklvl2, groupLock, threelock,
        baby,
        stone_heart,
        mithril_ingot,
        Apple,
        smallhealthup, smallmagicup, specialattup, specialammorefill,
        apple_scrap, applepie_scrap, bread_scrap, grape_scrap, meat_scrap,
        itembox_bone, itembox_apple, itembox_pie, itembox_pickaxe,
        cardcollection,
        card,
        cg_attackup,
        cg_healup,
        cg_tempmana,
        cg_playerlife,
        cg_castle_icon,
        cg_board,
        cg_deck,
        cg_platform1,
        cg_platform2,
        ApplePie,
        healup_effect, magicup_effect,
        shield_symbol,
        temp_block,
        temp_block_animated,
        hit_target,
        Bone, loot_leather, loot_feather, loot_sack,
        present,
        horsetransport,
        eggnest, beehive, 
        i, enemyattention, enemy_expression, ZZZ, target_warning, stun,
        bottle,
        
        barrel, barrelX, snowman,
        herb_fire, herb_leaf, herb_red, herb_rose,
        mining_pile,
        goldchest,
        building_hero,
        //LockedDoor1,
#endregion

        //--
        CATEGORY_WARS_6,
#region LFWARS
        party_restbar,
        Arrow,
        slingstone,
        boulder_proj,
        Pig,
        Hen,
        little_kingman,
        little_kingorc,
        little_hirdman,
        little_hirdorc,
        
        little_archerman,
        little_archerorc,
        little_crossbowman,
        wars_crossbow,
        little_javelinman,
        little_javelinorc,
        
        little_dogman,
        little_dogorc,
        war_dogneutral,
        little_fatman,
        little_fatorc,
        little_fatneutral,
        little_healman,
        little_slingman,
        little_slingorc,
        little_soldierman,
        little_soldierorc,
        wars_deserter,
        war_recruit,
        wars_soldier,
        wars_soldier_i2,
        wars_soldier_i3,
        wars_twohand,
        war_archer,
        war_archer_i2,
        little_longswordman,
        little_longswordorc,
        war_spearman,
        little_spearorc,
        war_sailor,
        war_sailor_i2,
        little_vikingorc,
        war_knight,
        war_knight_i2,
        war_knight_i3,
        little_knightorc,
        war_worker,
        war_gnome,
        little_workerorc,
        war_ballista,
        war_ballista_i2,
        wars_darklord,

        wars_ironsiegecannon,
        wars_ironmancannon,
        wars_bronzemancannon,
        wars_bronzesiegecannon,
        wars_catapult,
        wars_manuballista,
        wars_longsword,
        wars_mithrilman,
        wars_mithrilarcher,
        wars_culvertin,
        wars_handcannon,
        wars_slingman,
        wars_javelin,
        wars_hammer,
        city_water,
        city_quarry,
        city_logistic,

        wars_shipcrew,
        wars_shipmelee,
        wars_captain,
        wars_soldier_ship,
        wars_viking_ship,
        wars_knight_ship,
        wars_archer_ship,
        wars_ballista_ship,
        wars_folk_ship,

        wars_loading_anim,
        wars_shipbuild,

        little_ballistaorc,
        little_bombcatapultman,
        little_bombcatapultorc,
        little_rocketlauncherman,
        little_rocketlauncherorc,
        little_ramman,
        little_ramorc,
        
        war_ballista_proj,
        war_cannonball,
        war_gunblast,

        war_folkman,
        wars_piker,
        wars_spearman,
        wars_trollcannon,
        little_farmerorc_v2,
        war_farmerneutral,
        war_pigneutral,
        little_scoutman,
        little_scoutorc,
        little_scout_knife,
        little_javelin,
        little_boltarrow,
        little_firebomb,
        little_rocketarrow,
        little_bannerman,
        war_bannerman,
        banner,
        horsebanner,
        citybanner,
        armybanner,
        armystand,
        cityicon,
        buildarea,
        wars_borderstick,
        //city_tower24,
        city_pen,
        city_workerhut,
        city_cobblestone,
        city_square,
        city_stonehall,
        city_smallhouse,
        city_bighouse,
        city_dirtwall,
        city_dirttower,
        city_woodwall,
        city_woodtower,
        city_stonewall,
        city_stonetower,
        city_tavern,
        city_storehouse,
        city_bank,
        city_postal,
        /*city_recruitment*/
        city_barracks,
        city_mine,
        city_workstation,
        city_carpenter,
        city_nobelhouse,

        horse_white, horse_brown,
        
        stupid_board,
        stupid_gate,
        little_flag,
        little_flagtower,
        little_flagmill,
        //little_wall,

        little_hpbar_blue,
        little_hpbar_red,
        little_bufficons,
        little_tiredicon,
        little_waitingordericon,

        little_wallman,
        little_wallorc,
        little_wallsparta,
        little_weapon_shop,


        little_javelinbarbarian,
        little_persianmagician,
        little_soldierimmortal,
        little_immortalgiant,
        little_scoutpersian,
        little_archerpersian,

        little_archerarcade,
        little_soldierarcade,
        little_swordspartan,
        little_spearspartan,
        little_kingspartan,

        //warmap_grass1,
        //warmap_grassdark1,
        //warmap_mountain1,
        //warmap_mountaindark1,
        //warmap_sand1,
        //warmap_sanddark1,

        war_town1,
        war_town2,
        war_town3,
        war_town_factory,
        war_workerhut,

        little_land_grass2,
        little_land_grass3,
        little_land_grass4,

        little_land_forest1,
        little_land_grassfloodNS1,
        little_land_grassfloodWE1,

        little_land_grassfloodNE1,
        little_land_grassfloodES1,
        little_land_grassfloodSW1,
        little_land_grassfloodNW1,

        little_land_grassfloodNES1,
        little_land_grassfloodNSW1,        

        little_land_grassbridgeNS1,
        little_land_grassbridgeWE1,
        little_land_grassbridgeMID1,
        little_land_grasspuddleNS1,
        little_land_grasspuddleWE1,

        little_land_grasswaterNEW1,
        little_land_grasswaterESW1,
        
        little_land_grass_sandy1,
        little_land_grass_sandy2,
        little_land_sandyforest1,
        little_land_grass_sandy_roadWE1,

        little_land_sparta1,
        little_land_sparta2,
        little_land_spartamountainopen1,
        little_land_spartawaterESW1,
        little_land_spartawateropen1,

        little_land_spartaforest1,
        little_land_spartapuddle1,
        little_land_spartaroadW,
        little_land_spartaroadWE,

        little_trees1,
        little_drytrees1,

        little_evil_grass,
        little_evil_grass2,
        little_evil_bridge_NS,
        little_evil_bridge_WE,
        little_evil_wateropen1,
        little_evilcastle_towermid,
        little_evilcastle_towerN,
        little_evilcastle_towerS,

        fol_tree_hard,
        fol_tree_soft,
        fol_tree_dry,
        fol_sprout,
        fo_stone1,
        fol_bush1,
        fol_stoneblock,
        fol_tallgrass,
        fol_herbs,
        fol_farmculture,
        fol_farmculture2,

        resource_tree,
        resource_rubble,

        decor_statue,
        city_pavement,
        #endregion

        NUM_NON
    }

    enum ModelCategory
    {
        Character,
        Weapon,
        Appearance,
        Terrain,
        BlockPattern,
        Other,
        Wars,
        All,
        NUM_NON
    }
}
