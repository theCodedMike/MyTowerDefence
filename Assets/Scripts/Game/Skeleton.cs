using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class Skeleton : MonoBehaviour
    {
        [Header("总血量(保持不变)")]
        public int totalHp;

        public static Action<float> OnDamage; // 受到伤害事件
        public static Action<int> OnDeath; // 死亡事件

        private static readonly int Dead = Animator.StringToHash("Dead");

        private int remainingHp; // 剩余的血量
        private Transform end;
        private NavMeshAgent agent;
        private Animator animator;
        private Rigidbody rigidbody;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            end = GameObject.FindWithTag("EndPoint").transform;
            animator = GetComponent<Animator>();
            remainingHp = totalHp;
            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (agent.enabled)
                agent.SetDestination(end.position);
        }


        /// <summary>
        /// 受伤
        /// </summary>
        /// <param name="loss">失去的血量值</param>
        public void Damage(int loss = 30)
        {
            remainingHp -= loss;
            if (remainingHp <= 0)
            {
                // 僵尸死亡
                remainingHp = 0;
                agent.enabled = false;
                animator.SetTrigger(Dead);
                SpawnSkeleton.Instance.RemoveSkeleton(this.transform);
                OnDeath(10);
            }

            OnDamage((float)remainingHp / totalHp);
        }

        private void StartSinking()
        {
            transform.Find("Canvas").gameObject.SetActive(false);
            //transform.GetComponent<CapsuleCollider>().isTrigger = true;
            rigidbody.useGravity = true;
            Invoke(nameof(Destroy), 1.5f);
        }

        private void Destroy()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                Damage();
            }
        }
    }
}
