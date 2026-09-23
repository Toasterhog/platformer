using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace platformer {
    public class Scene {
        
        private readonly List<Entity> entities;
        
        public Scene() {
            entities = new List<Entity>();
        }
        
        public void Spawn(Entity entity) { //spawnar
            entities.Add(entity);
        }

        public void UpdateAll(float deltaTime)
        {
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
        
    }
    
}