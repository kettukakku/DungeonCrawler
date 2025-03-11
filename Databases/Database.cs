using Godot;
using System;

public class Database : Node
{
    public static Database Instance;

    public ItemDatabase Items = new ItemDatabase();
    // enemy database
    // 

    public override void _Ready()
    {
        Instance = this;
        LoadItemDB();
    }

    void LoadItemDB()
    {
        File file = new File();
        Error err = file.Open("res://Databases/ItemDB.json", File.ModeFlags.Read);

        if (err == Error.Ok)
        {
            string content = file.GetAsText();
            Database.Instance.Items.LoadFromJson(content);
            file.Close();
        }
        else
        {
            GD.PrintErr("Error reading file: ", err);
        }
    }
}
