using Microsoft.Xna.Framework.Input;
using VikingEngine.Input;

namespace VikingEngine.ToGG
{
    class EditorInputMap
    {
        public IButtonMap freePaint;
        public IButtonMap pickTool;
        public IButtonMap bucketTool;

        public EditorInputMap()
        {
            freePaint = new Input.AlternativeButtonsMap(
                new KeyboardButtonMap(Keys.LeftControl), new KeyboardButtonMap(Keys.RightControl));

            pickTool = new Input.AlternativeButtonsMap(
                new KeyboardButtonMap(Keys.LeftAlt), new KeyboardButtonMap(Keys.RightAlt));

            bucketTool = new Input.AlternativeButtonsMap(
                new KeyboardButtonMap(Keys.LeftShift), new KeyboardButtonMap(Keys.RightShift));
        }
    }
}
