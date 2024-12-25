using UnityEngine;

namespace Game.Fsm
{
    public enum Transition
    {
        NoneTransition,
        SeeSkeleton,  // 看到敌人
        LoseSkeleton, // 看不到敌人
    }

    public enum StateId
    {
        NoneStateId,
        Idle,   // 独处状态
        Attack, // 攻击状态
    }
}
