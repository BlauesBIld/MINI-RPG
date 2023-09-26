using UnityEngine;

public class DroppedItemController : MonoBehaviour
{
    public string name;

    public GameObject floatingMesh;

    private MeshFilter floatingMeshFilter;
    private MeshRenderer floatingMeshRenderer;

    private PlayerManager pickingUpPlayer = null;

    private void Awake()
    {
        floatingMeshFilter = floatingMesh.GetComponent<MeshFilter>();
        floatingMeshRenderer = floatingMesh.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        transform.localScale *= 0.7f;
    }

    private void Update()
    {
        transform.Rotate(0f, 70f * Time.deltaTime, 0f);

        if (pickingUpPlayer != null)
        {
            FloatToPlayer();
        }
    }

    private void FloatToPlayer()
    {
        transform.Translate(GetNormalizedVectorToPlayer() * Time.deltaTime * 5f, Space.World);
    }

    private Vector3 GetNormalizedVectorToPlayer()
    {
        return (pickingUpPlayer.transform.position - transform.position).normalized;
    }

    public void SetMeshForFloatingItem(Material[] _sharedMaterial, Mesh _sharedMesh)
    {
        floatingMeshFilter.mesh = _sharedMesh;
        floatingMeshRenderer.materials = _sharedMaterial;
    }

    public void SetPlayerWhoIsPickingUp(int _playerId)
    {
        pickingUpPlayer = GameManager.players[_playerId];
    }
}