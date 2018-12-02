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
	$dataArray = array('query' => false, 'success' => false, 'msg' => 'Please try again later');

	// Only proceed if data was actually POSTed
	if(!empty($_POST['mail'])) {
		
		// Get POSTed values from Unity
		$mail = trim($_POST['mail']);
		
		// Create a unique code for email verification
		$code = md5(rand(0, 1000));
		
		// Build Query: check if email already exists
		$stmt = $db->prepare("SELECT userName, userStatus FROM users WHERE userEmail=?");
		$stmt->bind_param('s', $mail);
		$stmt->execute();
		$result = $stmt->get_result();

		// Check for result from query		
		if($result && $result->num_rows > 0) {
			$row = $result->fetch_object();
			
			// Check for active status, user cannot reset password until account is active
			if($row->userStatus == "Active") {
				$userName = $row->userName;
				// Build Query: update userStatus with code to authorize password change
				$stmt2 = $db->prepare("UPDATE users SET userCode = ? WHERE userEmail = ?");
				$stmt2->bind_param('ss', $code, $mail);
				$stmt2->execute();
				
				if($stmt2->affected_rows > 0) {
					
					// Success: set msg for unity and send reset email
					$dataArray = array('query' => true, 'success' => true, 'msg' => '', 'email' => "$mail");
					send_email($mail, $code, $userName);
				}
			}				
			else {
				// Outstanding email, set error msg for unity
				$dataArray = array('query' => true, 'success' => false, 'msg' => 'Account must be active');
			}			
		}	
	}
	
	
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
	
	
	// Sends an email for account verification
	function send_email($mail, $code, $userName) {
		// Set email subject line
		$subject = 'Tankware | Forgotten Password';
		
		// Set email message body
		$message = '
		 
		Hello '.$userName.',
		
		You\'re receiving this message because someone requested to reset the password on this account.
		If that was not you, please disregard.
		 
		Otherwise, please follow this link to reset your password:
		http://www.ninjalive.com/tanks/reset_pass.php
		Authorization Code: '.$code;
		
		// Set message headers
		$headers = 'From: noreply@ninjalive.com' . "\r\n";
		// Send the email
		mail($mail, $subject, $message, $headers);
	}
	
?>