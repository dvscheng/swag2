using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScript : MonoBehaviour {
    public Canvas UI;
    private MapGenerator1 mg;

    private void Start()
    {
        mg = FindObjectOfType(typeof(MapGenerator1)) as MapGenerator1;
    }

    public void onButtonClick()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        if (go != null)
        {
            mg.setBlockType(go.name);
            Debug.Log("Clicked on : " + go.name);
        }
        else
        {
            mg.setBlockType("Dirt");
            Debug.Log("currentSelectedGameObject is null");
        }
    }
}
