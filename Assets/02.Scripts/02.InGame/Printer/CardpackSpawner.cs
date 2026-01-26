using UnityEngine;

public class CardpackSpawner : MonoBehaviour
{
    public GameObject PackPrefab;

    //스폰 확률에 따라 다른 카드팩을 스폰
    public void SpawnPack()
    {
        CardpackObject pack = Instantiate(PackPrefab, transform.position, Quaternion.identity).GetComponent<CardpackObject>();

        pack.Initialize();
    }

}
