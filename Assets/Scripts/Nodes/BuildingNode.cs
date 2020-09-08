using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNode : MonoBehaviour
{
    public enum Status
    {
        free,
        occupied
    }
    public Status status;
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] GameObject structureOnMe;
    [SerializeField] float maxDistance;
    private void Start()
    {
        BuildManager.Instance.AddNode(this);
    }
    private void OnMouseDown()
    {
        float distance = Vector3.Distance(transform.position, PathManager.Instance.GetLastWaypointPosition(transform.position));
        if (distance < maxDistance)
            BuildManager.Instance.Build(this,0);
        else
        {
            AudioManager.Instance.Play("CantBuild");
            Debug.Log("Too far // TODO Display on screen");
        }
    }
    public GameObject GetStructureOnMe()
    {
        return structureOnMe;
    }

    public void SetStructure(GameObject structure)
    {
        structureOnMe = structure;
    }

    public Vector3 GetSpawnpoint()
    {
        return spawnPoint;
    }
}
