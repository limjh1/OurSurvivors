using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnRadius = 10.0f;
    public GameObject mosnterPrefab;
    public Transform player;
    public float spawnInterval = 3.0f;

    public float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnMonsterAtEdge();
            timer = 0f;
        }
    }

    void SpawnMonsterAtEdge()
    {
        Vector3 spawnPos = GetRandomPointOnCircleEdge(player.position, spawnRadius);
        GameObject monster = Instantiate(mosnterPrefab, spawnPos, Quaternion.identity); // 회전값 X
        monster.GetComponent<Monster_Movement>()?.Init(player);
    }

    Vector3 GetRandomPointOnCircleEdge(Vector3 center, float radius)
    {
        // 중심점 기준, 반지름 거리만큼 떨어진 원의 가장자리에 랜덤하게 스폰
        float angle = Random.Range(0.0f, Mathf.PI * 2f);
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        return new Vector3(center.x + x, center.y, center.z + z);
    }
}
