using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class RestartButtonScript:MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}