using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseHuman : MonoBehaviour
{
    public Vector3 walkDirection;
    public float walkSpeed;
    public Renderer renderer;
    public Rigidbody rb;
    public Animator animator;
    public bool onAir;
    public bool canWalk;

    protected virtual void Walk()
    {
        transform.position += walkDirection * Time.deltaTime * walkSpeed;
    }


    public void SetHuman()
    {
        canWalk = true;
        renderer.material.color = Random.ColorHSV();
        walkDirection = transform.parent.forward;
        transform.forward = walkDirection;
    }

    protected virtual void Update()
    {
        if (canWalk)
        {
            Walk();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TurnEffector>())
        {
            DOVirtual.Vector3(walkDirection, other.GetComponent<TurnEffector>().direction, .5f, (value =>
            {
                walkDirection = value;
                transform.forward = walkDirection;
            }));
        }
        else if (other.GetComponent<JumpEffector>())
        {
            rb.AddForce(Vector3.up * other.GetComponent<JumpEffector>().jumpPower, ForceMode.Impulse);
            animator.SetBool("fall", true);
            onAir = true;
        }
        else if (other.GetComponent<FallLimit>())
        {
            EventManager.HumanHitTheEnd(this);
        }
        else if (other.GetComponent<End>())
        {
            canWalk = false;
            rb.isKinematic = true;
            //transform.DOMoveX(other.transform.position.x, 1f);
            transform.DOMoveZ(other.transform.position.z, 1f);
            transform.DOMoveY(30, 5);
        }
        else if (other.GetComponent<BaseCollectable>())
        {
            EventManager.CollectableHit(other.GetComponent<BaseCollectable>());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground") && onAir)
        {
            animator.SetBool("fall", false);
            onAir = false;
        }

        if (collision.transform.GetComponent<ClimbableWall>())
        {
            canWalk = false;
            animator.SetBool("climb", true);
            animator.SetBool("fall", false);
            rb.isKinematic = true;
            transform.DOMoveY(collision.transform.GetComponent<ClimbableWall>().climbPoint.position.y, 3f).OnComplete(
                () =>
                {
                    canWalk = true;
                    animator.SetBool("climb", false);
                    rb.isKinematic = false;
                });
        }
    }
}