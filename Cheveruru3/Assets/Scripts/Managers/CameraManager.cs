using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook cam;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        Recenter();
    }

    public void Recenter()
    {
        cam.m_YAxisRecentering.RecenterNow();
        cam.m_XAxis.Value = 0;
        cam.m_YAxis.Value = 0;
    }
}
