using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class MainMenuPlayBT : MonoBehaviour
{
    public GameObject screenImg;
    public GameObject drumImg;
    private SpriteRenderer screenSprite;
    public Image uiOverlay;

    public Button btn;
    public float rotationSpeed;
    public float hoverSpeed = 0.25f;
    public float fadeInSpeed = 0.1f;

    private bool rotating = false;
    private float dt;
    private float alpha = 0;
    public float fadeInDelay = -0.18f;

    private float crimsonValue;
    public float crimsonValueChangeRate;
    public float crimsonValueTarget;
    
    void Start()
    {
        crimsonValue = 0;
        rotating = false;
        alpha = fadeInDelay;
        screenSprite = screenImg.GetComponent<SpriteRenderer>();
    }

    public void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        SceneManager.LoadScene("GameplayLevel");
    }

    // Update is called once per frame
    void Update()
    {
        dt += Time.deltaTime;
        screenImg.transform.localPosition = new Vector3(0, Mathf.Sin(dt) * hoverSpeed);

        alpha = alpha + Time.deltaTime * fadeInSpeed;
        float a = MathF.Min(alpha, 1.0f);
        a = MathF.Max(a, 0f);
        Color c = screenSprite.color;
        c.a = a;
        screenSprite.color = c;

        if (rotating)
        {
            drumImg.transform.Rotate(0, rotationSpeed*Time.deltaTime, 0, Space.Self);
            crimsonValue = crimsonValue + Time.deltaTime*crimsonValueChangeRate;
        }
        else
        {
            drumImg.transform.rotation = Quaternion.Euler(Vector3.zero);
            crimsonValue = crimsonValue - Time.deltaTime*crimsonValueChangeRate;
        }
        
        crimsonValue = MathF.Min(crimsonValue, crimsonValueTarget);
        crimsonValue = MathF.Max(crimsonValue, 0f);
        c = uiOverlay.color;
        c.a = crimsonValue;
        uiOverlay.color = c;
    }

    private void MouseUp()
    {
    }

    public void MouseEnter()
    {
        rotating = true;
    }

    public void MouseExit()
    {
        rotating = false;
    }
}