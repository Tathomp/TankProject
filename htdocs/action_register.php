<?php
	// Register a new user account:
	// -Check for existing email
	// -Check for existing username
	// -Create record
	// -Set "UNVERIFIED ACCOUNT" status
	// -Send verification E-mail

	
	// Database connection
	require_once('dbconnect.php');
	
	// Get POSTed values from Unity
	$mail = $_POST['mail'];
	$pass = $_POST['pass'];
	$user = $_POST['user'];
	
	// Clean data
	$mail = strip_tags($mail);
	$pass = strip_tags($pass);
	$user = strip_tags($user);
	
	// Hash the password
	$pass = hash('sha256', $pass);
	
	// Build query statement
	$query = "SELECT userID FROM users WHERE userEmail='$mail'";
	
	// Execute query
	$result = mysqli_query($db, $query);
	
	// Check for result
	$row = mysqli_fetch_row($result);
	if($row) {
		$dataArray = array('success' => true, 'error' => 'exists');
	} else {
		$query2 = "INSERT INTO users (userName, userPass, userEmail) VALUES ('$user', '$pass', '$mail')";
		if($result2 = mysqli_query($db, $query2)) {
			$dataArray = array('success' => true, 'error' => '', 'email' => "$mail");
		} else {
			$dataArray = array('success' => false, 'error' => 'try again');
		}
	}
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
	
?>