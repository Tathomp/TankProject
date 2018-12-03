<!doctype html>
<html>
<head>
	<meta charset="utf-8">
	<title>Avatar Uploader</title>
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js">	</script>
	
	<style>
		.filebutton img {
			width: 200px;
			height: 200px;
			border-radius: 160px;
			overflow: hidden;
		}
				
		label.filebutton {
			width:120px;
			height:40px;
			overflow:hidden;
			position:relative;
			background-color:#ccc;
			
		}

		label span input {
			z-index: 999;
			line-height: 0;
			position: absolute;
			top: -2px;
			left: -700px;
			opacity: 0;
			filter: alpha(opacity = 0);
			-ms-filter: "alpha(opacity=0)";
			cursor: pointer;
			_cursor: hand;
			margin: 0;
			padding:0;
		}
		
	</style>
</head>
<body>

	<form id="form1" runat="server" method="post" enctype="multipart/form-data">		
		<input type="hidden" value="<?php echo htmlspecialchars($_GET["id"]);?>" name="id" />
		<label class="filebutton">
			<img id="blah" src="<?php echo htmlspecialchars($_GET["img"]);?>" alt="your image">
			<span><input type="file" id="imgInp" name="files[]"></span>
		</label>
		<p><em>Click image to replace.</em></p>
		<input type="submit" id="Up" value="Upload Image" name="submit">
	</form>
	
	
	<script src="upload.js"></script>
	<script>
	$( document ).ready(function() {
        console.log( "document loaded" );
    });
 
    $( window ).on( "load", function() {
        console.log( "window loaded" );
    });
	function readURL(input) {
		  if (input.files && input.files[0]) {
			var reader = new FileReader();

			reader.onload = function(e) {
			  document.getElementById('blah').src=e.target.result
			}

			reader.readAsDataURL(input.files[0]);
		  }
		}

		$("#imgInp").change(function() {
		  readURL(this);
		});
	</script>

</body>
</html>