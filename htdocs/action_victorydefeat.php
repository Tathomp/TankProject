<?php
	// Update user high score for given level
	// Update unlocked level
	// Update total credits
	
	// Database connection
	require_once('dbconnect.php');
		
	// Get POSTed values from Unity
	$id = $_POST['id'];	
	$credit = $_POST['credit'];
	$maxLevel = $_POST['level'];
	$score = $_POST['score'];
	$levelPlayed = $_POST['played'];
	
	// Build Query: update userCredit and maxLevel
	$stmt = $db->prepare("UPDATE users SET maxLevel = ?, userCredits = ? WHERE userID = ?");
	$stmt->bind_param('iii', $maxLevel, $credit, $id);
	$stmt->execute();
	
	// Build Query: update high score for player and level if new score is larger
	$stmt2 = $db->prepare("INSERT INTO levelscores (userID, levelID, score) VALUES (?, ?, ?) ON DUPLICATE KEY UPDATE score = IF(score < ?, ?, score)");
	$stmt2->bind_param('iiiii', $id, $levelPlayed, $score, $score, $score);
	$stmt2->execute();
	
?>