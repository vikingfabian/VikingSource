using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.CardDesign.Entity;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.CardDesign.CardData
{
    class GameDb : IHasId, IHasText
    {
        //public static GameDb Current = new GameDb();

        public GameMeta meta = new GameMeta();

        /// <summary>
        /// Tags and Resources
        /// </summary>
        public Dictionary<Id, AbsTagType> tagDic = new Dictionary<Id, AbsTagType>(8);

        public Dictionary<Id, CardEntity> cards = new Dictionary<Id, CardEntity>(64);
        public Dictionary<Id, FieldUnit> unitTypes = new Dictionary<Id, FieldUnit>(64);

        public PlayerSupply playerSupply = new PlayerSupply();
        public PlayerSupply commonSupply = new PlayerSupply();
        public MapType mapType = MapType.Lanes;

        public GameDb() 
        {
            meta.name = new Text("Game " + Ref.rnd.Int(1000));
        }

        public Id Id { get { return meta.id; } }

        public Text GetName(TextType type) { return meta.GetName(type); }
        public void SetName(TextType type, Text name) { meta.GetName(type); }
    }
}
