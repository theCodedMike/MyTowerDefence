using System;
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

        public Action<float> OnDamage; // 受到伤害事件
        public static Action<int> OnDeath; // 死亡事件
        public static Action OnArriveEnd; // 到达目的地时间

        private static readonly int Dead = Animator.StringToHash("Dead");


        private string prefabName; // 预制体的名字
        private int remainingHp; // 剩余的血量
        private Transform end;
        private bool arriveEnd; // 到达终点
        private NavMeshAgent agent;
        private Animator animator;
        private Rigidbody rb;
        private AudioSource audioSource;


        private void OnEnable()
        {
            print("this is OnEnable....");
            agent = GetComponent<NavMeshAgent>();
            end = GameObject.FindWithTag("EndPoint").transform;
            animator = GetComponent<Animator>();
            remainingHp = totalHp;
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (agent.enabled)
                agent.SetDestination(end.position);

            if (!arriveEnd)
            {
                CheckArriveDest();
            }
        }


        private void CheckArriveDest()
        {
            if ((transform.position - end.position).sqrMagnitude <= 2)
            {
                arriveEnd = true;
                OnArriveEnd();
            }
        }

        public void SetPrefabName(string prefabName)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                throw new ArgumentNullException("参数prefabName为空");
            }

            this.prefabName = prefabName;
        }

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
                SpawnSkeleton.Instance.RemoveSkeleton(this.transform);
                animator.SetTrigger(Dead);
                OnDeath(10);
            }

            OnDamage((float)remainingHp / totalHp);
        }

        // 动画触发事件
        private void StartSinking()
        {
            transform.Find("Canvas").gameObject.SetActive(false);
            //transform.GetComponent<CapsuleCollider>().isTrigger = true;
            rb.useGravity = true;
            Invoke(nameof(DestroySkeleton), 1f);
        }

        private void DestroySkeleton()
        {
            //Destroy(this.gameObject);
            GameObjectPool.Instance.Put(prefabName, this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                Damage(other.GetComponent<MyBullet>().Damage);
            }
        }

        private void OnDisable()
        {
            rb.useGravity = false;
            transform.Find("Canvas").gameObject.SetActive(true);
        }
    }
}
