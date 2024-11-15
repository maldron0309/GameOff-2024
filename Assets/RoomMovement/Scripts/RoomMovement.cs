using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMovement : MonoBehaviour
{
    [SerializeField] SpriteRenderer directionSign;
    [SerializeField] Sprite up;
    [SerializeField] Sprite down;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;
    [SerializeField] Sprite locked;

    Dictionary<string, Sprite> s = new Dictionary<string, Sprite>();
    public GameObject currentRoom;
    public GameObject collidingDoor;
     

    // Start is called before the first frame update
    void Start()
    {
        s.Add("Up", up);
        s.Add("Right", right);
        s.Add("Left", left);
        s.Add("Down", down);
        s.Add("Locked", locked);

    }

    // Update is called once per frame
    void Update()
    {
        if(collidingDoor != null && collidingDoor.GetComponent<DoorInfo>().locked == false)
        {
            if ((collidingDoor.GetComponent<DoorInfo>().dir == "Up" && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
                || (collidingDoor.GetComponent<DoorInfo>().dir == "Down" && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
                || (collidingDoor.GetComponent<DoorInfo>().dir == "Right" && transform.position.x > collidingDoor.transform.position.x + 0.1f)
                || (collidingDoor.GetComponent<DoorInfo>().dir == "Left" && transform.position.x < collidingDoor.transform.position.x - 0.1f)
                )
            {
                //StartCoroutine(MoveDelay());
                transform.position = collidingDoor.GetComponent<DoorInfo>().to.transform.position;
                collidingDoor.GetComponent<DoorInfo>().to.transform.parent.gameObject.SetActive(true);
                currentRoom = collidingDoor.GetComponent<DoorInfo>().to.transform.parent.gameObject;
                //collidingDoor.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            collidingDoor = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            directionSign.gameObject.SetActive(false);
            if (collidingDoor == collision.gameObject)
                collidingDoor = null;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door" && collision.gameObject.GetComponent<DoorInfo>().locked == false)
        {
            directionSign.sprite = s[collision.gameObject.GetComponent<DoorInfo>().dir];
            directionSign.gameObject.SetActive(true);  
        }
        else
        {
            directionSign.sprite = locked;
            directionSign.gameObject.SetActive(true);
        }
    }

    IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(1f);
    }
}
