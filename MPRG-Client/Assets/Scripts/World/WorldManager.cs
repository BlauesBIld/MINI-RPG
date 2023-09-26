using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;

    public Light worldLight;

    public string currentWorld = "HomeWorld";
    public float spawnRotationY = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("GameManager Instance already exists, destroying object!");
            Destroy(this);
        }

        SetLayerConfigurations();
    }

    void Start()
    {
        SceneManager.LoadScene("HomeWorld", LoadSceneMode.Additive);
    }

    private void SetLayerConfigurations()
    {
        Physics.IgnoreLayerCollision(6, 6);
        Physics.IgnoreLayerCollision(6, 0);
    }

    private void LoadWorld(string _worldName)
    {
        SceneManager.LoadScene(_worldName, LoadSceneMode.Additive);
    }

    private void UnloadWorld(string _worldName)
    {
        SceneManager.UnloadSceneAsync(_worldName);
    }

    public void SwitchToWorld(string _worldName)
    {
        UnloadWorld(currentWorld);
        LoadWorld(_worldName);
        currentWorld = _worldName;
        ApplyWorldSettings();
    }

    private void ApplyWorldSettings()
    {
        WorldSettingsObj _worldSettings =
            JsonConvert.DeserializeObject<WorldSettingsObj>(Resources
                .Load<TextAsset>("WorldSettings/" + currentWorld)
                .text);
        SetWorldSettings(_worldSettings);
    }

    private void SetWorldSettings(WorldSettingsObj _worldSettings)
    {
        worldLight.intensity = _worldSettings.LightIntensity;
        switch (_worldSettings.LightShadowType)
        {
            case "NONE":
                worldLight.shadows = LightShadows.None;
                break;
            case "HARD":
                worldLight.shadows = LightShadows.Hard;
                break;
            default:
                worldLight.shadows = LightShadows.Soft;
                break;
        }

        spawnRotationY = _worldSettings.SpawnRotationY;
    }
}