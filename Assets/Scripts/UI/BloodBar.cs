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
        void Start()
        {
            bloodBar.value = 1f;
            mainCamera = Camera.main;
            uiCamera = Camera.allCameras[1];
            transform.parent.GetComponent<Skeleton>().OnDamage = UpdateBloodBar;
            bloodBarPos = transform.parent.Find("BloodBarPos");
        }

        private void UpdateBloodBar(float value)
        {
            bloodBar.value = value;
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
