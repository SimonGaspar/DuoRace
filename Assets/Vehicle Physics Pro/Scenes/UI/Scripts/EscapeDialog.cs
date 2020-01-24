//--------------------------------------------------------------
//      Vehicle Physics Pro: advanced vehicle physics kit
//          Copyright © 2011-2019 Angel Garcia "Edy"
//        http://vehiclephysics.com | @VehiclePhysics
//--------------------------------------------------------------

// EscapeDialog: controls the Escape dialog and its options


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace VehiclePhysics.UI
{

public class EscapeDialog : MonoBehaviour
	{
	public VehicleBase vehicle;
	public KeyCode escapeKey = KeyCode.Escape;


    [Header("Buttons")]
	public Button continueButton;
	public Button resetCarButton;
	public Button resetButton;
    public Button optionsButton;
        public Button mainMenuButton;
        public Button quitButton;


	 float m_currentTimeScale;


        void PauseAllAudio()
        {
            AudioListener.pause = true;
        }

        void ResumeAllAudio()
        {
            AudioListener.pause = false;
        }

        void OnEnable ()
		{
		AddListener(continueButton, OnContinue);
        AddListener(optionsButton, OnOptions);
        AddListener(resetCarButton, OnResetCar);
		AddListener(resetButton, OnReset);
            AddListener(mainMenuButton, OnMainMenu);
            AddListener(quitButton, OnQuit);

		 m_currentTimeScale = Time.timeScale;
		 Time.timeScale = 0.0f;

            PauseAllAudio();
		}


	void OnDisable ()
		{
		RemoveListener(continueButton, OnContinue);
        RemoveListener(optionsButton, OnOptions);
        RemoveListener(resetCarButton, OnResetCar);
		RemoveListener(resetButton, OnReset);
		RemoveListener(quitButton, OnQuit);

		 Time.timeScale = m_currentTimeScale;

            ResumeAllAudio();
		}


	void Update ()
		{            
            if (Input.GetKeyDown(escapeKey))
			this.gameObject.SetActive(false);
		}


	// Listeners


	void OnContinue ()
		{
		this.gameObject.SetActive(false);
		}       

        void OnOptions ()
        {
            Debug.Log("options");   
            var foundMenuObject = FindObjectOfType<MenuOverlay>();
            foundMenuObject.SetEnabled(foundMenuObject.setupDialog, true);
        }

        void OnResetCar ()
		{
		if (vehicle != null)
			{
			VPResetVehicle resetScript = vehicle.GetComponent<VPResetVehicle>();
			if (resetScript != null)
				{
				resetScript.DoReset();
				this.gameObject.SetActive(false);
				}
			}
		}


	void OnReset ()
		{
            Debug.Log("reset");
		EdyCommonTools.SceneReload.Reload();
		}

        void OnMainMenu()
        {
            SceneManager.LoadScene("Main_Menu");
        }

        void OnQuit ()
		{
		Application.Quit();
		}


	void AddListener (Button button, UnityAction method)
		{
		if (button != null) button.onClick.AddListener(method);
		}


	void RemoveListener (Button button, UnityAction method)
		{
		if (button != null) button.onClick.RemoveListener(method);
		}

	}
}
