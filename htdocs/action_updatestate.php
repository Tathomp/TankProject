<?php
	// Update user state data
	
	// Database connection
	require_once('dbconnect.php');
		
	// Get POSTed values from Unity
	$id = $_POST['id'];
	$email = trim($_POST['email']);
	$level = $_POST['level'];
	$profile = trim($_POST['profile']);
	$purchased = trim($_POST['purchased']);
	$active = trim($_POST['active']);
	
	// Default Error: set msg for debugging
	$dataArray = array('query' => false, 'success' => false, 'msg' => 'No query');
	
	// Build Query: update player state
	$stmt = $db->prepare("UPDATE users SET userImage = ?, maxLevel = ?, activeUpgrades = ?, purchasedUpgrades = ? WHERE userID = ? AND userEmail = ?");
	$stmt->bind_param('sissis', $profile, $level, $active, $purchased, $id, $email);
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