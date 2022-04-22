using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Se accede a todos los elementos del UI, tanto paneles como textos para informacion del jugador.
    public GameObject panelInicio, panelPerder, particleExplotion;
    public Text bestScoreInicio, scoreText, gamesPlayedInicio, loseScoreText, bestScoreLose, gamesPlayedLose;

    [HideInInspector]
    public int score;
    private int gamesPlayed;

    public static GameManager instance;

    void Awake()
    {
        //Se utilizan playerprefs para guardar las partidas y el puntaje maximo, asi se le refleja al jugador en el lobby y en el final.
        instance = this;
        gamesPlayed = PlayerPrefs.GetInt("Partidas");
        gamesPlayedInicio.text = "GAMES PLAYED: " + gamesPlayed.ToString();
        bestScoreInicio.text = "BEST SCORE: " + PlayerPrefs.GetInt("Hiscore").ToString();
    }

    public void StartGame()
    {
        panelInicio.SetActive(false);
    }

    public void AddPoint()
    {
        //Puntaje que se muestra al fondo del jugador, sobre un world canvas, para que este detras del jugador.
        score++;
        scoreText.text = score.ToString("D2");
    }

    public void Lose()
    {
        //Al perder lo que mas se hace es entregarle informacion al jugador, de su puntaje, partidas jugadas y su maximo puntaje, y es donde se hacen los guardados del playerprefs.
        panelPerder.SetActive(true);
        //loseScoreText.text = score.ToString() + "\nPOINTS";
        gamesPlayed++;
        PlayerPrefs.SetInt("Partidas", gamesPlayed);
        gamesPlayedLose.text = "GAMES PLAYED: " + gamesPlayed.ToString();

        if(score > PlayerPrefs.GetInt("Hiscore"))
        {
            PlayerPrefs.SetInt("Hiscore", score);
        }
        bestScoreLose.text = "BEST SCORE: " + PlayerPrefs.GetInt("Hiscore").ToString();
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(0); //Reinicia nivel.
    }

    public void ExplodeObstacle(Vector3 position)
    {
        Instantiate(particleExplotion,position,Quaternion.identity);     
       
    }
}
