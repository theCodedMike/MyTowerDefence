using System;
using UI;
using MyBullet = Game.Bullets.Bullet;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Game.Skeletons
{
    public class Skeleton : MonoBehaviour
    {
        [Header("总血量(保持不变)")]
        public int totalHp;

        public static Action<int> OnDeath; // 死亡事件
        public static Action OnArriveEnd; // 到达目的地事件

        private static readonly int Dead = Animator.StringToHash("Dead");
        
        private string prefabName; // 预制体的名字，用作对象池存储时的key
        private int remainingHp; // 剩余的血量
        private Transform endPoint;
        private bool arriveEnd; // 到达终点
        private NavMeshAgent agent;
        private Animator animator;
        private Rigidbody rb;
        private AudioSource audioSource;
        private BloodBar bloodBar;

        private void OnEnable()
        {
            agent = GetComponent<NavMeshAgent>();
            endPoint = GameObject.FindWithTag("EndPoint").transform;
            animator = GetComponent<Animator>();
            remainingHp = totalHp;
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            bloodBar = transform.Find("Canvas").GetComponent<BloodBar>();
        }

        // Update is called once per frame
        void Update()
        {
            if (agent.enabled)
                agent.SetDestination(endPoint.position);

            if (!arriveEnd)
                CheckArriveDest();
        }


        private void CheckArriveDest()
        {
            if ((transform.position - endPoint.position).sqrMagnitude <= 2)
            {
                arriveEnd = true;
                OnArriveEnd();
            }
        }

        public void SetPrefabName(string nameOfPrefab)
        {
            if (string.IsNullOrEmpty(nameOfPrefab))
                throw new ArgumentNullException("参数nameOfPrefab为空");
            
            prefabName = nameOfPrefab;
        }

        public void SetAgentEnabled(bool value) => agent.enabled = value;
        

        /// <summary>
        /// 受伤
        /// </summary>
        /// <param name="loss">失去的血量值</param>
        public void Damage(int loss = 30)
        {
            if (remainingHp == 0)
                return;

            remainingHp -= loss;
            if (remainingHp <= 0)
            {
                audioSource.Play();
                // 僵尸死亡
                remainingHp = 0;
                agent.enabled = false;
                SpawnSkeleton.Instance.RemoveSkeleton(transform);
                animator.SetTrigger(Dead);
                OnDeath(10);
            }

            bloodBar.UpdateValue((float)remainingHp / totalHp);
        }

        // 动画触发事件
        private void StartSinking()
        {
            bloodBar.Disappear();
            rb.useGravity = true;
            Invoke(nameof(DestroySkeleton), 3f);
        }

        private void DestroySkeleton()
        {
            GameObjectPool.Instance.Put(prefabName, gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
                Damage(other.GetComponent<MyBullet>().Damage);
        }

        private void OnDisable()
        {
            rb.useGravity = false;
            bloodBar.ResetData();
        }
    }
}
