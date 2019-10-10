using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController_BreadthFirst : AIController
{
    [System.Serializable]
    public class NodeRecord
    {
        public Node node;
        public NodeConnection connection;
    }

    [Header("Variables")]
    public float speed = 3.5f;
    public float turnSpeed = 360;
    public int currentNodeInPath = 0;

    [Header("Lists for Pathfinding")]
    public Node startNode;
    public List<NodeConnection> path;
    public List<NodeRecord> openList;
    public List<NodeRecord> closedList;

    public override void Start()
    {
        startNode = GameManager.instance.startNode;
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
        
        // Initialize the Record
        NodeRecord startRecord = new NodeRecord();
        startRecord.node = startNode;
        startRecord.connection = null;

        // Initialize Lists and Variables
        openList = new List<NodeRecord>();
        closedList = new List<NodeRecord>(openList);
        NodeRecord current = null;
        NodeRecord endNodeRecord = null;

        // NOTE: Start with just the start record in the open list
        openList.Add(startRecord);

        // NEXT FRAME
        yield return null;

        // Iterate through each node
        while (openList.Count > 0)
        {
            // Find the first element in the open list
            current = openList[0];

            // If this is the goal, then terminate
            if (current.node == GameManager.instance.targetNode)
            {
                break;
            }

            // Otherwise, get its outgoing connections
            // Loop through each connection
            foreach (NodeConnection connection in current.node.connections)
            {
                // Skip if the node is closed
                // NOTE: SEE "ListContains(list, node)" helper function below
                if (ListContains(closedList, connection.toNode))
                {
                    continue;
                }

                // ... or if it is open 
                else if (ListContains(openList, connection.toNode))
                {
                    // Here we find the record in the open list corresponding to the endNode
                    endNodeRecord = FindInList(openList, connection.toNode);
                }
                else
                {
                    // Otherwie, we know we are at an unvisited node
                    // Make a record for it 
                    // NOTE: Use the same variable "endNodeRecord", so that we can update either this new one, OR the open one with the same line of code below
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.node = connection.toNode;
                }

                // We're here if we need to update the node 
                // NOTE: If we had been closed, we would have "continue"'d out of here - OR - if we had been open, but worse, we would have "continue"'d out of here              
                endNodeRecord.connection = connection;

                // Add it to the open list
                // NOTE: Make sure it isn't already there
                if (!ListContains(openList, endNodeRecord.connection.toNode))
                {
                    openList.Add(endNodeRecord);
                }

                // (NOTE: Next frame -- so this process occurs over time)
                yield return null;
            }

            // We've looked at all the connections for the current node. 
            // Add it to closed list.
            closedList.Add(current);
            // Remove it from open list.
            openList.Remove(current);

            // (NOTE: Next frame -- so this process occurs over time)
            yield return null;
        }

        // We're here if we've either found the goal (NOTE: If we are the goal, we would "break" out to this point)
        //     OR we have no more nodes to search ( openList.Count <= 0 )
        // Find out which:
        if (current.node != GameManager.instance.targetNode )
        {
            // We ran out of nodes without finding a goal
            // Clear the path
            path.Clear();

            // Quit the function
            yield break;
        }


        // Compile the list of connections in the path
        path = new List<NodeConnection>();

        // Work back through the path, accumulating connections
        while (current.node != GameManager.instance.startNode)
        {
            //(NOTE: Add the connection to the path)
            path.Add(current.connection);
            //(NOTE: Move to the previous connection)
            current = FindInList(closedList, current.connection.fromNode);
        }

        // Reverse the path and save it
        path.Reverse();

        // Start at node zero
        currentNodeInPath = 0;

        // End of frame draw
        yield return null; 
    }

    private NodeRecord FindInList (List<NodeRecord> targetList, Node testNode)  
    {
        foreach (NodeRecord record in targetList)
        {
            if (record.node == testNode)
            {
                return record;
            }             
        }
        return null;
    }

    private bool ListContains (List<NodeRecord> testList, Node testNode)
    {
        foreach (NodeRecord record in testList)
        {
            if (record.node == testNode)
            {
                return true;
            }
        }
        return false;
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
        // Animate the pawn based on our movement velocity
        pawn.Animate();
    }
    protected override IEnumerator OnAddObstacle()
    {
        // Save if they were running
        bool wasRunning = isRunning;

        // Stop running
        isRunning = false;

        // Keep track of my current node
        if (path.Count > currentNodeInPath)
        {
            startNode = path[currentNodeInPath].toNode;            
        } else
        {
            startNode = GameManager.instance.startNode;
        }

        // Recalculate Path (if needed)
        StopCoroutine("CalculatePath");
        yield return StartCoroutine("CalculatePath");

        // Anything after the path is calculated that needs to be done

        // Return to previous state
        isRunning = wasRunning;

        // End of frame draw
        yield return null;
    }
    protected override void Move()
    {
        // If we are not at the end of the goal
        if (currentNodeInPath < path.Count)
        {
            // Look at the next node in the path
            pawn.tf.LookAt(path[currentNodeInPath].toNode.tf.position);

            // Move towards the next waypoint
            pawn.tf.position = Vector3.MoveTowards(pawn.tf.position, path[currentNodeInPath].toNode.tf.position, speed * Time.deltaTime);

            // If "close enough" to count
            if (Vector3.Distance(pawn.tf.position, path[currentNodeInPath].toNode.tf.position) < 0.1f)
            {
                // Advance to next waypoint
                currentNodeInPath++;
            }
        }
        // else we reached the end
        else
        {
            // TODO: Add work to do if we are at the end
        }

    }

}

    
