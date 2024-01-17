using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Test : MonoBehaviour
{
    public Animator animator;


    bool r = false;
    bool s = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            r = !r;
            animator.SetBool("Running", r);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            s = !s;
            animator.SetBool("Shooting", s);
        }



    }
}
