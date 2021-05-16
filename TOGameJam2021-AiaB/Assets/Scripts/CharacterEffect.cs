using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    private float spriteSwitchFrequency;
    private float switchTime;
    private int currentSprite;
    [SerializeField]
    private float fadeInDuration;
    [SerializeField]
    private float fullyVisibleDuration;
    [SerializeField]
    private float fadeOutDuration;
    private float elapsedTime;
    private SpriteRenderer m_sr;

    private void OnValidate()
    {
        if (sprites.Length > 0)
        {
            currentSprite = sprites.Length > 1 ? currentSprite % sprites.Length : 0;
            m_sr = GetComponent<SpriteRenderer>();
            m_sr.sprite = sprites[currentSprite];
        }
    }

    private void Start()
    {
        elapsedTime = 0.0f;
        switchTime = 0.0f;
        currentSprite = 0;
        m_sr = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0)
            m_sr.sprite = sprites[currentSprite];
    }

    void Update()
    {
        switchTime += Time.deltaTime;
        if (switchTime > spriteSwitchFrequency)
        {
            switchTime -= spriteSwitchFrequency;
            currentSprite++;
            if (sprites.Length > 0)
            {
                currentSprite = sprites.Length > 1 ? currentSprite % sprites.Length : 0;
                m_sr.sprite = sprites[currentSprite];
            }
        }

        float totalTime = fadeInDuration + fullyVisibleDuration + fadeOutDuration;
        if (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= totalTime)
                gameObject.SetActive(false);
        }

        CalcOpacity();
    }

    public void OnDisplay()
    {
        gameObject.SetActive(true);
        elapsedTime = 0.0f;
    }

    private void CalcOpacity()
    {
        Color c = m_sr.color;
        
        if (elapsedTime < fadeInDuration)
        {
            c.a = elapsedTime / fadeInDuration;
        }
        else if (elapsedTime < fadeInDuration + fullyVisibleDuration)
        {
            c.a = 1.0f;
        }
        else if (elapsedTime < fadeInDuration + fullyVisibleDuration + fadeOutDuration)
        {
            float t = (fadeInDuration + fullyVisibleDuration + fadeOutDuration) - elapsedTime;
            c.a = t / fadeOutDuration;
        }
        else
        {
            c.a = 0.0f;
        }

        m_sr.color = c;
    }
}
