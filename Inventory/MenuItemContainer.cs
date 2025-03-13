using Godot;
using System;

public class MenuItemContainer : ColorRect
{
    TextureRect textureRect;
    public override void _Ready()
    {
        textureRect = GetNode<TextureRect>("TextureRect");
    }

    public void SetImg(Texture texture)
    {
        textureRect.Texture = texture;
    }

    public void ClearImg()
    {
        textureRect.Texture = null;
    }
}
