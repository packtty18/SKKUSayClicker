using UnityEngine;

public enum EUpgradeType
{
    ProgressPerClick,   //클릭당 생산속도 증가
    CritRate,           //크리티컬 클릭 확률 증가
    CritValue,          //크리티컬 클릭 증가량 증가           
    ProductTimeReduce,  //프린터의 생산속도 감소량 증가
    ProgressPerTime,    //프린터의 자동생산속도 증가

    Count
}
