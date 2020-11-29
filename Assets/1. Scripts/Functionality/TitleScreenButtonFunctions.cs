using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenButtonFunctions : MonoBehaviour
{
        [SerializeField] private RulePage RulePage;
        public void LoadGame()
        {
                SceneManager.LoadScene("Game");
        }
        public void LoadCredits()
        {
                SceneManager.LoadScene("Credits");
        }
        public void LoadRules()
        {
                SceneManager.LoadScene("Rules");
        }

        public void Back()
        {
                SceneManager.LoadScene("StartScreen");
        }
        
        public void NextPage()
        {
        
                RulePage.Pages[RulePage.CurrentPage].SetActive(false);
        
                if (RulePage.CurrentPage == RulePage.Pages.Count - 1)
                {
                        RulePage.CurrentPage = 0;
                }
                else
                {
                        RulePage.CurrentPage++;
                }
        
                RulePage.Pages[RulePage.CurrentPage].SetActive(true);
        }

        public void PrevPage()
        {
                RulePage.Pages[RulePage.CurrentPage].SetActive(false);
                if (RulePage.CurrentPage == 0)
                {
                        RulePage.CurrentPage = RulePage.Pages.Count - 1;
                }
                else
                {
                        RulePage.CurrentPage--;
                }
                RulePage.Pages[RulePage.CurrentPage].SetActive(true);
        }
}