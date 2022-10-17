

namespace GalaxyLogicGame.Planet_objects
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Blueberry : PlanetBase
	{
		public Blueberry ()
		{
			InitializeComponent ();
		}
        public override string Text { get => "X"; set { } }
        public override int Type { get => 5; set { } }
        public override BinaryIndicator Binary => binary;

    }
}