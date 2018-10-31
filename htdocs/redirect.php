<?php
	// Takes in a url and redirects browser to that url
	function redirect($url)
	{
		header('Location: '.$url);
		die("<html>
			<meta http-equiv='refresh' content='0; $url'>
			<body onload=\"location.replace('$url');\">
			<a href='$url'>$url</a>
			</body></html>");
	}
?>