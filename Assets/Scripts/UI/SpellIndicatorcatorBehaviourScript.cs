using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpellIndicatorcatorBehaviourScript : MonoBehaviour
{

    [SerializeField] private Image nextImage;
    [SerializeField] private Image currentImage;
    [SerializeField] private Image previousImage;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private SpellStateBehaviourScript spellStateBehaviourScript;
    
    private void Awake()
    {
        Debug.Assert(nextImage != null, gameObject);
        Debug.Assert(currentImage != null, gameObject);
        Debug.Assert(previousImage != null, gameObject);
        
        Debug.Assert(text != null, gameObject);
    }

    void Start()
    {
        spellStateBehaviourScript = FindObjectOfType<SpellStateBehaviourScript>();
        
        spellStateBehaviourScript.onSpellStateChange.AddListener(UpdateSpellIndicatorcator);
        UpdateSpellIndicatorcator();
    }

    private void OnDisable()
    {
        spellStateBehaviourScript.onSpellStateChange.RemoveListener(UpdateSpellIndicatorcator);
    }

    public void UpdateSpellIndicatorcator()
    {
        ISpellBehaviourScript next = spellStateBehaviourScript.GetNexSpell();
        if (next != null && !(next is SpellPlaceholderBehaviourScript))
        {
            nextImage.sprite = next.GetSprite();
            nextImage.gameObject.SetActive(true);
        }
        else
        {
            nextImage.gameObject.SetActive(false);
        }
        ISpellBehaviourScript current = spellStateBehaviourScript.GetCurrentSpell();
        if (current != null && !(current is SpellPlaceholderBehaviourScript))
        {
            currentImage.sprite = current.GetSprite();
            currentImage.gameObject.SetActive(true);
            text.text = current.GetName();
            text.gameObject.SetActive(true);
        }
        else
        {
            currentImage.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }
        ISpellBehaviourScript previous = spellStateBehaviourScript.GetPreviousSpell();
        if (previous != null && !(previous is SpellPlaceholderBehaviourScript))
        {
            previousImage.sprite = previous.GetSprite();
            previousImage.gameObject.SetActive(true);
        }
        else
        {
            previousImage.gameObject.SetActive(false);
        }
    }
    
}
