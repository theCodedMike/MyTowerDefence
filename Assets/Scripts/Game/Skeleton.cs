using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class Skeleton : MonoBehaviour
    {
        [Header("总血量(保持不变)")]
        public int totalHp;

        public static Action<float> OnDamage;

        private int remainingHp; // 失去的血量
        private Transform end;
        private NavMeshAgent agent;
        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            end = GameObject.FindWithTag("EndPoint").transform;
            animator = GetComponent<Animator>();
            remainingHp = totalHp;
        }

        // Update is called once per frame
        void Update()
        {
            if (agent.enabled)
                agent.SetDestination(end.position);

            if (Input.GetKeyDown(KeyCode.D))
            {
                Damage();
            }
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
                //animator.SetTrigger("Dead");
            }

            OnDamage((float)remainingHp / totalHp);
        }
    }
}
