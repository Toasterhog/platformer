using SFML.Graphics;
using SFML.System;
using SFML.Window;
namespace platformer;

public class Player : Entity
{
    private bool faceRight = false;
    
    public Player()
    {
        sprite.TextureRect = new IntRect(0, 0, 24, 24);
        sprite.Origin = new Vector2f(12, 12);
        sprite.Texture = textures["characters"];
    }
    
    
    public override void Update(Scene scene, float deltaTime) {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) {
            Position -= new Vector2f(100 * deltaTime, 0);
            faceRight = false;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            Position += new Vector2f(100 * deltaTime, 0);
            faceRight = true;
        }
        
    }

    public override void Render(RenderTarget target)
    {
        sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1);
        base.Render(target);
    }
}