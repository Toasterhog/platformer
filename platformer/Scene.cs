using SFML.Graphics;
using SFML.System;

namespace platformer {
    public class Scene {
        
        private readonly List<Entity> entities;
        private string currentScene;
        private string? nextScene;
        public Scene() {
            entities = new List<Entity>();
        }
        public int money = 0;
        public int savedMoney = 0; 
        
        public void Spawn(Entity entity) { //spawnar
            entities.Add(entity);
        }

        public void UpdateAll(float deltaTime)
        {
            HandleSceneChange();
            for (int i = entities.Count - 1; i >= 0; i--) //updatear entitys
            {
                Entity entity = entities[i];
                entity.Update(this, deltaTime);
            }

            for (int i = 0; i < entities.Count;) //clear dead entities
            {
                Entity entity = entities[i];
                if (entity.Dead) entities.RemoveAt(i);
                else i++;
            }
        }

        public void RenderAll(RenderTarget target) //renderar entity för varje entity i 
        {
            foreach (Entity entity in entities)
            {
                entity.Render(target);
            }
        }
        
        public bool TryMove(Entity entity, Vector2f movement) {
            entity.Position += movement;
            bool collided = false;
            
            for (int i = 0; i < entities.Count; i++) {
                Entity other = entities[i];
                if (!other.Solid) continue;
                if (other == entity) continue;
                FloatRect boundsA = entity.Bounds;
                FloatRect boundsB = other.Bounds;
                if (Collision.RectangleRectangle(boundsA, boundsB, out Collision.Hit hit)) //kollar en rectangle rectangle collision
                {
                    entity.Position += hit.Normal * hit.Overlap;
                    i = -1; //dethär kolla om allt 1 gång till
                    collided = true;
                    if (hit.Normal.Y > 0 && other is BreakablePlatform && entity is Player) /////// DETTE ÄR NYTT /////// det är nog bättra att göra det i breakableplatform klasen men jag hitta inget enkelt sätt att göra det på
                    {
                        other.Dead = true;
                        ////avkommentera dätta när/om du gjort Coin bonusen
                        // Coin coin = new Coin();
                        // coin.position = other.Position;
                        // //coin.verticalSpeed = -50f;
                        // Spawn(coin);
                    }
                }
            }
            return collided;
        }
        private void HandleSceneChange() 
        {
            if (nextScene == null) return;
            entities.Clear();
            Spawn(new Background());
            string file = $"assets/{nextScene}.txt";
            Console.WriteLine($"Loading scene '{file}'");
            // Loads scene from text file
            foreach (var line in File.ReadLines(file, System.Text.Encoding.UTF8)) 
            {
                if (nextScene == null) return;
                savedMoney = money;
                entities.Clear();
                Spawn(new Background());
                string file = $"assets/{nextScene}.txt";
                Console.WriteLine($"Loading scene '{file}'");
                // Loads scene from text file
                foreach (var line in File.ReadLines(file, System.Text.Encoding.UTF8)) 
                {
                   string parsed = line.Trim();
                   int commentAt = parsed.IndexOf('#'); //returerar -1 om "#" inte finns
                   if (commentAt >= 0) {
                       parsed = parsed.Substring(0, commentAt);
                       parsed = parsed.Trim();
                   }
                   if (parsed.Length == 0) continue;
                   string[] words = parsed.Split(" ");
                   Entity entity; //frågetecken betyder att det är ok att den är null (onödigt för klasser eftersom de är referenstyper)
                   switch(words[0]) {
                       case "w":
                           entity = new Platform();
                           entity.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                           break;
                       case "b":
                           entity = new BreakablePlatform();
                           entity.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                           break;
                       case "d":
                           entity = new Door();
                           entity.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                           Door d = entity as Door; // jag är polymorphism profs /s HJÄÖLP HÄP
                           d.nextScene = words[3];
                           entity = d;
                           break;
                       case "k":
                           entity = new Key();
                           entity.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                           break;
                       case "h":
                           entity = new Player();
                           entity.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                           break;
                       case "c":
                           entity = new Coin();
                           entity.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                           break;
                       default: //för att jag inte fick null att funka
                           entity = new Platform();
                           entity.Position = new Vector2f(-6767f, 0f);
                           break;
                   }
                   if (!(entity.Position.X == -6767f))
                   {
                       Spawn(entity);
                   }
                       
                } 
                currentScene = nextScene; 
                nextScene = null;


        public bool FindByType<T>(out T found) where T : Entity // säger bara att T måste vara en entity eller att den måste ärva något från entity
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is T match)
                {
                    found = match;
                    return true;
                }
                Entity entity = entities[i];
                if (!entity.Dead && entity is T typed)
                {
                    found = typed;
                    return true;
                }
            }
            found = null;
            return false;
        }
        
        public void Load(String LevelName)
        {
            nextScene = LevelName;
        }

        public void Reload()
        {
            nextScene = currentScene;
        }
    }
}