using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Skeletons
{
    public class SpawnSkeleton : MonoBehaviour
    {
        [Header("生成位置")]
        public Transform start;

        public static SpawnSkeleton Instance;



        private GameObject[] skeletonPrefabs;

        private List<Transform> aliveSkeletons = new(32);


        private void Awake()
        {
            Instance = this;
        }


        // Start is called before the first frame update
        private void Start()
        {
            skeletonPrefabs = Resources.LoadAll<GameObject>("Prefabs/Skeletons");
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.gameOver)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            int idx = Random.Range(0, skeletonPrefabs.Length);

            GameObject obj = Instantiate(skeletonPrefabs[idx], start.position, Quaternion.identity);
            if (aliveSkeletons.Contains(obj.transform))
            {
                Debug.LogError($"生成了重复的Skeleton？？？{obj.transform}");
                throw new DuplicateNameException("重复的Transform");
            }

            aliveSkeletons.Add(obj.transform);
        }

        // 当僵尸死亡时，移除它
        public void RemoveSkeleton(Transform transform)
        {
            if (!aliveSkeletons.Remove(transform))
            {
                Debug.LogError($"移除Skeleton的Transform失败：{transform.gameObject}");
            }
        }

        // 获取所有僵尸位置
        public List<Transform> GetAliveSkeletons() => aliveSkeletons;
    }
}
