using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float timeToDestroy = 3f;
    private float spawnTime;
    public Vector3 direction;

    private void Awake()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time > spawnTime + timeToDestroy)
        {
            Destroy(gameObject);
        }

        transform.position += direction;
    }
}
