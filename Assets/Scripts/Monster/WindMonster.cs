using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMonster : MonsterBase
{


#if UNITY_EDITOR

    public void Test_WindMonster_Attack()
    {
        Debug.Log($"WindMonster가 Attack 상태로 전환");
        MonsterState = MonsterState.Attack;
    }

#endif
}
