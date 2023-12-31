using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidBodies;
    Animator animator;

    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidBodies in rigidBodies)
            rigidBodies.isKinematic = true;
        animator.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBodies in rigidBodies)
            rigidBodies.isKinematic = false;
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
