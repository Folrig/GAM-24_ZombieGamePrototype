using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        animator.Play("Open");
    }
}
