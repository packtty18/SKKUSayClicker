using MoreMountains.Feedbacks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] EProductType _theme;
    [SerializeField] ECardRank _rank;
    [SerializeField] ECardSpecial _spcial;

    [SerializeField] private Image _rankFrame;          //등급에 따른 색변화
    [SerializeField] private Image _background;         //카드의 테마에 따른 색변화
    [SerializeField] private Image _mainImage;          //캐릭터의 색변화

    [SerializeField] private TextMeshPro _blueText;
    [SerializeField] private TextMeshPro _yellowText;
    [SerializeField] private TextMeshPro _redText;

    public void SetCardData(CardData data)
    {
        _rank = data.Rank;

        _mainImage.sprite = data.FrontImage;
        _rankFrame.color = SetRankColor(_rank);

        _blueText.text = data.BlueValue.ToString();
        _yellowText.text = data.YellowValue.ToString();
        _redText.text = data.RedValue.ToString();
            
    }

    public void SetCardSpecial(ECardSpecial special= ECardSpecial.Normal)
    {
        //노말 => 없음
        //홀로그램 => 홀로그램 이미지 띄우기
        //퍼스트 에디션 => 움직이는 그림으로 변환
    }

    private Color SetRankColor(ECardRank rank)
    {
        //커먼 => 흰색
        //레어 => 파란색
        //에픽 => 보라색
        //유니크 => 노란색
        //레전더리 => 녹색

        switch (rank)
        {
            case ECardRank.Common:
                return Color.white;
            case ECardRank.Rare:
                return Color.blue;
            case ECardRank.Epic:
                return Color.magenta;
            case ECardRank.Unique:
                return Color.yellow;
            case ECardRank.Legendary:
                return Color.green;
        }

        return Color.white;
    }
}
