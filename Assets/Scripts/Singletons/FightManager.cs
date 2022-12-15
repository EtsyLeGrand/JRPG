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
    [SerializeField] private List<CharacterInstance> partyData = new List<CharacterInstance>();
    [SerializeField] private int[] difficultyRange;
    [SerializeField] private Vector3[] enemyPositions;
    [SerializeField] private Vector3[] playerPositions;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private GameObject playerParent;
    [SerializeField] private FightInfoCanvas infoCanvas;
    [SerializeField] private List<EnemyInstance> enemies = new List<EnemyInstance>();

    private List<FightActor> turnOrder = new List<FightActor>();
    private int currentTurn = -1;

    private int difficulty;

    private const int MAX_ENEMIES_PER_FIGHT = 4;

    private void Start()
    {
        EventManager.StartListening("NewTurn", NewTurn);
        difficulty = Random.Range(difficultyRange[0], difficultyRange[1] + 1);
        Debug.Log("Difficulty: " + difficulty);
        InitFightData();
    }

    private void InitFightData()
    {
        GenerateEnemies();
        GeneratePlayers();

        StartFight();
    }

    private T RandomFromList<T>(List<T> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }

    private void GeneratePlayers()
    {
        partyData = GameManager.Instance.party.Where(x => x.currentStats.hp > 0).ToList();
        for (int i = 0; i < partyData.Count(); i++)
        {
            GameObject player = new GameObject("Player " + i);
            player.transform.parent = playerParent.transform;
            player.transform.localPosition = playerPositions[i];
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

            SpriteRenderer renderer = player.AddComponent<SpriteRenderer>();
            renderer.sprite = partyData[i].character.sprite;
            renderer.sortingOrder = i + 1;

            Animator animator = player.AddComponent<Animator>();
            animator.runtimeAnimatorController = partyData[i].character.animatorController;

            BoxCollider2D collider2D = player.AddComponent<BoxCollider2D>();
            collider2D.offset = partyData[i].character.fightColliderOffset;
            collider2D.size = partyData[i].character.fightColliderSize;

            FightActor fightActor = player.AddComponent<FightActor>();
            fightActor.fighterIndex = i;
            fightActor.fightInfoCanvas = infoCanvas;
            fightActor.initiative = partyData[i].currentStats.initiative;
            turnOrder.Add(fightActor);
        }
    }

    private void GenerateEnemies()
    {
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
            enemyObject.transform.parent = enemyParent.transform;
            enemyObject.transform.position = enemyPositions[i];
            enemyObject.transform.localScale = enemyCharacter.spriteScale;

            FightActor fightActor = enemyObject.AddComponent<FightActor>();
            fightActor.fighterIndex = i;
            fightActor.initiative = enemyCharacter.speed;
            turnOrder.Add(fightActor);

            SpriteRenderer renderer = enemyObject.AddComponent<SpriteRenderer>();
            renderer.sprite = enemyCharacter.sprite;
            renderer.sortingOrder = i + 10;

            EnemyInstance inst = enemyObject.AddComponent<EnemyInstance>();
            inst.Init(enemyCharacter, i + 1);
            enemies.Add(inst);
            inst.linkedCharacter = enemyCharacter;
        }
    }

    private void StartFight()
    {
        turnOrder.Sort((e1, e2) => e2.initiative.CompareTo(e1.initiative));
        EventManager.TriggerEvent("NewTurn", new Dictionary<string, object>());
    }

    public CharacterInstance GetMemberByIndex(int index)
    {
        return partyData[index];
    }

    private void NewTurn(Dictionary<string, object> args)
    {
        currentTurn++;

        if (isEveryEnemyDead())
        {
            // win fight
        }
        else if (isEveryPlayerDead())
        {
            // lose fight
        }

        Debug.Log("Enemy Turn");

        // Enemy Turn
        if (turnOrder[currentTurn].gameObject.TryGetComponent(out EnemyInstance enemy))
        {
            EnemyCharacter enemyCharacter = enemy.linkedCharacter;
            Skill skill = enemyCharacter.rpgClass.SkillUnlocks[Random.Range(0, enemyCharacter.rpgClass.SkillUnlocks.Count)].skill;
            int targetIndex = Random.Range(0, partyData.Where(x => x.currentStats.hp > 0).ToList().Count);
            CharacterInstance target = partyData[targetIndex];
            Transform targetTransform = playerParent.transform.GetChild(targetIndex);

            Dictionary<string, object> skillData = new Dictionary<string, object>()
            {
                { "skill", skill },
                { "target", target },
                { "targetTransform", targetTransform }
            };
            
            EventManager.TriggerEvent("EnemyAttack", skillData);
        }
        // Player Turn
        else
        {
            Debug.Log("Player Turn");
            // FAIRE LE UI DE COMBAT
        }
    }

    private bool isEveryEnemyDead()
    {
        return enemies.All(x => x.currentStats.hp <= 0);
    }
    
    private bool isEveryPlayerDead()
    {
        return partyData.All(x => x.currentStats.hp <= 0);
    }
}
