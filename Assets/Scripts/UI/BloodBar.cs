using Game.Skeletons;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BloodBar : MonoBehaviour
    {
        [Header("血条")]
        public Slider bloodBar;

        private Transform bloodBarPos;
        private Camera mainCamera;
        private Camera uiCamera;

        // Start is called before the first frame update
        private void Start()
        {
            mainCamera = Camera.main;
            uiCamera = Camera.allCameras[1];
            bloodBarPos = transform.parent.Find("BloodBarPos");
            bloodBar.value = 1f;
        }

        // 更新血量值
        public void UpdateValue(float value)
        {
            bloodBar.value = value;
        }

        // 消失
        public void Disappear()
        {
            gameObject.SetActive(false);
        }

        // 重置数据
        public void ResetData()
        {
            gameObject.SetActive(true);
            bloodBar.value = 1f;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 pos = mainCamera.WorldToViewportPoint(bloodBarPos.position);
            Vector3 uiPos = uiCamera.ViewportToWorldPoint(pos);
            transform.position = uiPos;

            transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        }
    }
}
