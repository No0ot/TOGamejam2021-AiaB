using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CastMember : MonoBehaviour
{
    [SerializeField]
    float m_fMaxConfidence;
    float m_fCurrentConfidence;

    [SerializeField]
    GameObject ConfidenceMeter;
    [SerializeField]
    GameObject ConfidenceMeterBackground;
    [SerializeField]
    public int[] m_ActingEffectiveness;
    [SerializeField]
    public int[] m_CostumeEffectiveness;
    [SerializeField]
    public int[] m_InterestEffectiveness;
    [SerializeField]
    public Sprite m_Headshot;
    [SerializeField]
    public string m_CharacterDescription;
    public string m_CharacterName;

    int m_NumofAttacks;

    // Animation properties
    private SpriteRenderer m_sr;
    private SpriteRenderer m_cmsr;
    private SpriteRenderer m_cmbgsr;
    private bool fadingIn; // Whether the character is currently fading in or out.
    private float fadeTime; // Amount of time that the current animation should play for.
    private float elapsedTime; // Amount of time since the current animation started.

    private void Awake()
    {
        m_sr = GetComponent<SpriteRenderer>();
        m_cmsr = ConfidenceMeter.GetComponent<SpriteRenderer>();
        m_cmbgsr = ConfidenceMeterBackground.GetComponent<SpriteRenderer>();
        
        SpriteRenderer[] srs = { m_sr, m_cmsr, m_cmbgsr };
        foreach (SpriteRenderer sr in srs)
        {
            Color c = sr.color;
            c.a = 0.0f;
            sr.color = c;
        }
        
        elapsedTime = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CharacterName = gameObject.name;
        m_fCurrentConfidence = m_fMaxConfidence;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime <= fadeTime)
        {
            elapsedTime += Time.deltaTime;
            CalcOpacity();
            if (m_sr.color.a <= 0.0f)
                gameObject.SetActive(false);
        }

        if (fadingIn) // The cast member fades out only after it survives or is eliminated, which always happens after it kills itself. In this way, this will only be called until the audition is over.
        {
            if (gameObject.activeInHierarchy)
            {
                if (m_fCurrentConfidence <= 0)
                {
                    ConfidenceMeter.SetActive(false);
                    KillSelf();
                }
            }

            if (m_NumofAttacks == 3)
            {
                GameManager.Instance.OnSurvive(this.gameObject);
            }
        }
    }

    public void TakeDamage(InsultDamageType damagetype)
    {
        switch(damagetype)
        {
            case InsultDamageType.NO_EFFECT:
                //nothing
                break;
            case InsultDamageType.MILDLY_EFFECTIVE:
                m_fCurrentConfidence -= 10.0f;
                break;
            case InsultDamageType.EFFECTIVE:
                m_fCurrentConfidence -= 20.0f;
                break;
            case InsultDamageType.SUPER_EFFECTIVE:
                m_fCurrentConfidence -= 40.0f;
                break;
            default:
                break;
        }
        Vector3 ConfidencePercentage = new Vector3(m_fCurrentConfidence / m_fMaxConfidence, 1.0f,1.0f);
        ConfidenceMeter.transform.localScale = ConfidencePercentage;
        m_NumofAttacks++;
    }

    void KillSelf()
    {
        GameManager.Instance.OnEliminate(this.gameObject);
        //Removes itself from the gamemanagers list
    }

    public void OnFadeIn(float secondsToFade)
    {
        fadingIn = true;
        fadeTime = secondsToFade;

        // Set elapsed time to the portion of the new animation that it would be based on the current alpha value
        elapsedTime = m_sr.color.a * fadeTime;
        CalcOpacity();
    }

    public void OnFadeOut(float secondsToFade)
    {
        fadingIn = false;
        fadeTime = secondsToFade;

        // Set elapsed time to the portion of the new animation that it would be based on the current alpha value
        elapsedTime = (1.0f - m_sr.color.a) * fadeTime;
        CalcOpacity();
    }

    private void CalcOpacity()
    {
        float t = Mathf.Clamp(elapsedTime / fadeTime, 0.0f, 1.0f);
        float a = fadingIn ? Mathf.Lerp(0.0f, 1.0f, t) : Mathf.Lerp(1.0f, 0.0f, t);

        SpriteRenderer[] srs = { m_sr, m_cmsr, m_cmbgsr };
        foreach (SpriteRenderer sr in srs)
        {
            Color c = sr.color;
            c.a = a;
            sr.color = c;
        }
    }
}
