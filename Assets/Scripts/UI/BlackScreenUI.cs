using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenUI : MonoBehaviour
{
    public static BlackScreenUI instance;
    public Animator anim;
    private void Awake()
    {
        instance = this;
    }
    public void FadeIn()
    {
        anim.Play("FadeIn");
    }
    public void FadeOut()
    {
        anim.Play("FadeOut");
    }

}
