using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] Node[] levelButtons;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached",1);
        print(levelReached);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if(i+1 > levelReached)
            {
                levelButtons[i].ToggleColor();
            }
        }
    }
}
