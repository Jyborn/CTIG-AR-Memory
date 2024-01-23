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
        public Button easyButton, normalButton, hardButton;
        [SerializeField] public TMP_Dropdown dropdown;
        public void Start()
        {
            //Adds a listener to the main slider and invokes a method when the value changes.
            numPairsSlider.onValueChanged.AddListener(delegate {ChangeNumPairs(); });
            numPairsSlider.value = PlayerPrefs.GetInt("NumPairs", 2);

            string storedModel = PlayerPrefs.GetString("Models", "ChristmasModels");
            int index = dropdown.options.FindIndex(option => option.text == storedModel);
            dropdown.value = index >= 0 ? index : 0;

            // Add a listener to the dropdown's value change event
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        }
        public void ChangeNumPairs()
        {
            numPairsText.text = numPairsSlider.value.ToString();
            PlayerPrefs.SetInt("NumPairs", (int)numPairsSlider.value);
            PlayerPrefs.Save();
        }

        void OnDropdownValueChanged(int value)
        {
            string modelPackageName = dropdown.options[value].text;
            logger.Log("MODEL NAME: " + modelPackageName);
            PlayerPrefs.SetString("Models", modelPackageName);
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