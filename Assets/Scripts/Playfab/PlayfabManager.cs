using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Singleton;
    [HideInInspector] public const string titleID = "F6211";
    public TMP_InputField usernameInput_Register;
    public TMP_InputField emailInput_Register;
    public TMP_InputField passwordInput_Register;
    public TMP_InputField emailInput_Login;
    public TMP_InputField passwordInput_Login;
    public TMP_InputField emailInput_Password;
    public TextMeshProUGUI messageText;
    public GameObject SignUpPage, LoginPage, PlayMenuBg, errorBG;

    private const string _LoginRememberKey = "RememberMe";
    private SaveInfo saveInfo = null;
    public bool rememberMe;
    public class SaveInfo
    {
        public string email, password;

        public SaveInfo(string _email, string _password)
        {
            this.email = _email;
            this.password = _password;
        }
    }
    //public TMP_InputField emailInput_Password;
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        if (PlayerPrefs.HasKey(_LoginRememberKey))
        {
            string json = PlayerPrefs.GetString(_LoginRememberKey);
            saveInfo = JsonUtility.FromJson<SaveInfo>(json);

            var request = new LoginWithEmailAddressRequest
            {
                Email = saveInfo.email,
                Password = saveInfo.password,
                TitleId = titleID
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
        }
    }

    public void ResgisterButton()
    {
        if (usernameInput_Register.text.Length < 4)
        {
        }
        else if (String.IsNullOrEmpty(emailInput_Register.text))
        {
        }
        else if (passwordInput_Register.text.Length < 6)
        {
        }

        var request = new RegisterPlayFabUserRequest
        {
            TitleId = titleID,
            DisplayName = usernameInput_Register.text,
            Username = usernameInput_Register.text,
            Email = emailInput_Register.text,
            Password = passwordInput_Register.text,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Register Successful");
        SignUpPage.SetActive(false);
        PlayMenuBg.SetActive(true);
        errorBG.SetActive(false);
    }

    void OnError(PlayFabError error)
    {
        errorBG.SetActive(true);
        Invoke(nameof(ErrorStopBG), 3f);
        CancelInvoke();
    }

    void ErrorStopBG()
    {
        errorBG.SetActive(false);
    }
    public void LoginButton()
    {
       
        if (String.IsNullOrEmpty(emailInput_Login.text))
        {
        }
        else if (passwordInput_Login.text.Length < 6)
        {
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput_Login.text,
            Password = passwordInput_Login.text,
            TitleId = titleID
            
            //if(remeberme)

        };
        if (rememberMe)
        {
            saveInfo = new SaveInfo(request.Email, request.Password);
        }
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
        
    }

    void OnLoginSuccess(LoginResult result)
    {
        if (saveInfo != null) { string json = JsonUtility.ToJson(saveInfo);
            if (!PlayerPrefs.HasKey(_LoginRememberKey)) { PlayerPrefs.SetString(_LoginRememberKey, json); }       
        }
        Debug.Log("Login Successful"); 
        LoginPage.SetActive(false);
        PlayMenuBg.SetActive(true);
        errorBG.SetActive(false);
    }

    public void ResetPasswordButton()
    {
        if (string.IsNullOrEmpty(emailInput_Password.text))
        {
            errorBG.SetActive(true);
            messageText.text = "Enter a vaild e-mail id!";
            Invoke(nameof(ErrorStopBG), 3f);
            return;
        }

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput_Password.text,
            TitleId = titleID
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        errorBG.SetActive(true);
        messageText.text = "Password reset mail sent!";
        Invoke(nameof(ErrorStopBG), 3f);
    }

    public void SetRememberMe(Toggle toggle)
    {
        rememberMe = toggle.isOn;
    }

}
