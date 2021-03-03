using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame() => SceneManager.LoadScene(1);

    public void ToggleHowToPlay() => GetComponent<Animator>().SetBool("onMainMenu", !GetComponent<Animator>().GetBool("onMainMenu"));
}
