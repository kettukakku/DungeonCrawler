using Godot;

public class FloorNumberText : Control
{
    Label label;

    public override void _Ready()
    {
        label = GetNode<Label>("MarginContainer/Label");
    }

    public void SetText(int floorNum)
    {
        label.Text = $"Floor {floorNum}";
    }
}
