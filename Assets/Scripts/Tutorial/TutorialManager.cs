using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        /*
        Text textGameObject
TextEvent textBoxes[]
uint currentTextBox
NextButton()
MainMenuButton()
Update() // only calls current textEvent tick

        */
        [SerializeField]
        Text TextGameObject;
        [SerializeField]
        List<TextEvent> TextEvents;
        [SerializeField]
        int CurrentTextBox;

        void Start()
        {
            CurrentTextBox = -1;
            Pause.Instance.StartPauseTimer(999999.0f);
            NextButton();
        }

        public void NextButton()
        {
            if (CurrentTextBox + 1 >= TextEvents.Count)
            {
                MainMenuButton();
            }
            else
            {
                if (CurrentTextBox >= 0)
                {
                    TextEvents[CurrentTextBox].Exit(TextGameObject);
                }

                TextEvents[++CurrentTextBox].Enter(TextGameObject);
            }
        }

        void Update()
        {
            if (CurrentTextBox < TextEvents.Count)
            {
                TextEvents[CurrentTextBox].Tick();
            }
        }

        public void MainMenuButton()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
