using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    public void OnFXOver()
    {
        EventManager.TriggerEvent("NewTurn", new Dictionary<string, object>());
        Destroy(gameObject);
    }
}
