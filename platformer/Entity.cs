using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Entity {
    private readonly string textureName;
    protected readonly Sprite sprite;
    public bool Dead;
    public virtual bool Solid => false;
    protected Entity(string textureName) {
        this.textureName = textureName;
        sprite = new Sprite();
    }

    public Vector2f Position {
        get => sprite.Position;
        set => sprite.Position = value;
    }
    public virtual FloatRect Bounds =>
        sprite.GetGlobalBounds();
    
    public virtual void Create(Scene scene) {
        sprite.Texture = scene.LoadTexture(textureName);
    }
    
    public virtual void Render(RenderTarget target) {
        target.Draw(sprite);
    }
    public virtual void Update(Scene scene, float deltaTime){}
    
}