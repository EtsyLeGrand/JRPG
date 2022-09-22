using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpriteRenderer : MonoBehaviour
{
    [SerializeField] Sprite pot;
    [SerializeField] Sprite equipment;
    [SerializeField] Sprite sign;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            GameObject obj = child.gameObject;
            if (obj.TryGetComponent(out SuperTiled2Unity.SuperObject superObj))
            {
                if (superObj.m_Type == "Pot" ||
                    superObj.m_Type == "Equip" ||
                    superObj.m_Type == "Sign")
                {
                    SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
                    renderer.drawMode = SpriteDrawMode.Sliced;
                    renderer.sortingOrder = 1;

                    switch (superObj.m_Type)
                    {
                        case "Pot": renderer.sprite = pot; break;
                        case "Equip": renderer.sprite = equipment; break;
                        case "Sign": renderer.sprite = sign; break;
                    }
                }
            }
        }
    }
}
