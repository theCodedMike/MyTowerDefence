using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkeleton : MonoBehaviour
{
    [Header("生成位置")]
    public Transform start;

    private GameObject skeletonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        skeletonPrefab = Resources.Load<GameObject>("Prefabs/Skeletons/CuteSkeleton");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        Instantiate(skeletonPrefab, start.position, Quaternion.identity);
    }
}
