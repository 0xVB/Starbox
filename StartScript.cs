using System;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class StartScript : MonoBehaviour
{
    public GameObject Character;
    public GameObject Ground;
    public PlanetGenerator PGen;

    public InputField CompField;

    public InputField MassField;
    void Start()
    {
        Element.Populate();
    }

    float GetParam(string PName)
    {
        return float.Parse(GameObject.Find(PName).GetComponent<TMP_InputField>().text);
    }

    public GameObject FindInactiveObject(string objectName)
    {
        foreach (GameObject obj in GameObject.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (obj.name == objectName)
            {
                return obj;
            }
        }
        return null;
    }

    public void OnClick()
    {
        Character = FindInactiveObject("Character");
        Ground = GameObject.Find("Ground");
        PGen = Ground.GetComponent<PlanetGenerator>();

        Debug.Log(Character);
        Debug.Log(PGen);
        Debug.Log(Ground);

        string Comp = GameObject.Find("Composition").GetComponent<TMP_InputField>().text;
        float PMass = GetParam("PMass");
        float SunD = GetParam("SunD");
        float AstChance = GetParam("Ast");

        PGen.Erraticness = 1 / PMass;
        PGen.Brightness = 5 / SunD;
        PGen.Craters.MinCraters = (int)AstChance;
        PGen.Craters.MaxCraters = (int)AstChance;
        PGen.Composition = Element.Elements[Comp];

        Character.SetActive(true);
        PGen.enabled = true;

        Destroy(GameObject.Find("MainCam"));
        Destroy(GameObject.Find("PSettings"));
    }

    void Update()
    {
        
    }
}
