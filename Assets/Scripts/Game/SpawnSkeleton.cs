using UnityEngine;

namespace Game
{
    public class SpawnSkeleton : MonoBehaviour
    {
        [Header("生成位置")]
        public Transform start;

        private GameObject[] skeletonPrefabs;

        // Start is called before the first frame update
        void Start()
        {
            skeletonPrefabs = Resources.LoadAll<GameObject>("Prefabs/Skeletons");
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
            int idx = Random.Range(0, skeletonPrefabs.Length);

            Instantiate(skeletonPrefabs[idx], start.position, Quaternion.identity);
        }
    }
}
