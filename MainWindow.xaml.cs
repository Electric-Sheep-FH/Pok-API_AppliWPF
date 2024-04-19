using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TP_PokéAPI_WPFtest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void pokemon_Button_Click(object sender, RoutedEventArgs e)
        {
            string pokemonName = pokemon_TextBox.Text;

            string pokeapi = @"https://pokeapi.co/api/v2/pokemon/" + pokemonName.ToLower();

            string json = new WebClient().DownloadString(pokeapi);

            JsonObject root = JsonNode.Parse(json).AsObject();
            JsonNode imageUrl = root["sprites"]["front_default"];

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(imageUrl.ToString(), "image.png");
            }

            Image pokemonImage = new Image();
            BitmapImage bitmapPokemon = new BitmapImage();

            bitmapPokemon.BeginInit();
            bitmapPokemon.UriSource = new Uri(imageUrl.ToString(), UriKind.Absolute);
            //bitmapPokemon.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"image.png");
            bitmapPokemon.EndInit();

            pokemon_Image.Source = bitmapPokemon;

            string toDisplay ="";

            JsonNode name = root["name"];
            JsonNode id = root["id"];
            JsonNode types = root["types"];
            JsonNode movesNode = root["moves"];
            toDisplay += "Pokemon name : " + name + "\n" + "ID : " + id + "\n" + "Pokemon type(s) : ";


            foreach (var type in types.AsArray())
            {
                toDisplay += type["type"]["name"] + ",";
            }
            toDisplay += "\nMoves : ";

            int countAttack = 0;
            foreach (var move in movesNode.AsArray())
            {
                countAttack++;
                toDisplay += "- " + move["move"]["name"] + "\n";
            }
            toDisplay += "### Numbers of moves : " + countAttack;

            pokemon_result.Text = toDisplay;
        }

        private void pokemon_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            pokemon_TextBox.Text = "";
        }
    }
}