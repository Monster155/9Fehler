using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NineFehler.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
    
        void Start()
        {
            _startGameButton.onClick.AddListener(StartGameButton_OnClick);
        }

        private void StartGameButton_OnClick()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
