<?php
	// Get user state data
	
	// Database connection
	require_once('dbconnect.php');
		
	// Get POSTed values from Unity
	$id = $_POST['id'];
	$email = trim($_POST['email']);
		
	// Default Error: set msg for debugging
	$dataArray = array('query' => false, 'success' => false, 'msg' => 'No query');
	
	// Build Query: match email and password hash, return userID
	$stmt = $db->prepare("SELECT userName, userImage, maxLevel, activeUpgrades, purchasedUpgrades FROM users WHERE userID=? AND userEmail=?");
	$stmt->bind_param('is', $id, $email);
	$stmt->execute();
	$result = $stmt->get_result();
	
	
	// Check for result from query
	if($result && $result->num_rows > 0) {
		$row = $result->fetch_object();
		$dataArray = array( 'query' => true,
							'success' => true,
							'msg' => 'Login successful...',
							'userName' => $row->userName,
							'userImage' => $row->userImage,
							'maxLevel' => $row->maxLevel,
							'activeUpgrades' => $row->activeUpgrades,
							'purchasedUpgrades' => $row->purchasedUpgrades);
							
	else {
		// Failure: set msg for debugging
		$dataArray = array('query' => true, 'success' => false, 'msg' => 'Query failed');
	}

	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
?>