using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  public GameObject [] hazards;
  public Vector3 spawnValues;
  public int hazardCount;
  public float spawnWait;
  public float startWait;
  public float waveWait;

  public AudioSource bgMusic;
  public AudioSource winMusic;
  public AudioSource loseMusic;

  public Text pointsText;
  public Text restartText;
  public Text gameOverText;
  public Text hardModeText;
  public GameObject backgroundScroll;

  private int points;
  private bool gameOver;
  private bool restart;
  private bool hardMode;

  void Start()
  {
    points = 0;
    gameOver = false;
    restart = false;
    hardMode = false;
    restartText.text = "";
    gameOverText.text = "";
    hardModeText.text = "Hard Mode Off.";
    UpdateScore();
    StartCoroutine(SpawnWaves());
  }

  void Update()
  {
      if (Input.GetKey("escape"))
      {
        Application.Quit();
      }

      if (restart)
      {
        if (Input.GetKeyDown (KeyCode.Return))
        {
          SceneManager.LoadScene("Scene1");
        }
      }

      UpdateHardMode();
  }

  IEnumerator SpawnWaves()
  {
    yield return new WaitForSeconds(startWait);
    while (true)
    {
      for (int i = 0; i < hazardCount; i++)
      {
      /*  GameObject hazard = hazards [Random.Range (0, hazards.Length)];
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(hazard, spawnPosition, spawnRotation);
        yield return new WaitForSeconds(spawnWait); */

        if (hardMode == false)
        {
          GameObject hazard = hazards [Random.Range (0, hazards.Length)];
          Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
          Quaternion spawnRotation = Quaternion.identity;
          Instantiate(hazard, spawnPosition, spawnRotation);
          yield return new WaitForSeconds(spawnWait);
        }

        else if (hardMode != false)
        {
          GameObject hazard = hazards [Random.Range (0, 4)];
          Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
          Quaternion spawnRotation = Quaternion.identity;
          Instantiate(hazard, spawnPosition, spawnRotation);
          yield return new WaitForSeconds(spawnWait);
        }
      }
      yield return new WaitForSeconds(waveWait);
      if (gameOver)
      {
        restartText.text = "Press Enter to Restart";
        restart = true;
        break;
      }
    }
  }

  public void AddScore(int newScoreValue)
  {
    points += newScoreValue;
    UpdateScore();
  }

  void UpdateScore()
  {
    pointsText.text = "Points: " + points;
    if (points >= 100)
    {
      bgMusic.Pause();
      loseMusic.Pause();
      winMusic.PlayDelayed(0);
      loseMusic.Pause();
      backgroundScroll.GetComponent<BGScroller>().scrollSpeed = (-10);
      gameOverText.text = "You Win! Game created by Melissa Wells.";
      gameOver = true;
      restart = true;
    }
  }

  public void GameOver()
  {
    bgMusic.Pause();
    loseMusic.PlayDelayed(0);
    gameOverText.text = "Game Over!";
    gameOver = true;
  }

  public void UpdateHardMode()
  {
    if (Input.GetKeyDown (KeyCode.Z))
    {
      if (hardMode == false)
      {
        hardMode = true;
        hazardCount = 40;
        waveWait = 1;
        hardModeText.text = "Hard Mode On.";
      }

      else if (hardMode != false)
      {
        hardMode = false;
        hazardCount = 10;
        waveWait = 4;
        hardModeText.text = "Hard Mode Off.";
      }
    }
  }
}
