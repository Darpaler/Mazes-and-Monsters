using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_AStar : AIController
{
    // TODO: Create this Class to use A* Pathfinding!
    //       Use AIController_BreadthFirst as an example!

    // TODO: Define what a "Node" looks like in A*
    //       You may want to use a "property" for finding F, so you don't have to store it!
    [System.Serializable]
    public class NodeRecord
    {
        // TODO: Define what one "node record" is according to A*
    }

    [Header("Variables")]
    public float speed = 3.5f;
    public float turnSpeed = 360;
    public int currentNodeInPath = 0;

    [Header("Lists for Pathfinding")]
    public Node startNode;
    public List<NodeConnection> path;
    // TODO: There are more lists! What are they?

    // Start is called before the first frame update
    public override void Start()
    {
        // TODO: Add anything your AI needs at start

        // Call the parentclass Start()
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
    public override IEnumerator RunRace()
    {
        // Calculate the path
        yield return StartCoroutine("CalculatePath");

        // TODO: Add anything here needed to start running the race

        // When that is done, start moving
        isRunning = true;
        yield return null; // End of one frame draw
    }
    protected override void LookAndAnimate()
    {
        // TODO: Most pathfinding code requres your AI to turn to look at a target, 
        //       do that here!

        // This will make the pawn "animate" the Naruto run if it is moving fast enough
        pawn.Animate();
    }

    public override IEnumerator CalculatePath()
    {
        // Calculate the path 

        // Remember to do a "yield return null" after every step.
        //        This allows the game to continue (another frame draw) while
        //        this AI is still calculating
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

        //TODO: Anything after the path is calculate that needs to be done

        // Return to previous state
        isRunning = wasRunning;

        // End of frame draw
        yield return null;
    }

    protected override void Move()
    {
        // TODO: Add any code that the AI needs to move
    }

    // TODO: Use helper functions!
    //       See Breadth-First for help!
}
