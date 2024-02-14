using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Data.Property
{

    abstract class AbsProperty
    {
        virtual public SpriteName Icon => SpriteName.NO_IMAGE;
        abstract public string Name { get; }
        abstract public string Desc { get; }

        virtual public List<HUD.RichBox.AbsRichBoxMember> AdvancedCardDisplay()
        {
            return null;
        }

        virtual public ToggEngine.Display2D.AbsExtToolTip[] DescriptionKeyWords() { return null; }

        virtual public bool EqualType(AbsProperty obj)
        {
            return this.GetType() == obj.GetType();
        }

        virtual public void writePropertyType(System.IO.BinaryWriter w)
        {
            w.Write((byte)MainType);
        }
        virtual public void writeData(System.IO.BinaryWriter w)
        {
        }
        virtual public void readData(System.IO.BinaryReader r)
        {
        }

        public void writeProperty(System.IO.BinaryWriter w)
        {
            writePropertyType(w);
            writeData(w);
        }

        public static AbsProperty ReadProperty(System.IO.BinaryReader r)
        {
            PropertyMainType mainType = (PropertyMainType)r.ReadByte();
            AbsProperty result;

            switch (mainType)
            {
                case PropertyMainType.UnitProperty:
                    result = AbsUnitProperty.ReadUnitPropertyType(r);
                    break;
                case PropertyMainType.MonsterAction:
                    result = AbsMonsterAction.ReadType(r);
                    break;

                default:
                    throw new NetworkException("AbsUnitAction " + mainType.ToString(), (int)mainType);
            }

            result.readData(r);

            return result;
        }

        virtual public PropertyMainType MainType => PropertyMainType.None;

        virtual public AbsExtToolTip[] ExtendedTooltipKeyWords()
        {
            return null;
        }

        //virtual public SpriteName HudBackgroundTexture => SpriteName.NO_IMAGE;
    }

    class DamageTrapProperty : AbsProperty
    {
        Damage damage;
        public DamageTrapProperty(Damage damage)
        {
            this.damage = damage;
        }

        public override string Name => "Damage Trap " + damage.ValueToString();

        public override string Desc => "On enter: Gives " + damage.description();

        public override bool EqualType(AbsProperty obj)
        {
            DamageTrapProperty uProp = obj as DamageTrapProperty;

            return uProp != null && uProp.damage.Equals(this.damage);
        }
    }

    class StaminaMoveCost : AbsProperty
    {
        public StaminaMoveCost()
        {
        }

        public override string Name => LanguageLib.Stamina + " drain";

        public override string Desc => "It costs 1 " + LanguageLib.Stamina + " to leave the square";
    }

    

    enum PropertyMainType
    {
        None,
        UnitProperty,
        MonsterAction,
        TerrainProperty,
    }
}
