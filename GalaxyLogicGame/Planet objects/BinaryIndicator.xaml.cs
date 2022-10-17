

namespace GalaxyLogicGame.Planet_objects
{

    public partial class BinaryIndicator : AbsoluteLayout
    {
        public BinaryIndicator()
        {
            InitializeComponent();
        }

        public string BinaryString { get => label.Text; set { label.Text = value; } }
    }
}