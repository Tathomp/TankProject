<?php
	// Authorize password reset on existing user account:
	// -Check for existing email
	// -Check for active userStatus
	// -Add reset flag to userStatus
	// -Generate validation code and add to userStatus
	// -Send reset E-mail
	header("Access-Control-Allow-Origin: *");
	
	// Database connection
	require_once('dbconnect.php');
	
	// Set default failure error msg for unity
	$dataArray = array('success' => false, 'error' => 'try again');

	// Only proceed if data was actually POSTed
	if(!empty($_POST['mail'])) {
		
		// Get POSTed values from Unity
		$mail = trim($_POST['mail']);
		
		// Create a unique code for email verification
		$code = md5(rand(0, 1000));
		
		// Build Query: check if email already exists
		$stmt = $db->prepare("SELECT userStatus FROM users WHERE userEmail=?");
		$stmt->bind_param('s', $mail);
		$stmt->execute();
		$result = $stmt->get_result();

		// Check for result from query		
		if($result && $result->num_rows > 0) {
			$row = $result->fetch_object();
			
			// Check for active status, user cannot reset password until account is active
			if($row->userStatus == "Active") {
				
				// Build Query: update userStatus with code to authorize password change
				$stmt2 = $db->prepare("UPDATE users SET userCode = ? WHERE userEmail = ?");
				$stmt2->bind_param('ss', $code, $mail);
				$stmt2->execute();
				
				if($stmt2->affected_rows > 0) {
					
					// Success: set msg for unity and send reset email
					$dataArray = array('success' => true, 'error' => '', 'email' => "$mail");
					send_email($mail, $code);
				}
			}				
			else {
				// Outstanding email, set error msg for unity
				$dataArray = array('success' => false, 'error' => 'outstanding');
			}			
		}	
	}
	
	
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
	
	
	// Sends an email for account verification
	function send_email($mail, $code) {
		// Set email subject line
		$subject = 'Tankware | Forgotten Password';
		
		// Set email message body
		$message = '
		 
		You\'re receiving this message because someone requested to reset the password on this account.
		If that was not you, please disregard.
		 
		Click this link to reset your password:
		https://www.ninjalive.com/tanks/reset_pass.php?mail='.$mail.'&code='.$code.'
		 
		';
		// Set message headers
		$headers = 'From: noreply@ninjalive.com' . "\r\n";
		// Send the email
		mail($mail, $subject, $message, $headers);
	}
	
?>