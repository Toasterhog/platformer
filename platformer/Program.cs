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
                // TODO: Initialize
                Entity.InitEntitytextures(); //viktigt att detta är innan dze andra innit sakerna
                
                
                Scene scene = new Scene();
                scene.Load("level0");
                // scene.Spawn(new Background());
                //
                // Door door = new Door();
                // door.Position = new Vector2f(18f*5, 18f * 10);
                // scene.Spawn(door);
                //
                // Key key = new Key();
                // key.Position = new Vector2f(240f, 120f);
                // scene.Spawn(key);
                //
                // Player player = new Player();
                // player.Position = new Vector2f(18, 18);
                // scene.Spawn(player);
                //
                // for (int i = 0; i < 20; i++) {
                //     scene.Spawn(new Platform {
                //         Position = new Vector2f(18 + i * 18, 24f*10)
                //     });
                // }
                
                window.SetView(new View(
                    new Vector2f(200, 150),
                    new Vector2f(400, 300)
                )); 
                
                Clock clock = new Clock();
                while (window.IsOpen) {
                    window.DispatchEvents();
                    float deltaTime = clock.Restart().AsSeconds();
                    // TODO: Updates
                    scene.UpdateAll(deltaTime);
                    
                    window.Clear();
                    // TODO: Drawing
                    scene.RenderAll(window);
                    
                    window.Display();
                }
            }
        }
    }
}