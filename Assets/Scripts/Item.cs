using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    Vector3 position;
    private void Awake()
    {
        position = transform.position;
    }
    public void ResetPosition()
    {
        transform.parent = null;
        transform.position = position;
        GetComponent<Collider>().enabled = true;
    }
}
