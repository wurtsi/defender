using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public float xOffset = 6;
    public float cameraSpeed = 0.3f;
    private float velocity = 0f;

    void Update()
    {
        if (player.playerLeft)
        {
            transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, player.transform.position.x + xOffset, ref velocity, cameraSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, player.transform.position.x - xOffset, ref velocity, cameraSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }
    }
}
