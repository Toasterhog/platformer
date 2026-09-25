using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Entity {
    private readonly string textureName;
    protected readonly Sprite sprite;
    public bool Dead;
    public virtual bool Solid => false;
    public static Dictionary<string, Texture> textures = new(); 
    
    protected Entity() { // här skapas entity:n med bara en sprite
        sprite = new Sprite();
    }

    public Vector2f Position {
        get => sprite.Position;
        set => sprite.Position = value;
    }
    public virtual FloatRect Bounds =>
        sprite.GetGlobalBounds();
    
    public static void InitEntitytextures() 
    {
        if (textures.Count > 0)
        {
            textures.Clear();
        }
        foreach (string textureName in new string[] {"tileset", "background","characters","Coins"})
        {
            string fileName = $"assets/{textureName}.png";
            Texture texture = new Texture(fileName);
            textures.Add(textureName, texture);
            Console.WriteLine($"texture name : {textureName} loaded");
            
        }
    }
    
    public virtual void Render(RenderTarget target) {
        target.Draw(sprite);
    }
    public virtual void Update(Scene scene, float deltaTime){}
}