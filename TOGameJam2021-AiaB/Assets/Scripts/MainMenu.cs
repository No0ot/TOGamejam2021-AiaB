using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //these text objects will be used to update UI of settings menu
    [SerializeField] Text NumberOfEnemiesText;
    [SerializeField] Text NumberOfRoundsText;    
    private int _numberOfEnemies = 4; //number of characters auditioning
    private int _numberOfRounds = 3; //number of gameplay rounds
    public int NumberOfEnemies
    {
        get { return _numberOfEnemies; }
        set
        {
            _numberOfEnemies = value;
            NumberOfEnemiesText.text = _numberOfEnemies.ToString();
        }

    }
    public int NumberOfRounds
    {
        get { return _numberOfRounds; }
        set
        {
            _numberOfRounds = value;
            NumberOfRoundsText.text = _numberOfRounds.ToString();
        }
    }

    public void IncreaseNumberOfEnemies()
    {
        if (NumberOfEnemies < 8)
            NumberOfEnemies++;
    }
    public void DecreaseNumberOfEnemies()
    {
        if (NumberOfEnemies > 4)
            NumberOfEnemies--;
    }
    public void IncreaseNumberOfRounds()
    {
        if (NumberOfRounds < 8) 
            NumberOfRounds++;
    }
    public void DecreaseNumberOfRounds()
    {
        if (NumberOfRounds >3)
            NumberOfRounds--;
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("NumberOfRounds", NumberOfRounds);
        PlayerPrefs.SetInt("NumberOfAuditioners", NumberOfEnemies);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
        Debug.Log("Placeholder for game quitting");
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
