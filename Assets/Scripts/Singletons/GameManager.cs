using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<CharacterInstance> party = new List<CharacterInstance>();
    public List<BaseCharacter> startingParty = new List<BaseCharacter>();

    private GameObject character;
    [SerializeField] private Vector2 defaultCharacterMapPosition;

    private SaveData importedData;

    private List<EnemyCharacter> possibleEnemies = new List<EnemyCharacter>();

    public Vector2 DefaultCharacterMapPosition { get => defaultCharacterMapPosition; }

    private void Start()
    {
        EventManager.StartListening("OnMapSceneLoaded", OnMapSceneLoaded);
        EventManager.StartListening("OnFightSceneLoaded", OnFightSceneLoaded);
    }

    private void OnMapSceneLoaded(Dictionary<string, object> _)
    {
        CreateParty();
        character = FindObjectOfType<MapCharacter>().gameObject;
        if (importedData != null)
        {
            character.transform.localPosition = importedData.playerPosition;
        }
        else
        {
            character.transform.position = defaultCharacterMapPosition;
        }
    }

    private void OnFightSceneLoaded(Dictionary<string, object> _)
    {
        List<EnemyCharacter> enemies = new List<EnemyCharacter>();

        for (int i = 0; i <= 2; i++) // 2 ennemis
        {
            enemies.Add(possibleEnemies[Random.Range(0, possibleEnemies.Count)]);
        }

        FightManager.Instance.SetFightData(enemies);
    }


    public void CreateParty()
    {
        foreach (var character in startingParty)
        {
            CharacterInstance characterInstance = new CharacterInstance(character);
            party.Add(characterInstance);
        }
    }

    public void Load(SaveData data)
    {
        importedData = data;
    }
}
