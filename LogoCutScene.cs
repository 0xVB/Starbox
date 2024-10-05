using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;
using System;
using Unity.Mathematics;
using UnityEngine.Rendering;

public class IntroManager : MonoBehaviour
{
    public float LogoFadeSpeed = 1.0f;
    public float GlowFadeSpeed = 1.5f;
    public float TransitionSpeed = 1.0f;
    public float TargetY = -50.0f;

    private int Phase = 0;
    private UnityEngine.UI.Image LogoImage;
    private UnityEngine.UI.Image GlowImage;
    private RectTransform RT;
    private float GlowAmount = 0.0f;
    private float PosT = 0.0f;
    private float YStart, YDelta;
    void Start()
    {
        LogoImage = GetComponent<UnityEngine.UI.Image>();
        GlowImage = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        RT = GetComponent<RectTransform>();
        YStart = RT.position.y;
        YDelta = TargetY - YStart;
    }

    void Update()
    {
        switch (Phase)
        {
            default:
            break;

            case 0:
            Color iCol = LogoImage.color;
            iCol.a += Mathf.Min(1.0f, Time.deltaTime * LogoFadeSpeed);
            LogoImage.color = iCol;
            if (iCol.a >= 1.0f) Phase++;
            break;

            case 1:
            case 2:
            case 3:
            GlowAmount += Time.deltaTime;

            iCol = GlowImage.color;
            iCol.a = (Mathf.Sin(GlowAmount * GlowFadeSpeed - Mathf.PI / 2) + 1) / 2;
            GlowImage.color = iCol;
            if (Phase == 1 && GlowAmount * GlowFadeSpeed >= Mathf.PI) Phase = 2;
            if (Phase == 2)
            {
                PosT += Time.deltaTime * TransitionSpeed;
                if (PosT >= 1.0f)
                {
                    Phase = 3;
                    PosT = 1.0f;

                    GameObject.Find("PSettings").GetComponent<Canvas>().enabled = true;
                }

                float T = Mathf.Sin(PosT * Mathf.PI / 2);
                float sVal = 0.5f - 0.3f * T;
                transform.localScale = new Vector3(sVal, sVal, sVal);

                sVal = 0.5f + (T / 2);
                RT.anchorMin = new Vector2(0.5f, sVal);
                RT.anchorMax = new Vector2(0.5f, sVal);
                RT.pivot = new Vector2(0.5f, sVal);
                RT.Translate(0, TargetY * Time.deltaTime, 0);
            }
            break;
        }
    }
}