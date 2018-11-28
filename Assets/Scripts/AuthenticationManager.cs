using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// User class to hold JSON results
[Serializable]
public class User
{
    // Query response logic
    public bool query;
    public bool success;
    public string msg;

    // Player attributes
    public string userName;
    public string userEmail;
    public int userID;
    public string userImage;
    public int maxLevel;
    public int activeUpgrades;
    public int purchasedUpgrades;
    public int userCredits;
}


public class AuthenticationManager : MonoBehaviour
{
    // Player state reference, initialized in Start()
    PlayerState ps;

    // Web form used in coroutines for db queries
    WWWForm form;

    // Serverside script names referenced by WWWForms
    private readonly string URLLOGIN = "action_login.php";
    private readonly string URLREGISTER = "action_register.php";
    private readonly string URLREQUESTRESET = "action_requestreset.php";


    /***************************************
                Game Objects
     **************************************/
    // Panels
    public GameObject forgotPassUI, logInUI, passConfUI, signUpConfUI;
    // Fields
    public GameObject fieldConfirmPass, fieldUser;
    // Buttons
    public GameObject btnCancel, btnForgotPass, btnFPCancel, btnFPSubmit, btnLogIn, btnSignUp, btnSubmit;
    public GameObject btnPassConfOK, btnSignUpOK;
    // Input Text
    public InputField txtConfirmPass, txtFPEmail, txtEmail, txtPass, txtUser;
    // Text
    public Text msgDivider, txtFeedback, txtFPFeedback;

    
    // Use this for initialization
    void Start()
    {
        // Initialize player state reference
        ps = PlayerState.GetCurrentPlayerState();

        // Set default player state 
        ps.StartState();

        // Activate the login panel
        DisplayLoginPanel();
    }


    // Check for empty or blank string
    bool IsEmpty(string s)
    {
        if (Equals(s.Trim(), ""))
            return true;    // String is empty
        else
            return false;   // String is not empty
    }


    /***************************************
                Display Functions
     **************************************/
    // Activate the Password Confirmation Panel
    public void DisplayPassConfPanel()
    {
        // Disable ForgotPassword panel and enable PassConf panel
        forgotPassUI.SetActive(false);
        passConfUI.SetActive(true);
    }

    // Activate the Forgot Password Panel
    // Should only be called from LogIn panel
    public void DisplayForgotPasswordPanel()
    {
        // Disable LogIn panel and enable ForgotPass panel
        logInUI.SetActive(false);
        forgotPassUI.SetActive(true);
    }

    // Activate the LogIn panel, restrict Game Access
    // Cancel any active user session, dump variables
    public void DisplayLoginPanel()
    {
        // Reset player instance just incase
        ps.StartState();

        // Disable other panels in this canvas and activate LogIn panel
        passConfUI.SetActive(false);
        forgotPassUI.SetActive(false);
        signUpConfUI.SetActive(false);
        logInUI.SetActive(true);

        // Disable SignUp specific fields
        fieldConfirmPass.SetActive(false);
        fieldUser.SetActive(false);

        // Reset input field values
        txtUser.text = "";
        txtPass.text = "";
        txtConfirmPass.text = "";
        txtEmail.text = "";

        // Set button's statuses for LogIn
        btnSubmit.SetActive(false);
        btnCancel.SetActive(false);
        btnForgotPass.SetActive(true);
        btnLogIn.SetActive(true);
        btnSignUp.SetActive(true);
        
        // Activate text objects
        msgDivider.gameObject.SetActive(true);

        // Reset the Feedback text string
        txtFeedback.text = "";
    }


    // Activate the SignUp Confirmation Panel
    public void DisplaySignUpConfPanel()
    {
        logInUI.SetActive(false);
        signUpConfUI.SetActive(true); 
    }

    // Load user state and enter the game
    public void ProcessPlay(User u)
    {
        ps.RestoreState(u);
        ps.DisplayMenuCanvas();
    }

    /***************************************
                Button Actions
     **************************************/
    // Cancel current panel and return to LogIn panel
    public void CancelButtonTapped()
    {
        DisplayLoginPanel();
    }

    // Allows existing user to request a password reset
    // Disable LogIn menu and enable Forgotten Password Menu
    public void ForgotPasswordButtonTapped()
    {
        DisplayForgotPasswordPanel();
    }

    // Attempts to log the user into the game
    // Validate imput fields, request user credential verification from server
    // Sucess exits this script's scope, activates Menus Canvas
    public void LoginButtonTapped()
    {
        // Check for empty or blank fields
        if (IsEmpty(txtEmail.text))
            txtFeedback.text = "E-mail cannot be blank";
        else if (IsEmpty(txtPass.text))
            txtFeedback.text = "Password cannot be blank";
        else
        {
            // Proceed to process login
            txtFeedback.text = "Logging in...";
            StartCoroutine("RequestLogin");
        }
    }

