using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField]
    [Tooltip("Should the order in which the auditions are presented be randomized each round?")]
    private bool randomizeAuditionOrder;
    [SerializeField]
    [Tooltip("Complete list of all potential auditions, from which a random selection will be chosen.")]
    private List<GameObject> fullAuditionsList;
    private List<GameObject> selectedAuditionsList;
    private List<GameObject> survivingAuditionsList;
    [SerializeField]
    [Tooltip("The number of auditions that will be chosen from the full list, must be less than the number of auditions in the list.")]
    [Range(1, 100)]
    private int numAuditions;
    private GameObject currentAudition;
    [SerializeField]
    [Tooltip("The number of rounds that the player has in which to knock out all of the auditions.")]
    [Range(1, 100)]
    private int numRounds;
    private int currentRound;
    private List<GameObject> auditionsLeftInRound;
    private List<List<GameObject>> auditionsEliminatedByRound;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Import the number of rounds and auditions from the settings set by user in main menu
        numRounds = PlayerPrefs.GetInt("NumberOfRounds") ;
        numAuditions = PlayerPrefs.GetInt("NumberOfAuditioners");
        if (fullAuditionsList.Count < 1)
            Debug.LogError("Full Auditions List is not yet populated!");
        if (numAuditions < 1)
            Debug.LogError("Num Auditions is < 1!");
        if (numRounds < 1)
            Debug.LogError("Num Rounds is < 1!");

        SelectAuditions();

        currentRound = -1;
        NextRound();
    }


    // ----- ----- ----- ----- ----- Game Loop functions ----- ----- ----- ----- -----


    // This function is to select the initial pool of auditions that will be in the game from the total list of all available characters.
    private void SelectAuditions()
    {
        numAuditions = Mathf.Min(numAuditions, fullAuditionsList.Count);
        List<GameObject> availableAuditionsList = new List<GameObject>();
        foreach (GameObject audition in fullAuditionsList)
            availableAuditionsList.Add(audition);
        
        selectedAuditionsList = new List<GameObject>();
        for (int i = 0; i < numAuditions; i++)
        {
            int r = Random.Range(0, availableAuditionsList.Count);
            selectedAuditionsList.Add(availableAuditionsList[r]);
            availableAuditionsList.RemoveAt(r);
        }

        survivingAuditionsList = new List<GameObject>();
        foreach (GameObject selectedAudition in selectedAuditionsList)
        {
            GameObject audition = Instantiate(selectedAudition);
            survivingAuditionsList.Add(audition);
            audition.SetActive(false);
        }
    }

    // This function displays the recap scene at the end of the round, from which the player can then move on to the next round.
    private void ShowRecapScene()
    {
        Debug.Log("ShowRecapScene function not yet implemented.");
    }

    // This function is to select the next audition who will be shown, from the list of auditions remaining in the round.
    private void NextAudition()
    {
        // Deactivate the previous audition, if it wasn't already inactive
        if (currentAudition != null)
            currentAudition.SetActive(false);

        // Check if the round is now over
        if(auditionsLeftInRound.Count < 1)
            ShowRecapScene();

        // Pick the next audition
        if (randomizeAuditionOrder)
        {
            int r = Random.Range(0, auditionsLeftInRound.Count);
            currentAudition = auditionsLeftInRound[r];
            auditionsLeftInRound.RemoveAt(r);
        }
        else
        {
            currentAudition = auditionsLeftInRound[0];
            auditionsLeftInRound.RemoveAt(0);
        }

        // Activate the new audition
        currentAudition.SetActive(true);

        // Set any relevant visuals
        currentAudition.transform.position = Vector2.zero;
    }

    // This function is to proceed to the next round. It will create a new list of auditions for the start of the round
    // from the list of auditions who survived the last one. In this way, a record is kept of who was eliminated in each round.
    private void NextRound()
    {
        currentRound++;
        if (survivingAuditionsList.Count < 1)
        {
            WinGame();
            return;
        }
        if (currentRound > numRounds)
        {
            LoseGame();
            return;
        }
        else
        {
            // Ensure that the relevant lists are instantiated.
            if (auditionsEliminatedByRound == null)
                auditionsEliminatedByRound = new List<List<GameObject>>();
            if (auditionsLeftInRound == null)
                auditionsLeftInRound = new List<GameObject>();

            // Add the new eliminations list for the new round
            auditionsEliminatedByRound.Add(new List<GameObject>());

            // Shallow copy the survivors list to the new round's auditions list
            foreach (GameObject audition in survivingAuditionsList)
                auditionsLeftInRound.Add(audition);
            survivingAuditionsList.Clear();
        }

        //InsultManager.Instance.OnNewRound();
        NextAudition();
    }

    private void LoseGame()
    {
        Debug.Log("LoseGame function not yet implemented.");
    }
    private void WinGame()
    {
        Debug.Log("WinGame function not yet implemented.");
    }


    // ----- ----- ----- ----- ----- Public functions ----- ----- ----- ----- -----


    // Returns the currently auditioning character.
    public GameObject GetCurrentAudition()
    {
        return currentAudition;
    }

    // Callbacks
    public void OnSurvive(GameObject castMember)
    {
        castMember.SetActive(false);
        survivingAuditionsList.Add(castMember);
    }
    public void OnEliminate(GameObject castMember)
    {
        castMember.SetActive(false);
        auditionsEliminatedByRound[auditionsEliminatedByRound.Count - 1].Add(castMember);
    }
    public void OnEliminate(CastMember castMemberc)
    {
        GameObject castMember = castMemberc.gameObject;
        castMember.SetActive(false);
        auditionsEliminatedByRound[auditionsEliminatedByRound.Count - 1].Add(castMember);
    }
}
