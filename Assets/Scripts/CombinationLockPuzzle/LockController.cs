using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour
{
    public int currentSlot;

    [SerializeField] GameObject[] slots;
    public int[] slotValues;
    [SerializeField] GameObject highlight;

    bool changing = false;


    // Start is called before the first frame update
    void Start()
    {
        slotValues = new int[slots.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && changing == false)
        {
            currentSlot -= 1;
            if (currentSlot < 0) { currentSlot = 5; }
            highlight.transform.position = slots[currentSlot].transform.position;
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && changing == false)
        {
            currentSlot += 1;
            if (currentSlot > 5) { currentSlot = 0; }
            highlight.transform.position = slots[currentSlot].transform.position;
        }

        if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && changing == false)
        {
            StartCoroutine(changeNumberDown(currentSlot));
            slotValues[currentSlot] += 1;
            if (slotValues[currentSlot] > 9) { slotValues[currentSlot] = 0; }
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && changing == false)
        {
            StartCoroutine(changeNumberUp(currentSlot));
            slotValues[currentSlot] -= 1;
            if (slotValues[currentSlot] < 0) { slotValues[currentSlot] = 9; }
        }
        if (Input.GetKeyDown(KeyCode.Q) && changing == false)
        {
            gameObject.SetActive(false);
        }
    }

    public int currentCode()
    {
        return slotValues[0] * 100000 + slotValues[1] * 10000 + slotValues[2] * 1000 + slotValues[3] * 100 + slotValues[4] * 10 + slotValues[5];
    }

    IEnumerator changeNumberUp(int slot)
    {
        changing = true;
        if (slots[currentSlot].transform.Find("Numbers").transform.localPosition.y <= 1.95)
        {
            slots[currentSlot].transform.Find("Numbers").transform.localPosition = new Vector3(0, 7.1f, 0);
        }

        for (int i = 0; i < 10; i++)
        {
            slots[currentSlot].transform.Find("Numbers").transform.localPosition -= new Vector3(0, 0.0525f, 0);
            yield return new WaitForSeconds(0.05f);
        }
        changing = false;
    }

    IEnumerator changeNumberDown(int slot)
    {
        changing = true;
        if (slots[currentSlot].transform.Find("Numbers").transform.localPosition.y >= 7.08)
        {
            slots[currentSlot].transform.Find("Numbers").transform.localPosition = new Vector3(0, 1.85f, 0);
        }

        for (int i = 0; i < 10; i++)
        {
            slots[currentSlot].transform.Find("Numbers").transform.localPosition += new Vector3(0, 0.0525f, 0);
            yield return new WaitForSeconds(0.05f);
        }
        changing = false;
    }

    private void OnDisable()
    {
        PlayerMove.instance.canMove = true;      
    }
    private void OnEnable()
    {
        PlayerMove.instance.canMove = false;
    }
}
