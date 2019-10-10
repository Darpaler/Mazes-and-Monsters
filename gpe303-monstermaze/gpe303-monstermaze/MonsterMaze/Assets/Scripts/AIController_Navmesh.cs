using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This is the AI Controller that uses Unity's NavMesh to move. 

public class AIController_Navmesh : AIController
{
    // NavMeshAgent is a Unity component that handles pathfinding
    public NavMeshAgent agent;
    
    // Start is called before the first frame update
    public override void Start()
    {
        // Call the parentclass Start()
        base.Start();

        // Get the NavMeshAgent
        agent = pawn.gameObject.AddComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("ERROR: Pawn MUST contain a NavMeshAgent component.");
        }
    }

    public override void Update()
    {
        base.Update();
    }
    public override IEnumerator RunRace()
    {
        // Calculate the path
        yield return StartCoroutine("CalculatePath");
        // When that is done, start moving
        isRunning = true;
        yield return null; // End of one frame draw
    }
    protected override void LookAndAnimate()
    {   
        pawn.tf.LookAt(agent.nextPosition);
        pawn.Animate();        
    }

    public override IEnumerator CalculatePath()
    {
        // Calculate the path to targetLocation.tf.position       
        agent.SetDestination(GameManager.instance.targetNode.tf.position);
        agent.isStopped = true;
        yield return null;
    }

    protected override IEnumerator OnAddObstacle()
    {
        // Save if they were running
        bool wasRunning = isRunning;

        // Stop running
        isRunning = false;

        // Recalculate Path (if needed)
        yield return StartCoroutine("CalculatePath");

        // Anything after the path is calculate that needs to be done

        // Return to previous state
        isRunning = wasRunning;

        // End of frame draw
        yield return null;
    }

    protected override void Move()
    {
        // NavMeshAgent moves automatically, we only need to start it up again if we are stopped
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
    }
}
