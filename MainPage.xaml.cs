
using System.Collections.Generic;

namespace MyNotes
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            Load();
        }
        List<Grid> grids = [];
        private void Button_Clicked(object sender, EventArgs e)
        {
            NewGrid(vsl);
        }
        public void NewGrid(VerticalStackLayout vsl, string text = "")
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
                Placeholder = "my notes",
                Text = text,
            };
            ety.TextChanged += (sender, e) =>
            {
                Save();
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

            grids.Add(grid);
        }
        public void Save()
        {
            List<string> items = [];
            foreach (var item in grids)
            {
                var text = item.Children.ToArray().GetValue(0);
                if (text != null)
                {
                    if (text is Entry entry)
                    {
                        // Use the entry object here
                        items.Add(entry.Text); // Example of accessing Entry properties
                    }
                }

            }
            for (int i = 0; i < items.Count; i++)
            {
                Preferences.Set("key" + i, items[i]);

            }

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Save();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            Load();
        }

        private void Load()
        {
            foreach (Grid grd in grids)
            {
                vsl.Remove(grd);
            }
            string item = "";
            List<string> items = [];
            int x = 0;
            do
            {
                item = Preferences.Get("key" + x, "");
                items.Add(item);

                x++;
            } while (item != null && item != "");

            foreach (string value in items)
            {
                NewGrid(vsl, value);
            }
        }
    }
}
