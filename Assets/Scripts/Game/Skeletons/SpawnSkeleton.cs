using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Game.Skeletons
{
    public class SpawnSkeleton : MonoBehaviour
    {
        public static SpawnSkeleton Instance;

        private Transform startPoint;
        private GameObject[] skeletonPrefabs;
        private List<Transform> aliveSkeletons = new(32);


        private void Awake()
        {
            Instance = this;
        }


        // Start is called before the first frame update
        private void Start()
        {
            //startPoint = GameObject.FindWithTag("StartPoint").transform;
            startPoint = Env.Instance.GetChildTransform("Points").Find("Start");
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
            //int idx = Random.Range(0, skeletonPrefabs.Length);
            int idx = 0;
            GameObject obj = GameObjectPool.Instance.Get(
                skeletonPrefabs[idx].name, skeletonPrefabs[idx], startPoint, Quaternion.identity);
            obj.GetComponent<Skeleton>().SetPrefabName(skeletonPrefabs[idx].name);

            //GameObject obj = Instantiate(skeletonPrefabs[idx], start.position, Quaternion.identity);
            if (aliveSkeletons.Contains(obj.transform))
            {
                Debug.LogError($"生成了重复的Skeleton？？？{obj.transform}");
                throw new DuplicateNameException("重复的Transform");
            }

            aliveSkeletons.Add(obj.transform);
        }

        // 当僵尸死亡时，移除它
        public void RemoveSkeleton(Transform trans)
        {
            if (!aliveSkeletons.Remove(trans))
            {
                Debug.LogError($"移除Skeleton的Transform失败：{trans.gameObject}");
            }
        }

        // 获取所有僵尸位置
        public List<Transform> GetAliveSkeletons() => aliveSkeletons;
    }
}
