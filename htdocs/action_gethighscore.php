<?php
	// Return the player's highest score 
	
	// Database connection
	require_once('dbconnect.php');

	// Get POSTed values from Unity
	$id = $_POST['id'];
	
	// Build Query		
	$stmt = $db->prepare("SELECT score FROM levelscores WHERE userID=? ORDER BY score DESC LIMIT 1");
	$stmt->bind_param('i', $id);
	$stmt->execute();
	$result = $stmt->get_result();
	
	// Check for result from query
	if($result->num_rows > 0) {
		
		// Move first row results into string objects 
		$row = $result->fetch_object();
		$score = (string)$row->score;
				
	} else {
		// Failure: set error msg for unity
		$score = (string)0;
	}
	
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	echo $score;
?>