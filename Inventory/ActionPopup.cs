using System;
using Godot;

public class ActionPopup : CanvasLayer
{
    int index = -1;
    Label nameLabel;
    Button useButton;
    Button tossButton;
    public event Action<int> OnItemUsed;
    public event Action<int> OnItemTossed;
    public override void _Ready()
    {
        nameLabel = GetNode<Label>("PanelContainer/VBoxContainer/Label");

        useButton = GetNode<Button>("PanelContainer/VBoxContainer/Use");
        useButton.Connect("pressed", this, nameof(OnUseButtonPressed));

        tossButton = GetNode<Button>("PanelContainer/VBoxContainer/Toss");
        tossButton.Connect("pressed", this, nameof(OnTossButtonPressed));

        Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            Visible = false;
        }
    }

    public void SetPopupItem(int i, string itemName)
    {
        index = i;
        nameLabel.Text = itemName;
    }

    void OnUseButtonPressed()
    {
        OnItemUsed?.Invoke(index);
        Visible = false;
    }

    void OnTossButtonPressed()
    {
        OnItemTossed?.Invoke(index);
        Visible = false;
    }
}
