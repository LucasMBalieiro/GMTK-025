using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInUI : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private DayEndUI dayEnd;
    private Tweener tweener;

    private void Start()
    {
        GameManager.Instance.OnEndDay += EndDay;
        dayEnd.gameObject.SetActive(false);
        fadeScreen.DOFade(0f, 0.1f);
    }
    
    public void EndDay()
    {
        if (tweener != null || tweener.IsActive())
        {
            tweener.Kill();
        }
        AudioManager.Instance.MuteSFX(true);
        tweener = fadeScreen.DOFade(1f, 3f)
            .OnComplete(() =>
            {
                if (GameManager.Instance.CheckEnd())
                    SceneManager.LoadScene("game.over");
                
                if (dayEnd)
                {
                    dayEnd.gameObject.SetActive(true);
                    dayEnd.OpenUI();
                }
            });
    }
    
    public void RestartDay()
    {
        AudioManager.Instance.MuteSFX(false);
        AudioManager.Instance.PlaySFX("Machine_Grab");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
