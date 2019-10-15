using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_ObstacleAvoidance : AIController
{
    // TODO: This is the simple Obstacle Avoidance from the GPE205 Tank Game Class
    //       DO NOT SUBMIT THIS AS IT IS
    //       You need to add some of the additional functionality from the book.
    //       This can be the "Pursue" (Head them off at the pass) technique.
    //       OR: it can be multiple "feelers" / raycasts 
    //       OR: You can use Vector3.Dot() to add some intelligent turning.

    public float turnDistance = 5;
    public float speed = 3.5f;
    public float turnSpeed = 360;
    public float avoidMoveTime = 1.5f;
    private float enterStateTime;
    public enum AvoidState { None, TurnToAvoid, MoveToAvoid };
    public AvoidState moveState = AvoidState.None;

    public override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override IEnumerator CalculatePath()
    {
        // TODO: Any calculations for finding the path. Be sure to put a "yield return null" at the end of each step, 
        //       so it happens over multiple frames

        // End of frame draw
        yield return null; 
    }
    public override IEnumerator RunRace()
    {
        // Calculate the path
        yield return StartCoroutine("CalculatePath");

        // TODO: Add any code required to start running the race

        // When that is done, start moving
        isRunning = true;
        yield return null; // End of one frame draw
    }
    protected override void LookAndAnimate()
    {
        // Animate the pawn based on our movement velocity
        pawn.Animate();
    }
    protected override IEnumerator OnAddObstacle()
    {
        // Save if they were running
        bool wasRunning = isRunning;

        // Stop running
        isRunning = false;

        // Recalculate Path (if needed)
        yield return StartCoroutine("CalculatePath");

        // TODO: Anything after the path is calculate that needs to be done

        // Return to previous state
        isRunning = wasRunning;

        // End of frame draw
        yield return null;
    }
    protected override void Move()
    {
        if (moveState == AvoidState.None)
        {
            // Look at target
            pawn.tf.LookAt(GameManager.instance.targetNode.tf.position);
            // Move forward
            pawn.tf.position += pawn.tf.forward * speed * Time.deltaTime;
            // If we CAN'T move forward, change to turn to avoid state
            if (!CanMoveForward())
            {
                moveState = AvoidState.TurnToAvoid;
                enterStateTime = Time.time;
            }
        }
        else if (moveState == AvoidState.TurnToAvoid)
        {
            pawn.tf.Rotate(0, turnSpeed * Time.deltaTime, 0);

            if (CanMoveForward())
            {
                moveState = AvoidState.MoveToAvoid;
                enterStateTime = Time.time;
            }
        }
        else if (moveState == AvoidState.MoveToAvoid)
        {
            // If you can move forward, do so for X seconds
            if (CanMoveForward())
            {
                // Move forward
                pawn.tf.position += pawn.tf.forward * speed * Time.deltaTime;

                // If Time is up
                if (Time.time >= enterStateTime + avoidMoveTime)
                {
                    // Go back to normal state
                    moveState = AvoidState.None;
                    enterStateTime = Time.time;
                }

            } else
            {
                // Can't move forward, so go back to turning to avoid
                moveState = AvoidState.TurnToAvoid;
                enterStateTime = Time.time;
            }
        }
    }

    private bool CanMoveForward()
    {
        RaycastHit hitInfo;


        // Return false if we cannot move forward
        //if (Physics.SphereCast(new Ray(pawn.tf.position, pawn.tf.forward), 0.35f, out hitInfo, speed * Time.deltaTime * 2, GameManager.instance.notANodeNotFloor))
        if(Physics.SphereCast(pawn.transform.position, turnDistance, pawn.tf.forward, out hitInfo, speed * Time.deltaTime * 2, GameManager.instance.notANodeNotFloor))
        {
            if (Vector3.Dot(pawn.tf.forward, hitInfo.transform.position - pawn.tf.position) >= 0.5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            // Otherwise, return true
            return true;
        }
    }
}
    
