import 'ace-builds/src-noconflict/ace';
import 'ace-builds/src-noconflict/mode-javascript';
import 'ace-builds/src-noconflict/theme-monokai';
import axios from 'axios';

document.mainAceEditor = ace.edit("main_editor", {
	theme: "ace/theme/monokai",
	mode: "ace/mode/javascript",
	autoScrollEditorIntoView: false,
	maxLines: 30,
	minLines: 30,
});
document.mainAceEditor.session.setMode("ace/mode/javascript");
document.animationAceEditor = ace.edit("animation_editor", {
	theme: "ace/theme/monokai",
	mode: "ace/mode/javascript",
	autoScrollEditorIntoView: false,
	maxLines: 30,
	minLines: 30,
});
document.animationAceEditor.session.setMode("ace/mode/javascript");

var canvas = document.getElementById('view');
var gl = canvas.getContext('webgl', {preserveDrawingBuffer: true});

if (!gl)
{
	alert('Unable to initialize WebGL. Your browser may not support it.');
}

var thumbnailData = null;

document.getElementById('saveBtn').addEventListener('click',
	function ()
	{
		var name = prompt("Enter project name:");
		var description = prompt("Enter project description:");

		var jsonData = {
			name: name,
			description: description,
			thumbnail: thumbnailData,
			files: {
				"main.js": document.mainAceEditor.getValue()
				"animation.js": document.animationAceEditor.getValue()
			}
		};

		axios.post('/api/projects', jsonData)
			.then(response => {
				console.log('Success:', response.data);
				window.location.href = '/editor/' + response.data;
			})
			.catch(error => {
				console.error('Error:', error);
			});
	});
document.getElementById('executeBtn').addEventListener('click',
	function ()
	{
		try
		{
			eval(document.aceEditor.getValue(), gl=gl, canvas=canvas);
		} catch (error)
		{
			console.error('Error executing code:', error);
		}
	});

document.getElementById('saveThumbnailBtn').addEventListener('click',
	function ()
	{
		thumbnailData = canvas.toDataURL();
	});
