using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    [SerializeField]
    private float FadeInDuration;
    [SerializeField]
    private float FullyVisibleDuration;
    [SerializeField]
    private float FadeOutDuration;
    private float ElapsedTime;
    private SpriteRenderer m_sr;

    private void Start()
    {
        ElapsedTime = 0.0f;
        m_sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float totalTime = FadeInDuration + FullyVisibleDuration + FadeOutDuration;
        if (ElapsedTime < totalTime)
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime >= totalTime)
                gameObject.SetActive(false);
        }

        CalcOpacity();
    }

    public void OnDisplay()
    {
        gameObject.SetActive(true);
        ElapsedTime = 0.0f;
    }

    private void CalcOpacity()
    {
        Color c = m_sr.color;
        
        if (ElapsedTime < FadeInDuration)
        {
            c.a = ElapsedTime / FadeInDuration;
        }
        else if (ElapsedTime < FadeInDuration + FullyVisibleDuration)
        {
            c.a = 1.0f;
        }
        else if (ElapsedTime < FadeInDuration + FullyVisibleDuration + FadeOutDuration)
        {
            float t = (FadeInDuration + FullyVisibleDuration + FadeOutDuration) - ElapsedTime;
            c.a = t / FadeOutDuration;
        }
        else
        {
            c.a = 0.0f;
        }

        m_sr.color = c;
    }
}
