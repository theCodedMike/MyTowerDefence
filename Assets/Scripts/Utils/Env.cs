using UnityEngine;

namespace Utils
{
    public class Env : MonoBehaviour
    {
        public static Env Instance;
        private void Awake()
        {
            Instance = this;
        }

        // 获取环境中孩子的Transform
        public Transform GetChildTransform(string childName)
        {
            if (string.IsNullOrEmpty(childName))
            {
                Debug.LogError($"参数childName为空...");
                return null;
            }

            Transform childTrans = this.transform.Find(childName);
            if (childTrans is null)
            {
                Debug.LogError("childTrans is null...");
                return null;
            }

            return childTrans;
        }
    }
}
