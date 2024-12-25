using UnityEngine;

namespace Game.Fsm
{
    public class AttackState : FsmState
    {
        private TowerType type;
        public AttackState(FsmSystem fsm, TowerType type) : base(fsm)
        {
            stateId = StateId.Attack;
            this.type = type;
        }

        /// <summary>
        /// 攻击状态下执行的逻辑
        /// </summary>
        /// <param name="obj"></param>
        public override void CurrStateAction(GameObject obj)
        {
            
        }

        /// <summary>
        /// 转移到下一状态(静止状态)的判断逻辑
        /// </summary>
        /// <param name="obj"></param>
        public override void NextStateAction(GameObject obj)
        {
            
        }
    }
}
