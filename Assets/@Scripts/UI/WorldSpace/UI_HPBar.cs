using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HPBar : UI_Base
{
    enum Sliders
    {
        HPBar
    }

    protected override void Awake()
    {
        base.Awake();
        BindSliders(typeof(Sliders));
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.rotation = Camera.main.transform.rotation;

        float ratio = Managers.Game.Player.Hp / (float)Managers.Game.Player.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        GetSlider((int)Sliders.HPBar).value = ratio;
    }

}
