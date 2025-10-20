
using System.Collections.Generic;

namespace MyNotes
{
    public partial class MainPage : ContentPage
    {
        // Constructor for MainPage
        public MainPage()
        {
            InitializeComponent(); // Initialize UI components
            Load(); // Load saved notes on startup
        }

        // List to keep track of all dynamically created grids (each representing a note)
        List<Grid> grids = [];

        // Event handler for the "Add" button click
        private void Button_Clicked(object sender, EventArgs e)
        {
            NewGrid(vsl); // Add a new note grid to the layout
        }

        // Method to create a new note grid and add it to the provided VerticalStackLayout
        public void NewGrid(VerticalStackLayout vsl, string text = "")
        {
            // Create a new grid with two columns: one for Entry, one for Button
            Grid grid = new()
            {
                ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Star }, // Entry fills remaining space
                        new ColumnDefinition { Width = GridLength.Auto }  // Button takes minimal space
                    }
            };

            // Create an Entry for note text
            Entry ety = new()
            {
                Placeholder = "my notes",
                Text = text,
            };
            // Save notes whenever the text changes
            ety.TextChanged += (sender, e) =>
            {
                Save();
            };
            grid.Add(ety, 0, 0); // Add Entry to the first column

            // Create a Delete button for the note
            Button btn = new()
            {
                Text = "Del"
            };
            // Event handler to remove the note grid when the button is clicked
            btn.Clicked += (x, args) =>
            {
                vsl.Children.Remove(grid); // Remove grid from layout
                grids.Remove(grid); // Remove grid from tracking list
                Save(); // Save changes
            };

            grid.Add(btn, 1, 0); // Add Button to the second column

            vsl.Add(grid); // Add the grid to the layout

            grids.Add(grid); // Track the grid in the list
        }

        // Save all notes to Preferences
        public void Save()
        {
            // Remove all existing note keys from Preferences
            int i = 0;
            while (Preferences.ContainsKey("key" + i))
            {
                Preferences.Remove("key" + i);
                i++;
            }

            // Collect all non-empty note texts from the grids
            List<string> items = [];
            foreach (var grid in grids)
            {
                var child = grid.Children.ToArray().GetValue(0);
                if (child is Entry entry && !string.IsNullOrEmpty(entry.Text))
                {
                    items.Add(entry.Text);
                }
            }
            // Save each note text to Preferences with a unique key
            for (int j = 0; j < items.Count; j++)
            {
                Preferences.Set("key" + j, items[j]);
            }
        }

        // Event handler for the "Save" button click
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Save(); // Save all notes
        }

        // Event handler for the "Load" button click
        private void Button_Clicked_2(object sender, EventArgs e)
        {
            Load(); // Load notes from Preferences
        }

        // Load notes from Preferences and display them in the layout
        private void Load()
        {
            // Remove all existing grids from the layout and clear the grids list
            foreach (Grid grd in grids)
            {
                vsl.Remove(grd);
            }
            grids.Clear();

            // Retrieve all saved notes from Preferences
            List<string> items = [];
            int x = 0;
            string item;
            // Only add non-empty items, stop when an empty string is found
            while (true)
            {
                item = Preferences.Get("key" + x, "");
                if (string.IsNullOrEmpty(item))
                    break;
                items.Add(item);
                x++;
            }

            // Create a new grid for each saved note
            foreach (string value in items)
            {
                NewGrid(vsl, value);
            }
        }

        // Output all note texts as a single string to the output label
        //public void outputF(object sender, EventArgs e)
        //{
        //    string oto = "";
        //    foreach (var grid in grids)
        //    {
        //        var child = grid.Children.ToArray().GetValue(0);
        //        if (child is Entry entry && !string.IsNullOrWhiteSpace(entry.Text))
        //        {
        //            oto += entry.Text + " ";
        //        }
        //    }
        //    output.Text = oto.Trim(); // Display concatenated notes
        //}
    }
}

