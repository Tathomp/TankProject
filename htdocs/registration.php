<?php

	header("Access-Control-Allow-Origin: *");

	include('header.inc');	
	
	if($_GET['failed'] == 1) {
		$msg = "Something is wrong with your activation code.
		You can get this error if you have already activated your account.
		If you believe this may be the case, try signing into the game by clicking <a href=\"https://www.tanks.claytonmichaelphoto.com/index.html\">here</a>.";
	} else if($_GET['failed'] == 2) {
		$msg = "We're sorry, something went wrong and we couldn't activate your account at this time.
		Please try again later.";
	} else {
		$msg = "Thank you for verifying your email address, your account has been activated.<br>
		You may now log-in and play!<br>
		Click <a href=\"https://www.tanks.claytonmichaelphoto.com/index.html\">here</a> to enjoy the game!";
	}
?>

<div id='admin'>
	<h2>Tankware Engineering</h2>
	<h4>Thank you for registering!</h4>
	<?php
		print $msg;
	?>
	<br>
</div>

<?php include('footer.inc'); ?>