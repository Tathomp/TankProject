<?php

	// Database Connection for unity scripts
	$db = new mysqli('localhost', 'tcevcg7_unity', 'unity1@3$', 'tcevcg7_tankware', 3306);
	if($db->connect_error) die("DB Error");
	
?>