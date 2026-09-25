using System.Data.SqlTypes;
using SFML.Graphics;
using SFML.System;
namespace platformer;

public class Platform : Entity
{
    public override bool Solid => true; //gör så att andra entities som player colliederar med platforms
    public Platform() // constructorn till platforms
    {
        sprite.TextureRect = new IntRect(0, 0, 18, 18); 
        sprite.Origin = new Vector2f(9, 9);
        sprite.Texture = textures["tileset"];
    }
}

public class BreakablePlatform : Platform // breakable platforms classen som är child till platform
{
    public BreakablePlatform() : base()
    {
        sprite.TextureRect = new IntRect(0, 36, 18, 18);
    }
}


public class Background : Entity
{
    public Background() //constructorn till bakgrunden
    {
        sprite.TextureRect = new IntRect(0, 0, 24, 24);
        sprite.Origin = new Vector2f(12, 12);
        sprite.Texture = textures["background"];
    }
    public override void Render(RenderTarget target) 
    {
        View view = target.GetView();
        Vector2f topLeft = view.Center - 0.5f * view.Size;
        int tilesX = (int)MathF.Ceiling(view.Size.X / 24); // kollar hela x av vyn och delar upp den i 24 delar för att tilea backgrunden, vise värsa till y under.
        int tilesY = (int)MathF.Ceiling(view.Size.Y / 24);
        for (int row = 0; row <= tilesY; row++)
        {
            for (int col = 0; col <= tilesX; col++)
            {
                if (row < 5) sprite.TextureRect = new IntRect(0, 0, 24, 24); // himmlen 
                else if (row == 5) sprite.TextureRect = new IntRect(24, 0, 24, 24); // mellanraden
                else if (row > 5) sprite.TextureRect = new IntRect(48, 0, 24, 24); // moln
                sprite.Origin = new Vector2f();
                sprite.Position = topLeft + 24 * new Vector2f(col, row);
                target.Draw(sprite);
            }
        }
    }
} 

public class Coin : Entity // coins class
{
    public Coin() // coinens constructor
    {
        sprite.TextureRect = new IntRect(0, 0, 20, 20);
        sprite.Origin = new Vector2f(10, 10);
        sprite.Texture = textures["Coins"];
    }
    public override void Update(Scene scene, float deltatime) // coinens update kollar om spelaren colliderar med boundsen av coin så går money up och coinen försvinner.
    {
        if (scene.FindByType(out Player player))
        {
            FloatRect boundsA = this.Bounds;
            FloatRect boundsB = player.Bounds;
            if (Collision.RectangleRectangle(boundsA, boundsB, out _))
            {
                scene.money += 1;
                Console.WriteLine($"{scene.money}");
                Dead = true;
            }
        }
    }
}
public class Door : Entity 
{
    public string nextScene = "level0"; //bara default värde
    private bool _unlocked = false;
    public bool unlocked // property med en custom setter, varje gång unlocked säts så körs setter logiken och sätter färgen av dörren till grå.
    {
        get;
        set
        {
            field = value;
            if (value) sprite.Color = new Color(70, 70, 70);
            else sprite.Color = Color.White;
        }
    } = false;
    public Door() //constructorn till dörren
    {
        unlocked = false;
        sprite.TextureRect = new IntRect(180, 103, 18, 23);
        sprite.Origin = new Vector2f(9, 14); //eller 14 eller 23/2 sprite är 18,23 tror jag
        sprite.Texture = textures["tileset"];
    }
    public override void Update(Scene scene, float deltaTime) {
        if (unlocked) // för varje frame som dörren är uplåst så kommer dörrens bounds kolla om dem overlapar med spelarens för att byta till nästa scene.
        {
            scene.FindByType(out Player player);
            FloatRect boundsA = this.Bounds;
            FloatRect boundsB = player.Bounds;
            if (Collision.RectangleRectangle(boundsA, boundsB, out _)) // understreck värkar va nån "jag bryr mig inte om den här variabeln, den används när fler värden ska kunna returnas
            {
                Console.WriteLine($"money :{scene.savedMoney}");
                scene.Load(nextScene);
                scene.savedMoney = scene.money;
            }
        }
    }
}
public class Key : Entity
{
    public Key() // key constructorn
    {
        sprite.TextureRect = new IntRect(126, 18, 18, 18);
        sprite.Origin = new Vector2f(9, 9); // mitten av 18, 18
        sprite.Texture = textures["tileset"];
    }
    public override void Update(Scene scene, float deltaTime)
    {
        scene.FindByType(out Player player);
        FloatRect boundsA = this.Bounds;
        FloatRect boundsB = player.Bounds;
        if (Collision.RectangleRectangle(boundsA, boundsB, out _)) // understreck värkar va nån "jag bryr mig inte om den här variabeln, skit i den"-grej, den används när fler värden ska kunna returnas
        {
            if (scene.FindByType(out Door door)) // if ifall dörr inte finns i scenen
            {
                door.unlocked = true;
            }
            Dead = true;
        }
    }
}

