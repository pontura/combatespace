﻿using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class AvatarCustomizer : MonoBehaviour {

    public bool isMyAvatar;

    public Styles styles;

    public AvatarCustomizerPart[] avatarCustomizerParts;
    public CharacterHead head;
    public Material BoxerMaterial;

    void Start()
    {
        //foreach (SkinnedMeshRenderer sm in GetComponentsInChildren<SkinnedMeshRenderer>())
        //{
        //    for (int a = 0; a < sm.materials.Length; a++ )
        //    {
        //        sm.materials[a] = BoxerMaterial;
        //    }
        //}
        if (Data.Instance.customizerData.data != null)
        {
            if (isMyAvatar)
                styles = Data.Instance.playerSettings.heroData.styles;
            else
                styles = Data.Instance.playerSettings.characterData.styles;

            string style = styles.style;
            if (style.Length < 8)
                style = SetRandomStyle();
            ParseStyles(style);
            LoopRandomFaces();
        }
        Events.OnCustomizerChangePart += OnCustomizerChangePart;        
    }
    void LoopRandomFaces()
    {
        int num = UnityEngine.Random.Range(0, 10);
        if (num < 3)
            Events.OnAvatarExpresion(AvatarExpresiones.types.CERRADA, isMyAvatar);
        Invoke("LoopRandomFaces", 4);
    }
    void OnDestroy()
    {
        Events.OnCustomizerChangePart -= OnCustomizerChangePart;
    }
    public void OnCustomizerChangePart(string partName, int partID)
    {
        SetStyle(partName, partID);
        OnChangePart(partName, partID);
	}
    public void OnChangePart(string partName, int partID)
    {
        int id = 0;
        foreach (CustomizerPartData data in Data.Instance.customizerData.data)
        {
            if (data.name == partName)
            {
                if (id == partID)
                {
                    if (partName == "pantalon_tela" || partName == "guantes_tela" || partName == "tatoo")
                    {
                        ChangeTexture(data.name, data.texture);
                    } else
                    if (data.color.a > 0)
                        ChangePart(data.name, "", data.color, 0);

                    for (int a = 0; a < data.url.Count; a++)
                        ChangePart(data.name, data.url[a], data.color, a);
                    return;
                }
                id++;
            }
        }
    }
    public void ChangePart(string partName, string partType, Color color, int materialID)
    {
        //print("ChangePart  " + partName + "  partType: " + partType + " color: " + color + " materialID:" + materialID);

        if (partName == "peinados" || partName == "cejas" || partName == "narices" || partName == "barbas")
        {
            head.SetMeshPart(int.Parse(partType), partName);
        }
        else
        if (partName == "cabezas")
        {
            head.SetCabeza(int.Parse(partType));
        }
        else if (partName == "piel")
            head.ChangePiel(partType, color);
        else if (partName == "pelos")
            head.ChangePelos(partType, color);

        int partNum = materialID + 1;
        foreach (AvatarCustomizerPart part in avatarCustomizerParts)
        {
            if (part.data.name == partName + partNum)
                part.ChangeTexture(partName, partType, color);
        }
    }
    void ChangeTexture(string partName, string partTexture)
    {
        foreach (AvatarCustomizerPart part in avatarCustomizerParts)
        {
            if (part.data.name == "pantalon1" && partName == "pantalon_tela")
                part.ChangeTexture(partName, partTexture);
            else if (part.data.name == "guantes1" && partName == "guantes_tela")
                part.ChangeTexture(partName, partTexture);
            else if (part.data.name == "piel1" && partName == "tatoo")
                part.ChangeTexture(partName, partTexture);
        }
    }
    void SetStyle(string partName, int partID)
    {
        switch (partName)
        {
            case "cabezas": styles.cabezas = partID; break;
            case "piel": styles.piel = partID; break;            
            case "pelos": styles.pelos = partID; break;
            case "peinados": styles.peinados = partID; break;
            case "cejas": styles.cejas = partID; break;
            case "narices": styles.narices = partID; break;
            case "barbas": styles.barbas = partID; break;
            case "guantes": styles.guantes = partID; break;
            case "pantalon": styles.pantalon = partID; break;
            case "botas": styles.botas = partID; break;
            case "pantalon_tela": styles.pantalon_tela = partID; break;
            case "guantes_tela": styles.guantes_tela = partID; break;
            case "tatoo": styles.tatoo = partID; break;
        }
    }
    public string SetRandomStyle()
    {
        string style = "";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4) + "-";
        style += UnityEngine.Random.Range(1, 4);

        return style;
    }
    public void ParseStyles(string style)
    {
        if (style == "") return;

       // Debug.Log("content: " + style);

        string[] allData = Regex.Split(style, "-");

        if (allData.Length > 8)
        {
            OnChangePart("cabezas", int.Parse(allData[0]));
            OnChangePart("peinados", int.Parse(allData[1]));
            OnChangePart("pelos", int.Parse(allData[2]));
            OnChangePart("piel", int.Parse(allData[3]));
            OnChangePart("narices", int.Parse(allData[4]));
            OnChangePart("barbas", int.Parse(allData[5]));
            OnChangePart("cejas", int.Parse(allData[6]));
            OnChangePart("guantes", int.Parse(allData[7]));
            OnChangePart("pantalon", int.Parse(allData[8]));
            OnChangePart("botas", int.Parse(allData[9]));
            OnChangePart("tatoo", int.Parse(allData[10]));
            if (allData.Length > 11)
            {
                OnChangePart("pantalon_tela", int.Parse(allData[11]));
                OnChangePart("guantes_tela", int.Parse(allData[12]));
            }
           
        }
        else
        {
            Debug.Log("Styles vacio o insuficiente");
        }
    }
}
