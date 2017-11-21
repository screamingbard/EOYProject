using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour {

    //The player tag
    public string m_sPlayerTag;

    //The animation that'll be played
    public Animation m_animAnimation;

    //The animator that'll play the animations
    public Animator m_animator;

    //The public control over if the animation will play more than once
    public bool m_bPlayMoreThanOnce;

    //The name of the machanim bool that controls that animations
    public string m_stAnimationBool;

    //A private controller bool
    bool m_bPlayMoreThanOncePrivate = true;
    
    void OnTriggerEnter2D(Collider2D a_colCollider)
    {
        //If the player enters the trigger
        if (a_colCollider.gameObject.tag == m_sPlayerTag)
        {
            //If the private variable, play more than once private is true, play the animation
            if (m_bPlayMoreThanOncePrivate)
            {
                //If the animation isn't already playing
                if (!m_animAnimation.isPlaying)
                {
                    //Set the animators bool to true to play the animation
                    m_animator.SetBool(m_stAnimationBool, true);
                    m_bPlayMoreThanOncePrivate = m_bPlayMoreThanOnce;
                }
            }
        }
    }
}
