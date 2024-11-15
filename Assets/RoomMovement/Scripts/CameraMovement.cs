using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject cam;
    public float camsize;
    [SerializeField] float roomsize;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject curRoom = gameObject.GetComponent<RoomMovement>().currentRoom;
        roomsize = curRoom.transform.Find("Ground").localScale.x;
        if (((transform.position.x - camsize > -roomsize && transform.position.x < 0) || (transform.position.x + camsize < roomsize && transform.position.x > 0))&& curRoom.transform.Find("Ground").transform.localScale.x>8)
        {
            cam.transform.position = new Vector3(transform.position.x, 0, -10);
        }
        else if (curRoom.transform.Find("Ground").transform.localScale.x <= 8)
        {
            cam.transform.position = new Vector3(0, 0, -10);
        }
        else if(transform.position.x < 0)
        {
            cam.transform.position = new Vector3(camsize-roomsize, 0, -10);
        }
        else if (transform.position.x > 0)
        {
            cam.transform.position = new Vector3(roomsize- camsize, 0, -10);
        }
    }
}
