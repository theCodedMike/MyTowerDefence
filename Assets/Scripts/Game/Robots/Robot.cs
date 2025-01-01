using Game.Fsm;
using UnityEngine;

namespace Game.Robots
{
    public class Robot : MonoBehaviour
    {
        [Header("攻击距离")]
        public float attackDistance;
        [Header("子弹生成位置")]
        public Transform[] bulletSpawnPoint;
        [Header("子弹预制体")]
        public GameObject bulletPrefab;

        private FsmSystem fsmSystem;

        private Animator animator;

        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            InitFsm();
        }

        // Update is called once per frame
        void Update()
        {
            fsmSystem.UpdateState(this.gameObject);
        }

        private void InitFsm()
        {
            fsmSystem = new FsmSystem();

            PatrolState patrolState = new(this.gameObject.name, attackDistance, fsmSystem);
            AttackState attackState = new(animator, attackDistance, fsmSystem);
            attackState.bulletSpawnPoint = this.bulletSpawnPoint;
            attackState.bulletPrefab = this.bulletPrefab;
            attackState.audioSource = this.audioSource;

            // 从巡逻状态转移到攻击状态
            patrolState.AddTransition(Transition.RobotSeeSkeleton, StateId.RobotAttack);
            // 从攻击状态转移到巡逻状态
            attackState.AddTransition(Transition.RobotLoseSkeleton, StateId.RobotPatrol);

            fsmSystem.AddState(patrolState); // 初始为巡逻状态
            fsmSystem.AddState(attackState);
        }

        private void Shoot() { }
    }
}
