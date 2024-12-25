using UnityEngine;
using DG.Tweening;

namespace Game.Fsm
{
    public class IdleState : FsmState
    {
        private TowerType type;

        public IdleState(FsmSystem fsm, TowerType type) : base(fsm)
        {
            stateId = StateId.Idle;
            this.type = type;
        }

        /// <summary>
        /// 静止状态下执行的逻辑
        /// </summary>
        /// <param name="obj"></param>
        public override void CurrStateAction(GameObject obj)
        {
            if (type == TowerType.LaserTower)
            {
                obj.transform.Rotate(Vector3.up, 1);
            } else if (type == TowerType.CannonTower)
            {
                Vector3 targetAngle = obj.transform.localEulerAngles + new Vector3(0, 90, 0);
                //obj.transform.DOLocalRotate(targetAngle, 1).SetAs(tParam).SetLoops(-1, LoopType.Yoyo);
            }
        }

        /// <summary>
        /// 转移到下一状态(攻击状态)的判断逻辑
        /// </summary>
        /// <param name="obj"></param>
        public override void NextStateAction(GameObject obj)
        {
            
        }
    }
}
