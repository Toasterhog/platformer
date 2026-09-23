using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Entity {
    private readonly string textureName;
    protected readonly Sprite sprite;
    public bool Dead;
    public virtual bool Solid => false;
    public static Dictionary<string, Texture> textures = new(); // bästa fix

    public static void InnitEntitytextures() 
    {
        foreach (string textureName in new string[] {"tileset", "background","characters"})
        {
            if (textures.TryGetValue(textureName, out Texture found))
            {
                Console.WriteLine("tried loading " + textureName + "twice");
                continue;
            }
            string fileName = $"assets/{textureName}.png";
            Texture texture = new Texture(fileName);
            textures.Add(textureName, texture);
            Console.WriteLine($"texture name : {textureName} loaded");
            
        }
    }
    
    protected Entity() {
        sprite = new Sprite();
    }

    public Vector2f Position {
        get => sprite.Position;
        set => sprite.Position = value;
    }
    public virtual FloatRect Bounds =>
        sprite.GetGlobalBounds();
    
    public virtual void Render(RenderTarget target) {
        target.Draw(sprite);
    }
    public virtual void Update(Scene scene, float deltaTime){}
}