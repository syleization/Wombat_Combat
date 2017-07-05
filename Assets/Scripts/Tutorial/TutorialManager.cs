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
        TutorialTools Tools;
        [SerializeField]
        Text TextGameObject;
        [SerializeField]
        List<TextEvent> TextEvents;
        [SerializeField]
        int CurrentTextBox;
        [SerializeField]
        Pause PauseJenk;

        void Start()
        {
            if(FindObjectOfType<Pause>() == null)
            {
                Instantiate(PauseJenk);
                if(Pause.Instance == null)
                {
                    TextGameObject.text += "Pause Is NULL";
                }
                if (GlobalSettings.Instance == null)
                {
                    TextGameObject.text += "Globalsettings Is NULL";
                }
            }
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
            var manager = FindObjectOfType<UnityEngine.Networking.NetworkManager>();
            manager.StopHost();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
