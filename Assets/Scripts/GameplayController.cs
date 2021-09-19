using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    public int itemCollectBonus = 50;

    [SerializeField]
    public int obstaclePassBonus = 100;

    [SerializeField]
    private int gameplayTimeInSeconds = 60;

    [HideInInspector]
    public float startTime;

    [HideInInspector]
    public int score = 0;

    public void restartGame()
    {
        SceneManager.LoadScene("Tunnel");
    }

    private void Update()
    {
        GameObject player = GameObject.Find("Player");
        GameObject scoreLabel = GameObject.Find("Score Label");
        GameObject remainingTimeLabel = GameObject.Find("Remaining Time Label");
        GameObject gameOverPanel = GameObject.Find("Game Over Panel");
        GameObject finalMessageLabel = GameObject.Find("Final Message Label");

        scoreLabel.GetComponent<UpdateLabel>().updateLabel($"Wynik: {score}");

        float remainingTime = player.GetComponent<PlayerMovement>().didBeginMovement ? gameplayTimeInSeconds - Time.time + startTime : gameplayTimeInSeconds;

        if (remainingTime <= 0)
        {
            player.GetComponent<Rigidbody>().isKinematic = true;
            scoreLabel.transform.localScale = Vector3.zero;
            remainingTimeLabel.transform.localScale = Vector3.zero;
            gameOverPanel.transform.localScale = Vector3.one;
            finalMessageLabel.transform.localScale = Vector3.one;
            finalMessageLabel.GetComponent<UpdateLabel>().updateLabel($"KONIEC GRY\n\nTwój wynik: {score}\n\nZa chwilê rozpocznie siê nowa gra...");

            if (remainingTime <= -5) restartGame();
        }
        else
        {
            string remainingTimeString = remainingTime.ToString();
            char delimiter;

            if (remainingTimeString.Contains(",")) delimiter = ',';
            else if (remainingTimeString.Contains(".")) delimiter = '.';
            else
            {
                remainingTimeString += ".00";
                delimiter = '.';
            }

            string[] remainingTimeStringComponents = remainingTimeString.Split(delimiter);
            string secondsString = remainingTimeStringComponents[0];

            if (secondsString.Length == 1) secondsString = "0" + secondsString;

            string milisecondsString = remainingTimeStringComponents[1].Substring(0, 2);

            remainingTimeLabel.GetComponent<UpdateLabel>().updateLabel($"{secondsString}:{milisecondsString}");
        }
    }
}