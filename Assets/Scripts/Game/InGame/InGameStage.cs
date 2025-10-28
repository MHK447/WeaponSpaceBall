using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UniRx;
using DG.Tweening;
using System.Linq;
using BanpoFri;
using UnityEngine.UI;
using System.Collections;
public class InGameStage : MonoBehaviour
{

    public void StartPlaying()
    {
   
    }

    public void HighScoreInit()
    {
       
    }



    public void CallStartGame()
    {
        GameRoot.Instance.UISystem.OpenUI<PopupInGame>(popup=> popup.Init());

        
    }



}
