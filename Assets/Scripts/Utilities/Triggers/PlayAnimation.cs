using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour {

    //
    public string m_sPlayerTag;

    //
    public Animation m_animAnimation;

    //
    public Animator m_animatorAnimator;

    //
    public bool m_bPlayMoreThanOnce;

    //
    public string m_stAnimationBool;

    //
    bool m_bPlayMoreThanOncePrivate = true;
    
    void start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D a_colCollider)
    {
        if (a_colCollider.gameObject.tag == m_sPlayerTag)
        {
            //If true will play the animation
            if (m_bPlayMoreThanOncePrivate)
            {
                if (!m_animAnimation.isPlaying)
                {
                    m_animatorAnimator.SetBool(m_stAnimationBool, true);
                    m_bPlayMoreThanOncePrivate = m_bPlayMoreThanOnce;
                }
            }
        }
    }
}
