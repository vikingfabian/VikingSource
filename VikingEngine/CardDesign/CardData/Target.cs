using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.DSSWars;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.ToGG;

namespace VikingEngine.CardDesign.CardData
{
    //class TargetOptions
    //{
    //    public List<TargetOptions>
    //}

    class Target
    {
        Number targetCount = new Number(1);
        bool mayRepeatOneTarget = false;
        List<Select> select = new List<Select> { new Select() };
        List<Filter> filter = new List<Filter>();
        UnitPropertyType splashProperty = UnitPropertyType.Attack;
        SplashType splash = SplashType.None;
        SplashFallOff splashFallOff = SplashFallOff.Equal;

        //public bool includeSelf = false;

        public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu, bool fromUnit)
        {
            content.h2("Target", DSSWars.HudLib.TitleColor_Head2);
            new NumberEditor().DragButton(content, menu, "Target count", Number.EndlessPositiveBounds, TargetCountProperty);
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("May repeat one target") }, MayRepeatProperty));
            content.text("Target will happen to all included, minus all excluded", DSSWars.HudLib.InfoYellow_Light);
            DSSWars.HudLib.Label(content, "Select");
            content.newLine();
            
            for (int i = 0; i < select.Count; i++)
            {
                //content.newLine();
                DropDownBuilder dropdown = new DropDownBuilder("select type" + i.ToString());
                {
                    for (TargetSelectType val = 0; val < TargetSelectType.NUM; val++)
                    {
                        if (!fromUnit)
                        {
                            switch (val)
                            {
                                case TargetSelectType.Self:
                                case TargetSelectType.Closest:
                                case TargetSelectType.Opposite:
                                case TargetSelectType.Adjacent:
                                case TargetSelectType.LeftOfMe:
                                case TargetSelectType.RightMost:
                                case TargetSelectType.FrontOfMe:
                                case TargetSelectType.BehindMe:
                                    continue;
                            }
                        }

                        dropdown.AddOption(IconName.Select(val), val == select[i].type, false,
                            new RbAction2Arg<int, TargetSelectType>((int index, TargetSelectType value) => {
                                var m = select[index];
                                m.type = value;
                                select[index] = m;
                                menu.CloseDropDown(); }, i, val), null);
                    }

                    dropdown.Build(content, SpriteName.NO_IMAGE, "Select type", menu);
                    content.space();
                    cHud.DeleteButton(content, new RbAction1Arg<int>(select.RemoveAt, i));
                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Inclusive") }, SelectIsInclusive) { propertyTag = i, });
                    content.Add(new RbSeperationLine());
                    content.newParagraph();
                }
            }
            content.newLine();
            cHud.AddButton(content, "select option", new RbAction( () => { select.Add(new Select()); }));
            content.newParagraph();

            DSSWars.HudLib.Label(content, "Filter");
            content.newLine();

            for (int i = 0; i < filter.Count; i++)
            {
                int index = i;
                //content.newLine();
                DropDownBuilder dropdown = new DropDownBuilder("filter type" + i.ToString());
                {
                    for (TargetFilterType val = 0; val < TargetFilterType.NUM; val++)
                    {
                        dropdown.AddOption(IconName.Filter( val), val == filter[i].type, false,
                            new RbAction2Arg<int, TargetFilterType>((int index, TargetFilterType value) => {
                                var m = filter[index];
                                m.type = value;
                                m.id = Id.Empty;
                                filter[index] = m;
                                menu.CloseDropDown();
                            }, i, val), null);
                    }

                    dropdown.Build(content, SpriteName.NO_IMAGE, "Filter type", menu);
                    content.space();
                    cHud.DeleteButton(content, new RbAction1Arg<int>(filter.RemoveAt, i));
                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Inclusive") }, FilterIsInclusive) { propertyTag = i, });

                    switch (filter[i].type)
                    {
                        case TargetFilterType.HasTag:
                            content.newLine();
                            new TagEditor().SelectTagToEditor(content, menu, true, filter[i].id, (Id id) =>
                            {
                                var m = filter[index];
                                m.id = id;
                                filter[index] = m;
                            });
                            break;
                        case TargetFilterType.HasResource:
                            content.newLine();
                            new TagEditor().SelectTagToEditor(content, menu, false, filter[i].id, (Id id) =>
                            {
                                var m = filter[index];
                                m.id = id;
                                filter[index] = m;
                            });
                            break;
                    }

                    content.Add(new RbSeperationLine());
                    content.newParagraph();
                }
            }
            content.newLine();
            cHud.AddButton(content, "filter option", new RbAction(() => { filter.Add(new Filter()); }));

            content.newParagraph();
            DropDownBuilder splashdropdown = new DropDownBuilder("splash");
            for (SplashType s = 0; s < SplashType.NUM; s++)
            {
                splashdropdown.AddOption(s.ToString(), s == splash, s == SplashType.None,
                    new RbAction1Arg<SplashType>((SplashType s) => { splash = s; menu.CloseDropDown(); }, s), null);
            }
            splashdropdown.Build(content, SpriteName.NO_IMAGE, "Splash", menu);

            if (splash != SplashType.None)
            {
                DropDownBuilder falloffdropdown = new DropDownBuilder("splash fo");
                for (SplashFallOff fo = 0; fo < SplashFallOff.NUM; fo++)
                {
                    falloffdropdown.AddOption(fo.ToString(), fo == splashFallOff, false,
                        new RbAction1Arg<SplashFallOff>((SplashFallOff fo) => { splashFallOff = fo; menu.CloseDropDown(); }, fo), null);
                }
                falloffdropdown.Build(content, SpriteName.NO_IMAGE, "Splash falloff", menu);
            }
            //if (fromUnit)
            //{
            //    content.newLine();
            //    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("including self") },
            //        selfProperty));
            //}
        }

        int TargetCountProperty(object tag, bool set, int value)
        {
            if (set)
            {
                targetCount.value = value;
            }
            return targetCount.value;
        }

        bool SelectIsInclusive(object tag, bool set, bool value)
        {
            int index = (int)tag;
            if (set)
            {
                var m = select[index];
                m.include = value;
                select[index] = m;
            }
            return select[index].include;
        }
        bool FilterIsInclusive(object tag, bool set, bool value)
        {
            int index = (int)tag;
            if (set)
            {
                var m = filter[index];
                m.include = value;
                filter[index] = m;
            }
            return filter[index].include;
        }
        bool MayRepeatProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                mayRepeatOneTarget = value;
            }
            return mayRepeatOneTarget;
        }

        //public bool selfProperty(object tag, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        includeSelf = value;
        //    }
        //    return includeSelf;
        //}

        public void ToAttackMenu(RichBoxContent content)
        {
            //if (type != TargetFilterType.Friendly &&
            //    side !=  TargetSide.Enemy)
            //{
            //    content.Add(new RbText("Attack " + Description()));
            //    content.newLine();
            //}
        }

        public string Description()
        {
            string desc = "target: ";// + type.ToString() + " " + side.ToString();

            foreach (var m in select)
            {
                desc += (m.include ? "+" : "-") + IconName.Select(m.type) + ", ";               
            }
            foreach (var m in filter)
            {
                desc += (m.include ? "+" : "-") + IconName.Filter(m.type);
                if (!m.id.empty)
                {
                    desc += " " + TextLib.Quote( cref.current.game.tagDic[m.id].name.ToString());
                }
                desc += ", ";
            }
            //if (includeSelf)
            //{
            //    desc += ", including self";
            //}
            return desc;
        }
    }


    struct Select
    {
        public TargetSelectType type;
        public bool include;

        public Select()
        {
            include = true;
        }

        
    }
    struct Filter
    {
        public Id id;
        public TargetFilterType type;
        public bool include;

        public Filter()
        {
            include = true;
            id = Id.Empty;
        }
    }
    enum TargetSelectType //Iclusive or exclusive
    {   
        First,
        Self,
        All,
        ManualSelect,
        Area,
        Row,
        Lane,
        Random, //may repeat?
        Closest,
        Opposite, //may target two?
        LeftMost,
        CenterMost,
        RightMost,
        Flank,
        Adjacent,
        LeftOfMe,
        RightOfMe,
        FrontOfMe,
        BehindMe,
        NUM
    }

    enum TargetFilterType //Iclusive or exclusive
    {
        HasTag,
        HasResource,

        HasHealth,
        HasAttack,
        
        Friendly,
        Enemy,
        NUM
    }
    enum SplashType //How many repeats
    {
        None,
        AllOfSameTag,
        AllOfSamePropertyAmount,
        Adjacent,
        OfTargetType,
        NUM
    }

    enum SplashFallOff
    {
        Equal,
        Stepping, //+-value 
        Set, //Specified value
        Remaining, //example healing that wasnt used
        NUM
    }

    //enum TargetSelectType
    //{ 
    //    Self,
    //    Adjacent,
    //    Area,
    //    Row,
    //    Lane,
    //    All,
    //    ManualSelect,
    //    ManualSelectArea,
    //    ManualSelectRow,
    //    ManualSelectLane,


        //    ManualSelectFlank,
        //    ManualSelectCenter,

        //}



    enum TargetPlacementType
    { 
        Selected,
        All,
        Random,
    }

    enum TargetSide
    { 
        Any,
        Friendly,
        Enemy,
        NUM
    }

    
}
