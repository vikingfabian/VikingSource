using System;
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
        modsoldier_debug,
        modsoldier_body1,
        modsoldier_body_beef1,
        modsoldier_body3lady,
        modsoldier_face1,
        modsoldier_face_skull,
        modsoldier_face_orc,
        modsoldier_hat_soldier_all,
        modsoldier_hat_custom_all,
        modsoldier_leg1,
        
        modsoldier_larm_empty1,
        modsoldier_larm_shield1,
        modsoldier_rarm_sword1,
        modsoldier_rarm_bow1v2,

        modsoldier_larm_empty2naked,
        modsoldier_larm_shield2naked,
        modsoldier_rarm_sword2naked,
        modsoldier_rarm_bow2naked,

        modsoldier_addons,
        modsoldier_face_access,
        modweapon_sword1,

        modshield_javelin,
        modshield_roman,
        modshield_knightsmallside,
        
        modweapon_blunderbuss,
        modweapon_crossbow,
        modweapon_culvertin,
        modweapon_hammer,
        modweapon_handcannon,
        modweapon_javelin,
        modweapon_longbow,
        modweapon_mithrilbow,
        modweapon_mithrilsword,
        modweapon_rifle,
        modweapon_settler,
        modweapon_sharpstick,
        modweapon_shortbow,
        modweapon_sling,
        modweapon_spear,
        modweapon_twohand,

        modweapon_shortsword,
        modweapon_longsword,
        modweapon_bronzesword,

        #endregion

        //        //--
        //        CATEGORY_WEAPON_1,
        //#region WEAPON

        //#endregion


        //--
        CATEGORY_WARS_1,
#region LFWARS
        ErrorCube,
        party_restbar,
        Arrow,
        slingstone,
        boulder_proj,
        Pig,
        Hen,
        Pheasant,
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

        wars_rosewarrior,
        wars_rosetank,
        wars_rosedog,

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
        wars_flag,
        horsebanner,
        citybanner,
        armybanner,
        armystand,
        armystand_detail,
        cityicon,
        unclaimed_icon,
        buildarea,
        godfire,
        wars_borderstick,
        //city_tower24,
        city_flagpole,
        city_pen,
        city_tenthut,
        city_workerhut,
        city_workerhut_long,
        city_guard_house,
        city_cobblestone,
        city_square,
        city_stonehall,
        city_smallhouse,
        city_bighouse,
        city_dirtwall,
        city_dirttower,
        city_palisade,
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
        city_tent,
        city_research,

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
        fol_tree_hard_lava,
        fol_tree_soft_lava,
        fol_tree_hard_snow,
        fol_tree_soft_snow,
        fol_sprout,
        fo_stone1,
        fol_bush1,
        fol_stoneblock,
        fol_tallgrass,
        fol_herbs,
        fol_farmculture,
        fol_farmculture2,
        fol_greenfoliage,

        resource_tree,
        resource_rubble,

        decor_statue,
        city_pavement,
        city_garden,
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
