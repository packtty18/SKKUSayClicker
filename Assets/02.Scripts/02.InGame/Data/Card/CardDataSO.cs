using UnityEngine;

public enum ECardRank
{ 
    Common,
    Rare,
    Epic,
    Unique,
    Legendary,
}

public enum ECardSpecial
{
    Normal,
    Hologram,
    FirstEdition,
}


//현재 카드 자체의 데이터
public class CardDataSO : ScriptableObject
{
    public int Id;
    public CardData Data;

    public float CardDropRate;
    public float BaseSellPrice;

}

//오로지 카드를 묘사하는데 필요한 데이터
public class CardData
{
    public string Name;             //카드의 이름
    public ECardRank Rank;          //카드의 랭크 : 카드 배경의 색

    //대충 수치조절값
    public int BlueValue;           //파란색의 값
    public int YellowValue;         //노란색의 값
    public int RedValue;            //빨간색의값

    public Sprite FrontImage;       //몬스터의 이미지
}


