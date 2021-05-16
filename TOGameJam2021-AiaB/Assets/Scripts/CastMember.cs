using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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

    private int m_attacksTakenInRound;
    private int m_attacksTakenInTotal;
    private float m_timeTakenInRound;
    private float m_timeTakenInTotal;
    private int m_eliminatedInRound;

    [SerializeField]
    GameObject m_SpeechBubble;
    [SerializeField]
    TMP_Text m_SpeechText;
    [SerializeField]
    string m_PerformanceText;
    [SerializeField]
    string m_QuitQuote;


    // Animation properties
    private SpriteRenderer m_sr;
    private SpriteRenderer m_sbsr;
    private SpriteRenderer m_cmsr;
    private SpriteRenderer m_cmbgsr;
    private bool fadingIn; // Whether the character is currently fading in or out.
    private float fadeTime; // Amount of time that the current animation should play for.
    private float speechTime; // Amount of time that the final speech bubble stays up for before fading out can commence.
    private float elapsedTime; // Amount of time since the current animation started.

    private void Awake()
    {
        m_sr = GetComponent<SpriteRenderer>();
        m_sbsr = m_SpeechBubble.GetComponent<SpriteRenderer>();
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

        m_SpeechText.text = m_PerformanceText;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_sr = GetComponent<SpriteRenderer>();
        m_sbsr = m_SpeechBubble.GetComponent<SpriteRenderer>();
        m_cmsr = ConfidenceMeter.GetComponent<SpriteRenderer>();
        m_cmbgsr = ConfidenceMeterBackground.GetComponent<SpriteRenderer>();
        m_fCurrentConfidence = m_fMaxConfidence;
        m_SpeechText.color = new Color(0, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime <= fadeTime + speechTime)
        {
            elapsedTime += Time.deltaTime;
            CalcOpacity();
            if (m_sr.color.a <= 0.0f)
                gameObject.SetActive(false);
            if (m_sr.color.a >= 1.0f)
            {
                m_SpeechBubble.SetActive(true);
                m_SpeechText.gameObject.SetActive(true);
            }
        }

        if (speechTime > 0.0f && elapsedTime > speechTime) // speechTime is 0.0f if there is no speech to time.
        {
            elapsedTime -= speechTime;
            speechTime = 0.0f;
            m_SpeechBubble.SetActive(false);
            m_SpeechText.gameObject.SetActive(false);
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

            if (m_attacksTakenInRound == 3)
            {
                GameManager.Instance.OnSurvive(this.gameObject);
            }
        }
    }

    public int GetNumAttacks()
    {
        return m_attacksTakenInRound;
    }
    public int GetEliminatedInRound()
    {
        return m_eliminatedInRound;
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
        m_attacksTakenInRound++;
        m_attacksTakenInTotal++;
    }

    void KillSelf()
    {
        m_SpeechText.text = m_QuitQuote;
        m_eliminatedInRound = GameManager.Instance.GetCurrentRound();
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

        m_SpeechBubble.SetActive(false);
        m_SpeechText.gameObject.SetActive(false);
    }

    public void OnFinalSpeech(float finalSpeechTime)
    {
        speechTime = finalSpeechTime;
    }

    public void OnFadeOut(float secondsToFade)
    {
        fadingIn = false;
        fadeTime = secondsToFade;

        // Set elapsed time to the portion of the new animation that it would be based on the current alpha value
        elapsedTime = (1.0f - m_sr.color.a) * fadeTime;
        CalcOpacity();

        m_SpeechBubble.SetActive(true);
        m_SpeechText.gameObject.SetActive(true);
    }

    private void CalcOpacity()
    {
        if (speechTime > 0.0f)
            return;

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

    private void DisplayPerformanceText()
    {

    }

    private void DisplayQuitQuote()
    {

    }
}
