using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlidingNode : MonoBehaviour
{
    [SerializeField] Renderer barRenderer;
    [SerializeField] Gradient gradient;
    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;
    Train train;
    MaterialPropertyBlock block;
    Camera viewCamera;

    [System.Serializable]
    public class Event : UnityEvent<float> { }
    [SerializeField] Event OnChangeEvent;

    bool locked;
    private void Awake()
    {
        block = new MaterialPropertyBlock();
        SetColor(.5f);
    }
    private void Start()
    {
        viewCamera = Camera.main;
        
    }
    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!locked)
                MoveHandler();
        }
    }
    public void MoveHandler()
    {
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (transform.localPosition.y <= maxHeight && transform.localPosition.y >= minHeight)
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z); 
        }

        if (transform.localPosition.y > maxHeight)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, maxHeight, transform.localPosition.z);
        }
        if (transform.localPosition.y < minHeight)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, minHeight, transform.localPosition.z);
        }
        OnChangeEvent.Invoke((transform.localPosition.y + 1f) / 2f * 100f);
    }
    public void SetColor(float percent)
    {
        block.SetColor("_BaseColor", gradient.Evaluate(percent/100f));
        barRenderer.SetPropertyBlock(block);
    }
    public void ToggleLock()
    {
        locked = !locked;
        if (locked)
        {
            block.SetColor("_BaseColor", Color.black);
            barRenderer.SetPropertyBlock(block);
        }
        else
        {
            SetColor(Mathf.Clamp((transform.localPosition.y + 1f) / 2f * 100f, 0f, 100f)/100f);
        }
    }
}
