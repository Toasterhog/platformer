using SFML.Graphics;
using SFML.System;
using SFML.Window;
namespace platformer;

public class Player : Entity
{
    private bool faceRight = false;
    public const float WalkSpeed = 100.0f;
    public const float JumpForce = 250.0f;
    public const float GravityForce = 380.0f;
    
    private float verticalSpeed = 100.0f;
    private bool isGrounded = false;
    private bool isUpPressed = false;
    public override FloatRect Bounds {
        get {
            var bounds = base.Bounds;
            bounds.Left += 3;
            bounds.Width -= 6;
            bounds.Top += 3;
            bounds.Height -= 3;
            return bounds;
        }
    }
    
    public Player()
    {
        sprite.TextureRect = new IntRect(0, 0, 24, 24);
        sprite.Origin = new Vector2f(12, 12);
        sprite.Texture = textures["characters"];
    }
    
    public override void Update(Scene scene, float deltaTime)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            scene.TryMove(this, new Vector2f(-100*deltaTime, 0)); // så att det matchar det nya trymove functionen i scene
            faceRight = false;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            scene.TryMove(this, new Vector2f(100 * deltaTime, 0));
            faceRight = true;
        }
      
        verticalSpeed += GravityForce * deltaTime;
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) {
            if (isGrounded && !isUpPressed) {
                verticalSpeed = -JumpForce;
                isUpPressed = true;
            }
            else {
                isUpPressed = false;
            }
        }
            
        Vector2f velocity = new Vector2f(0, verticalSpeed * deltaTime);
        if (scene.TryMove(this, velocity)) { 
            if (verticalSpeed > 0.0f) {
                isGrounded = true;
            }
            verticalSpeed = 0.0f;
            
        }
        else {
            isGrounded = false;
        }

        
        if (Position.Y > 300f)
        {
            scene.Reload();
        }
    }

    public override void Render(RenderTarget target)
    {
        sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1);
        base.Render(target);
    }
}