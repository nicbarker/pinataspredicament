using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerBehaviour : MonoBehaviour
{
    public Animator animator;

    public async void FadeToScene(int sceneIndex)
    {
        animator.SetTrigger("FadeOut");
        await Task.Delay(1500);
        SceneManager.LoadScene(sceneIndex);
    }
}
