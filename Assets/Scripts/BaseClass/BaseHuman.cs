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
    public bool canWalk;
    public List<GameObject> climbedWalls;

    protected virtual void Walk()
    {
        if (state == HumanStates.WalkingOnGround || state == HumanStates.WalkingOnWall)
        {
            rb.velocity = walkDirection * walkSpeed;
        }
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
            Debug.Log("ksdnfşlknsd");
            TurnByDirection(other.GetComponent<TurnEffector>().direction);
        }
        else if (other.GetComponent<JumpEffector>())
        {
            Jump(other.GetComponent<JumpEffector>().jumpPower);
        }
        else if (other.GetComponent<FallLimit>())
        {
            EventManager.HumanHitTheEnd(this);
        }
        else if (other.GetComponent<End>())
        {
            GoToHeaven(other.transform.position);
        }
        else if (other.GetComponent<BaseCollectable>())
        {
            EventManager.CollectableHit(other.GetComponent<BaseCollectable>());
        }
        else if (other.transform.CompareTag("Fall"))
        {
            
            FallFromGround();
        }
    }

    void TurnByDirection(Vector3 direction)
    {
        DOVirtual.Vector3(walkDirection, direction, Random.Range(.1f,.5f), (value =>
        {
            walkDirection = value;
            transform.forward = walkDirection;
        }));
    }

    void Jump(float power)
    {
        state = HumanStates.OnAir;
        //rb.velocity = Vector3.zero;
        rb.AddForce((Vector3.up * power) , ForceMode.Impulse);
        animator.SetBool("fall", true);
    }

    void GoToHeaven(Vector3 middlePoint)
    {
        state = HumanStates.GoingToHeaven;
        rb.isKinematic = true;
        var point = middlePoint + Random.insideUnitSphere*.3f;
        transform.DOMoveX(point.x, 1f);
        transform.DOMoveZ(point.z, 1f);
        transform.DOMoveY(30, 5);
        animator.SetTrigger("rise");
    }

    void FallFromGround()
    {
        animator.SetBool("fall", true);
        rb.isKinematic = false;
        canWalk = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.GetComponent<ClimbableWall>())
        {
            if (state==HumanStates.WalkingOnWall)
            {
                FallFromWall();
            }
            
        }

        
        
    }

    void FallFromWall()
    {
        animator.SetBool("fall", true);
        state = HumanStates.OnAir;
    }

    void ClimbWall(GameObject wall)
    {
        state = HumanStates.ClimbingWall;
        climbedWalls.Add(wall);
        animator.SetBool("climb", true);
        animator.SetBool("fall", false);
        rb.isKinematic = true;
        transform.DOMoveY(wall.transform.GetComponent<ClimbableWall>().climbPoint.position.y - .85f, 1f)
            .OnComplete(
                () =>
                {
                    animator.SetTrigger("climbUp");

                    transform.DOMoveY(wall.transform.GetComponent<ClimbableWall>().climbPoint.position.y,
                        .95f);
                    DOVirtual.Float(0, 1, .95f, (x) => { }).OnComplete(() =>
                    {
                        animator.SetBool("climb", false);
                        state = HumanStates.WalkingOnWall;
                        rb.isKinematic = false;
                    });
                });
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

        if (collision.transform.GetComponent<ClimbableWall>() && state!= HumanStates.WalkingOnWall)
        {
           ClimbWall(collision.gameObject);
        }
    }
}