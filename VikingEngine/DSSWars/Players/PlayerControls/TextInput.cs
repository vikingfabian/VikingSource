using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Players.PlayerControls
{
    class TextInput: AbsTextInputUpdate
    {
        AbsGameObject gameObject;
        public TextInput(AbsGameObject gameObject)
            :base()
        {
            this.gameObject = gameObject;
            string name = gameObject.Name(out _);
            init(name, name, null);
            InitComplete();
        }

        public override void textInput_refresh(bool textLengthChanged)
        {
            foreach (var p in DssRef.state.localPlayers)
            {
                p.hud.needRefresh = true;
            }
        }

        public override void textInput_complete(string result, object tag)
        {
            gameObject.NameEditEvent(result, tag);
            base.textInput_complete(result, tag);
        }
    }
}
