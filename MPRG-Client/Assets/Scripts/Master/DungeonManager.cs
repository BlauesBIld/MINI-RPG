using UnityEngine;

public enum Dungeons
{
    HOMEWORLD = 0,
    ELKANAH,
    
}

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("DungeonManager Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void LoadDungeon(Dungeons _dungeon, int _clientId)
    {
        WorldManager.instance.SwitchToWorld(_dungeon.ToString());
        GameManager.players[_clientId].transform.rotation = Quaternion.Euler( 0f, WorldManager.instance.spawnRotationY, 0f);
    }
}
