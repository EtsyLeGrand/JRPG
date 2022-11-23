using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;

public class FightManager : Singleton<FightManager>
{
    [System.Serializable]
    public struct EnemyDifficulty
    {
        public EnemyCharacter enemy;
        public int[] difficulties;
    }

    [SerializeField] private List<EnemyDifficulty> possibleEnemies = new List<EnemyDifficulty>();
    [SerializeField] private List<PlayerCharacter> partyData = new List<PlayerCharacter>(); // à faire
    [SerializeField] private int[] difficultyRange;
    [SerializeField] private Vector3[] enemyPositions;
    [SerializeField] private Vector3[] playerPositions;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private GameObject playerParent;
    private List<GameObject> enemies = new List<GameObject>();
    private int difficulty;

    private const int MAX_ENEMIES_PER_FIGHT = 4;

    public override void Awake()
    {
        difficulty = Random.Range(difficultyRange[0], difficultyRange[1] + 1);
        Debug.Log("Difficulty: " + difficulty);
        InitFightData();
    }

    private void InitFightData()
    {
        // Enemies

        List<EnemyCharacter> possibleFightEnemies = new List<EnemyCharacter>();
        possibleEnemies.ForEach(enemyDifficulty =>
        {
            if (enemyDifficulty.difficulties.Contains(difficulty))
            {
                possibleFightEnemies.Add(enemyDifficulty.enemy);
            }
        });

        int currentFightEnemies = Random.Range(1, MAX_ENEMIES_PER_FIGHT + 1);
        Debug.Log("Enemies in fight: " + currentFightEnemies);
        for (int i = 0; i < currentFightEnemies; i++)
        { 
            EnemyCharacter enemyCharacter = RandomFromList(possibleFightEnemies);
            GameObject enemyObject = new GameObject(enemyCharacter.name);
            EnemyInstance inst = enemyObject.AddComponent<EnemyInstance>();
            SpriteRenderer renderer = enemyObject.AddComponent<SpriteRenderer>();

            renderer.sprite = enemyCharacter.sprite;
            renderer.sortingOrder = i + 10;

            inst.Init(enemyCharacter, i + 1);
            enemies.Add(enemyObject);

            enemyObject.transform.parent = enemyParent.transform;
            enemyObject.transform.position = enemyPositions[i];
            enemyObject.transform.localScale = enemyCharacter.spriteScale;
        }
    }

    private T RandomFromList<T>(List<T> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }

    private void Update()
    {
        
    }
}
