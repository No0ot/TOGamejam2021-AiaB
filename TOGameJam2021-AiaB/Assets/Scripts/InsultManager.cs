using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InsultType
{
    NONE,
    ACTING,
    COSTUME,
    INTEREST,
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

    InsultType insultType;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

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
        switch (insultType)
        {
            case InsultType.ACTING:
                for (int i = 0; i < 4; i++)
                {
                    if (usedinsult == m_InsultCards[i])
                    {
                        m_ActingInsultUsed[i] = true;
                        //Debug.Log("ACTINGINSULT" + i);

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
    }

    public void OnRoundEnded()
    {
        for (int i = 0; i < m_ActingInsultUsed.Length; i++)
        {
            m_ActingInsultUsed[i] = false;
            m_CostumeInsultUsed[i] = false;
            m_InterestInsultUsed[i] = false;
        }
    }

    public void OnAuditionStarted()
    {
        m_OverallUICanvas.SetActive(true);
    }
}
