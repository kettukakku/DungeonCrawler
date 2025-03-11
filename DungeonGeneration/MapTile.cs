using Godot;
using System.Collections.Generic;

public class MapTile : TextureRect
{
    Label positionLabel;
    RoomData linkedRoom;

    public override void _Ready()
    {
        positionLabel = GetNode<Label>("VBoxContainer/Position");
        Modulate = new Color(0, 0, 0, 0);
    }

    public void SetLinkedRoom(RoomData room)
    {
        linkedRoom = room;
        positionLabel.Text = $"{linkedRoom.position.x}, {linkedRoom.position.y}";
        linkedRoom.OnExitsChanged += SetExits;
        linkedRoom.OnEnter += Enter;
        linkedRoom.OnExit += Exit;
    }

    void SetExits(Direction directions)
    {
        Texture = GD.Load<Texture>(TileMap[directions]);
    }

    void Enter()
    {
        Modulate = new Color("15b59f");
    }

    public void SetColor(Color color)
    {
        Modulate = color;
    }

    void Exit()
    {
        Modulate = new Color("ffffff");
    }

    public override void _ExitTree()
    {
        if (linkedRoom != null)
        {
            linkedRoom.OnExitsChanged -= SetExits;
            linkedRoom.OnEnter -= Enter;
            linkedRoom.OnExit -= Exit;
        }
    }

    readonly Dictionary<Direction, string> TileMap = new Dictionary<Direction, string>()
    {
        {Direction.North, "res://PlaceholderArt/maptiles/N.png"},
        {Direction.South, "res://PlaceholderArt/maptiles/S.png"},
        {Direction.East, "res://PlaceholderArt/maptiles/E.png"},
        {Direction.West, "res://PlaceholderArt/maptiles/W.png"},

        {Direction.North | Direction.South, "res://PlaceholderArt/maptiles/NS.png"},
        {Direction.North | Direction.East, "res://PlaceholderArt/maptiles/NE.png"},
        {Direction.North | Direction.West, "res://PlaceholderArt/maptiles/NW.png"},
        {Direction.South | Direction.East, "res://PlaceholderArt/maptiles/SE.png"},
        {Direction.South | Direction.West, "res://PlaceholderArt/maptiles/SW.png"},
        {Direction.East | Direction.West, "res://PlaceholderArt/maptiles/EW.png"},

        {Direction.North | Direction.South | Direction.East, "res://PlaceholderArt/maptiles/NSE.png"},
        {Direction.North | Direction.South | Direction.West, "res://PlaceholderArt/maptiles/NSW.png"},
        {Direction.North | Direction.East | Direction.West, "res://PlaceholderArt/maptiles/NEW.png"},
        {Direction.South | Direction.East | Direction.West, "res://PlaceholderArt/maptiles/SEW.png"},

        {Direction.North | Direction.South | Direction.East | Direction.West, "res://PlaceholderArt/maptiles/All.png"}
    };
}
