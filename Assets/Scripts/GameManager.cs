using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Queue<CharacterInstance> party = new Queue<CharacterInstance>();
    public List<BaseCharacter> startingParty = new List<BaseCharacter>();

    public void CreateParty()
    {
        foreach (var character in startingParty)
        {
            CharacterInstance characterInstance = new CharacterInstance(character);
            party.Enqueue(characterInstance);
        }
    }
}
