using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInUI : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private Button resetDayButton;
    private Tweener tweener;

    private void Start()
    {
        GameManager.Instance.OnEndDay += EndDay;
    }
    
    public void EndDay()
    {
        if (tweener != null || tweener.IsActive())
        {
            tweener.Kill();
        }
        tweener = fadeScreen.DOFade(1f, 3f)
            .OnComplete(() =>
            {
                if (resetDayButton)
                {
                    resetDayButton.gameObject.SetActive(true);
                    resetDayButton.interactable = true;
                }
            });
    }
    
    public void RestartDay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
