<?php
	// Password reset for unknown password
	
	// Database connection
	require_once('dbconnect.php');
	
	// Display a reset password form
	$msg = "
		<h3>Reset Tankware Engineering password below:</h3>
		<table id='login'>
			<form action='reset_pass.php' method='post'>
				<tr>
					<td>Email:</td>
					<td><input type='text' name='email'></td>
				</tr>
				<tr>
					<td>Authorization Code:</td>
					<td><input type='text' name='code'></td>
				</tr>
				<tr>
					<th colspan='2'><button type='submit'>Reset Password</button></th>
				</tr>
			</form>
		</table>";
	
	if(!empty($_POST['email']) && !empty($_POST['code'])) {
		
		// Get POSTed values from Unity
		$mail = trim($_POST['email']);
		$code = trim($_POST['code']);
		$blank = "";
		$passLength = 8;
		
		// Random new password
		$newPass = random_password($passLength);
		
		// Hash the password
		$passHash = hash('sha256', $newPass);
		
		// Query for existing user by id
		$stmt = $db->prepare("UPDATE users SET userPass = ?, userCode = ? WHERE userEmail = ? AND userCode = ?");
		$stmt->bind_param('ssss', $passHash, $blank, $mail, $code);
		$stmt->execute();
						
		if($stmt->affected_rows > 0) {	
			// Success:
			$msg = "
				<h3>Success! Your password has been changed.</h3>
				Your new password has been emailed to you.
				Click <a href=\"https://www.ninjalive.com/tanks/index.html\">here</a> to login.";
			send_email($mail, $newPass);
		}
		else {
			// Failure:
			$msg = "
				<h3>Sorry, that code is no longer valid.</h3>
				If you need to, a new code can be requested from the Forgot Password link <a href=\"http://www.ninjalive.com/tanks/index.html\">here</a>.";
		}
	}
	
	// Close db connection
	$db->close();
	
	// Display web page
	include('header.inc');
	print $msg;
	include('footer.inc');
	
	// Sends an email for account verification
	function send_email($mail, $newPass) {
		// Set email subject line
		$subject = 'Tankware | New Password';
		
		// Set email message body
		$message = '
		 
		Your password was reset successfully.
		You can now login with this temporary password: '.$newPass.'
		
		After you login you can choose your own password from the profile screen.
		
		Login here: https://www.ninjalive.com/tanks/index.html
		
		Have fun playing Tankware Engineering!';
		
		// Set message headers
		$headers = 'From: noreply@ninjalive.com' . '\r\n';
		// Send the email
		mail($mail, $subject, $message, $headers);
	}
	
	// Generate random password
	function random_password($length) {
		$chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-=+;:,.?";
		for ($i = 0; $i < $length; $i++) {
			$password .= $chars{mt_rand(0, strlen($chars) - 1)};
		}
		return $password;
	}
?>