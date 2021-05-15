using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsultManager : MonoBehaviour
{
    private static InsultManager instance;
    public static InsultManager Instance { get { return instance; } }

    public GameObject InsultPrefab;

    [SerializeField]
    GameObject m_UICanvas;
    [SerializeField]
    GameObject[] m_InsultCards;
    [SerializeField]
    GameObject m_BackButton;
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

        int[] tempeffectiveness = GameManager.Instance.GetCurrentAudition().GetComponent<CastMember>().m_ActingEffectiveness;
        m_InsultCards[0].GetComponent<Insult>().setText(" Insult Voice");
        m_InsultCards[1].GetComponent<Insult>().setText(" Insult Showmanship");
        m_InsultCards[2].GetComponent<Insult>().setText(" Insult Accuracy");
        m_InsultCards[3].GetComponent<Insult>().setText(" Insult Experience");

        for(int i = 0; i < 4; i++ )
        {
            m_InsultCards[i].GetComponent<Insult>().setDamageType(tempeffectiveness[i]);
            m_InsultCards[i].SetActive(true);
        }

        m_UICanvas.SetActive(false);
        m_BackButton.SetActive(true);
    }

    public void DisplayCostumeInsults()
    {
        int[] tempeffectiveness = GameManager.Instance.GetCurrentAudition().GetComponent<CastMember>().m_CostumeEffectiveness;
        m_InsultCards[0].GetComponent<Insult>().setText(" Insult hat");
        m_InsultCards[1].GetComponent<Insult>().setText(" Insult shoes");
        m_InsultCards[2].GetComponent<Insult>().setText(" Insult hair");
        m_InsultCards[3].GetComponent<Insult>().setText(" Insult shirt");

        for (int i = 0; i < 4; i++)
        {
            m_InsultCards[i].GetComponent<Insult>().setDamageType(tempeffectiveness[i]);
            m_InsultCards[i].SetActive(true);
        }

        m_UICanvas.SetActive(false);
        m_BackButton.SetActive(true);
    }
    public void DisplayInterestsInsults()
    {
        int[] tempeffectiveness = GameManager.Instance.GetCurrentAudition().GetComponent<CastMember>().m_InterestEffectiveness;
        m_InsultCards[0].GetComponent<Insult>().setText(" Insult Hobby");
        m_InsultCards[1].GetComponent<Insult>().setText(" Insult Theatre genre");
        m_InsultCards[2].GetComponent<Insult>().setText(" Insult Favorite food");
        m_InsultCards[3].GetComponent<Insult>().setText(" Insult favorite subject");

        for (int i = 0; i < 4; i++)
        {
            m_InsultCards[i].GetComponent<Insult>().setDamageType(tempeffectiveness[i]);
            m_InsultCards[i].SetActive(true);
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
    }
}
