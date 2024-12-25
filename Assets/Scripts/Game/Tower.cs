using Game.Fsm;
using UnityEngine;

namespace Game
{
    public enum TowerType
    {
        LaserTower,
        KnifeTower,
        CannonTower,
    }

    public enum MoreType
    {
        Upgrade,
        Sell,
    }

    public class Tower : MonoBehaviour
    {
        [Header("当塔与僵尸的距离<=该距离时，塔开始攻击")]
        public float attackDistance;
        [Header("开火点")]
        public Transform firePoint;

        private TowerType type;

        private FsmSystem fsmSystem;


        // Start is called before the first frame update
        void Start()
        {
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

            IdleState idleState = new IdleState(fsmSystem, type, attackDistance);
            AttackState attackState = new AttackState(fsmSystem, type, attackDistance);

            // 从Idle状态转移到Attack状态
            idleState.AddTransition(Transition.SeeSkeleton, StateId.Attack);
            // 从Attack状态转移到Idle状态
            attackState.AddTransition(Transition.LoseSkeleton, StateId.Idle);

            fsmSystem.AddState(idleState); // 初始状态
            fsmSystem.AddState(attackState);
        }

        public void SetTowerType(TowerType type)
        {
            this.type = type;
        }
    }
}
