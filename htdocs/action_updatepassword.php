<?php
	// Update user's password:
	// -Validate users current password
	// -Update with new password
	
		
	// Database connection
	require_once('dbconnect.php');
	
	// Only proceed if data was actually POSTed and includes values
	if(!empty($_POST['id']) && !empty($_POST['currentPass']) && !empty($_POST['newPass'])) {
		
		// Get POSTed values from Unity
		$id = $_POST['id'];
		$currentPass = trim($_POST['currentPass']);
		$newPass = trim($_POST['newPass']);
				
		// Hash the password
		$currentPass = hash('sha256', $currentPass);
		$newPass = hash('sha256', $newPass);
					
		// Query for existing user by id
		$stmt = $db->prepare("UPDATE users SET userPass = ? WHERE userID = ? AND userPass = ?");
		$stmt->bind_param('sis', $newPass, $id, $currentPass);
		$stmt->execute();
						
		if($stmt->affected_rows > 0) {			
			// Success:
			$dataArray = array('query' => true, 'success' => true, 'msg' => 'Success!');
		}
		else {
			// Failure: set msg for debugging
			$dataArray = array('query' => true, 'success' => false, 'msg' => 'Invalid Password!');
		}
	}
		
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
?>