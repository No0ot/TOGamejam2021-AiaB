using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [Header("Game Parameters")]
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

    [Header("Timing Parameters")]
    [SerializeField]
    [Tooltip("The amount of time, in seconds, it takes for the actor to say their final lines before they begin to fade out.")]
    [Range(0, 10)]
    private float finalSpeechTime;
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

    [Header("Visual Parameters")]
    [SerializeField]
    [Tooltip("The amount of the screen taken up by the actors when arrayed together in the recap screen.")]
    [Range(0, 100)]
    private float recapScreenPercent;
    [SerializeField]
    [Tooltip("The amount to darken the characters to when they are eliminated. 0 is fully dark (black), 1 is fully light (normal - not white).")]
    [Range(0, 1)]
    private float darkenAmount;
    [SerializeField]
    [Tooltip("The material used when setting characters to be fully greyscale.")]
    private Material greyscaleMaterial;
    [SerializeField]
    [Tooltip("The material used when setting characters to be fully coloured in.")]
    private Material defaultMaterial;
    [SerializeField]
    [Tooltip("Reference to the character profile object.")]
    private GameObject characterProfile;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Import the number of rounds and auditions from the settings set by user in main menu
        numRounds = PlayerPrefs.GetInt("NumberOfRounds") ;
        //numAuditions = PlayerPrefs.GetInt("NumberOfAuditioners");
        numAuditions = 2;
        if (fullAuditionsList.Count < 1)
            Debug.LogError("Full Auditions List is not yet populated!");
        if (numAuditions < 1)
            Debug.LogError("Num Auditions is < 1!");
        if (numRounds < 1)
            Debug.LogError("Num Rounds is < 1!");

        SelectAuditions();

        currentRound = 0;
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
                    if (!showingRecap)
                    {
                        timeSinceAuditionStarted = 0.0f; // When this reaches fadeInTime (or immediately if CanInsultWhileAnimating is true), this will cause update to call InsultManager.Instance.OnAuditionStarted())
                        arrivingOnStage = true;
                        currentAudition.SetActive(true);
                        CastMember cm = currentAudition.GetComponent<CastMember>();
                        cm.SetSilent(false);
                        cm.OnFadeIn(fadeInTime);
                    }
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
            GameObject audition = Instantiate(availableAuditionsList[r]);
            availableAuditionsList.RemoveAt(r);

            selectedAuditionsList.Add(audition);
            audition.SetActive(false);
        }
    }

    // This function displays the recap scene at the end of the round, from which the player can then move on to the next round.
    private void ShowRecapScene()
    {
        showingRecap = true;
        List<CastMember> cmlist = new List<CastMember>();
        foreach (GameObject audition in selectedAuditionsList)
        {
            CastMember cm = audition.GetComponent<CastMember>();
            if (cm.GetEliminatedInRound() == currentRound) // Actor was eliminated this round
                cmlist.Add(cm);
            else if (cm.GetEliminatedInRound() == 0) // Actor is still in the game
                cmlist.Add(cm);
        }

        int auditionsInRound = cmlist.Count;
        float midpoint = (auditionsInRound - 1) / 2.0f;
        int i = 0;
        foreach (CastMember cm in cmlist)
        {
            cm.SetSilent(true);
            cm.OnFadeIn(1.0f);
            cm.gameObject.SetActive(true);

            float pc = recapScreenPercent / 100.0f;
            float offset = ((i - midpoint) / (auditionsInRound + 1)) * pc;
            Vector2 pos = Camera.current.ViewportToWorldPoint(new Vector3(0.5f + offset, 0.5f, 0.0f));
            cm.gameObject.transform.position = pos;

            if (cm.GetEliminatedInRound() == currentRound)
            {
                SpriteRenderer sr = cm.GetComponent<SpriteRenderer>();
                sr.material = greyscaleMaterial;
                Color c = sr.color;
                c.r *= darkenAmount;
                c.g *= darkenAmount;
                c.b *= darkenAmount;
                sr.color = c;
            }

            i++;
        }
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
        characterProfile.SetActive(true);
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
            if (auditionsLeftInRound == null)
                auditionsLeftInRound = new List<GameObject>();

            // Shallow copy the survivors list to the new round's auditions list
            foreach (GameObject audition in selectedAuditionsList)
            {
                CastMember cm = audition.GetComponent<CastMember>();
                if (!cm)
                    Debug.LogError("Selected audition prefab does not have the CastMember component!");
                else if(cm.GetEliminatedInRound() == 0)
                {
                    auditionsLeftInRound.Add(audition);
                }
            }
        }

        timeSinceAuditionEnded = auditionInterval; // Will trigger NextAudition() on next update.
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
        CastMember cm = currentAudition.GetComponent<CastMember>();
        cm.OnFinalSpeech(finalSpeechTime);
        cm.OnFadeOut(fadeOutTime);
        characterProfile.SetActive(false);
        timeSinceAuditionEnded = 0.0f;
    }
    public void OnEliminate(GameObject castMember)
    {
        InsultManager.Instance.OnAuditionEnded();
        CastMember cm = currentAudition.GetComponent<CastMember>();
        cm.OnFinalSpeech(finalSpeechTime);
        cm.OnFadeOut(fadeOutTime);
        characterProfile.SetActive(false);
        timeSinceAuditionEnded = 0.0f;
    }
}
