using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandoObstacleGeneration : MonoBehaviour
{
    public Transform[] Row1Prefabs;
    public Transform[] Row2Prefabs;
    public Transform[] Row3Prefabs;
    public Transform[] CopRowPrefabs;
    public GameObject[] Obstacles;
    public GameObject PoliceCar;
    public bool medium = false;
    public bool hard = false;
    private static int lastSpawned = 0;
    // Start is called before the first frame update
    void Start()
    {
        SpawnRandomObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This will spawn the obstacles on the instatiation of the tile
    void SpawnRandomObstacles()
    {
        lastSpawned++;
        if ((medium || hard) && lastSpawned > 4)
        {
            if(UnityEngine.Random.Range(1, medium ? 10 : 5) == 1)
            {
                int randomIndex = UnityEngine.Random.Range(0, CopRowPrefabs.Length);
                Vector3 pos = CopRowPrefabs[randomIndex].position;
                pos.y = 0;
                GameObject go = Instantiate(PoliceCar, pos, PoliceCar.transform.rotation, transform);
                lastSpawned = 0;
            }
        }

        int randomIndex1 = UnityEngine.Random.Range(0, Row1Prefabs.Length);
        int randomIndex2 = UnityEngine.Random.Range(0, Row2Prefabs.Length);
        int randomIndex3 = UnityEngine.Random.Range(0, Row3Prefabs.Length);
        int randomGameObs1 = UnityEngine.Random.Range(0, Obstacles.Length);
        int randomGameObs2 = UnityEngine.Random.Range(0, Obstacles.Length);
        int randomGameObs3 = UnityEngine.Random.Range(0, Obstacles.Length);

        GameObject go1;
        GameObject go2;
        GameObject go3;
        go1 = Instantiate(Obstacles[randomGameObs1], Row1Prefabs[randomIndex1].position, Row1Prefabs[randomIndex1].rotation, transform) as GameObject;
        go2 = Instantiate(Obstacles[randomGameObs2], Row2Prefabs[randomIndex2].position, Row2Prefabs[randomIndex2].rotation, transform) as GameObject;
        go3 = Instantiate(Obstacles[randomGameObs3], Row3Prefabs[randomIndex3].position, Row3Prefabs[randomIndex3].rotation, transform) as GameObject;
    }
}
