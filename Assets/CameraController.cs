using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;

    public CinemachineFreeLook lookCam;

    private Vector3 clickStartPos;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            lookCam.m_XAxis.Value += (clickStartPos - Input.mousePosition).normalized.x * speed;
        }
    }
}
