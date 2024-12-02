document.getElementById('addControl').addEventListener('click', function ()
{
    var container = document.getElementById('dynamicControls');
    var newDiv = document.createElement('div');
    newDiv.innerHTML = '<input type="text" name="StringKeys" /> <input type="text" name="DecimalValues" />';
    container.appendChild(newDiv);
});