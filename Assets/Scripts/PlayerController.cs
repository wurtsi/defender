using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool playerLeft;
    private float yVelocity = 0f;
    public float velocity = 0f;
    public float acceleration = 0.3f;
    public float maxVelocity = 0.25f;
    public float verticalSpeed = 0.4f;
    private float targetYPos = 0f;

    public float laserCooldown = 1f;
    private float currentLaserCooldown = 0f;
    public GameObject laserPrefab;

    private Material material;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public void Accelerate(Vector3 touchPos)
    {
        if (Camera.main.WorldToScreenPoint(touchPos).y < Screen.height / 2f)
        {
            Vector3 targetPos = Camera.main.WorldToScreenPoint(touchPos) * 2f;
            targetYPos = Camera.main.ScreenToWorldPoint(targetPos).y;
        }

        if (Camera.main.WorldToScreenPoint(touchPos).x < Screen.width / 5)
        {
            playerLeft = true;
            material.mainTextureScale = new Vector2(1, 1);
            velocity += acceleration;

            if (velocity > maxVelocity)
                velocity = maxVelocity;
        }
        else if (Camera.main.WorldToScreenPoint(touchPos).x > Screen.width - (Screen.width / 5))
        {
            playerLeft = false;
            material.mainTextureScale = new Vector2(-1, 1);
            velocity -= acceleration;

            if (velocity < -maxVelocity)
                velocity = -maxVelocity;
        }

        Move();
    }

    public void NoInput()
    {
        if (velocity > 0.3f)
            velocity -= acceleration / 3;
        else if (velocity < -0.03f)
            velocity += acceleration / 3;
        else
            velocity = 0f;

        Move();
    }

    private void Move()
    {
        transform.position = new Vector3(transform.position.x + (velocity * Time.deltaTime), Mathf.SmoothDamp(transform.position.y, targetYPos, ref yVelocity, verticalSpeed * Time.deltaTime, 15f), 0f);
    }

    public void Shoot()
    {
        if (currentLaserCooldown > 0)
            return;

        Vector3 offset;

        if (playerLeft)
            offset = new Vector3(2f, 0f, 0f);
        else
            offset = new Vector3(-2f, 0f, 0f);

        currentLaserCooldown = laserCooldown;
        GameObject laser = Instantiate(laserPrefab, transform.position + offset, Quaternion.identity);
        laser.GetComponent<LaserScript>().direction = offset / 4;
    }

    private void Update()
    {
        if (currentLaserCooldown > 0)
            currentLaserCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            GameManager.instance.TakeLife();
            ScreenShake.instance.StartShake(0.2f, 0.2f);
            Destroy(other.gameObject);
        }
    }
}
