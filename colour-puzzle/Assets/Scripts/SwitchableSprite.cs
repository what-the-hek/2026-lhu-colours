using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteState
{
    [SerializeField]
    public List<GameObject> spritesToHide;
    [SerializeField]
    public List<GameObject> spritesToShow;
}
public class SwitchableSprite : MonoBehaviour
{

    public string currentState;

    [SerializeField]
    public Dictionary<string, SpriteState> StringToSpriteStateMap;

    public void SwitchSprite(string condition)
    {
        if (StringToSpriteStateMap.ContainsKey(condition))
        {
            foreach(var sprite in StringToSpriteStateMap[condition].spritesToHide)
            {
                sprite.gameObject.SetActive(false);
            }
            foreach (var sprite in StringToSpriteStateMap[condition].spritesToShow)
            {
                sprite.gameObject.SetActive(true);
            }
        }
    }
}