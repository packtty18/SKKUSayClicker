using UnityEngine;


//프린터의 생산 설정
public class PrinterConfigSO : ScriptableObject
{
    //카드팩의 생성 시간
    public float defaultProductionTime = 30f;      
    //해당 프린터가 생성할 카드팩
    public ECardPackType CardPackType = ECardPackType.None;
}