using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class RestartButtonScript:MonoBehaviour, IPointerClickHandler
    {

        //Кнопка перезапуску гри
        public void OnPointerClick(PointerEventData eventData)
        {
            SceneManager.LoadScene(0);
        }
    }
}