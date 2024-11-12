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
    bool canMove;
     

    // Start is called before the first frame update
    void Start()
    {
        s.Add("Up", up);
        s.Add("Right", right);
        s.Add("Left", left);
        s.Add("Down", down);
        s.Add("Locked", locked);

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Door")
        {
            directionSign.sprite = s[collision.gameObject.GetComponent<DoorInfo>().dir];
            directionSign.gameObject.SetActive(true);
        }
    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            directionSign.gameObject.SetActive(false);
        }
        if(canMove == false)
        {
            canMove = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door" && collision.gameObject.GetComponent<DoorInfo>().locked == false)
        {
            directionSign.sprite = s[collision.gameObject.GetComponent<DoorInfo>().dir];
            directionSign.gameObject.SetActive(true);

            if (canMove == true)
            {
                
                if (collision.gameObject.GetComponent<DoorInfo>().dir == "Right")
                {
                    if (transform.position.x > collision.gameObject.transform.position.x + 0.1f)
                    {
                        
                        //StartCoroutine(MoveDelay());
                        transform.position = collision.gameObject.GetComponent<DoorInfo>().to.transform.position;
                        collision.transform.parent.gameObject.SetActive(false);
                        collision.gameObject.GetComponent<DoorInfo>().to.transform.parent.gameObject.SetActive(true);
                        currentRoom = collision.gameObject.GetComponent<DoorInfo>().to.transform.parent.gameObject;
                        canMove = false;
                    }
                }
                else if (collision.gameObject.GetComponent<DoorInfo>().dir == "Left")
                {
                    if (transform.position.x < collision.gameObject.transform.position.x - 0.1f)
                    {
                        
                        //StartCoroutine(MoveDelay());
                        transform.position = collision.gameObject.GetComponent<DoorInfo>().to.transform.position;
                        collision.transform.parent.gameObject.SetActive(false);
                        collision.gameObject.GetComponent<DoorInfo>().to.transform.parent.gameObject.SetActive(true);
                        currentRoom = collision.gameObject.GetComponent<DoorInfo>().to.transform.parent.gameObject;
                        canMove = false;
                    }
                }
                else if (collision.gameObject.GetComponent<DoorInfo>().dir == "Up")
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                    {
                        
                        //StartCoroutine(MoveDelay());
                        transform.position = collision.gameObject.GetComponent<DoorInfo>().to.transform.position;
                        collision.transform.parent.gameObject.SetActive(false);
                        collision.gameObject.GetComponent<DoorInfo>().to.transform.parent.gameObject.SetActive(true);
                        currentRoom = collision.gameObject.GetComponent<DoorInfo>().to.transform.parent.gameObject;
                        canMove = false;
                    }
                }
                else if (collision.gameObject.GetComponent<DoorInfo>().dir == "Down")
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                    {
                        
                        //StartCoroutine(MoveDelay());
                        transform.position = collision.gameObject.GetComponent<DoorInfo>().to.transform.position;
                        collision.transform.parent.gameObject.SetActive(false);
                        collision.gameObject.GetComponent<DoorInfo>().to.transform.parent.gameObject.SetActive(true);
                        currentRoom = collision.gameObject.GetComponent<DoorInfo>().to.transform.parent.gameObject;
                        canMove = false;
                    }
                }
            }
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
        canMove = true;
    }
}
