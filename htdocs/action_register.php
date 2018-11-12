<?php
	// Register a new user account:
	// -Check for existing email
	// -Check for existing username
	// -Create record
	// -Set "UNVERIFIED ACCOUNT" status
	// -Send verification E-mail

	
	// Database connection
	require_once('dbconnect.php');
	
	// Only proceed if data was actually POSTed and includes values
	if(!empty($_POST['mail']) && !empty($_POST['pass']) && !empty($_POST['user'])) {
		
		// Get POSTed values from Unity
		$mail = trim($_POST['mail']);
		$pass = trim($_POST['pass']);
		$user = trim($_POST['user']);
				
		// Hash the password
		$pass = hash('sha256', $pass);
		
		// Create a unique code for email verification
		$code = md5(rand(0, 1000));
			
		// Query for existing user by email
		// Build Query: check if email already exists
		$stmt = $db->prepare("SELECT userID, userStatus FROM users WHERE userEmail=?");
		$stmt->bind_param('s', $mail);
		$stmt->execute();
		$result = $stmt->get_result();

		// Check for result from query		
		if($result && $result->num_rows > 0) {
			// Email exists, set error msg for unity
			$dataArray = array('success' => false, 'error' => 'exists');
		} 
		// No result from query
		else {
			// Build Query: insert new user info
			$stmt2 = $db->prepare("INSERT INTO users (userName, userPass, userEmail, userStatus) VALUES (?, ?, ?, ?)");
			$stmt2->bind_param('ssss', $user, $pass, $mail, $code);
			$stmt2->execute();
			
			// Re-Query for user by email
			$stmt->execute();
			$result2 = $stmt->get_result();
			$row = $result2->fetch_object();
						
			// Check for successful insert by comparing status codes
			if($row->userStatus == $code) {
				// Success: set msg for unity and send activation email
				$dataArray = array('success' => true, 'error' => '', 'email' => "$mail");
				send_email($user, $mail, $code, $row->userID);
			} else {
				// Failure: set error msg for unity
				$dataArray = array('success' => false, 'error' => 'try again');
			}
		}
	}
	
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
	
	
	// Sends an email for account verification
	function send_email($user, $mail, $code, $ID){
		// Set email subject line
		$subject = 'Tankware Sign-up | Verification';
		
		// Set email message body
		$message = '
		 
		Thanks for signing up, '.$user.'!
		 
		Please click this link to activate your account:
		https://www.ninjalive.com/tanks/verify.php?id='.$ID.'&code='.$code.'
		 
		';
		// Set message headers
		$headers = 'From: noreply@ninjalive.com' . "\r\n";
		// Send the email
		mail($mail, $subject, $message, $headers); // Send our email
	}
	
?>