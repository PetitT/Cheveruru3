using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToSpawnTrigger : MonoBehaviour
{
    public GameObject spawnPoint;
    public float minY;

    private void Update()
    {
        if(transform.position.y < minY)
        {
            transform.position = spawnPoint.transform.position;
        }
    }
}
