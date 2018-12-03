<?php 

print($_POST['id']);

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    if (isset($_FILES['files'])) {
		//$id = $_POST['id'];
        $errors = [];
        $path = '/tanks/avatars/';
        $extensions = ['jpg', 'jpeg', 'png', 'gif'];

        $all_files = count($_FILES['files']['tmp_name']);

        for ($i = 0; $i < $all_files; $i++) {  
            $file_name = $_FILES['files']['name'][$i];
            $file_tmp = $_FILES['files']['tmp_name'][$i];
            $file_type = $_FILES['files']['type'][$i];
            $file_size = $_FILES['files']['size'][$i];
            $file_ext = strtolower(end(explode('.', $_FILES['files']['name'][$i])));

            $file = $path . $file_name;

            if (!in_array($file_ext, $extensions)) {
                $errors[] = 'Extension not allowed: ' . $file_name . ' ' . $file_type;
            }

            if ($file_size > 2097152) {
                $errors[] = 'File size exceeds limit: ' . $file_name . ' ' . $file_type;
            }

            if (empty($errors)) {
                //move_uploaded_file($file_tmp, $file);
				copy($file_tmp, $file);
				//updateRecord($id, $file_name);
            }
        }

        if ($errors) print_r($errors);
    }
}

function updateRecord($id, $url) {
	// Update user image
	
	// Database connection
	require_once('dbconnect.php');
	
	// Default Error: set msg for debugging
	$dataArray = array('query' => false, 'success' => false, 'msg' => 'No query');
	
	// Build Query: update player state
	$stmt = $db->prepare("UPDATE users SET userImage = ? WHERE userID = ?");
	$stmt->bind_param('si', $profile, $id);
	$stmt->execute();
					
			
	// Close db connection
	$db->close();
}