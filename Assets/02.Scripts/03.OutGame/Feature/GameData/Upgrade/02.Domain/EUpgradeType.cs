using UnityEngine;

public enum EUpgradeType
{
    ClickProgress,   //클릭당 생산속도 증가
    CriticalChance,           //크리티컬 클릭 확률 증가
    CriticalMultiplier,          //크리티컬 클릭 증가량 증가           
    ProgressTarget,  //프린터의 생산속도 감소량 증가
    ProgressByTime,    //프린터의 자동생산속도 증가

    Count
}
