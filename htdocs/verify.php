<?php
	// Email verification / Account Activation
	// -Accessed via email sent at user registration
	// -Checks the hash code provided against the code in user table
	// -Updates user status to active if codes match

	header("Access-Control-Allow-Origin: *");
	
	// Include external scripts
	require_once('dbconnect.php');
	require_once('redirect.php');
		
	// Only proceed if data was actually POSTed with values
	if(!empty($_GET['id']) && !empty($_GET['code'])){
	
		// Get POSTed values from Unity
		$id = trim($_GET['id']);
		$code = trim($_GET['code']);
				
		// Build Query: match userID, return userStatus
		$stmt = $db->prepare("SELECT userStatus FROM users WHERE userID=?");
		$stmt->bind_param('i', $id);
		$stmt->execute();
		$result = $stmt->get_result();
		
		// Check results
		if($result) {
			$row = $result->fetch_object();
		} else {
			// Failure: UserID does not match, redirect to error page
			redirect('https://www.ninjalive.com/tanks/registration.php?failed=1');
		}
		// Match userStatus code
		if($row->userStatus == $code) {
			// Success: Code Matches
			
			// Build Query: update userStatus
			$stmt2 = $db->prepare("UPDATE users SET userStatus='Active' WHERE userID=?");
			$stmt2->bind_param('i', $id);
			$stmt2->execute();
			
			// Check if a record was updated
			if($stmt2->affected_rows > 0) {
				// Update Success: redirect to confirmation page
				redirect('https://www.ninjalive.com/tanks/registration.php');
			} else {
				// Update Failed: redirect to error page
				redirect('https://www.ninjalive.com/tanks/registration.php?failed=2');
			}
		} else {
			// Failure: Code does not match, redirect to error pageS
			redirect('https://www.ninjalive.com/tanks/registration.php?failed=1');
		}
	}
	
?>