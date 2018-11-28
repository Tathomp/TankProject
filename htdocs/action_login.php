<?php
	// Sign In Validation of email and password

	header("Access-Control-Allow-Origin: *");
	
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
		$stmt = $db->prepare("SELECT userID, userStatus, userName, userEmail, userImage, maxLevel, activeUpgrades, purchasedUpgrades, userCredits FROM users WHERE userEmail=? AND userPass=?");
		$stmt->bind_param('ss', $mail, $pass);
		$stmt->execute();
		$result = $stmt->get_result();
		
		
		// Check for result from query
		if($result && $result->num_rows > 0) {
			// Success: User & Password Match
			$row = $result->fetch_object();
			
			// Check registration status
			if($row->userStatus == "Active") {
				// Success: Active account
				$dataArray = array( 'query' => true,
									'success' => true,
									'msg' => 'Login successful...',
									'userName' => $row->userName,
									'userEmail' => $row->userEmail,
									'userID' => $row->userID,
									'userImage' => $row->userImage,
									'maxLevel' => $row->maxLevel,
									'activeUpgrades' => $row->activeUpgrades,
									'purchasedUpgrades' => $row->purchasedUpgrades,
									'userCredits' => $row->userCredits);
			} else {
				// Failure: Inactive account
				$dataArray = array('query' => true, 'success' => false, 'msg' => 'Account is not active. Verify your Email address');
			}
		} else {
			// Failure: set error msg for unity
			$dataArray = array('query' => false, 'success' => false, 'msg' => 'Invalid Email or Password');
		}
		
		
	}
	
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
	
?>