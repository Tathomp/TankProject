using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationManager : MonoBehaviour
{

    //LogIn-SignUp Panel Objects
    public GameObject menuLogIn, menuSignUpConf, menuForgotPass;
    public GameObject fieldConfirmPass, fieldUser;
    public GameObject btnSignUp, btnForgotPass, btnLogIn, btnSubmit, btnCancel;
    public InputField txtUser, txtPass, txtConfirmPass, txtEmail;
    public Text msgDivider, txtFeedback;
    WWWForm form;


    // Use this for initialization
    void Start()
    {
        displayLoginPanel();

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Activate the LogIn panel, restrict Game Access
    //Cancel active user session, dump variables
    public void displayLoginPanel()
    {
        menuLogIn.SetActive(true);
        txtFeedback.text = "";
        //TODO - check for active user session & end it
    }

    //Attempts to log the user into the game
    //Validate imput fields, request user credential verification from server
    //Sucess exits this script's scope, activates Menus Canvas
    public void LoginButtonTapped()
    {
        txtFeedback.text = "Logging in...";
        StartCoroutine("RequestLogin");
    }

    //Display the new account sign up menu
    //Display additional input fields for account creation
    //Hide LogIn specific items
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

    //Cnacel SignUp fields and revert to LogIn menu
    public void CancelButtonTapped()
    {
        fieldConfirmPass.SetActive(false);
        fieldUser.SetActive(false);
        btnSubmit.SetActive(false);
        btnCancel.SetActive(false);
        btnLogIn.SetActive(true);
        btnSignUp.SetActive(true);
        msgDivider.gameObject.SetActive(true);
        btnForgotPass.SetActive(true);
        txtFeedback.text = "";
    }

    //Submits new account registration information to server
    //Validate user input fields
    //Check for duplicate Email & Username on server
    //Add user profile with hold, send verification Email, redirect to confirmation
    //Success exits this script's scope, opens SignUp Conf panel 
    public void SubmitButtonTapped()
    {
        txtFeedback.text = "Processing registration...";
        StartCoroutine("RequestUserRegistration");
    }

    //Allows existing user to request a password reset
    //Disable LogIn menu and enable Forgotten Password Menu
    //Success exists this script's scope
    public void ForgotPasswordButtonTapped()
    {
        menuLogIn.SetActive(false);
        menuForgotPass.SetActive(true);
    }

    public IEnumerator RequestLogin()
    {
        string email = txtEmail.text;
        string password = txtPass.text;

        form = new WWWForm();
        form.AddField("mail", email);
        form.AddField("pass", password);

        WWW logInReq = new WWW("http://www.tanks.claytonmichaelphoto.com/action_login.php", form);
        yield return logInReq;

        //Check for a returned json string
        if (string.IsNullOrEmpty(logInReq.error)) {
            // Successful db connection
            // Match error messages: (replace with switch?)
            if (logInReq.text.Contains("invalid"))
            {
                txtFeedback.text = "Invalid Email or Password";
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
            txtFeedback.text = "An error occured talking to the server.";
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
        

        WWW register = new WWW("http://www.tanks.claytonmichaelphoto.com/action_register.php", form);
        yield return register;

        //Check for a returned json string
        if (string.IsNullOrEmpty(register.error))
        {
            // Successful DB connection
            // Match error messages: (replace with switch?)
            if (register.text.Contains("exists"))
            {
                txtFeedback.text = "E-mail is already registered.";
            }
            else if (register.text.Contains("taken"))
            {
                txtFeedback.text = "Username is taken.";
            }
            else if (register.text.Contains("try again"))
            {
                txtFeedback.text = "Could not create account, please try again later.";
            }
            // No Error:
            else
            {
                txtFeedback.text = "Registration successful...";
            }

            // TODO: send to confirmaiton screen
        }
        else
        {
            // DB connection failed
            txtFeedback.text = "An error occured talking to the server.";
        }
    }
}