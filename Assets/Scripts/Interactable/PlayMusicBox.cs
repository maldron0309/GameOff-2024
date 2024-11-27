using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicBox : MonoBehaviour, IInteractable
{
    public DialogGraph dialog;
    public AudioClip music;
    public void Interact()
    {
        StartCoroutine(PlayBox()) ;
    }
    public IEnumerator PlayBox()
    {
        gameObject.SetActive(false);
        PlayerMove player = FindAnyObjectByType<PlayerMove>();
        player.canMove = false;
        BackgroundMusicManager.Instance.StopBGM();
        SoundEffectsManager.Instance.PlaySound(music);

        yield return new WaitForSeconds(10);


        BackgroundMusicManager.Instance.PlayBackgroundTrack();
        player.canMove = true;

        DialogSystem.instance.StartDialog(dialog);
    }
    
}
