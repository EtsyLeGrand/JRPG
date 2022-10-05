using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour
{
    [SerializeField] private Button backButton;

    public void OnBackButtonPressed()
    {
        SceneManager.ChangeScene("Map");
    }
}
