using Game.Fsm;
using UnityEngine;
using Utils;

namespace Game.Towers
{
    public class Tower : MonoBehaviour
    {
        [Header("攻击距离")]
        public float attackDistance;
        [Header("开火点")]
        public Transform firePoint;
        [Header("激光发射音效")]
        public AudioClip laserClip;
        [Header("炮弹发射音效")]
        public AudioClip nonLaserClip;

        public TowerType type { get; private set; }
        public Level level { get; private set; }
        public int damage { get; private set; }
        public int price { get; private set; }

        private FsmSystem fsmSystem;
        private AudioSource audioSource;
        private string prefabName;
        
        // Start is called before the first frame update
        void Start()
        {
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

            IdleState idleState = new(fsmSystem, type, attackDistance);
            AttackState attackState = new(fsmSystem, type, level, attackDistance);
            attackState.audioSource = audioSource;
            attackState.laserClip = laserClip;
            attackState.nonLaserClip = nonLaserClip;

            // 从Idle状态转移到Attack状态
            idleState.AddTransition(Transition.TowerSeeSkeleton, StateId.TowerAttack);
            // 从Attack状态转移到Idle状态
            attackState.AddTransition(Transition.TowerLoseSkeleton, StateId.TowerIdle);

            fsmSystem.AddState(idleState); // 初始为Idle状态
            fsmSystem.AddState(attackState);
        }

        public void SetTowerInfo(TowerInfo info)
        {
            this.type = info.type;
            this.level = info.level;
            this.damage = info.damage;
            this.price = info.price;
        }
        
        public void SetPrefabName(string nameOfPrefab) => prefabName = nameOfPrefab;
        public string GetPrefabName() => prefabName;
    }



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

}
