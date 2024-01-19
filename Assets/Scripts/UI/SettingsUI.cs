using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
         private static ILogger logger = Debug.unityLogger;

        public Slider numPairsSlider;
        public TextMeshProUGUI numPairsText;
        public Dropdown dropdown;
        public TextMeshProUGUI modelsToUse;
        public void Start()
        {
            //Adds a listener to the main slider and invokes a method when the value changes.
            numPairsSlider.onValueChanged.AddListener(delegate {ChangeNumPairs(); });
            numPairsSlider.value = PlayerPrefs.GetInt("NumPairs", 2);
            dropdown = GetComponent<Dropdown>();
            PlayerPrefs.SetString("Models", dropdown.options[dropdown.value].text);
            dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(dropdown); });
            dropdown.options[dropdown.value].text = PlayerPrefs.GetString("Models", "ChristmasModels");
        }
        public void ChangeNumPairs()
        {
            numPairsText.text = numPairsSlider.value.ToString();
            PlayerPrefs.SetInt("NumPairs", (int)numPairsSlider.value);
            PlayerPrefs.Save();
        }

        void DropdownValueChanged(Dropdown change)
        {
            modelsToUse.text = dropdown.options[dropdown.value].text;
            logger.Log("MODEL NAME: " + modelsToUse.text);
            PlayerPrefs.SetString("Models", dropdown.options[dropdown.value].text);
            PlayerPrefs.Save();

            // Debug.Log("Selected: " + dropdown.options[dropdown.value].text);
            // //string ModelName = dropdown.options[dropdown.value].text;
            // Debug.Log("MODEL NAME: " + modelsToUse.text);
        }

        //  public string GetSelectedValue()
        // {
        //     return dropdown.options[dropdown.value].text;
        // }
        public void OnBackButton()
        {
            SceneManager.LoadScene(0);
        }
    }
}