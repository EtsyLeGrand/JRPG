using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<CharacterInstance> party = new List<CharacterInstance>();
    public List<BaseCharacter> startingParty = new List<BaseCharacter>();
    public static Vector2 characterMapPosition;
    [SerializeField] private Vector2 defaultCharacterMapPosition;

    public void Start()
    {
        characterMapPosition = defaultCharacterMapPosition;
        CreateParty();
        EventManager.StartListening("RegisterNewMapPosition", SetNewMapCharacterPosition);
    }

    public void CreateParty()
    {
        foreach (var character in startingParty)
        {
            CharacterInstance cInstance = new CharacterInstance(character);
            party.Add(cInstance);
        }
    }
    
    public void NewGame(int index)
    {
        SaveData saveData = new SaveData()
        {
            index = index,
            playerPosition = defaultCharacterMapPosition,
            //party = new List<CharacterInstance>(),
            //sceneName = "Overworld"
        };
        
        SaveManager.Save(saveData);

        characterMapPosition = saveData.playerPosition;
        
        //Start Game
    }

    public void LoadGame(SaveData saveData)
    {
        characterMapPosition = saveData.playerPosition;
        if (!SceneManager.IsSceneLoaded("Map"))
            SceneManager.ChangeScene("Map");
    }

    private static void SetNewMapCharacterPosition(Dictionary<string, object> args)
    {
        args.TryGetValue("x", out object x);
        args.TryGetValue("y", out object y);

        characterMapPosition.x = (float)x;
        characterMapPosition.y = (float)y;
        Debug.Log("SAVED POS: " + characterMapPosition);
    }
}
