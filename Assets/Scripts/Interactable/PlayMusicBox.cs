using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicBox : MonoBehaviour, IInteractable
{
    public DialogGraph dialog;
    public AudioClip music;
    public BaseItem pictureItem;
    public BaseItem musicBoxItem;
    public BaseItem leverItem;
    public GameObject MotherOldLocation;
    public GameObject MotherNextLoaction;
    public GameObject MirrorInteraction;
    public GameObject musicBoxObj;

    bool playing = false;
    public void Interact()
    {
        if(playing == false)
        {
            StartCoroutine(PlayBox());
            playing = true;
        }
        
    }
    public IEnumerator PlayBox()
    {
        PlayerInventory inventory = FindAnyObjectByType<PlayerInventory>();
        musicBoxObj.SetActive(true);
        inventory.RemoveItem(musicBoxItem);
        inventory.RemoveItem(leverItem);
        PlayerMove player = FindAnyObjectByType<PlayerMove>();
        player.canMove = false;
        BackgroundMusicManager.Instance.StopBGM();
        SoundEffectsManager.Instance.PlaySound(music);

        yield return new WaitForSeconds(10);

        BackgroundMusicManager.Instance.PlayBackgroundTrack();
        player.canMove = true;

        inventory.AddItem(pictureItem);
        DialogSystem.instance.StartDialog(dialog);
        yield return new WaitForSeconds(1);
        MotherOldLocation.SetActive(false);
        MotherNextLoaction.SetActive(true);
        MirrorInteraction.SetActive(true);
        gameObject.SetActive(false);
    }
    
}
