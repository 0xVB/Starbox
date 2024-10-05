using UnityEngine;
using SimplexNoise;
using UnityEditor;
using System;
using Unity.VisualScripting;

public class PlanetGenerator : MonoBehaviour
{
    public class CraterAttributes
    {
        public int MinCraters = 10;
        public int MaxCraters = 30;
        public float MinCraterRadius = 5.0f;
        public float MaxCraterRadius = 10.0f;
        public float MinCraterDepth = 0.15f;
    }

    private Terrain aTerr;
    private Light aLight;
    public float Erraticness = 0.10f;
    public float MaxHeight = 50.0f;
    public float BaseHeight = 0.2f;
    public int GenSeed = 1337; 
    public int TerrainResolution = 9;
    public int TerrSize = 1000;

    public float Brightness = 1.0f;

    public Element Composition = null;

    public CraterAttributes Craters = new CraterAttributes();

    void Start()
    {
        
    }

    public void ReSeed()
    {
        if (!aTerr)
        {
            aLight = GameObject.Find("Camera").GetComponent<Light>();
            aTerr = gameObject.AddComponent<Terrain>();
            gameObject.AddComponent<TerrainCollider>();
            aTerr.terrainData = new TerrainData();
            aTerr.terrainData.heightmapResolution = (int)Math.Pow(2, TerrainResolution);
            aTerr.terrainData.alphamapResolution = aTerr.terrainData.heightmapResolution + 1;
            aTerr.Flush();
        }

        TerrainData aData = aTerr.terrainData;
        Vector3 aSize = aData.size;

        GetComponent<TerrainCollider>().terrainData = aData;
        transform.Translate(-TerrSize / 2, 0, -TerrSize / 2);
        Noise.Seed = GenSeed;
        aSize.y = MaxHeight;
        aSize.x = TerrSize;
        aSize.z = TerrSize;
        aData.size = aSize;

        aLight.color = Composition.ElementColor;
        aLight.intensity = Brightness;

        TerrainLayer normalLayer = new TerrainLayer();
        TerrainLayer craterLayer = new TerrainLayer();

        normalLayer.diffuseTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Terrain/dirt.png");
        craterLayer.diffuseTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Terrain/lava.png");

        // Apply the element color to the texture
        normalLayer.diffuseTexture = TintTextureWithElementColor(normalLayer.diffuseTexture, Composition.ElementColor);
        normalLayer.tileSize = new Vector2(5, 5);

        // Assign the layers to the terrain data
        aData.terrainLayers = new TerrainLayer[] { normalLayer, craterLayer };

        // Create and assign a simple material with a terrain shader
        Material aMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Terrain.mat");
        aTerr.materialTemplate = aMat;

        int aW = aData.heightmapResolution;
        int aH = aData.heightmapResolution;

        float[,] hVals = aData.GetHeights(0, 0, aW, aH);
        float[,,] aLayerDat = aData.GetAlphamaps(0, 0, aW, aH);

        for (int y = 0; y < aH; y++)
            for (int x = 0; x < aW; x++)
                hVals[x, y] = BaseHeight + Noise.CalcPixel2D(x, y, Erraticness) / 255.0f * (1 - BaseHeight);

        // Craters
        GenerateCraters(hVals, aLayerDat, aW, aH, aData);

        aData.SetAlphamaps(0, 0, aLayerDat);
        aData.SetHeights(0, 0, hVals);
        aTerr.Flush();
    }

    void GenerateCraters(float[,] heights, float[,,] aLayerDat, int width, int height, TerrainData terrainData)
    {
        int craterCount = UnityEngine.Random.Range(Craters.MinCraters, Craters.MaxCraters);      
        for (int i = 0; i < craterCount; i++)
        {
            // Random position for the crater
            int craterX = UnityEngine.Random.Range(0, width);
            int craterY = UnityEngine.Random.Range(0, height);

            // Random radius and depth for the crater
            float craterRadius = UnityEngine.Random.Range(Craters.MinCraterRadius, Craters.MaxCraterRadius);
            float craterDepth = UnityEngine.Random.Range(Craters.MinCraterDepth, BaseHeight);

            float rimRadius = craterRadius * 1.35f; // Define the radius for the crater rim
            float rimHeightFactor = craterDepth * 0.10f; // Rim bump height as a fraction of crater depth

            // Create crater shape
            for (int y = craterY - (int)craterRadius * 2; y < craterY + (int)craterRadius * 2; y++)
            {
                for (int x = craterX - (int)craterRadius * 2; x < craterX + (int)craterRadius * 2; x++)
                {
                    if (x < 0 || y < 0 || x >= width || y >= height) continue;

                    // Distance from the center of the crater
                    float distanceToCraterCenter = Vector2.Distance(new Vector2(craterX, craterY), new Vector2(x, y));

                    // Check if the point is within the crater radius
                    if (distanceToCraterCenter < craterRadius)
                    {
                        // Inverted hemisphere effect for crater depth
                        float craterFactor = Mathf.Cos((distanceToCraterCenter / craterRadius) * Mathf.PI / 2);
                        heights[x, y] = Mathf.Clamp(heights[x, y] - craterDepth * craterFactor, 0, 1);
                        // Apply the crater texture
                        aLayerDat[x, y, 1] = 1f;
                    }
                    // Check if the point is within the rim radius (but outside the crater)
                    else if (distanceToCraterCenter < rimRadius)
                    {
                        // Bump up the terrain height slightly to create the rim
                        heights[x, y] = Mathf.Clamp(heights[x, y] + rimHeightFactor, 0, 1);
                        aLayerDat[x, y, 1] = 1f;
                    }
                }
            }
        }
    }

    public Texture2D TintTextureWithElementColor(Texture2D originalTexture, Color elementColor)
    {
        Texture2D tintedTexture = new Texture2D(originalTexture.width, originalTexture.height);

        for (int y = 0; y < originalTexture.height; y++)
        {
            for (int x = 0; x < originalTexture.width; x++)
            {
                Color originalColor = originalTexture.GetPixel(x, y);
                // Multiply each pixel by the element color to create the tint
                Color tintedColor = originalColor * elementColor;
                tintedTexture.SetPixel(x, y, tintedColor);
            }
        }

        tintedTexture.Apply();
        return tintedTexture;
    }
    void Update()
    {
        
    }
}
