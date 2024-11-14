
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : Menu
{
   public static GameOverMenu instance=null;
    public Text scoreReaot = null;
    public Text hiScoreReaot = null;
    void Start()
    {
        if (instance)
        {
            Debug.LogError("Trying to create more than one GameOver menu!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Update is called once per frame
    public void OnContinueButton()
    {
        if (ScoreManager.instance.IsHiScore(GameManager.Instance.playerDatas[0].score, (int)GameManager.Instance.gameSession.hardness))
        {
            ScoreManager.instance.AddScore(GameManager.Instance.playerDatas[0].score, (int)GameManager.Instance.gameSession.hardness,"Player");

            ScoreManager.instance.SaveScore();
        }


            SceneManager.LoadScene("MainMenusScene");
    }
    public void GameOver()
    {
        TurnOn(null);
        AudioManager.instance.PlayMusic(AudioManager.Tracks.GameOver,true,0.5f);
        scoreReaot.text = GameManager.Instance.playerDatas[0].score.ToString();// for sceond player

        if (ScoreManager.instance.IsHiScore(GameManager.Instance.playerDatas[0].score,(int)GameManager.Instance.gameSession.hardness))
        {
            hiScoreReaot.gameObject.SetActive(true);

        }
        else
        {
            hiScoreReaot.gameObject.SetActive(false);
        }

    }
}
