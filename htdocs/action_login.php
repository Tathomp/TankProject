<?php
	// Sign In Validation of email and password

	// Database connection
	require_once('dbconnect.php');
	
	// Only proceed if data was actually POSTed and includes values
	if(!empty($_POST['mail']) && !empty($_POST['pass'])){
	
		// Get POSTed values from Unity
		$mail = trim($_POST['mail']);
		$pass = trim($_POST['pass']);
				
		// Hash the password
		$pass = hash('sha256', $pass);
		
		// Build Query: match email and password hash, return userID
		$stmt = $db->prepare("SELECT userStatus FROM users WHERE userEmail=? AND userPass=?");
		$stmt->bind_param('ss', $mail, $pass);
		$stmt->execute();
		$result = $stmt->get_result();
		
		
		// Check for result from query
		if($result && mysqli_num_rows($result) > 0) {
			// Success: User & Password Match
			$row = $result->fetch_object();
			
			// Check registration status
			if($row->userStatus == "Active") {
				// Success: Active account
				$dataArray = array('success' => true, 'error' => '');
			} else {
				// Failure: Inactive account
				$dataArray = array('success' => false, 'error' => 'inactive');
			}
		} else {
			// Failure: set error msg for unity
			$dataArray = array('success' => false, 'error' => 'invalid');
		}
		
		
	}
	
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
	
?>