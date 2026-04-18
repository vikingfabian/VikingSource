using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.EngineSpace.HUD.RichBox
{
    class RbControllerSection : AbsRichBoxMember
    {
        string id;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">null is ending section</param>
        public RbControllerSection(string id)
        {
            this.id = id;
        }

        public override void Create(RichBoxGroup group)
        {
            if (group.controllerSections.Count > 0)
            {
                var previous = group.controllerSections[group.controllerSections.Count - 1];
                if (!previous.finalized)
                {
                    previous.end(group.carriage.position.Y);
                    group.controllerSections[group.controllerSections.Count - 1] = previous;
                }
            }

            if (id != null)
            {
                group.controllerSections.Add(new ControllerSection(id, group.carriage.position.Y));
            }
        
        }
    }

    struct ControllerSection
    {
        public VectorRect area;
        public string id;
        public bool finalized;

        public ControllerSection(string id, float y)
        {
            this.id = id;
            this.area.Position.Y = y;
        }

        public void end(float y)
        {
            this.area.SetBottom(y, true);
            finalized = true;
        }
    }
}
