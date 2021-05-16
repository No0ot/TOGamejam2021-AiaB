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
    private GameObject currentAudition;
    private List<GameObject> auditionsLeftInRound;
    [SerializeField]
    [Tooltip("The number of auditions that will be chosen from the full list, must be less than the number of auditions in the list.")]
    [Range(1, 100)]
    private int numAuditions;
    [SerializeField]
    [Tooltip("The number of rounds that the player has in which to knock out all of the auditions.")]
    [Range(1, 100)]
    private int numRounds;
    private int currentRound;
    [SerializeField]
    [Tooltip("The amount of time, in seconds, it takes for an actor to fade from view after their audition ends.")]
    [Range(0, 10)]
    private float fadeOutTime;
    [SerializeField]
    [Tooltip("The amount of time, in seconds, it takes for an actor to fade into view after their audition starts.")]
    [Range(0, 10)]
    private float fadeInTime;
    [SerializeField]
    [Tooltip("The amount of time, in seconds, from the end of one audition until the next one starts. Note that if this is less than the value of the Fade Out Time, there may be two actors on stage at once.")]
    [Range(0, 20)]
    private float auditionInterval;
    private float timeSinceAuditionStarted;
    private float timeSinceAuditionEnded;
    [SerializeField]
    [Tooltip("Can the player issue insults to the new actor while they are still fading into view?")]
    private bool canInsultWhileStillArriving;
    private bool arrivingOnStage;
    private bool showingRecap;

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

    private void Update()
    {
        if (!showingRecap)
        {
            // After the audition ends, timeSinceAuditionEnded is set to 0. When this value exceeds auditionInterval, the new actor starts arriving on stage whether or not the last one has finished leaving yet.
            if (timeSinceAuditionEnded <= auditionInterval)
            {
                timeSinceAuditionEnded += Time.deltaTime;

                if (timeSinceAuditionEnded > auditionInterval)
                {
                    NextAudition();
                    timeSinceAuditionStarted = 0.0f; // When this reaches fadeInTime (or immediately if CanInsultWhileAnimating is true), this will cause update to call InsultManager.Instance.OnAuditionStarted())
                    arrivingOnStage = true;
                    currentAudition.SetActive(true);
                    currentAudition.GetComponent<CastMember>().OnFadeIn(fadeInTime);
                }
            }
            // The TimeSinceAuditionStarted does not tick up after the end of the previous audition, but it does not reset until the start of the new one (when the new actor starts fading in - after the auditionInterval).
            // Once TimeSinceAuditionStarted has ticked up past fadeInTime (or immediately if canInsultWhileStillArriving is true), InsultManager.Instance.OnAuditionStarted() is called. arrivingOnStage exists to ensure this only happens once per audition.
            else
            {
                timeSinceAuditionStarted += Time.deltaTime;

                if (arrivingOnStage && (canInsultWhileStillArriving || timeSinceAuditionStarted > fadeInTime))
                {
                    arrivingOnStage = false;
                    InsultManager.Instance.OnAuditionStarted();
                }
            }
        }
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
    }

    // This function displays the recap scene at the end of the round, from which the player can then move on to the next round.
    private void ShowRecapScene()
    {
        showingRecap = true;
        Debug.Log("ShowRecapScene function not yet implemented.");
    }


    // This function is to select the next audition who will be shown, from the list of auditions remaining in the round.
    private void NextAudition()
    {
        // Check if the round is now over
        if(auditionsLeftInRound.Count < 1)
        {
            InsultManager.Instance.OnRoundEnded();
            ShowRecapScene();
            return;
        }

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

        // Set any relevant visuals
        currentAudition.transform.position = Vector2.zero;
    }

    // This function is to proceed to the next round. It will create a new list of auditions for the start of the round
    // from the list of auditions who survived the last one. In this way, a record is kept of who was eliminated in each round.
    private void NextRound()
    {
        currentRound++;
        showingRecap = false;
        if (currentRound > numRounds)
        {
            LoseGame();
            return;
        }
        else
        {
            // Ensure that the relevant lists are instantiated.
            //if (auditionsEliminatedByRound == null)
            //    auditionsEliminatedByRound = new List<List<GameObject>>();

            // Add the new eliminations list for the new round
            //auditionsEliminatedByRound.Add(new List<GameObject>());

            // Shallow copy the survivors list to the new round's auditions list
            //foreach (GameObject audition in survivingAuditionsList)
            //    auditionsLeftInRound.Add(audition);
            //survivingAuditionsList.Clear();

            // Fill/Refill the auditions left in round with all the auditions not eliminated from the previous one.
            if (auditionsLeftInRound == null)
                auditionsLeftInRound = new List<GameObject>();
            foreach (GameObject audition in selectedAuditionsList)
            {
                CastMember cm = audition.GetComponent<CastMember>();
                if (cm)
                {
                    if (cm.GetEliminatedInRound() == 0)
                        auditionsLeftInRound.Add(audition);
                }
            }
        }

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

    public int GetCurrentRound()
    {
        return currentRound;
    }

    // Callbacks
    public void OnSurvive(GameObject castMember)
    {
        InsultManager.Instance.OnAuditionEnded();
        currentAudition.GetComponent<CastMember>().OnFadeOut(fadeOutTime);
        timeSinceAuditionEnded = 0.0f;
    }
    public void OnEliminate(GameObject castMember)
    {
        InsultManager.Instance.OnAuditionEnded();
        currentAudition.GetComponent<CastMember>().OnFadeOut(fadeOutTime);
        timeSinceAuditionEnded = 0.0f;
    }
}
