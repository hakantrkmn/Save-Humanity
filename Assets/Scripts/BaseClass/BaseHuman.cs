using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseHuman : MonoBehaviour
{
    public HumanStates state;
    public Vector3 walkDirection;
    public float walkSpeed;
    public Renderer renderer;
    public Rigidbody rb;
    public Animator animator;
    public bool onAir;
    public bool canWalk;
    public List<GameObject> climbedWalls;
    private bool onWall;

    protected virtual void Walk()
    {
        if (state == HumanStates.WalkingOnGround || state == HumanStates.WalkingOnWall)
        {
            rb.velocity = walkDirection * walkSpeed;
        }
        //transform.position += walkDirection * Time.deltaTime * walkSpeed;
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
            state = HumanStates.OnAir;

            rb.AddForce(Vector3.up * other.GetComponent<JumpEffector>().jumpPower, ForceMode.Impulse);
            animator.SetBool("fall", true);
        }
        else if (other.GetComponent<FallLimit>())
        {
            EventManager.HumanHitTheEnd(this);
        }
        else if (other.GetComponent<End>())
        {
            state = HumanStates.GoingToHeaven;
            rb.isKinematic = true;
            var point = other.transform.position + Random.insideUnitSphere*.3f;
            transform.DOMoveX(point.x, 1f);
            transform.DOMoveZ(point.z, 1f);

            transform.DOMoveY(30, 5);
        }
        else if (other.GetComponent<BaseCollectable>())
        {
            EventManager.CollectableHit(other.GetComponent<BaseCollectable>());
        }
        else if (other.transform.CompareTag("Fall"))
        {
            animator.SetBool("fall", true);
                rb.isKinematic = false;
                canWalk = false;
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.GetComponent<ClimbableWall>() && climbedWalls.Contains(collision.gameObject))
        {
            if (state==HumanStates.WalkingOnWall)
            {
                animator.SetBool("fall", true);
                state = HumanStates.OnAir;
            }
            
        }

        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            if (state == HumanStates.OnAir)
            {
                animator.SetBool("fall", false);
                state = HumanStates.WalkingOnGround;
            }
        }

        if (collision.transform.GetComponent<ClimbableWall>() && !climbedWalls.Contains(collision.gameObject))
        {
            state = HumanStates.ClimbingWall;
            climbedWalls.Add(collision.gameObject);
            animator.SetBool("climb", true);
            animator.SetBool("fall", false);
            rb.isKinematic = true;
            transform.DOMoveY(collision.transform.GetComponent<ClimbableWall>().climbPoint.position.y - .85f, 1f)
                .OnComplete(
                    () =>
                    {
                        animator.SetTrigger("climbUp");

                        transform.DOMoveY(collision.transform.GetComponent<ClimbableWall>().climbPoint.position.y,
                            .95f);
                        DOVirtual.Float(0, 1, .95f, (x) => { }).OnComplete(() =>
                        {
                            animator.SetBool("climb", false);
                            state = HumanStates.WalkingOnWall;
                            rb.isKinematic = false;
                        });
                    });
        }
    }
}