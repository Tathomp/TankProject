<?php

	header("Access-Control-Allow-Origin: *");

	// Database Connection for unity scripts
	$db = new mysqli('localhost', 'unity', 'unity1@3$', 'tankware', 3306);
	if($db->connect_error)	die("DB Error");
?>