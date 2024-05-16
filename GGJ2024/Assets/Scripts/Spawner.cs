using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<GameObject> _enemyList = new List<GameObject>();
    [SerializeField] PlayerController _player;
    [SerializeField] LaughMeter _laughMeter;
    private EnemyController enemyController;

    private bool _spawnOnLeftSide = true;
    public float spawnTime = 10.5f;
    public static int maxEnemyAmount = 20;
    public static int enemyCount = 0;

    void Start()
    {

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int randomIndex;
        randomIndex = Random.Range(0, _enemyList.Count);

        while (enemyCount < maxEnemyAmount)
        {
            GameObject randomEnemy = _enemyList[randomIndex];
            Vector3 randomPos;
            Quaternion randomRot = randomEnemy.transform.rotation;

            if (_spawnOnLeftSide)
            {
                randomPos = new Vector3(-11.88f, -3.57f, transform.position.z);
                enemyController = Instantiate(randomEnemy, randomPos, randomRot).GetComponent<EnemyController>();
            }
            else if (!_spawnOnLeftSide)
            {
                randomPos = new Vector3(11.79f, -3.57f, transform.position.z);
                enemyController = Instantiate(randomEnemy, randomPos, randomRot).GetComponent<EnemyController>();
            }
            
            if (enemyController != null && _player != null && _laughMeter != null)
            {
                enemyController.SetPlayer(_player);
                enemyController.SetLaughMeter(_laughMeter);
            }

            enemyCount++;
            randomIndex = Random.Range(0, _enemyList.Count);
            _spawnOnLeftSide = !_spawnOnLeftSide;
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
// notes - change the speed rate of spawns, and create a range where they can spawn on the Y so they don't all come from the same spot