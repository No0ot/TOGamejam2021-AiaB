using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum InsultType
{
    NONE,
    ACTING,
    COSTUME,
    INTEREST,
    SILENCE,
    NUM_OF_INSULTS
}

public class InsultManager : MonoBehaviour
{
    private static InsultManager instance;
    public static InsultManager Instance { get { return instance; } }

    public GameObject InsultPrefab;
    [SerializeField]
    GameObject m_OverallUICanvas;
    [SerializeField]
    GameObject m_UICanvas;
    [SerializeField]
    GameObject[] m_InsultCards;
    [SerializeField]
    GameObject m_BackButton;

    [SerializeField]
    bool[] m_ActingInsultUsed = { false,false, false, false};
    [SerializeField]
    bool[] m_CostumeInsultUsed = { false, false, false, false };
    [SerializeField]
    bool[] m_InterestInsultUsed = { false, false, false, false };

    [SerializeField]
    string[] VoiceInsults;
    [SerializeField]
    string[] ShowmanshipInsults;
    [SerializeField]
    string[] AccuracyInsults;
    [SerializeField]
    string[] ExperienceInsults;
    [SerializeField]
    string[] StyleInsults;
    [SerializeField]
    string[] ShoesInsults;
    [SerializeField]
    string[] HairInsults;
    [SerializeField]
    string[] ShirtInsults;
    [SerializeField]
    string[] HobbyInsults;
    [SerializeField]
    string[] TheatreGenreInsults;
    [SerializeField]
    string[] FoodInsults;
    [SerializeField]
    string[] AcademicsInsults;
    [SerializeField]
    TMP_Text InsultText;
    [SerializeField]
    GameObject InsultSpeechBubble;
    [SerializeField]
    private float TimePerInsult;
    private float TimeRemaining;
    [SerializeField]
    private float TimeForResponse;
    private float TimeRemainingForResponse;
    private bool CountingDown;
    [SerializeField]
    Image TimeSprite;
    [SerializeField]
    Sprite[] StopWatchSprites;

