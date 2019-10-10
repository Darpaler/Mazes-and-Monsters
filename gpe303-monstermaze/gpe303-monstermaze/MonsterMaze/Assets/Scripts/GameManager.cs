using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public Player player;

    [Header("UI")]
    public UIController UIManager;

    [Header("AIs")]
    public List<AIController> AIList;
    public List<Racer> racers;

    [Header("Data for Blockers")]
    public Node currentBlocker;
    public float agentRadius = 0.9f;

    [Header("Data For Meshes")]
    public List<Node> nodes; // List of all nodes in the scene
    public List<Transform> startPositions; // Start points for racers - parallel to AIList
    public Transform targetFlag;
    public Node startNode;
    public Node targetNode;
    public LayerMask notANodeNotFloor; // For use in raycasting -- what can we click on?
    public GameObject nodePrefab;
    public Transform nodeParent;
    public Vector3 nodeOffset;
    public int numberOfRows = 10;
    public int numberOfCols = 10;

    [Header("Camera")]
    public CinemachineVirtualCamera mapCamera;
    public CinemachineVirtualCamera raceCamera;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }

        ClearAllNodes();
        PlaceAllNodes();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaceRacersAtStart();
        ActivateMapCamera();
    }

    public void ActivateMapCamera()
    {
        raceCamera.gameObject.SetActive(false);
        mapCamera.gameObject.SetActive(true);
    }

    public void ActivateRaceCamera()
    {
        mapCamera.gameObject.SetActive(false);
        raceCamera.gameObject.SetActive(true);
    }

    public void StartRace()
    {
        ConnectAllNodes();

        foreach (AIController AI in AIList) {
            AI.StartCoroutine("RunRace");
        }
        ActivateRaceCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (currentBlocker != null && currentBlocker.canBeBlocker) {
                currentBlocker.isBlocked = true;
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            if (currentBlocker != null) {
                currentBlocker.isBlocked = false;
            }
        }
    }

    void PlaceRacersAtStart()
    {
        for (int i=0; i<AIList.Count; i++)
        {
            AIList[i].pawn.tf.position = startPositions[i].position;
        }
    }

    void PlaceAllNodes()
    {
        for (int row = 0; row < numberOfRows; row++) {
            for (int col = 0; col < numberOfCols; col++) {

                // Create the node
                Vector3 position = targetFlag.position + new Vector3(nodeOffset.x * row, 0, nodeOffset.z * -col);
                GameObject tempNode = Instantiate<GameObject>(nodePrefab, position, Quaternion.identity, nodeParent);
                tempNode.name = "Node(" + row + "," + col + ")";

                // Target and Start Node corners set
                if (row == numberOfRows - 1 && col == numberOfCols - 1)  {
                    startNode = tempNode.GetComponent<Node>();
                    startNode.canBeBlocker = false;
                }
                if (row == 0 && col == 0) {
                    targetNode = tempNode.GetComponent<Node>();
                    targetNode.canBeBlocker = false;
                }
            }
        }
    }

    void ClearAllNodes()
    {
        foreach (Node node in GameManager.instance.nodes)
        {
            node.connections.Clear();
        }
    }


    int Index2D (int x, int y)
    {
        if (x<0 || x>numberOfCols || y <0 || y>numberOfRows) 
        {
            Debug.LogError("Error: (" + x + "," + y + ") is out of range.");
            return -1;
        }

        return (x + y * numberOfCols);        
    }

    void ConnectAllNodes()
    {
        ClearAllNodes();

        for (int row = 0; row < numberOfRows; row++) {


            // If not in the first row
            if (row > 0) {
                // Add all open tops                
                for (int col = 0; col < numberOfCols; col++) {
                    if (!nodes[Index2D(col, row - 1)].isBlocked) {
                        nodes[Index2D(col, row)].AddUniqueLinkTo(nodes[Index2D(col, row - 1)]);
                    }
                }
            }

            // If not in the last row, add all open bottoms
            if (row < numberOfRows - 1) {
                // Add all open bottoms                
                for (int col = 0; col < numberOfCols; col++) {
                    if (!nodes[Index2D(col, row + 1)].isBlocked) {
                        nodes[Index2D(col, row)].AddUniqueLinkTo(nodes[Index2D(col, row + 1)]);
                    }
                }
            }
        }

        for (int col = 0; col < numberOfCols; col++) {
            // If not in the first col, add all open lefts
            if (col > 0) {
                // Add all open lefts                
                for (int row = 0; row < numberOfRows; row++) {
                    if (!nodes[Index2D(col-1, row)].isBlocked) {
                        nodes[Index2D(col, row)].AddUniqueLinkTo(nodes[Index2D(col-1, row)]);
                    }
                }
            }

            // If not in the last col, add all open rights
            if (col < numberOfCols-1) {
                // Add all open lefts                
                for (int row = 0; row < numberOfRows; row++) {
                    if (!nodes[Index2D(col + 1, row)].isBlocked) {
                       nodes[Index2D(col, row)].AddUniqueLinkTo(nodes[Index2D(col + 1, row)]);
                    }
                }
            }

        }

    } 

}
