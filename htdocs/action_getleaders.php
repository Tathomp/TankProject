<?php
	// Return the 10 highest scores with attached user name 
	
	// Database connection
	require_once('dbconnect.php');

	$dataArray = array('query' => false, 'success' => false, 'msg' => 'Query failed');	
	
	// Build Query		
	$stmt = "SELECT users.userName, score FROM levelscores LEFT JOIN users ON (levelscores.userID = users.userID) ORDER BY score DESC LIMIT 10";
	$result = $db->query($stmt);
	
	// Check for result from query
	if($result && $result->num_rows > 0) {
		
		// Move first row results into string objects 
		$row = $result->fetch_object();
		$userNames = $row->userName;
		$scores = (string)$row->score;
		
		// Append remaining row results to string objects
		while($row = $result->fetch_object())
		{
			$userNames = $userNames . "," . $row->userName;
			$scores = $scores . "," . (string)$row->score;
		}
		
		// Success: set array for JSON encoding
		$dataArray = array('query' => true, 'success' => true, 'msg' => '', 'users' => "$userNames", 'scores' => "$scores");
		
	} else {
		// Failure: set error msg for unity
		$dataArray = array('query' => true, 'success' => false, 'msg' => 'Query null');
	}
		
	// Close db connection
	$db->close();
	
	// Return JSON to Unity
	header('Content-Type: application/json');
	echo json_encode($dataArray);
?>