    InsultType insultType;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CountingDown = false;
        InsultText.color = new Color(0, 0, 0, 1);
        TimeRemaining = TimePerInsult;
    }

    private void Update()
    {
        if (TimeRemainingForResponse > 0.0f)
        {
            InsultSpeechBubble.SetActive(true);
            m_BackButton.SetActive(false);

            TimeRemainingForResponse -= Time.deltaTime;
            if (TimeRemainingForResponse <= 0.0f)
            {
                InsultSpeechBubble.SetActive(false);
                if (!m_UICanvas.activeInHierarchy && m_OverallUICanvas.activeInHierarchy)
                    m_BackButton.SetActive(true);

                GameManager.Instance.GetCurrentAudition().GetComponent<CastMember>().OnRelax();
                TimeRemaining = TimePerInsult;
            }
        }
        else if (CountingDown)
            TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0.0f)
        {
            if (!InsultSpeechBubble.activeInHierarchy)
                InsultSpeechBubble.SetActive(true);
            InsultText.text = "...";

            GameManager.Instance.GetCurrentAudition().GetComponent<CastMember>().TakeDamage(InsultDamageType.NO_INSULT_GIVEN);

            TimeRemaining = TimePerInsult;
        }

        //TimeText.text = ((int)(TimeRemaining)).ToString();
        TimeSprite.sprite = StopWatchSprites[Mathf.Clamp((int)TimeRemaining, 0, 10)];
    }

    public void OnInsultGiven()
    {
        TimeRemainingForResponse = TimeForResponse;
    }

    public void OnStartCoundown()
    {
        CountingDown = true;
    }

    public void OnStopCountdown()
    {
        CountingDown = false;
        TimeRemaining = TimePerInsult;
    }
    public void DisplayActingInsults()
    {
        insultType = InsultType.ACTING;
        int[] tempeffectiveness = GameManager.Instance.GetCurrentAudition().GetComponent<CastMember>().m_ActingEffectiveness;

        if (!m_ActingInsultUsed[0])
        {
            m_InsultCards[0].GetComponent<Insult>().setText(" Insult Voice");
            m_InsultCards[0].SetActive(true);
        }
        if (!m_ActingInsultUsed[1])
        {
            m_InsultCards[1].GetComponent<Insult>().setText(" Insult Showmanship");
            m_InsultCards[1].SetActive(true);
        }
        if (!m_ActingInsultUsed[2])
        {
            m_InsultCards[2].GetComponent<Insult>().setText(" Insult Accuracy");
            m_InsultCards[2].SetActive(true);
        }
        if (!m_ActingInsultUsed[3])
        {
            m_InsultCards[3].GetComponent<Insult>().setText(" Insult Experience");
            m_InsultCards[3].SetActive(true);
        }

        for(int i = 0; i < 4; i++ )
        {
            m_InsultCards[i].GetComponent<Insult>().setDamageType(tempeffectiveness[i]);
        }

        m_UICanvas.SetActive(false);
        m_BackButton.SetActive(true);
    }

    public void DisplayCostumeInsults()
    {
        insultType = InsultType.COSTUME;
        int[] tempeffectiveness = GameManager.Instance.GetCurrentAudition().GetComponent<CastMember>().m_CostumeEffectiveness;

        if (!m_CostumeInsultUsed[0])
        {
            m_InsultCards[0].GetComponent<Insult>().setText(" Insult hat");
            m_InsultCards[0].SetActive(true);
        }
        if (!m_CostumeInsultUsed[1])
        {
            m_InsultCards[1].GetComponent<Insult>().setText(" Insult shoes");
            m_InsultCards[1].SetActive(true);
        }
        if (!m_CostumeInsultUsed[2])
        {
            m_InsultCards[2].GetComponent<Insult>().setText(" Insult hair");
            m_InsultCards[2].SetActive(true);
        }
        if (!m_CostumeInsultUsed[3])
        {
            m_InsultCards[3].GetComponent<Insult>().setText(" Insult shirt");
            m_InsultCards[3].SetActive(true);
        }

        for (int i = 0; i < 4; i++)
        {
            m_InsultCards[i].GetComponent<Insult>().setDamageType(tempeffectiveness[i]);
        }

        m_UICanvas.SetActive(false);
        m_BackButton.SetActive(true);
    }
    public void DisplayInterestsInsults()
    {
        insultType = InsultType.INTEREST;
        int[] tempeffectiveness = GameManager.Instance.GetCurrentAudition().GetComponent<CastMember>().m_InterestEffectiveness;
        if (!m_InterestInsultUsed[0])
        {
            m_InsultCards[0].GetComponent<Insult>().setText(" Insult Hobby");
            m_InsultCards[0].SetActive(true);
        }
        if (!m_InterestInsultUsed[1])
        {
            m_InsultCards[1].GetComponent<Insult>().setText(" Insult Theatre genre");
            m_InsultCards[1].SetActive(true);
        }
        if (!m_InterestInsultUsed[2])
        {
            m_InsultCards[2].GetComponent<Insult>().setText(" Insult Favorite food");
            m_InsultCards[2].SetActive(true);
        }
        if (!m_InterestInsultUsed[3])
        {
            m_InsultCards[3].GetComponent<Insult>().setText(" Insult favorite subject");
            m_InsultCards[3].SetActive(true);
        }

        for (int i = 0; i < 4; i++)
        {
            m_InsultCards[i].GetComponent<Insult>().setDamageType(tempeffectiveness[i]);
        }

        m_UICanvas.SetActive(false);
        m_BackButton.SetActive(true);
    }

    public void ResetInsults()
    {
        m_UICanvas.SetActive(true);
        m_BackButton.SetActive(false);
        foreach (GameObject insult in m_InsultCards)
        {
            insult.SetActive(false);
        }
        insultType = InsultType.NONE;
    }

    public void UseInsult(GameObject usedinsult)
    {
        if(!InsultSpeechBubble.activeInHierarchy)
        {
            InsultSpeechBubble.SetActive(true);
        }

        switch (insultType)
        {
            case InsultType.ACTING:
                for (int i = 0; i < 4; i++)
                {
                    if (usedinsult == m_InsultCards[i])
                    {
                        m_ActingInsultUsed[i] = true;
                        //Debug.Log("ACTINGINSULT" + i);
                        int r = 0;
                        switch(i)
                        {
                            case 0:
                                r = Random.Range(0, VoiceInsults.Length);
                                InsultText.text = VoiceInsults[r];
                                break;
                            case 1:
                                r = Random.Range(0, ShowmanshipInsults.Length);
                                InsultText.text = ShowmanshipInsults[r];
                                break;
                            case 2:
                                r = Random.Range(0, AccuracyInsults.Length);
                                InsultText.text = AccuracyInsults[r];
                                break;
                            case 3:
                                r = Random.Range(0, ExperienceInsults.Length);
                                InsultText.text = ExperienceInsults[r];
                                break;
                            default: 
                                break;
                        }
                    }
                }
                break;
            case InsultType.COSTUME:
                for (int i = 0; i < 4; i++)
                {
                    if (usedinsult == m_InsultCards[i])
                    {
                        m_CostumeInsultUsed[i] = true;
                        //Debug.Log("COSTUMEINSULT" + i);
                        int r = 0;
                        switch (i)
                        {
                            case 0:
                                r = Random.Range(0, StyleInsults.Length);
                                InsultText.text = StyleInsults[r];
                                break;
                            case 1:
                                r = Random.Range(0, ShoesInsults.Length);
                                InsultText.text = ShoesInsults[r];
                                break;
                            case 2:
                                r = Random.Range(0, HairInsults.Length);
                                InsultText.text = HairInsults[r];
                                break;
                            case 3:
                                r = Random.Range(0, ShirtInsults.Length);
                                InsultText.text = ShirtInsults[r];
                                break;
                            default:
                                break;
                        }
                    }
                }
                break;
            case InsultType.INTEREST:
                for (int i = 0; i < 4; i++)
                {
                    if (usedinsult == m_InsultCards[i])
                    {
                        m_InterestInsultUsed[i] = true;
                        //Debug.Log("INTERESTINSULT" + i);
                        int r = 0;
                        switch (i)
                        {
                            case 0:
                                r = Random.Range(0, HobbyInsults.Length);
                                InsultText.text = HobbyInsults[r];
                                break;
                            case 1:
                                r = Random.Range(0, TheatreGenreInsults.Length);
                                InsultText.text = TheatreGenreInsults[r];
                                break;
                            case 2:
                                r = Random.Range(0, FoodInsults.Length);
                                InsultText.text = FoodInsults[r];
                                break;
                            case 3:
                                r = Random.Range(0, AcademicsInsults.Length);
                                InsultText.text = AcademicsInsults[r];
                                break;
                            default:
                                break;
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    public void OnAuditionEnded()
    {
        ResetInsults();
        m_OverallUICanvas.SetActive(false);
        if (InsultSpeechBubble.activeInHierarchy)
        {
            InsultSpeechBubble.SetActive(false);
        }
    }

    public void OnRoundEnded()
    {
        for (int i = 0; i < m_ActingInsultUsed.Length; i++)
        {
            m_ActingInsultUsed[i] = false;
            m_CostumeInsultUsed[i] = false;
            m_InterestInsultUsed[i] = false;
        }
        if (InsultSpeechBubble.activeInHierarchy)
        {
            InsultSpeechBubble.SetActive(false);
        }
    }

    public void OnAuditionStarted()
    {
        InsultSpeechBubble.SetActive(false);
        m_OverallUICanvas.SetActive(true);
        TimeRemaining = TimePerInsult;
        CountingDown = false;
    }
}
