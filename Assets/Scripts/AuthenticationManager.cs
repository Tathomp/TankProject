using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationManager : MonoBehaviour
{
    WWWForm form;

    /***************************************
                Game Objects
     **************************************/
    // Panels
    public GameObject menuForgotPass, menuLogIn, menuPassConf, menuSignUpConf;
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
        DisplayLoginPanel();

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Check for empty or blank string
    bool IsEmpty(string s)
    {
        if (Equals(s.Trim(), ""))
            return true;
        else
            return false;
    }


    /***************************************
                Display Functions
     **************************************/
    // Activate the Password Confirmation Panel
    public void DisplayPassConfPanel()
    {
        // Disable ForgotPassword panel and enable PassConf panel
        menuForgotPass.SetActive(false);
        menuPassConf.SetActive(true);
    }

    // Activate the Forgot Password Panel
    // Should only be called from LogIn panel
    public void DisplayForgotPasswordPanel()
    {
        // Disable LogIn panel and enable ForgotPass panel
        menuLogIn.SetActive(false);
        menuForgotPass.SetActive(true);
    }

    // Activate the LogIn panel, restrict Game Access
    // Cancel any active user session, dump variables
    public void DisplayLoginPanel()
    {
        // Disable other panels in this canvas and activate LogIn panel
        menuPassConf.SetActive(false);
        menuForgotPass.SetActive(false);
        menuSignUpConf.SetActive(false);
        menuLogIn.SetActive(true);

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

        // TODO - check for active user session & end it
    }

    // Activate the SignUp Confirmation Panel
    public void DisplaySignUpConfPanel()
    {
        menuLogIn.SetActive(false);
        menuSignUpConf.SetActive(true);
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
        if (IsEmpty(txtUser.text))
            txtFeedback.text = "Username cannot be blank";
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
        txtFPFeedback.text = "";
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
    public IEnumerator ForgottenPassword()
    {
        string email = txtFPEmail.text;

        form = new WWWForm();
        form.AddField("mail", email);

        WWW passResetReq = new WWW("http://www.ninjalive.com/tanks/action_requestreset.php", form);
        yield return passResetReq;

        // Check for a returned json string
        if (string.IsNullOrEmpty(passResetReq.error))
        {
            // Successful db connection
            // Match error messages: (replace with switch?)
            if (passResetReq.text.Contains("outstanding"))
            {
                txtFPFeedback.text = "Account must be activate";
            }
            // No Error: Successful reset request
            else
            {
                DisplayPassConfPanel();
            }
        }
        else
        {
            // DB connection failed
            txtFPFeedback.text = "An error occured talking to the server";
        }
    }

    public IEnumerator RequestLogin()
    {
        string email = txtEmail.text;
        string password = txtPass.text;

        form = new WWWForm();
        form.AddField("mail", email);
        form.AddField("pass", password);

        WWW logInReq = new WWW("http://www.ninjalive.com/tanks/action_login.php", form);
        yield return logInReq;

        // Check for a returned json string
        if (string.IsNullOrEmpty(logInReq.error)) {
            // Successful db connection
            // Match error messages: (replace with switch?)
            if (logInReq.text.Contains("invalid"))
            {
                txtFeedback.text = "Invalid Email or Password";
            }
            else if (logInReq.text.Contains("inactive"))
            {
                txtFeedback.text = "Account is not active. Verify your Email address";
            }
            // No Error:
            else
            {
                txtFeedback.text = "Loggin successful...";
            }
            // TODO: launch the game
        }
        else {
            // DB connection failed
            txtFeedback.text = "An error occured talking to the server";
        }
    }

    public IEnumerator RequestUserRegistration()
    {
        string username = txtUser.text;
        string password = txtPass.text;
        string confirmPassword = txtConfirmPass.text;
        string email = txtEmail.text;

        if (password.Length < 8)
        {
            txtFeedback.text = "Password neeeds to be at least 8 characters long";
            yield break;
        }
        if(password != confirmPassword)
        {
            txtFeedback.text = "Passwords do not match";
            yield break;
        }

        form = new WWWForm();
        form.AddField("mail", email);
        form.AddField("pass", password);
        form.AddField("user", username);
        

        WWW register = new WWW("http://www.ninjalive.com/tanks/action_register.php", form);
        yield return register;

        // Check for a returned json string
        if (string.IsNullOrEmpty(register.error))
        {
            // Successful DB connection
            // Match error messages: (replace with switch?)
            if (register.text.Contains("exists"))
            {
                txtFeedback.text = "E-mail is already registered";
            }
            else if (register.text.Contains("taken"))
            {
                txtFeedback.text = "Username is already taken";
            }
            else if (register.text.Contains("try again"))
            {
                txtFeedback.text = "Could not create account, please try again later";
            }
            // No Error:
            else
            {
                txtFeedback.text = "Registration successful...";
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
            // DB connection failed
            txtFeedback.text = "An error occured talking to the server";
        }
    }
}