using System;
using Bomberfox.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Bomberfox.UI
{
    public class MenuUIController : MonoBehaviour
    {
	    private Animator[] animators;
	    private bool skipped = false;

        void Start()
        {
            AudioManager.instance.PlayMusic("MainMenu");
            AudioManager.instance.PlayMusic("ThunderMenu");
            Button btn = GameObject.Find("StartGameButton").GetComponent<Button>();
            EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            es.SetSelectedGameObject(null);
            es.SetSelectedGameObject(btn.gameObject);
            animators = FindObjectsOfType<Animator>();
        }

        private void Update()
        {
	        if (!skipped && Input.anyKeyDown)
	        {
		        foreach (var a in animators)
		        {
			        AnimatorClipInfo[] info = a.GetCurrentAnimatorClipInfo(0);
			        a.Play(info[0].clip.name, 0, 1f);
                }

		        skipped = true;
	        }
	        
        }

        public void StartGame()
        {
            GameManager.Instance.Player = new PlayerData();
            AudioManager.instance.CheckGameMusic();
            AudioManager.instance.StopMusic("ThunderMenu");
            AudioManager.instance.StopMusic("MainMenu");
            GameManager.Instance.GoToStory();
        }

        public void QuitGame()
        {
	        Application.Quit();
        }

        public void EnableMusic(bool enable)
        {
	        if (enable) AudioManager.instance.EnableMusic();
	        else AudioManager.instance.MuteMusic();
        }

        public void EnableSound(bool enable)
        {
	        if (enable) AudioManager.instance.EnableSound();
	        else AudioManager.instance.MuteSound();
        }
    }
}
