using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGrappleArea : MonoBehaviour {

    public GameObject goplayer;

    void Awake()
    {
        goplayer = GameObject.FindGameObjectWithTag("Player");
    }

    void OnCollisionEnter2D(Collision2D collision2d)
    {
        if(collision2d.gameObject.tag == "Grapple"){
            goplayer.GetComponentInChildren<ShootOBJ>().StopShoot();
        }
    }

}
