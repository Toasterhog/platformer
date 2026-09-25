using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace platformer {
    class Program
    {   
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 600;
        
        static void Main(string[] args) {
            using (var window = new RenderWindow(
                       new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Platformer")) {
                window.Closed += (o, e) => window.Close();
                // Initializes
                Entity.InitEntitytextures(); //viktigt att detta är innan de andra innit sakerna
                Scene scene = new Scene();
                scene.Load("level0");
                window.SetView(new View(
                    new Vector2f(200, 150), // finns ingen specifik bakom värdena förrutom att det ser bättre för att spelet blir centrerat i vyn
                    new Vector2f(400, 300)
                ));
                
                //fonten
                Font font = new Font("assets/future.ttf");
                Text coins = new Text($"Money:", font, 12);
                coins.FillColor = Color.White;
                coins.OutlineColor = Color.Black;
                coins.OutlineThickness = 2;
                coins.Position = new Vector2f(10, 10);
                
                //Drawing och Updates
                Clock clock = new Clock();
                while (window.IsOpen)
                {
                    window.DispatchEvents();
                    float deltaTime = clock.Restart().AsSeconds();
                    scene.UpdateAll(deltaTime);
                    if (scene.FindByType(out Player player)) // UpdatesHud 
                    {
                        coins.DisplayedString = $"Money:{scene.money} ";
                    }
                    window.Clear();
                    // Drawing
                    scene.RenderAll(window);
                    window.Draw(coins); // ritar ut coins värde på skärmen
                    
                    window.Display();
                }
            }
        }
    }
}