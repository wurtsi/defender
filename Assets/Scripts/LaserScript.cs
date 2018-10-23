using UnityEngine;

public class LaserScript : MonoBehaviour {

    public float timeToDestroy = 1f;
    private float spawnTime;
    public Vector3 direction;

    private void Awake()
    {
        spawnTime = Time.time;    
    }

    void Update ()
    { 
        if(Time.time > spawnTime + timeToDestroy)
        {
            Destroy(gameObject);
        }

        transform.position += direction;
    }
}
