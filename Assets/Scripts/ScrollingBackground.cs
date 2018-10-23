using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    public PlayerController playerController;
    private Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;    
    }

    void Update ()
    {
        material.mainTextureOffset += new Vector2(playerController.velocity / 20, 0f) * Time.deltaTime;

        if (playerController.transform.position.x > 35)
        {
            playerController.transform.position = new Vector3(-25, playerController.transform.position.y, playerController.transform.position.z);
        }
        else if (playerController.transform.position.x < -35)
        {
            playerController.transform.position = new Vector3(25, playerController.transform.position.y, playerController.transform.position.z);
        }
    }
}
