using System.Collections.Generic;
using UnityEngine;

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
        public GameObject Get(string key, GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject gameObject;

            if (objMap.ContainsKey(key) && objMap[key].Count > 0)
            {
                gameObject = objMap[key].Dequeue();
                gameObject.SetActive(true);
            }
            else
            {
                gameObject = Object.Instantiate(prefab);
            }

            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;

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
                objMap.Add(key, new Queue<GameObject>(10));
            }

            obj.SetActive(false);
            objMap[key].Enqueue(obj);
        }
    }
}
