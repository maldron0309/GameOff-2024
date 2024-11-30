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
    public float counterToSkip = 0;
    public GameObject TextSkip;

    private bool finishCutscene = false;
    bool playing = false;
    PlayerInventory inventory;
    PlayerMove player;
    float oldMusicVolume;
    public void Interact()
    {
        if(playing == false)
        {
            StartCoroutine(PlayBox());
            playing = true;
        }
        
    }
    private void Update()
    {
        if (playing)
        {
            counterToSkip -= Time.deltaTime;
            if (counterToSkip < 0 && Input.GetKeyDown(KeyCode.Space))
            {
                finishCutscene = true;
            }
            if (finishCutscene)
            {
                TextSkip.SetActive(false);
                BackgroundMusicManager.Instance.PlayBackgroundTrack();
                player.canMove = true;

                inventory.AddItem(pictureItem);
                DialogSystem.instance.StartDialog(dialog);
                MotherOldLocation.SetActive(false);
                MotherNextLoaction.SetActive(true);
                MirrorInteraction.SetActive(true);
                gameObject.SetActive(false);
                BackgroundMusicManager.Instance.UpdateMusicVolume(oldMusicVolume);
                SoundEffectsManager.Instance.StopSound();
                playing = false;
            }
        }
    }
    public IEnumerator PlayBox()
    {
        inventory = FindAnyObjectByType<PlayerInventory>();
        musicBoxObj.SetActive(true);
        counterToSkip = 10;
        inventory.RemoveItem(musicBoxItem);
        inventory.RemoveItem(leverItem);
        player = FindAnyObjectByType<PlayerMove>();
        player.canMove = false;
        //BackgroundMusicManager.Instance.StopBGM();
        oldMusicVolume = BackgroundMusicManager.Instance.GetVolume();
        BackgroundMusicManager.Instance.UpdateMusicVolume(0f);
        SoundEffectsManager.Instance.PlaySound(music);

        yield return new WaitForSeconds(10);
        TextSkip.SetActive(true);

        yield return new WaitForSeconds(70);

        finishCutscene = true;

    }
    
}
