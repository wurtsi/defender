using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxVelocity;
    public float acceleration;
    public float verticalSpeed;
    public Vector2 randomJumpRange = new Vector2();

    private float targetYPos = 0f;
    private float yVelocity = 0f;
    private float velocity = 0f;

    private float currentLaserCooldown = 1f;
    public Vector2 laserCooldown = new Vector2();
    public GameObject laserPrefab;

    public Vector2 maxJumpCooldown = new Vector2();
    private float jumpCooldown = 0f;


    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, new Vector3(0f, 0f, -1f), out hit, Mathf.Infinity))
        {
            if(transform.parent != hit.collider.transform)
            {
                transform.SetParent(hit.collider.transform);
            }
        }

        if (jumpCooldown > 0)
        {
            jumpCooldown -= Time.deltaTime;
        }
        else
        {
            targetYPos = Random.Range(randomJumpRange.x, randomJumpRange.y);
            jumpCooldown = Random.Range(maxJumpCooldown.x, maxJumpCooldown.y);
        }

        Move();

        if (Mathf.Abs(GameManager.instance.playerPos.x - transform.position.x) > 10)
            return;

        Shoot();

        if (GameManager.instance.playerPos.x > transform.position.x)
        {
            velocity += acceleration;

            if (velocity > maxVelocity)
                velocity = maxVelocity;
        }
        else if (GameManager.instance.playerPos.x < transform.position.x)
        {
            velocity -= acceleration;

            if (velocity < -maxVelocity)
                velocity = -maxVelocity;
        }
    }

    private void Shoot()
    {
        if (currentLaserCooldown > 0)
        {
            currentLaserCooldown -= Time.deltaTime;
            return;
        }

        Vector3 offset = (GameManager.instance.playerPos - transform.position).normalized / 2;
        GameObject bullet = Instantiate(laserPrefab, transform.position + offset, Quaternion.identity);
        bullet.GetComponent<BulletScript>().direction = offset / 10;

        currentLaserCooldown = Random.Range(laserCooldown.x, laserCooldown.y);
    }

    private void Move()
    {
        transform.position = new Vector3(transform.position.x + (velocity * Time.deltaTime), Mathf.SmoothDamp(transform.position.y, targetYPos, ref yVelocity, verticalSpeed * Time.deltaTime, 15f), 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser")
        {
            GameManager.instance.enemies.Remove(this);
            GameManager.instance.AddScore(100);
            Destroy(gameObject);
        }
        else if (other.tag == "Player")
        {
            GameManager.instance.GameOver();
        }
    }
}
