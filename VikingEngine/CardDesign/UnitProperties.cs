using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.PJ.CarBall;
using VikingEngine.PJ.Joust;
using VikingEngine.ToGG;

namespace VikingEngine.CardDesign
{
    class UnitProperties
    {
        public AbsUnitProperty[] properties = new AbsUnitProperty[(int)UnitPropertyType.NUM_NONE];

        public void ToMenu(RichBoxContent content)
        {
            RichBoxContent clarify = new RichBoxContent();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i] != null && properties[i].enabled)
                {
                    properties[i].ToMenu(content, clarify);
                }
            }
            if (clarify.Count > 0)
            {
                content.newLine();
                content.AddRange(clarify);
            }
        }

        public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                bool enabled = properties[i] != null && properties[i].enabled;
                UnitPropertyType propertyType = (UnitPropertyType)i;
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText( propertyType.ToString()) }, Enabled){propertyTag = propertyType});

                if (enabled && properties[i].HasVariables)
                {
                    properties[i].ToEditor(content, menu);
                    content.Add(new RbSeperationLine());
                }
            }
        }

        public bool Enabled(object tag, bool set, bool val)
        {
            UnitPropertyType propertyType = (UnitPropertyType)tag;
            AbsUnitProperty property = properties[(int)propertyType];
            if (set)
            {
                if (val && property == null)
                {
                    create(propertyType);
                }
                else if (property != null)
                {
                    property.enabled = val;
                }
            }
            return property != null && property.enabled;
        }

        void create(UnitPropertyType propertyType)
        {
            AbsUnitProperty property = null;
            switch (propertyType)
            {
                case UnitPropertyType.Attack:
                    property = new AttackProperty();
                    break;
                case UnitPropertyType.Defence:
                    property = new DefenceProperty();
                    break;
                case UnitPropertyType.Pierce:
                    property = new Pierce();
                    break;
                case UnitPropertyType.Health:
                    property = new HealthProperty();
                    break;
                case UnitPropertyType.Ranged:
                    property = new Ranged();
                    break;
                case UnitPropertyType.Shield:
                    property = new ShieldProperty();
                    break;
            }

            properties[(int)propertyType] = property;

        }
    }

    abstract class AbsUnitProperty
    {
        public bool enabled = true;
        public int value;
        public int StartValue = 1;

        abstract public void ToMenu(RichBoxContent content, RichBoxContent clarify);
        virtual public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu) 
        {
            RbDragButton.RbDragButtonGroup(content, new List<float> { 1f }, new DragButtonSettings(Const.PositiveBounds, 1),
                ValueProperty, false);
        }

        public int ValueProperty(object tag, bool set, int value)
        {
            if (set)
            {
                this.StartValue = value;
            }
            return this.StartValue;
        }

        abstract public UnitPropertyType Type { get; }
        virtual public bool HasVariables => true;
    }

    class HealthProperty : AbsUnitProperty
    {
        //public EventType ResetEvent = EventType.TimeNever;
        public int damage = 0;

        public override void ToMenu(RichBoxContent content, RichBoxContent clarify)
        {
            content.Add(new RbText(StartValue.ToString()));
            content.hspace();
            content.Add(new RbImage(SpriteName.CardIconHealth));
            content.space(2);

            //if (ResetEvent != EventType.TimeNever)
            //{
            //    clarify.Add(new RbText("Health reset: " + ResetEvent.ToString()));
            //    clarify.newLine();
            //}
            if (damage > 0)
            {
                clarify.Add(new RbText("Damage: " + damage.ToString()));
                clarify.newLine();
            }
        }

        public override void ToEditor( RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            base.ToEditor(content, menu);

            //DropDownBuilder dropdown = new DropDownBuilder("damage reset");
            //{
            //    foreach (var m in resoutionPercOptions)
            //    {
            //        dropdown.AddOption(string.Format(Ref.langOpt.GraphicsOption_Resolution_PercentageOption, m),
            //            Engine.Screen.UseRecordingPreset == RecordingPresets.NumNon &&
            //            m == Engine.Screen.WindowScalePerc,
            //            m == 100,
            //            new RbAction1Arg<int>(setResolutionPercProperty, m), null);
            //    }

            //    dropdown.Build(content, SpriteName.NO_IMAGE, "Damage reset", menu);
            //}
        }

        public override UnitPropertyType Type => UnitPropertyType.Health;

        
    }

    //class PowerProperty : HealthProperty
    //{
    //    public override UnitPropertyType Type => UnitPropertyType.Power;
    //}

    class AttackProperty : AbsUnitProperty
    {
        public Target attackTarget = new Target();

        public override void ToMenu(RichBoxContent content, RichBoxContent clarify)
        {
            content.Add(new RbText(StartValue.ToString()));
            content.hspace();
            content.Add(new RbImage(SpriteName.CardIconAttack));
            content.space(2);

            attackTarget.ToAttackMenu(clarify);
        }

        public override UnitPropertyType Type => UnitPropertyType.Attack;
    }
    class DefenceProperty : AbsUnitProperty
    {
        public override void ToMenu(RichBoxContent content, RichBoxContent clarify)
        {
            content.Add(new RbText(StartValue.ToString()));
            content.hspace();
            content.Add(new RbImage(SpriteName.CardIconDefence));
            content.space(2);
        }
        public override UnitPropertyType Type => UnitPropertyType.Defence;
    }
    class ShieldProperty : AbsUnitProperty
    {
        //public EventType ResetEvent;

        public override void ToMenu(RichBoxContent content, RichBoxContent clarify)
        {
            content.Add(new RbText(StartValue.ToString()));
            content.hspace();
            content.Add(new RbImage(SpriteName.CardIconShield));
            content.space(2);

            //if (ResetEvent != EventType.TimeNever)
            //{
            //    clarify.Add(new RbText("Shield reset: " + ResetEvent.ToString()));
            //    clarify.newLine();
            //}
        }
        public override UnitPropertyType Type => UnitPropertyType.Shield;
    }
    class Pierce : AbsUnitProperty
    {
        public override void ToMenu(RichBoxContent content, RichBoxContent clarify)
        {
            clarify.Add(new RbText(string.Format("Pierce {0}", StartValue)));
            clarify.newLine();
        }
        
        public override UnitPropertyType Type => UnitPropertyType.Pierce;
        public override bool HasVariables => false;
    }
    class Ranged : AbsUnitProperty
    {
        public override void ToMenu(RichBoxContent content, RichBoxContent clarify)
        {
            clarify.Add(new RbText("Ranged attack"));
            clarify.newLine();
        }
        
        public override UnitPropertyType Type => UnitPropertyType.Ranged;
        public override bool HasVariables => false;
    }


    enum UnitPropertyType
    { 
        //Power,
        Attack,
        Defence, 
        Health, 
        Shield,
        Pierce,
        Ranged,
        NUM_NONE
    }

    enum UnitPropertiesDisplayType
    { 
        Power,
        HealthAttack,
    }

    enum UnitActivationStatus
    { 
        Stunned,
        Sleeping,
        Ready,
        Activated,
    }
}
