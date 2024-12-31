using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Utils
{
    public class GameObjectPool
    {
        private static GameObjectPool instance;

        public static GameObjectPool Instance
        {
            get
            {
                instance ??= new GameObjectPool();
                return instance;
            }
        }

        private Dictionary<string, Queue<GameObject>> objMap;

        private GameObjectPool()
        {
            objMap = new Dictionary<string, Queue<GameObject>>(32);
        }

        /// <summary>
        /// 获取游戏对象
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="prefab">预制件</param>
        /// <param name="position">位置</param>
        /// <param name="rotation">旋转</param>
        /// <returns></returns>
        public GameObject Get(string key, GameObject prefab, Transform genPoint, Quaternion rotation)
        {
            GameObject gameObject;

            if (objMap.ContainsKey(key) && objMap[key].Count > 0)
            {
                gameObject = objMap[key].Dequeue();
                gameObject.SetActive(true);
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
                gameObject.transform.position = genPoint.position;
                gameObject.transform.rotation = rotation;
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                Debug.Log("position: " + genPoint.position);
                Debug.Log("gb position: " + gameObject.transform.position);
                return gameObject;
            }

            gameObject = Object.Instantiate(prefab, genPoint.position, rotation);
            return gameObject;
        }

        /// <summary>
        /// 回收游戏对象
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">待回收的游戏对象</param>
        public void Put(string key, GameObject obj)
        {
            if (!objMap.ContainsKey(key))
            {
                objMap.Add(key, new Queue<GameObject>(8));
            }

            obj.SetActive(false);
            objMap[key].Enqueue(obj);
        }
    }
}
