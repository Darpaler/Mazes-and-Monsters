using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Node : MonoBehaviour
{
    public List<NodeConnection> connections;
    [HideInInspector] public Transform tf;

    public GameObject ghostBlocker;
    public GameObject realBlocker;
    public bool isBlocked = false;
    public bool canBeBlocker = true;

    // Start is called before the first frame update
    void Start()
    {
        // Save transform component
        tf = GetComponent<Transform>();  
        // Add all nodes to the list
        if (!GameManager.instance.nodes.Contains(this))
        {
            GameManager.instance.nodes.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if (!canBeBlocker) return;

        // Turn on "ghost" version of blocking object
        ghostBlocker.SetActive(true);
        realBlocker.SetActive(false);
        // Remember that this is the "selected" Node
        GameManager.instance.currentBlocker = this;
    }

    public void OnMouseExit() {
        // Turn off "ghost" version of blocking object
        ghostBlocker.SetActive(false);
        realBlocker.SetActive(isBlocked);
        // Forget that this was the "selected" Node
        GameManager.instance.currentBlocker = null;
    }

    public bool ContainsLinkTo(Node target)
    {
        foreach (NodeConnection connection in connections)
        {
            if (connection.toNode == target)
            {
                return true;
            }
        }
        return false;
    }

    public void AddUniqueLinkTo (Node target)
    {
        if (ContainsLinkTo(target))
        {
            return;
        }
        else
        {
            NodeConnection temp = new NodeConnection();
            temp.fromNode = this;
            temp.toNode = target;
            temp.cost = Vector3.Distance(this.tf.position, target.tf.position);
            connections.Add(temp);
        }

    }

    private void OnDrawGizmos()
    {
        foreach (NodeConnection connection in connections)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(connection.fromNode.transform.position, connection.toNode.transform.position);
        }
    }
}

[System.Serializable]
public class NodeConnection
{
    public Node fromNode;
    public Node toNode;
    public float cost;
}
