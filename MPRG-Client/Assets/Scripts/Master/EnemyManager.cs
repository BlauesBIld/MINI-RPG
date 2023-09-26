using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    
    public int id;
    public float health;
    public float maxHealth = 100f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("EnemyManager Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    
    public void Initialize(int _id)
    {
        id = _id;
        health = maxHealth;
    }

    public void SetHealth(float _health)
    {
        health = _health;

        if (health <= 0f)
        {
            GameManager.enemies.Remove(id);
            Destroy(gameObject);
        }
    }
}
