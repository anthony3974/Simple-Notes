
namespace MyNotes
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }
        List<Grid> grids = new List<Grid>();
        private void Button_Clicked(object sender, EventArgs e)
        {
            newGrid(vsl);
        }
        public void newGrid(VerticalStackLayout vsl)
        {
            Grid grid = new()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star }, // Entry fills remaining space
                    new ColumnDefinition { Width = GridLength.Auto }  // Button takes minimal space
                }
            };

            Entry ety = new()
            {
                Placeholder = "my notes"
            };
            grid.Add(ety, 0, 0); // Add Entry to the first column

            Button btn = new()
            {
                Text = "Del"
            };
            btn.Clicked += (x, args) =>
            {
                if (x != null)
                    vsl.Children.Remove(grid);
            };

            grid.Add(btn, 1, 0); // Add Button to the second column



            vsl.Add(grid);
        }
        public void save()
        {

        }
    }
}
