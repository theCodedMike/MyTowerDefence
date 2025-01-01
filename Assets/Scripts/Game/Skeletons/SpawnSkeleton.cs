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
        private readonly List<Transform> aliveSkeletons = new(32);


        private void Awake()
        {
            Instance = this;
        }


        // Start is called before the first frame update
        private void Start()
        {
            startPoint = GameObject.FindWithTag("StartPoint").transform;
            skeletonPrefabs = Resources.LoadAll<GameObject>("Prefabs/Skeletons");
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameManager.gameOver)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
                Spawn();
            
        }

        private void Spawn()
        {
            int idx = Random.Range(0, skeletonPrefabs.Length);
            GameObject obj = GameObjectPool.Instance.Get(
                skeletonPrefabs[idx].name, skeletonPrefabs[idx], startPoint.position, Quaternion.identity);
            Skeleton skeleton = obj.GetComponent<Skeleton>();
            skeleton.SetPrefabName(skeletonPrefabs[idx].name);
            skeleton.SetAgentEnabled(true);
            
            if (aliveSkeletons.Contains(obj.transform))
            {
                Debug.LogError($"生成了重复的Skeleton??? - {obj.transform}");
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
