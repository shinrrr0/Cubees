using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataSpace;

public class ProgressCheck : MonoBehaviour
{
    public List<GameObject> buttons;
    private JsonFile save = new JsonFile();
    private PlayerData data;

    void Start()
    {
        data = save.getPlayerData();
        for (int i = 0; i < buttons.Count; ++i) {
            if (data.doneLevels.Contains(buttons[i].name)) {
                buttons[i].SetActive(true);
            }
        }
    }

}
