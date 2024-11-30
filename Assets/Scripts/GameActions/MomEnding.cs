using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomEnding : BaseAction
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public override void ActivateEffect()
    {
        StartCoroutine(RunEnding());
    }
    public IEnumerator RunEnding()
    {
        BlackScreenUI.instance.FadeOut();

        yield return new WaitForSeconds(2);

        EndScreenUI.instance.ShowSceen("Bad ending");
        EndScreenUI.instance.images2.SetActive(true);
        BackgroundMusicManager.Instance.PlayEndingTrack2();

        //
    }
}
