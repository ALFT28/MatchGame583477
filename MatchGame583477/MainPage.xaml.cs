using System;
using System.Diagnostics;

namespace MatchGame583477
{
    public partial class MainPage : ContentPage
    {

        // Cronómetro para medir el tiempo del juego
        private Stopwatch stopwatch;

        // Cuenta el número de parejas encontradas
        private int matchesFound;

        // Lista de botones en la página
        private List<Button> buttons;

        // Lista de emojis de animales para el juego
        private List<string> animalEmoji;

        // Generador de números aleatorios
        private Random random;

        public MainPage()
        {
            InitializeComponent();
            SetUpGame();
        }


        // Método para configurar el juego
        private void SetUpGame()
        {
            // Inicializa la lista de emojis de animales (dos de cada uno)
            animalEmoji = new List<string>()
            {
                "🐕","🐕",
                "🙈","🙈",
                "🐙","🐙",
                "🐘","🐘",
                "🦓","🦓",
                "🦒","🦒",
                "🐠","🐠",
                "🐬","🐬",
            };

            random = new Random(); // Inicializa el generador de números aleatorios

            // Obtiene todos los botones dentro del contenedor Grid1
            buttons = Grid1.Children.OfType<Button>().ToList();

            // Baraja y asigna los emojis a los botones
            ShuffleAndAssignEmojis();

            // Inicia el temporizador
            stopwatch = new Stopwatch();
            stopwatch.Start();

            // Resetea la cuenta de parejas encontradas
            matchesFound = 0;
        }

        // Método para barajar y asignar los emojis a los botones visible
        private void ShuffleAndAssignEmojis()
        {
            // Obtiene los botones visibles
            var visibleButtons = buttons.Where(b => b.IsVisible).ToList();

            // Filtra los emojis para los botones visibles
            var visibleEmojis = animalEmoji.Take(visibleButtons.Count).ToList();

            // Baraja los emojis visibles
            var shuffledEmojis = visibleEmojis.OrderBy(item => random.Next()).ToList();

            // Asigna los emojis barajados a los botones visibles
            for (int i = 0; i < visibleButtons.Count; i++)
            {
                visibleButtons[i].Text = shuffledEmojis[i];
            }
        }

        // Método para reiniciar el juego
        private async void RestartGame()
        {
            // Reiniciar el juego
            SetUpGame();
            // Hace visibles todos los botones
            foreach (Button button in buttons)
            {
                button.IsVisible = true;
            }
        }

        // Variable para almacenar el último botón clicado
        Button ultimoButtonClicked;

        // Bandera para indicar si se está buscando un par
        bool encontrandoMath = false;

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            // Si el botón es nulo o no está visible, retorna
            if (button == null || button.IsVisible == false)
                return;

            // Si no se está buscando un par
            if (encontrandoMath == false)
            {
                button.IsVisible = false;
                ultimoButtonClicked = button;
                encontrandoMath = true;
            }
            else if (button.Text == ultimoButtonClicked.Text)
            {
                button.IsVisible = false;
                encontrandoMath = false;

                // Incrementa la cuenta de parejas encontradas
                matchesFound++;

                // Reorganiza las figuras restantes
                ShuffleAndAssignEmojis();

                // Comprueba si se han encontrado todas las parejas
                if (matchesFound == 8) // Hay 8 parejas en total
                {
                    stopwatch.Stop(); // Detiene el cronómetro
                     // Muestra una alerta indicando que el juego ha terminado
                    bool restart = await DisplayAlert("Juego terminado", $"Has resuelto el juego en {stopwatch.Elapsed.TotalSeconds} segundos. ¿Quieres jugar de nuevo?", "Sí", "No");
                    if (restart)
                    {
                        RestartGame();
                    }
                }
            }
            else
            {
                // Si los textos no coinciden, vuelve a mostrar el último botón clicado
                ultimoButtonClicked.IsVisible = true;
                encontrandoMath = false;
            }
        }

    }

}
