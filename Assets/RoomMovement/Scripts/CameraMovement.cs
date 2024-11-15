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

        float offset = curRoom.transform.position.x;
        float y = curRoom.transform.position.y;
        roomsize = curRoom.transform.Find("Ground").localScale.x;
        if (((transform.position.x - camsize > offset-roomsize && transform.position.x < offset) || (transform.position.x + camsize < offset+roomsize && transform.position.x > offset))&& curRoom.transform.Find("Ground").transform.localScale.x>camsize+2)
        {
            cam.transform.position = new Vector3(transform.position.x, y, -10);
        }
        else if (curRoom.transform.Find("Ground").transform.localScale.x <= 8)
        {
            cam.transform.position = new Vector3(offset, y, -10);
        }
        else if(transform.position.x < offset)
        {
            cam.transform.position = new Vector3(camsize-roomsize+offset, y, -10);
        }
        else if (transform.position.x > offset)
        {
            cam.transform.position = new Vector3(roomsize- camsize+offset, y, -10);
        }
    }
}
