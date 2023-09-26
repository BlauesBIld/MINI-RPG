using UnityEngine;
using Random = UnityEngine.Random;

public class PortalStoneForElkanahController : MonoBehaviour
{
    public int id;

    public float alpha;
    public float radius;
    
    private Vector3 center = Vector3.zero;


    void Update()
    {
        MoveAroundInOvalShape();
        RotateRandomly();
    }

    private void RotateRandomly()
    {
        transform.rotation *= Quaternion.Euler(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
    }

    private void MoveAroundInOvalShape()
    {
        var alphaT = SetStartingPoint();
        transform.position = new Vector3(
            0f , 
            0f + (radius * MCos(alphaT) * MSin(45f)) + (radius/2 * MSin(alphaT) * MCos(45f)),
            0f + (radius * MCos(alphaT) * MCos(75f)) - (radius/2 * MSin(alphaT) * MSin(75f))) + center;
        alpha += 0.4f;
        if (alpha > 360)
        {
            alpha = 0f;
        }
    }

    private float SetStartingPoint()
    {
        float alphaT;
        if (radius >= 4f)
        {
            alphaT = alpha + id * 36f;
        }
        else
        {
            alphaT = alpha + (id-10) * 60f;
        }

        return alphaT;
    }

    float MCos(float value)
    {
        return Mathf.Cos(Mathf.Deg2Rad * value);
    }

    float MSin(float value)
    {
        return Mathf.Sin(Mathf.Deg2Rad * value);
    }

    public void SetCenter(Vector3 _centerPosition)
    {
        center = _centerPosition;
    }
}