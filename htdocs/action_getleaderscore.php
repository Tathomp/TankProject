<?php
	// Return the highest score with attached userName for given levelID 
	
	// Database connection
	require_once('dbconnect.php');

	// Get POSTed values from Unity
	$levelID = $_POST['levelID'];
	
	$dataArray = array('query' => false, 'success' => false, 'msg' => 'Query failed', 'users' => "", 'scores' => "");	
	
	// Build Query		
	$stmt = $db->prepare("SELECT users.userName, score FROM levelscores LEFT JOIN users ON (levelscores.userID = users.userID) WHERE levelID = ? ORDER BY score DESC LIMIT 1");
	$stmt->bind_param('i', $levelID);
	$stmt->execute();
	$result = $stmt->get_result();
	
	// Check for result from query
	if($result && $result->num_rows > 0) {
		
		// Move results into string objects 
		$row = $result->fetch_object();
		$userName = $row->userName;
		$score = (string)$row->score;
	
		// Success: set array for JSON encoding
		$dataArray = array('query' => true, 'success' => true, 'msg' => '', 'users' => "$userName", 'scores' => "$score");
		
	} else {
		// Failure: set error msg for unity
		$dataArray = array('query' => true, 'success' => false, 'msg' => 'Query null', 'users' => "", 'scores' => "");
	}
		
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
?>