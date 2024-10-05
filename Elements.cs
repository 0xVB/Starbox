using System.Collections.Generic;
using UnityEngine;
public class Element
{
    public enum MatterState
    {
        Solid,
        Liquid,
        Gas,
        Plasma
    };

    public string ChemicalSymbol;
    public string FullName;
    public Color ElementColor;
    public MatterState State;
    public float ElementOpacity;

    Element(string Symbol, string Name, Color Col, MatterState Type, float Opacity)
    {
        ChemicalSymbol = Symbol;
        FullName = Name;
        ElementColor = Col;
        State = Type;
        ElementOpacity = Opacity;
    }

    public static Dictionary<string, Element> Elements = new Dictionary<string, Element>();
    public static void Populate()
    {
        // Elements commonly found in planetary compositions
        Elements.Add("Si", new Element(
            "Si", "Silicon", new Color(0.65f, 0.65f, 0.55f), MatterState.Solid, 1.0f // Grayish tan color
        ));

        Elements.Add("Mg", new Element(
            "Mg", "Magnesium", new Color(0.70f, 0.72f, 0.66f), MatterState.Solid, 1.0f // Pale metallic
        ));

        Elements.Add("Al", new Element(
            "Al", "Aluminum", new Color(0.75f, 0.75f, 0.80f), MatterState.Solid, 1.0f // Light silvery
        ));

        Elements.Add("Ca", new Element(
            "Ca", "Calcium", new Color(0.9f, 0.9f, 0.9f), MatterState.Solid, 1.0f // Almost white
        ));

        Elements.Add("Na", new Element(
            "Na", "Sodium", new Color(0.95f, 0.95f, 0.85f), MatterState.Solid, 1.0f // Pale off-white
        ));

        Elements.Add("K", new Element(
            "K", "Potassium", new Color(0.75f, 0.5f, 0.35f), MatterState.Solid, 1.0f // Light brownish
        ));

        Elements.Add("S", new Element(
            "S", "Sulfur", new Color(1.0f, 0.85f, 0.2f), MatterState.Solid, 1.0f // Bright yellow
        ));

        // Compounds commonly found in planets
        Elements.Add("SiO2", new Element(
            "SiO2", "Silicon Dioxide", new Color(0.85f, 0.85f, 0.8f), MatterState.Solid, 1.0f // Light sandy color (quartz)
        ));

        Elements.Add("FeO", new Element(
            "FeO", "Iron(II) Oxide", new Color(0.3f, 0.3f, 0.35f), MatterState.Solid, 1.0f // Dark grayish black
        ));

        Elements.Add("CO2", new Element(
            "CO2", "Carbon Dioxide", new Color(0.7f, 0.75f, 0.8f), MatterState.Gas, 0.7f // Pale bluish-gray
        ));

        Elements.Add("H2O", new Element(
            "H2O", "Water", new Color(0.0f, 0.5f, 1.0f), MatterState.Liquid, 0.9f // Vivid blue
        ));

        Elements.Add("CH4", new Element(
            "CH4", "Methane", new Color(0.35f, 0.65f, 0.9f), MatterState.Gas, 0.6f // Light sky blue
        ));

        Elements.Add("NH3", new Element(
            "NH3", "Ammonia", new Color(0.85f, 0.9f, 1.0f), MatterState.Gas, 0.6f // Pale white-blue
        ));

        Elements.Add("NaCl", new Element(
            "NaCl", "Sodium Chloride (Salt)", new Color(0.9f, 0.9f, 0.85f), MatterState.Solid, 1.0f // Off-white, slightly yellowish
        ));

        Elements.Add("FeS", new Element(
            "FeS", "Iron Sulfide", new Color(0.45f, 0.45f, 0.3f), MatterState.Solid, 1.0f // Dull brownish
        ));

        Elements.Add("CO", new Element(
            "CO", "Carbon Monoxide", new Color(0.75f, 0.75f, 0.85f), MatterState.Gas, 0.6f // Pale, slightly blueish gray
        ));
    }
};