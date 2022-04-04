using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private MainInputActionsSettings input;
    public MovementAnimator playerAnimator;
    public CanvasGroup loosePanel;
    public GameObject looseClickTF;
    public CanvasGroup winPanel;
    public GameObject winClickTF;

    private float winFadeAlpha = 0f;
    private float looseFadeAlpha = 0f;
    public float alphaSpeed = 0.5f;
    public bool fadeLoose;
    public bool fadeWin;
    public bool clickContinues = false;

    // Start is called before the first frame update
    void Start()
    {
        input = FindObjectOfType<GameStateBehaviourScript>().mainInputActions;
        looseClickTF.gameObject.SetActive(false);
        winClickTF.gameObject.SetActive(false);
        fadeLoose = false;
        looseFadeAlpha = 0f;
    }


    private void OnBackRequest()
    {
        if (clickContinues)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            print("Not yet.");
        }
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (fadeLoose)
        {
            looseFadeAlpha += Time.deltaTime * alphaSpeed;
            loosePanel.alpha = looseFadeAlpha;
        }

        if (fadeWin)
        {
            winFadeAlpha += Time.deltaTime * alphaSpeed;
            winPanel.alpha = winFadeAlpha;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnBackRequest();
        }
        
    }


    public void ShowWinScreen()
    {
        Debug.LogWarning("A WINNER IS YOU");
        Invoke(nameof(InvokeWin1), 6);
        Invoke(nameof(InvokeWin2), 21);
    }

    public void ShowLooseScreen()
    {
        // playerAnimator.myMovementAnimator.fireEvents("Die");
        // TODO play anim

        Debug.LogWarning("LOST THE GAME");

        Invoke(nameof(InvokeLoose1), 6);
        Invoke(nameof(InvokeLoose2), 21);
    }

    public void InvokeLoose1()
    {
        fadeLoose = true;
    }

    public void InvokeLoose2()
    {
        fadeLoose = true;
        clickContinues = true;
        looseClickTF.gameObject.SetActive(true);
    }


    public void InvokeWin1()
    {
        fadeWin = true;
    }

    public void InvokeWin2()
    {
        fadeWin = true;
        clickContinues = true;
        winClickTF.gameObject.SetActive(true);
    }
}