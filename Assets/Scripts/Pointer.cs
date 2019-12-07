using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public float m_DefaultLength = 5;
    public GameObject m_Dot;
    public VRInputModule m_inputModule;

    private LineRenderer m_lineRenderer = null;

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        //line default or distance
        //PointerEventData data = m_inputModule.GetData();
        float targetLength = m_DefaultLength; //data.pointerCurrentRaycast.distance == 0f ? m_DefaultLength : data.pointerCurrentRaycast.distance;

        //Raycast
        RaycastHit hit = CreateRaycast(targetLength);

        //default end
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        //or based on the hit and chick for collider
        if(hit.collider != null)
        {
            endPosition = hit.point;
        }

        //set position of the dot
        m_Dot.transform.position = endPosition;

        //set positiion of the line renderer
        m_lineRenderer.SetPosition(0, transform.position);
        m_lineRenderer.SetPosition(1, endPosition);
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, m_DefaultLength);

        return hit;
    }
}
