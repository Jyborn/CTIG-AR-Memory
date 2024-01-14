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
        public void Start()
        {
            //Adds a listener to the main slider and invokes a method when the value changes.
            numPairsSlider.onValueChanged.AddListener(delegate {ChangeNumPairs(); });
            numPairsSlider.value = PlayerPrefs.GetInt("NumPairs", 2);
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
    }
}