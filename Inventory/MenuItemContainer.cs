using Godot;
using System;

public class MenuItemContainer : ColorRect
{
    int index = -1;
    TextureRect textureRect;
    public event Action<int> OnMenuItemClicked;
    public override void _Ready()
    {
        textureRect = GetNode<TextureRect>("TextureRect");
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton button && button.Pressed && button.ButtonIndex == 1 && textureRect.Texture != null)
        {
            OnMenuItemClicked?.Invoke(index);
        }
    }

    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetItem(Texture texture)
    {
        textureRect.Texture = texture;
    }

    public void ClearImg()
    {
        textureRect.Texture = null;
    }
}
