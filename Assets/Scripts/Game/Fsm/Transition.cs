namespace Game.Fsm
{
    public enum Transition
    {
        NoneTransition,

        TowerSeeSkeleton,  // 塔防看到敌人
        TowerLoseSkeleton, // 塔防看不到敌人

        RobotSeeSkeleton,  // 机器人看见敌人
        RobotLoseSkeleton, // 机器人看不到敌人
        RobotCatchUpSkeleton, // 机器人追上敌人
        RobotAwayFromSkeleton, // 机器人远离敌人
    }

    public enum StateId
    {
        NoneStateId,
        /*      TowerSeeSkeleton
         * Idle         ⇄         Attack
         *      TowerLoseSkeleton
         */
        TowerIdle,   // 塔防独处状态
        TowerAttack, // 塔防攻击状态


        /*
         *           RobotSeeSkeleton         RobotCatchUpSkeleton
         *   Patrol          ⇄         Chase            ⇄            Attack
         *           RobotLoseSkeleton        RobotAwayFromSkeleton          
         */
        RobotPatrol, // 机器人处于巡逻状态
        RobotChase,  // 机器人处于追逐状态
        RobotAttack, // 机器人处于攻击状态
    }
}
