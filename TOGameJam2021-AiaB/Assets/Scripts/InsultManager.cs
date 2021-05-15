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
        m_InsultCards[0].GetComponent<Insult>().setText(" Insult Voice");
        m_InsultCards[1].GetComponent<Insult>().setText(" Insult Body Language");
        m_InsultCards[2].GetComponent<Insult>().setText(" Insult Enunciation");
        m_InsultCards[3].GetComponent<Insult>().setText(" Insult Emotion");
        foreach (GameObject insult in m_InsultCards )
        {
            insult.SetActive(true);
        }

        m_UICanvas.SetActive(false);
        m_BackButton.SetActive(true);
    }

    public void DisplayCostumeInsults()
    {
        m_InsultCards[0].GetComponent<Insult>().setText(" Insult hat");
        m_InsultCards[1].GetComponent<Insult>().setText(" Insult shoes");
        m_InsultCards[2].GetComponent<Insult>().setText(" Insult hair");
        m_InsultCards[3].GetComponent<Insult>().setText(" Insult shirt");
        foreach (GameObject insult in m_InsultCards)
        {
            insult.SetActive(true);
        }
        m_UICanvas.SetActive(false);
        m_BackButton.SetActive(true);
    }
    public void DisplayInterestsInsults()
    {
        // this one might have to read from the current audition

        m_InsultCards[0].GetComponent<Insult>().setText(" Insult 1");
        m_InsultCards[1].GetComponent<Insult>().setText(" Insult 2");
        m_InsultCards[2].GetComponent<Insult>().setText(" Insult 3");
        m_InsultCards[3].GetComponent<Insult>().setText(" Insult 4");
        foreach (GameObject insult in m_InsultCards)
        {
            insult.SetActive(true);
        }
        m_UICanvas.SetActive(false);
        m_BackButton.SetActive(true);
    }

    public void DisplayExperienceInsults()
    {
        m_InsultCards[0].GetComponent<Insult>().setText(" Not Enough Musicals");
        m_InsultCards[1].GetComponent<Insult>().setText(" Not enough interpretive dance");
        m_InsultCards[2].GetComponent<Insult>().setText(" Not enough Shakespeare");
        m_InsultCards[3].GetComponent<Insult>().setText(" Not enough Improv");
        foreach (GameObject insult in m_InsultCards)
        {
            insult.SetActive(true);
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
