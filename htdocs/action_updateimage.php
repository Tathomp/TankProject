<?php
	// Update user image
	
	// Database connection
	require_once('dbconnect.php');
		
	// Get POSTed values from Unity
	$id = $_POST['id'];
	$profile = $_POST['url'];
	
	// Default Error: set msg for debugging
	$dataArray = array('query' => false, 'success' => false, 'msg' => 'No query');
	
	// Build Query: update player state
	$stmt = $db->prepare("UPDATE users SET userImage = ? WHERE userID = ?");
	$stmt->bind_param('si', $profile, $id);
	$stmt->execute();
					
	if($stmt->affected_rows > 0) {			
		// Success:
		$dataArray = array('query' => true, 'success' => true, 'msg' => '');
	}
	else {
		// Failure: set msg for debugging
		$dataArray = array('query' => true, 'success' => false, 'msg' => 'No records updated');
	}
		
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
?>