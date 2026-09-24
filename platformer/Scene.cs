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
            // TODO: Load scene from file
            foreach (var line in File.ReadLines(file, System.Text.Encoding.UTF8)) {
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
                    case "d":
                        entity = new Door();
                        entity.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                        Door d = entity as Door; // jag är polymorphism profs /s
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
                    default: //för att jag inte fick null att funka
                        entity = new Platform();
                        entity.Position = new Vector2f(-6767f, 0f);
                        break;
                }
                //om första ordet inte var w, d, k eller h -> entity = null
                // if (entity != null) //fattar ej varför denna rad ger error
                // {
                //     Spawn(entity);
                // }
                if (!(entity.Position.X == -6767f))
                {
                    Spawn(entity);
                }
                    
            } 
            currentScene = nextScene;
            nextScene = null;
        }

        public void Load(String LevelName)
        {
            nextScene = LevelName;
        }

        public void Reload()
        {
            nextScene = currentScene;
        }

        public bool FindEntityByType<T>(out T found) where T : Entity
        {
            foreach (Entity entity in entities)
            {
                if (!entity.Dead && entity is T typed) {
                    found = typed;
                    return true;
                }
            }
            found = default(T);
            return false;
        }
    }
    
}