using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    [SerializeField] Structure[] structures;
    [SerializeField] List<BuildingNode> buildingNodes;
    [Header("Special Nodes")]
    [SerializeField] BuildingNode startNode;
    [SerializeField] BuildingNode endNode;
    [SerializeField] int railsNumber;

    [SerializeField] Item[] items;
    int startRailsNumber;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        startRailsNumber = railsNumber;
        UIManager.Instance.SetRailsCounter(railsNumber,startRailsNumber -railsNumber);
        items = FindObjectsOfType<Item>();
    }
    public void Build(BuildingNode node, int structureIndex)
    {
        // Build restrictions 
        if (railsNumber == 0)
        {
            Debug.Log("No more rails // TODO Display on screen");
            AudioManager.Instance.Play("CantBuild");
            return;
        }
        if (!StartNodeOccupied() && node != startNode)
        {
            Debug.Log("Build on start node first // TODO Display on screen");
            AudioManager.Instance.Play("CantBuild");
            return;
        }
        if (node.status == BuildingNode.Status.occupied)
        {
            Debug.Log("Already occupied // TODO Display on screen");
            AudioManager.Instance.Play("CantBuild");
            return;
        }
        if (GameManager.Instance.GetTimer() > 0)
        {
            Debug.Log("You can't build when the game started // TODO Display on screen");
            AudioManager.Instance.Play("CantBuild");
            return;
        }
        // Build events
        if (node == startNode)
        {
            UIManager.Instance.DisplayInstruction(1);
        }
        if (node == endNode)
        {
            UIManager.Instance.DisplayInstruction(2);
        }
        // Build process
        railsNumber--;
        UIManager.Instance.SetRailsCounter(railsNumber,startRailsNumber - railsNumber);
        node.status = BuildingNode.Status.occupied;
        GameObject newRail = Instantiate(structures[structureIndex].GetPrefab(), node.transform.position, Quaternion.identity);
        newRail.transform.localPosition += node.GetSpawnpoint();
        node.SetStructure(newRail);
        PathManager.Instance.AddWaypoint(newRail.transform.position);
        AudioManager.Instance.Play("build_"+Random.Range(0,2));
    }

    public Structure GetStructure(int index)
    {
        return structures[index];
    }

    public void AddNode(BuildingNode node)
    {
        buildingNodes.Add(node);
    }

    public void ResetNodes()
    {
        railsNumber = startRailsNumber;
        UIManager.Instance.SetRailsCounter(railsNumber, startRailsNumber - railsNumber);
        foreach (BuildingNode node in buildingNodes)
        {
            node.status = BuildingNode.Status.free;
            Destroy(node.GetStructureOnMe());
        }

        foreach (Item item in items)
        {
            item.ResetPosition();
        }
    }

    public bool StartNodeOccupied()
    {
        return startNode.status == BuildingNode.Status.occupied;
    }
}

[System.Serializable]
public class Structure
{
    [SerializeField] GameObject prefab;
    public GameObject GetPrefab()
    {
        return prefab;
    }
}
