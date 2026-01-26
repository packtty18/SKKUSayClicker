using UnityEngine;

[System.Serializable]
public class SPrinterSaveData : MonoBehaviour
{
    // 각 업그레이드의 현재 레벨 (0부터 시작)
    public int clickPowerLevel = 0;
    public int luckyHandLevel = 0;
    public int multiPressLevel = 0;
    public int autoPrinterLevel = 0;
    public int slotLevel = 0;

    // 현재 진행 중인 작업 상태 (게임 껐다 켜도 이어하기 위해)
    public float currentProgress = 0f;
}
