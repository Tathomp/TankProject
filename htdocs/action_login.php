<?php
	// Sign In Validation of email and password

	// Database connection
	require_once('dbconnect.php');
	
	// Get POSTed values from Unity
	$mail = $_POST['mail'];
	$pass = $_POST['pass'];
	
	// Clean data
	$mail = strip_tags($mail);
	$pass = strip_tags($pass);
	
	// Hash the password
	$pass = hash('sha256', $pass);
	
	// Build query statement
	$query = "SELECT userID FROM users WHERE userEmail='$mail' AND userPass='$pass'";
	
	// Execute query
	$result = mysqli_query($db, $query);
	
	// Check for result
	$row = mysqli_fetch_row($result);
	if($row) {
		$dataArray = array('success' => true, 'error' => '');
	} else {
		$dataArray = array('success' => false, 'error' => 'invalid');
	}
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
	
?>