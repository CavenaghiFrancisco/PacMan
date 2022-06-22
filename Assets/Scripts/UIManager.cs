using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private int points = 0;
    private int lifes = 3;
    [SerializeField] private TMPro.TextMeshProUGUI pointsText;
    [SerializeField] private TMPro.TextMeshProUGUI pauseBttnText;
    [SerializeField] private List<GameObject> lifesON;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private void Start()
    {
        GameManager.OnAddedPoints += AddPoints;
        GameManager.OnLifeLose += LoseLife;
        GameManager.OnWin += ShowWin;
        GameManager.OnLose += ShowLose;
    }

    private void OnDestroy()
    {
        GameManager.OnAddedPoints -= AddPoints;
        GameManager.OnLifeLose -= LoseLife;
        GameManager.OnWin -= ShowWin;
        GameManager.OnLose -= ShowLose;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("PacmanGame"))
        {
            pointsText.text = points.ToString();
            pauseBttnText.text = Time.timeScale == 1 ? "PAUSE" : "RESUME";
            switch (lifes)
            {
                case 3:
                    lifesON[0].SetActive(true);
                    lifesON[1].SetActive(true);
                    lifesON[2].SetActive(true);
                    break;
                case 2:
                    lifesON[0].SetActive(true);
                    lifesON[1].SetActive(true);
                    lifesON[2].SetActive(false);
                    break;
                case 1:
                    lifesON[0].SetActive(true);
                    lifesON[1].SetActive(false);
                    lifesON[2].SetActive(false);
                    break;
                case 0:
                    lifesON[0].SetActive(false);
                    lifesON[1].SetActive(false);
                    lifesON[2].SetActive(false);
                    break;
                default:
                    break;
            }
        } 
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void TooglePause()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void AddPoints(int point)
    {
        points += point;
    }

    private void LoseLife(int lifesLeft)
    {
        lifes = lifesLeft;
    }

    private void ShowLose()
    {
        losePanel.SetActive(true);
    }

    private void ShowWin()
    {
        winPanel.SetActive(true);
    }
}
