using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SettingsUI : MonoBehaviour
    {

        public Slider numPairsSlider;
        public TextMeshProUGUI numPairsText;
        public Button easyButton, normalButton, hardButton;
        public void Start()
        {
            //Adds a listener to the main slider and invokes a method when the value changes.
            numPairsSlider.onValueChanged.AddListener(delegate {ChangeNumPairs(); });
            numPairsSlider.value = PlayerPrefs.GetInt("NumPairs", 2);

            easyButton = transform.Find("EasyButton").GetComponent<Button>();
            normalButton = transform.Find("NormalButton").GetComponent<Button>();  

        }
        public void ChangeNumPairs()
        {
            numPairsText.text = numPairsSlider.value.ToString();
            PlayerPrefs.SetInt("NumPairs", (int)numPairsSlider.value);
            PlayerPrefs.Save();
        }
        
        public void OnBackButton()
        {
            SceneManager.LoadScene(0);
        }
        public void OnEasyButtonClicked()
        {
            DifficultyManager.Instance.SetDifficulty(Difficulty.EASY);
            Debug.Log("Easy clicked");
        }
        public void OnNormalButtonClicked()
        {
            DifficultyManager.Instance.SetDifficulty(Difficulty.NORMAL);
            Debug.Log("Normal clicked clicked");
        }
        public void OnHardButtonClicked()
        {
            DifficultyManager.Instance.SetDifficulty(Difficulty.HARD);
            Debug.Log("Hard clicked");
        }
    }
}