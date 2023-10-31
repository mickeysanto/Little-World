using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public GameObject idMaker;
    public string objName;
    public float spawnRange = 1;
    public float maxSpawn = 4;
    public float spawnTime = 3;
    public bool startSpawned = true;
    public bool respawn = true;

    void Start()
    {
        if(startSpawned)
        {
            for(int i = 0; i < maxSpawn; i++)
            {
                spawn();
            }
        }

        StartCoroutine(manageSpawn());
    }

    public void spawn()
    {
        GameObject newObj = Instantiate(prefab, new Vector3(transform.position.x + Random.Range(-1*spawnRange, spawnRange), transform.position.y + Random.Range(-1*spawnRange, spawnRange), 0), Quaternion.identity);
        newObj.name = objName + idMaker.GetComponent<IdMaker>().getID();
        newObj.transform.parent = gameObject.transform;
    }

    public IEnumerator manageSpawn()
    {
        while(true)
        {
            yield return new WaitUntil(() => respawn && gameObject.transform.childCount < maxSpawn);
            yield return new WaitForSeconds(spawnTime);

            spawn();
        }
    }
}
