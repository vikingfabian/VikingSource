using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Players.Command
{
    class ClaimCityGommand : AbsCommand
    {
        IntVector2 subTile;

        public ClaimCityGommand(SoldierGroup group, IntVector2 subTile, bool queueCommand)
            : base(group, queueCommand)
        {
            this.subTile = subTile;
        }

        public override void begin(SoldierGroup group)
        {
            base.begin(group);
            new SettlerTransform(group, subTile);
        }

        protected override CommandType GetCommandType()
        {
            return CommandType.ClaimCity;
        }
    }
}
