public class Item
{
    public string name;
    public int amount;
    public string prefabPath;

    public Item() : this("woodenRod", 1, "/Weapons/WoodenRod")
    {
        
    }

    public Item(string name, int amount, string prefabPath)
    {
        this.name = name;
        this.amount = amount;
        this.prefabPath = prefabPath;
    }
}
