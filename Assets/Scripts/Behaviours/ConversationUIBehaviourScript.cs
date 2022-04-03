using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

struct DialogBubble
{
    private Sprite mySprite;
    private string myText;

    public DialogBubble([CanBeNull] Sprite mySprite, string myText)
    {
        this.mySprite = mySprite;
        this.myText = myText;
    }

    public Sprite MySprite
    {
        get => mySprite;
        set => mySprite = value;
    }

    public string MyText
    {
        get => myText;
        set => myText = value;
    }
}

public class ConversationUIBehaviourScript : MonoBehaviour
{

    public float msgDuration = 5f;
    public float secondsPerCharacter = 0.02f;
    
    public GameObject main;
    public Image imageObj;
    public TextMeshProUGUI textfeld;
    
    private float msgTimer = 0;
    
    private List<DialogBubble> msgQueue;

    private DialogBubble? _currentMsg = null;
    
    private float textBuildDelta = 0f;

    private void Awake()
    {
        Debug.Assert(main != null, gameObject);
        Debug.Assert(imageObj != null, gameObject);
        Debug.Assert(textfeld != null, gameObject);
    }

    private void Start()
    {
        msgQueue ??= new List<DialogBubble>();

        textBuildDelta = 0f;

        PushNextMsg();
    }

    private void Update()
    {
        if (msgTimer <= Time.deltaTime)
        {
            msgTimer = 0;
            PushNextMsg();
        }
        else
        {
            msgTimer -= Time.deltaTime;
        }
        
        if (_currentMsg != null && secondsPerCharacter > 0 && textfeld.maxVisibleCharacters < textfeld.text.Length)
        {
            textBuildDelta += Time.deltaTime;
            if (textBuildDelta > secondsPerCharacter / 1000f)
            {
                var n = Mathf.FloorToInt(textBuildDelta / secondsPerCharacter);
                textBuildDelta -= n * secondsPerCharacter;
                
                textfeld.maxVisibleCharacters += n;
            }
        }
    }

    public void AddMsg([CanBeNull] Sprite img, string msg)
    {
        msgQueue.Add(new DialogBubble(img, msg));
    }

    private void PushNextMsg()
    {
        if (msgQueue.Count > 0)
        {
            main.SetActive(true);
            msgTimer = msgDuration;
            var msg = msgQueue[0];
            msgQueue.Remove(msg);
            _currentMsg = msg;
            if (msg.MySprite != null)
            {
                imageObj.sprite = msg.MySprite;
                imageObj.gameObject.SetActive(true);
            }
            else
            {
                imageObj.gameObject.SetActive(false);
            }

            if (msg.MyText.Length > 0)
            {
                textfeld.text = msg.MyText;
                textfeld.gameObject.SetActive(true);
            }
            else
            {
                textfeld.gameObject.SetActive(false);
            }
        }
        else
        {
            main.SetActive(false);
            _currentMsg = null;
        }
    }
}
