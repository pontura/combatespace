﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomizerPartsButton : MonoBehaviour
{
    public string partName;
    public Image thumb;
    public Image Selector;
    private Texture2D texture;

    public void Init(string partName)
    {
        this.partName = partName;
        string thumbName = "";

        switch (partName)
        {
            case "cejas": thumbName = "icons_menu_eyebrows"; break;
            case "guantes": thumbName = "icons_menu_glove1"; break;
            case "peinados": thumbName = "icons_menu_hair"; break;
            case "pelos": thumbName = "icons_menu_hairColor"; break;
            case "cabezas": thumbName = "icons_menu_head"; break;
            case "narices": thumbName = "icons_menu_nose"; break;
            case "botas": thumbName = "icons_menu_shoes"; break;
            case "pantalon": thumbName = "icons_menu_shorts"; break;
            case "piel": thumbName = "icons_menu_skin"; break;
            case "barbas": thumbName = "icons_menu_moustache"; break;
            case "pantalon_tela": thumbName = "icons_menu_pantalontela"; break;
            case "guantes_tela": thumbName = "icons_menu_guantetela"; break;
            case "tatoo": thumbName = "icons_menu_tatoo"; break;
            default: thumbName = "icons_menu_moustache"; break;
        }
        texture = Resources.Load("Customizer/icons/" + thumbName) as Texture2D;
        thumb.sprite = Sprite.Create(texture, new Rect(0, 0, 511, 511), Vector2.zero);
    }
    public void OnClicked()
    {
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click3);
        Events.OnCustomizerRefresh(partName);
    }
    public void SetOn()
    {
        Selector.enabled = true;
    }
    public void SetOff()
    {
        Selector.enabled = false;
    }
    void OnDestroy()
    {
        texture = null;
    }
}
