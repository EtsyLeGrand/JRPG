using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<CharacterInstance> party = new List<CharacterInstance>();
    public List<BaseCharacter> startingParty = new List<BaseCharacter>();

    private GameObject character;
    [SerializeField] private Vector2 defaultCharacterMapPosition;
    public Vector2 lastSavedPosition = Vector2.zero;

    public bool isDifficultyOverridden = false;
    private SaveData importedData;

    //private List<EnemyCharacter> possibleEnemies = new List<EnemyCharacter>();

    public Vector2 DefaultCharacterMapPosition { get => defaultCharacterMapPosition; }

    private void Start()
    {
        EventManager.StartListening("OnMapSceneLoaded", OnMapSceneLoaded);
    }

    private void OnMapSceneLoaded(Dictionary<string, object> _)
    {
        if (party.Count == 0)
        {
            CreateParty();
        }
        
        character = FindObjectOfType<MapCharacter>().gameObject;
        if (importedData != null)
        {
            character.transform.localPosition = importedData.playerPosition;
        }
        else
        {
            character.transform.position = defaultCharacterMapPosition;
        }

        if (lastSavedPosition != Vector2.zero)
        {
            character.transform.localPosition = lastSavedPosition;
        }
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
