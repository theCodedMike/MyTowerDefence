using System;
using UnityEngine;
using Utils;

namespace Game.Bullets
{
    public class Bullet : MonoBehaviour
    {
        [Header("移动速度")]
        public float moveSpeed;
        [Header("击中物体的特效")]
        public GameObject HitSplash;

        private Rigidbody rb;
        public int Damage { get; set; }
        public string PrefabName { get; set; } // 预制体的名字，用作对象池的key
        
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            Invoke(nameof(DestroyBullet), 5f);
        }

        public void SetVelocity(Vector3 dir)
        {
            if (rb == null)
                rb = GetComponent<Rigidbody>();

            rb.velocity = dir * moveSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
                Invoke(nameof(DestroyBullet), 1f);
        }

        private void DestroyBullet()
        {
            if(gameObject.activeSelf)
                GameObjectPool.Instance.Put(PrefabName, gameObject);
        }
    }
}
