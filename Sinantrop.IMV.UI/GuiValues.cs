namespace Sinantrop.IMV.UI
{
    class GuiValues
    {
        private static GuiValues _instance;

        private GuiValues()
        {

        }

        public GuiValues MyProperty
        {
            get
            {
                if (_instance == null)
                    _instance = new GuiValues();

                return _instance;
            }
        }
    }
}
