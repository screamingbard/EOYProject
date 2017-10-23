using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour {

    //
    public string m_sPlayerTag;

    //
    public Animation m_animAnimation;

    //
    public bool m_bPlayMoreThanOnce;

    //
    public float m_fTimeBetweenAnimations;

    //
    float m_fTimerBetweenAnimations;

    //
    bool m_bPlayMoreThanOncePrivate = true;

    void start()
    {
        //
        m_fTimerBetweenAnimations = m_fTimeBetweenAnimations;
    }

    void Update()
    {

        if (m_bPlayMoreThanOncePrivate)
        {
            if (m_fTimerBetweenAnimations >= m_fTimeBetweenAnimations)
            {
                m_fTimerBetweenAnimations = 0;
            }
            else
            {
                m_fTimerBetweenAnimations += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider2D a_colCollider)
    {
        if (a_colCollider.gameObject.tag == m_sPlayerTag)
        {
            //If true will play the animation
            if (m_bPlayMoreThanOncePrivate)
            {
                if (m_fTimerBetweenAnimations >= m_fTimeBetweenAnimations)
                {
                    m_fTimerBetweenAnimations = 0;
                    m_animAnimation.Play();
                    m_bPlayMoreThanOncePrivate = m_bPlayMoreThanOnce;
                }
            }
        }
    }

}
