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
        public EnemyCharacter enemyCharacter;
        public int difficulty;
    }

    public struct ActorLink
    {
        public CharacterInstance instance;
        public FightActor actor;
        public Vector3 position;
        public GameObject parent;
    }

    [SerializeField] private List<CharacterInstance> partyData = new List<CharacterInstance>();

    [SerializeField] private int[] difficultyRange;
    [SerializeField] private List<EnemyDifficulty> possibleEnemies;

    [SerializeField] private Vector3[] enemyPositions;
    [SerializeField] private Vector3[] playerPositions;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private GameObject playerParent;

    [SerializeField] private FightInfoCanvas infoCanvas;
    [SerializeField] private PlayerFightUI playerUI;

    [SerializeField] private AudioSource winSfx;
    [SerializeField] private AudioSource loseSfx;

    private CharacterInstance target = null;
    public bool canTarget = false;

    private List<FightActor> turnOrder = new List<FightActor>();
    private List<ActorLink> fighters = new List<ActorLink>();

    private int difficulty;
    private int assignedId = 0;

    private bool hasInstanceFinishedTurn = false;

    private const int MAX_ENEMIES_PER_FIGHT = 4;

    private void Start()
    {
        if (!GameManager.Instance.isDifficultyOverridden)
        {
            difficulty = Random.Range(difficultyRange[0], difficultyRange[1] + 1);
        }
        else
        {
            difficulty = 4;
        }
        
        Debug.Log("Difficulty: " + difficulty);
        InitFightData();
    }

    private void InitFightData()
    {
        GenerateEnemies();
        GeneratePlayers();

        StartCoroutine(StartFight());
    }

    private void GenerateEnemies()
    {
        int currentFightEnemyCount = Random.Range(1, MAX_ENEMIES_PER_FIGHT + 1);
        List<EnemyDifficulty> possibleEnemiesInCurrentFight = possibleEnemies.Where(x => x.difficulty <= difficulty).ToList();
        if (difficulty == 4)
        {
            currentFightEnemyCount = 1;
            possibleEnemiesInCurrentFight = possibleEnemies.Where(x => x.difficulty == difficulty).ToList();
        }

        Debug.Log("Enemies in fight: " + currentFightEnemyCount);
        for (int i = 0; i < currentFightEnemyCount; i++)
        {
            EnemyCharacter enemyCharacter = possibleEnemiesInCurrentFight[Random.Range(0, possibleEnemiesInCurrentFight.Count)].enemyCharacter;

            GameObject enemy = new GameObject(enemyCharacter.rpgClass.className);
            enemy.transform.parent = enemyParent.transform;
            enemy.transform.localScale = enemyCharacter.spriteScale;
            enemy.transform.localPosition = enemyPositions[i];

            SpriteRenderer renderer = enemy.AddComponent<SpriteRenderer>();
            renderer.sprite = enemyCharacter.sprite;
            renderer.sortingOrder = i + 1;

            BoxCollider2D collider2D = enemy.AddComponent<BoxCollider2D>();
            collider2D.offset = enemyCharacter.fightColliderOffset;
            collider2D.size = enemyCharacter.fightColliderSize;

            FightActor actor = enemy.AddComponent<FightActor>();
            actor.team = FightActor.ActorTeam.Enemy;
            actor.fightInfoCanvas = infoCanvas;
            actor.id = assignedId++;
            actor.initiative = (int)enemyCharacter.rpgClass.GetInitiative(1); // Les ennemis sont toujours level 1
            turnOrder.Add(actor);

            ActorLink linkedActor = new ActorLink()
            {
                instance = new CharacterInstance(enemyCharacter),
                actor = actor,
                position = enemyPositions[i],
                parent = enemyParent
            };

            fighters.Add(linkedActor);
        }
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
            fightActor.id = assignedId++;
            fightActor.fightInfoCanvas = infoCanvas;
            fightActor.initiative = partyData[i].currentStats.initiative;
            fightActor.team = FightActor.ActorTeam.Hero;
            turnOrder.Add(fightActor);

            ActorLink linkedActor = new ActorLink()
            {
                instance = partyData[i],
                actor = fightActor,
                position = playerPositions[i],
                parent = playerParent
            };

            fighters.Add(linkedActor);
        }
    }

    private IEnumerator StartFight()
    {
        turnOrder.Sort((e1, e2) => e2.initiative.CompareTo(e1.initiative));

        int count = 0;

        yield return new WaitForSeconds(2);
        while (!(IsEveryEnemyDead() || IsEveryPlayerDead()))
        {
            for (int i = 0; i < turnOrder.Count(); i++)
            {
                if (!IsEveryEnemyDead())
                {
                    CharacterInstance turnInstance = fighters.Where(x => x.actor.id == turnOrder[i].id).ToList()[0].instance;
                    if (turnInstance.currentStats.hp <= 0) { continue; }
                    Debug.Log("New Turn " + count + ": " + turnInstance.character.rpgClass.className);
                    hasInstanceFinishedTurn = false;
                    target = null;
                    if (turnOrder[i].team == FightActor.ActorTeam.Enemy)
                    {
                        // Manage enemy turns
                        StartCoroutine(EnemyTurn(turnInstance));
                    }
                    else
                    {
                        // Manage player turns
                        StartCoroutine(PlayerTurn(turnInstance));
                    }
                    while (!hasInstanceFinishedTurn) { yield return null; } // Wait for turn to finish
                }
                CheckDeaths();
                count++;
                Debug.Log("Turn ended");
            }
        }
        Debug.Log("Fight Ended");

        if (IsEveryEnemyDead())
        {
            if (difficulty == 4)
            {
                infoCanvas.bossBeatenSection.SetActive(true);
            }
            else
            {
                infoCanvas.winSection.SetActive(true);
                foreach (CharacterInstance character in partyData)
                {
                    character.currentStats.xp += fighters.Where(x => x.actor.team == FightActor.ActorTeam.Enemy).ToList().Select(x => x.instance.character.rpgClass.xpDrop).ToList().Sum();
                    PlayerCharacter player = (PlayerCharacter)character.character;
                    if (character.currentStats.xp >= player.GetXPNeededByLevel(character.currentStats.level))
                    {
                        character.currentStats.level++;
                    }
                }
            }
            winSfx.Play();
        }
        else
        {
            infoCanvas.loseSection.SetActive(true);
            loseSfx.Play();
        }
    }

    private IEnumerator EnemyTurn(CharacterInstance source)
    {
        Skill usedSkill = source.character.rpgClass.SkillUnlocks[Random.Range(0, source.character.rpgClass.SkillUnlocks.Count())].skill;

        if (usedSkill.target == Skill.Target.Enemy)
        {
            CharacterInstance target;
            if (difficulty != 4)
            {
                target = partyData.Where(x => x.currentStats.hp > 0).ToList()[Random.Range(0, partyData.Where(x => x.currentStats.hp > 0).ToList().Count())];
            }
            else
            {
                target = partyData.Where(x => x.currentStats.hp > 0).ToList().OrderBy(x => x.currentStats.hp).ToList()[0];
            }
            
            int damage = Random.Range((int)usedSkill.skillPowerRange.x, (int)usedSkill.skillPowerRange.y + 1);
            int crit = 1;
            if (Random.Range(1, 101) < source.currentStats.luck) { crit = 2; }
            target.currentStats.hp -= crit * damage;

            Debug.Log(target.character.rpgClass.className + " hit by " + usedSkill.skillName + " for " + crit * damage + "hp");

            
            if (usedSkill.fx != null)
            {
                ActorLink link = fighters.Where(x => x.instance == target).ToList()[0];
                Vector3 fxPosition = link.position;
                Transform parent = link.parent.transform;
                GameObject fx = Instantiate(usedSkill.fx, parent);
                fx.transform.localPosition = fxPosition;
                while (fx != null)
                {
                    yield return null;
                }
            }
        }
        else // Self
        {

        }
        hasInstanceFinishedTurn = true;
        yield return null;
    }

    private IEnumerator PlayerTurn(CharacterInstance source)
    {
        playerUI.Show();
        playerUI.SetPlayerSource(source);
        playerUI.IsInUse = true;

        while (playerUI.IsInUse) { yield return null; }

        playerUI.Hide();
        hasInstanceFinishedTurn = true;
    }

    public CharacterInstance GetMemberByIndex(int index)
    {
        return fighters.Where(x => x.actor.id == index).ToList()[0].instance;
    }

    private bool IsEveryEnemyDead()
    {
        List<int> enemyHp = fighters.Where(x => x.actor.team == FightActor.ActorTeam.Enemy).ToList().Select(x => x.instance.currentStats.hp).ToList();
        return enemyHp.Where(x => x > 0).ToList().Count == 0;
    }
    
    private bool IsEveryPlayerDead()
    {
        return partyData.All(x => x.currentStats.hp <= 0);
    }

    public void SetAttackTarget(int targetID)
    {
        target = fighters.Where(x => x.actor.id == targetID).ToList()[0].instance;
    }

    public IEnumerator WaitPlayerChooseTarget(CharacterInstance source, Skill usedSkill)
    {
        canTarget = true;
        playerUI.IsInUse = true;
        while (!IsTargetValidFromPlayer(target, usedSkill)) { yield return null; }
        canTarget = false;

        int critDmg = 1;
        if (Random.Range(1, 101) < source.currentStats.luck)
        {
            critDmg++;
        }

        // Attaque ici. Target est valide et skill aussi
        if (usedSkill.target == Skill.Target.Enemy)
        {
            target.currentStats.hp -= critDmg * Random.Range((int)usedSkill.skillPowerRange.x, (int)usedSkill.skillPowerRange.y + 1);
        }
        else
        {
            target.currentStats.hp += critDmg * Random.Range((int)usedSkill.skillPowerRange.x, (int)usedSkill.skillPowerRange.y + 1);
        }

        Debug.Log("Used " + usedSkill.skillName + " on " + target.character.rpgClass.className);

        // VFX
        if (usedSkill.fx != null)
        {
            ActorLink link = fighters.Where(x => x.instance == target).ToList()[0];
            Vector3 fxPosition = link.position;
            Transform parent = link.parent.transform;
            GameObject fx = Instantiate(usedSkill.fx, parent);
            fx.transform.localPosition = fxPosition;
            while (fx != null)
            {
                yield return null;
            }
        }

        playerUI.IsInUse = false;
        target = null;
    }

    private bool IsInstanceEnemy(CharacterInstance inst)
    {
        if (inst != null) Debug.Log(inst.character.rpgClass.className);
        return fighters.Where(x => x.instance == inst).ToList()[0].actor.team == FightActor.ActorTeam.Enemy;
    }

    private bool IsTargetValidFromPlayer(CharacterInstance target, Skill skill)
    {
        if (target == null) return false;
        if (skill.target == Skill.Target.Enemy && IsInstanceEnemy(target))
        {
            return true;
        }
        if (skill.target == Skill.Target.Self && !IsInstanceEnemy(target))
        {
            return true;
        }
        return false;
    }

    private void CheckDeaths()
    {
        foreach (CharacterInstance instance in fighters.Select(x => x.instance))
        {
            if (instance.currentStats.hp <= 0)
            {
                FightActor actor = fighters.Where(x => x.instance == instance).ToList().Select(x => x.actor).ToList()[0];
                actor.gameObject.GetComponent<SpriteRenderer>().color = new Color()
                {
                    a = 0
                };
                Destroy(actor.GetComponent<BoxCollider2D>());
            }
        }
    }

    public void BackToMenu()
    {
        SceneManager.ChangeScene("MainMenu");
    }

    public void BackToMap()
    {
        GameManager.Instance.party = partyData;
        SceneManager.ChangeScene("Map");
    }

    public void TurnPass()
    {
        playerUI.Hide();
        playerUI.IsInUse = false;
        hasInstanceFinishedTurn = true;
    }
}
