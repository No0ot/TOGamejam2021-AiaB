using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField]
    [Tooltip("Complete list of all potential auditions, from which a random selection will be chosen.")]
    private List<CastMember> fullAuditionsList;
    private List<CastMember> selectedAuditionsList;
    private List<CastMember> survivingAuditionsList;
    [SerializeField]
    [Tooltip("The number of auditions that will be chosen from the full list, must be less than the number of auditions in the list.")]
    private int numAuditions;
    private CastMember currentAudition;
    [SerializeField]
    [Tooltip("The number of rounds that the player has in which to knock out all of the auditions.")]
    private int numRounds;
    private int currentRound;
    private List<CastMember> auditionsLeftInRound;
    private List<List<CastMember>> auditionsEliminatedByRound;

    void Start()
    {
        SelectAuditions();

        currentRound = -1;
        NextRound();
    }

    // This function is to select the initial pool of auditions that will be in the game from the total list of all available characters.
    private void SelectAuditions()
    {
        numAuditions = Mathf.Min(numAuditions, fullAuditionsList.Count);
        List<CastMember> availableAuditionsList = fullAuditionsList;
        selectedAuditionsList = new List<CastMember>(numAuditions);
        
        for (int i = 0; i < numAuditions; i++)
        {
            int r = Random.Range(0, availableAuditionsList.Count);
            selectedAuditionsList[i] = availableAuditionsList[r];
            availableAuditionsList.RemoveAt(r);
        }

        survivingAuditionsList = selectedAuditionsList;
    }

    // Returns the currently auditioning character.
    public CastMember GetCurrentAudition()
    {
        return currentAudition;
    }

    private void ShowRecapScene()
    {
        Debug.Log("ShowRecapScene function not yet implemented.");
    }

    // This function is to select the next audition who will be shown, from the list of auditions remaining in the round.
    private void NextAudition()
    {
        if(auditionsLeftInRound.Count < 1)
            ShowRecapScene();

        int r = Random.Range(0, auditionsLeftInRound.Count);
        currentAudition = auditionsLeftInRound[r];
        auditionsLeftInRound.RemoveAt(r);
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
            auditionsEliminatedByRound.Add(new List<CastMember>());
            auditionsLeftInRound = survivingAuditionsList;
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
}
