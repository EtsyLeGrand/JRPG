using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<CharacterInstance> party = new List<CharacterInstance>();
    public List<BaseCharacter> startingParty = new List<BaseCharacter>();

    public void Start()
    {
        CreateParty();
    }

    public void CreateParty()
    {
        foreach (var character in startingParty)
        {
            CharacterInstance cInstance = new CharacterInstance(character);
            party.Add(cInstance);
        }
    }
    
    public void NewGame(int _index)
    {
        SaveData saveData = new SaveData()
        {
            index = _index,
            //party = new List<CharacterInstance>(),
            playerPosition = Vector2.zero,
            //sceneName = "Overworld"
        };
        
        SaveManager.Save(saveData);
        
        //Start Game
    }
}