    // Display the new account sign up menu
    // Display additional input fields for account creation
    // Hide LogIn specific items
    public void SignUpButtonTapped()
    {
        fieldConfirmPass.SetActive(true);
        fieldUser.SetActive(true);
        btnSubmit.SetActive(true);
        btnCancel.SetActive(true);
        btnLogIn.SetActive(false);
        btnSignUp.SetActive(false);
        msgDivider.gameObject.SetActive(false);
        btnForgotPass.SetActive(false);
        txtFeedback.text = "";
    }

    // Close Confirmation and return to LogIn panel
    public void OkayButtonTapped()
    {
        DisplayLoginPanel();
    }

    // Submits new account registration information to server
    // Validate user input fields
    // Check for existing Email on server
    // Add reset to status, send reset-verification Email, redirect to confirmation
    public void SubmitForgotPassButtonTapped()
    {
        txtFPFeedback.text = "Processing...";
        // Check for empty or blank Email field
        if(!IsEmpty(txtFPEmail.text))
            StartCoroutine("ForgottenPassword");
        else
            txtFPFeedback.text = "E-mail cannot be blank";
    }

    // Submits new account registration information to server
    // Validate user input fields
    // Check for duplicate Email & Username on server
    // Add user profile with hold, send verification Email, redirect to confirmation
    public void SubmitRegistrationButtonTapped()
    {
        // Check for empty or blank fields
        if (IsEmpty(txtEmail.text))
            txtFeedback.text = "E-mail cannot be blank";
        else if (IsEmpty(txtPass.text))
            txtFeedback.text = "Password cannot be blank";
        else if (IsEmpty(txtConfirmPass.text))
            txtFeedback.text = "Confirm Password cannot be blank";
        else if (IsEmpty(txtUser.text))
            txtFeedback.text = "Username cannot be blank";
        else
        {
            // Proceed to process registration
            txtFeedback.text = "Processing registration...";
            StartCoroutine("RequestUserRegistration");
        }
    }


    /***************************************
                Coroutines
     **************************************/
    private IEnumerator ForgottenPassword()
    {
        // Build the form for submission
        form = new WWWForm();
        form.AddField("mail", txtFPEmail.text);

        WWW passResetReq = new WWW(ps.URL(URLREQUESTRESET), form);
        yield return passResetReq;

        // Check for successful web request
        if (string.IsNullOrEmpty(passResetReq.error))
        {
            // Convert response to JSON
            User user = JsonUtility.FromJson<User>(passResetReq.text);

            // Print the response message (error or success)
            txtFeedback.text = user.msg;

            // Only proceed if email exists and account is active
            if (user.query == true && user.success == true)
            {
                // Successful reset request, display the confirmation panel
                DisplayPassConfPanel();
            }
        }
        else
        {
            // Connection failed
            txtFeedback.text = "An error occured talking to the server";
        }
    }

    private IEnumerator RequestLogin()
    {
        // Build the form for submission
        form = new WWWForm();
        form.AddField("mail", txtEmail.text);
        form.AddField("pass", txtPass.text);

        WWW logInReq = new WWW(ps.URL(URLLOGIN), form);
        yield return logInReq;

        // Check for successful web request
        if (string.IsNullOrEmpty(logInReq.error))
        {
            // Convert response to JSON
            User user = JsonUtility.FromJson<User>(logInReq.text);
            
            // Print the response message (error or success)
            txtFeedback.text = user.msg;

            // Only proceed if credentials are valid and account is active
            if (user.query == true && user.success == true)
            {
                // Switch from authorization to gameplay
                ProcessPlay(user);
            }
        }
        else
        {
            // Connection failed
            txtFeedback.text = "An error occured talking to the server";
        }

    }

    private IEnumerator RequestUserRegistration()
    {
        // Verify password lenght and matching
        if (txtPass.text.Length < 8)
        {
            txtFeedback.text = "Password neeeds to be at least 8 characters long";
            yield break;
        }
        if(txtPass.text != txtConfirmPass.text)
        {
            txtFeedback.text = "Passwords do not match";
            yield break;
        }

        // Build the form for submission
        form = new WWWForm();
        form.AddField("mail", txtEmail.text);
        form.AddField("pass", txtPass.text);
        form.AddField("user", txtUser.text);
        
        WWW register = new WWW(ps.URL(URLREGISTER), form);
        yield return register;

        // Check for successful web request
        if (string.IsNullOrEmpty(register.error))
        {
            // Convert response to JSON
            User user = JsonUtility.FromJson<User>(register.text);

            // Print the response message (error or success)
            txtFeedback.text = user.msg;

            // Only proceed if credentials are valid and account is active
            if (user.query == true && user.success == true)
            {
                // Clear registration input values
                txtUser.text = "";
                txtPass.text = "";
                txtConfirmPass.text = "";
                txtEmail.text = "";
                // Send to confirmation panel
                DisplaySignUpConfPanel();
            }
        }
        else
        {
            // Connection failed
            txtFeedback.text = "An error occured talking to the server";
        }
    }
}