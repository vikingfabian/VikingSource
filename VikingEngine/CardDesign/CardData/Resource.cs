using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardGraphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.CardData
{
    struct Resource
    {
        public Id id;
        public Number amount;
        public AbsTagType Get()
        {
            return GameDb.Current.tagDic[id];
        }

        public void ToMenu(RichBoxContent content)
        {            
            var tag = Get();
            content.Add(new RbText(amount.ToString()));
            content.hspace();
            content.Add(new RbImage(tag.icon));
        }

        public string ToNameString()
        {
            if (id.empty)
            {
                return "-None-";
            }
            return GameDb.Current.tagDic[id].name;
        }

        public string ToAmountNameString()
        {
            return amount.ToString() + " " + ToNameString();
        }
    }

    class ResourceType : AbsTagType
    {
        public ResourceType(SpriteName icon, string name, List<Id> masterTo, Id? id) :
            base(icon, name, masterTo, id)
        {
        }
        public override bool IsTag => false;
    }


    class ResourceList : List<Resource>
    {
        public ResourceList() 
            :base(4)
        { }

        //public int mana;
        //public int redMana;
        //public int greenMana;
        //public int blueMana;
        //public int yellowMana;
        //public int whiteMana;
        //public int blackMana;
        //public int coin;
        //public int victoryPoint;
        //public int wildMana;
        //public int actionPoint;

        /// <summary>
        /// Returns the current value of the specified resource type.
        /// </summary>
        //public int Get(DefaultResourceType type)
        //{
        //    switch (type)
        //    {
        //        case DefaultResourceType.Mana: return mana;
        //        case DefaultResourceType.RedMana: return redMana;
        //        case DefaultResourceType.GreenMana: return greenMana;
        //        case DefaultResourceType.BlueMana: return blueMana;
        //        case DefaultResourceType.YellowMana: return yellowMana;
        //        case DefaultResourceType.WhiteMana: return whiteMana;
        //        case DefaultResourceType.BlackMana: return blackMana;

        //        // New Cases
        //        case DefaultResourceType.WildMana: return wildMana;
        //        case DefaultResourceType.ActionPoint: return actionPoint;

        //        case DefaultResourceType.Coin: return coin;
        //        case DefaultResourceType.VictoryPoint: return victoryPoint;

        //        case DefaultResourceType.NUM_NONE:
        //        default:
        //            return 0;
        //    }
        //}

        ///// <summary>
        ///// Sets the resource to a specific value, clamped between -MaxValue and +MaxValue.
        ///// </summary>
        //public void Set(DefaultResourceType type, int value)
        //{
        //    // Clamp the value to ensure it stays within bounds
        //    int clampedValue = Math.Clamp(value, -Number.MaxValue, Number.MaxValue);

        //    switch (type)
        //    {
        //        case DefaultResourceType.Mana: mana = clampedValue; break;
        //        case DefaultResourceType.RedMana: redMana = clampedValue; break;
        //        case DefaultResourceType.GreenMana: greenMana = clampedValue; break;
        //        case DefaultResourceType.BlueMana: blueMana = clampedValue; break;
        //        case DefaultResourceType.YellowMana: yellowMana = clampedValue; break;
        //        case DefaultResourceType.WhiteMana: whiteMana = clampedValue; break;
        //        case DefaultResourceType.BlackMana: blackMana = clampedValue; break;

        //        // New Cases
        //        case DefaultResourceType.WildMana: wildMana = clampedValue; break;
        //        case DefaultResourceType.ActionPoint: actionPoint = clampedValue; break;

        //        case DefaultResourceType.Coin: coin = clampedValue; break;
        //        case DefaultResourceType.VictoryPoint: victoryPoint = clampedValue; break;

        //        default: break;
        //    }
        //}

        ///// <summary>
        ///// Adds (or subtracts if negative) the amount to the specified resource.
        ///// </summary>
        //public void Add(DefaultResourceType type, int add)
        //{
        //    if (type == DefaultResourceType.NUM_NONE) return;

        //    int current = Get(type);
        //    Set(type, current + add);
        //}

        ///// <summary>
        ///// Returns true if any resource field has a non-zero value.
        ///// </summary>
        public bool HasValue
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.amount.value > 0)
                    { 
                        return true;
                    }
                }
                return false;
            }
        }
        //        return mana != 0 ||
        //               redMana != 0 ||
        //               greenMana != 0 ||
        //               blueMana != 0 ||
        //               yellowMana != 0 ||
        //               whiteMana != 0 ||
        //               blackMana != 0 ||
        //               wildMana != 0 ||
        //               actionPoint != 0 ||
        //               coin != 0 ||
        //               victoryPoint != 0;
        //    }
        //}

        public void ToMenu(RichBoxContent content)
        {
            if (HasValue)
            {
                //for (DefaultResourceType type = 0; type < DefaultResourceType.NUM_NONE; ++type)
                //{
                //    int value = Get(type);
                //    if (value != 0)
                //    {
                        //IconName.Resource(type, out var icon, out _);
                foreach (var item in this)
                {
                    item.ToMenu(content);
                    content.space(2);
                    //}
                }
            }
            else
            {
                content.Add(new RbText("None"));
            }
        }

        public void ToCard(List<Graphics.AbsDraw> images, Vector2 pos, float width)
        {
            float startX = pos.X;
            float right = pos.X + width;
            foreach (var item in this)
            {
                var tag = item.Get();
                //IconName.Resource(type, out var icon, out _);
                Graphics.Image iconImg = new Graphics.Image(tag.icon, pos, new Vector2(CardFace.IconSize), ImageLayers.Top4, false, false);
                var valueText = new SpriteText(item.amount.ToString(), iconImg.Area.PercentToPosition(0.7f, 0.7f), CardFace.IconSize * 0.6f, ImageLayers.Top0, new Vector2(0.5f), Color.White);

                pos.X += Math.Max(CardFace.IconSize * 1.2f, CardFace.IconSize * 0.8f + valueText.size.X * 0.5f);
                if (pos.X > right)
                {
                    pos.X = startX;
                    pos.Y += CardFace.IconSize * 1.2f;
                }

                images.Add(iconImg);
                images.AddRange(valueText.letters);                
            }
            
        }
    }

    //enum DefaultResourceType
    //{
    //    ActionPoint,
    //    Mana,
    //    RedMana,
    //    GreenMana,
    //    BlueMana,
    //    YellowMana,
    //    WhiteMana,
    //    BlackMana,
    //    WildMana,
    //    Coin,
    //    VictoryPoint,
    //    NUM_NONE
    //}
}
