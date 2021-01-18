using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MoveLearnMenu : MonoBehaviour
{
    // UI Elements
    GameObject rootUI;

    GameObject monSprite;
    List<GameObject> moveLearnButtons;

    // Underlying Model
    MonEntity activeMon;


    // Start is called before the first frame update
    void Start()
    {
        rootUI = GameObject.Find("MoveLearnMenu");
        monSprite = rootUI.transform.Find("MonSprite").gameObject;
        moveLearnButtons =
            Enumerable.Range(1, 2)
            .Select(number => GameObject.Find($"MoveLearnButton{number}").gameObject)
            .ToList();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
