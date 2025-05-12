
async function makeAjaxCall(url, type, data = null) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            type: type,
            contentType: 'application/json',
            data: data,
            success: function (response) {
                resolve(response);
                fnSetVal(response);
            },
            error: function (error) {
                reject(error);
            }
        });
    });
}
function fnClearExtra() {
    var stringKeysElements = document.getElementsByName('StringKeys');
    for (var i = 0; i < stringKeysElements.length; i++) {
        stringKeysElements[i].value = "";
    }

    // Reset textboxes with name="DecimalValues"
    var decimalValuesElements = document.getElementsByName('DecimalValues');
    for (var i = 0; i < decimalValuesElements.length; i++) {
        decimalValuesElements[i].value = "";
    }
}
function fetchData() {
    const selectedValue = incomeDropdown.value;
    const url = `/Visual_Grid/Change?Selected=${encodeURIComponent(selectedValue)}`;

    fetch(url)
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.json();
        })
        .then(data => {
            console.log("Received data:", data);
            if (data.Message && data.Message === "No data found") {
                window.location.href = '/Visual_Grid/Index';
            } else {
                updateTable(data);
            }
        })
        .catch(error => console.log("Error:", error));
}
async function fnColateExtraData(entryID) {
    try {
        console.log('Processing entry:', entryID);
        const containers = Array.from(document.getElementsByClassName('frmInputs'));
        console.log('Found containers:', containers);

        // Build data array with proper error handling
        const data = containers.map(container => {
            const tvElement = container.getElementsByClassName('tv')[0];
            const dvElement = container.getElementsByClassName('dv')[0];

            return {
                entryId: entryID,
                description: tvElement ? tvElement.value : '',
                amount: dvElement ? parseFloat(dvElement.value) || 0 : 0
            };
        });

        console.log('Collected data:', data);

        // Process requests sequentially
        for (const [index, item] of data.entries()) {
            try {
                const response = await fetch('/Visual_Grid/ExtraData', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(item)
                });

                if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

                const result = await response.json();
                console.log(`Item ${index + 1} success:`, result);

                // Update UI after each successful request
                fnSetVal(result);
                await makeGrid();
                fnClearExtra();

            } catch (error) {
                console.error(`Error processing item ${index + 1}:`, error);
                // Consider adding retry logic or user notification here
            }
        }

        console.log('All requests completed');
        return true;

    } catch (error) {
        console.error('Critical error in fnColateExtraData:', error);
        throw error; // Re-throw for calling function to handle
    }
}
async function makeGrid() {
    try {
        const entryId = document.getElementById('incomeDropdown').value;
//document.getElementById('hiddenID').textContent.trim();
        const url = `/Visual_Grid/Change?Selected=${encodeURIComponent(entryId)}`;

        const response = await fetch(url);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        console.log("Received data:", data);

        if (data.Message && data.Message === "No data found") {
            window.location.href = '/Visual_Grid/Index';
        } else {
            updateTable(data);
        }

    } catch (error) {
        console.error("Error in makeGrid:", error);
        // Optional: Add user-facing error message
    }
}
function fnSetVal(response) {
    document.getElementById('incomeInput').value = response.income;
    document.getElementById('costInput').value = response.costs;
    document.getElementById('netInput').value = response.income - response.costs;
}
function updateTable(data) {

    const table = document.getElementById('dataTable');
    const incomeInput = document.getElementById('incomeInput');
    const costInput = document.getElementById('costInput');
    const netInput = document.getElementById('netInput');
    const hiddenID = document.getElementById('hiddenID');

    if (!data || !Array.isArray(data) || data.length === 0) {
        // Clear the table
        table.innerHTML = ''; // Clear the table
        incomeInput.value = '';
        costInput.value = '';
        netInput.value = '';
        hiddenID.innerHTML = '';
        console.log("No data available. Table has been cleared.");
        return; // Exit the function early
    }

    // Build the table header
    const buildTableHeader = () => {
        return `
                <thead>
                    <tr>
                        <th scope="col">Entry ID</th>
                        <th scope="col">Description Time</th>
                        <th scope="col">Entry Name</th>
                        <th scope="col">Amount</th>
                        <th scope="col">Income</th>
                        <th scope="col">Save</th>
                        <th scope="col">Delete</th>
                    </tr>
                </thead>
            `;
    };

    const buildTableRow = (item) => {
        return `
                <tr>
                    <td>${item.entry_id}</td>
                    <td>${item.description_time}</td>
                    <td>${item.entry_name}</td>
                    <td><input type="text" class="amountInput" value="${item.amount}"></td>
                    <td>${item.income}</td>
                    <td><button type="button" class="btn btn-primary saveButton" data-entry-id="${item.entry_id}" data-amount="${item.amount}">Save</button></td>
                    <td><button type="button" class="btn btn-primary deleteButton" data-entry-id="${item.entry_id}">Delete</button></td>
                </tr>
            `;
    };

    // Calculate total costs
    const calculateTotalCosts = (data) => {
        return data.reduce((sum, item) => sum + item.amount, 0);
    };

    table.innerHTML = '';

    // Build the table
    let rows = buildTableHeader();
    let summedcosts = 0;
    let income = 0;
    let main_id = 0;


    data.forEach((item, index) => {
        if (
            //item && item.entry_id && item.description_time && item.entry_name && item.amount && item.income
            item &&
            typeof item.entry_id === 'number' &&  // Accepts negative numbers
            item.description_time &&
            item.entry_name &&
            typeof item.amount === 'number' &&    // Explicit number check
            typeof item.income === 'number'
        ) {
            if (index === 0) {
                income = item.income;
                main_id = item.id;
                hiddenID.innerHTML = main_id;
            }
            rows += buildTableRow(item);
            summedcosts += item.amount;
        }
        else {
            console.error('Invalid item:', item);
        }
    });

    table.innerHTML = rows;
    incomeInput.value = income;
    costInput.value = summedcosts;
    netInput.value = income - summedcosts;
}
async function saveIncome(entryId, amount) {
    console.log('Data:', { entryId: entryId, amount: amount });

    const url = '/Visual_Grid/Income_Amend';
    const type = 'POST';
    const data = JSON.stringify({ entryId, amount });
    console.log(JSON.stringify({ entryId, amount }));

    try {
        const response = await makeAjaxCall(url, type, data);
        console.log('Entry saved successfully:', response);
    }
    catch (error) {
        console.error('Error saving entry:', error);
    }

}
async function saveEntry(entryId, amount, id)
 {
     const url = '/Visual_Grid/Make';
    const type = 'POST';


    const payload = {
        entryId: entryId,
        amount: amount,
        id: id
    };

    console.log('Sending data:', payload);

     try
     {
         //const data = JSON.stringify({ entryId: entryId, amount: amount, id: id });

         //console.log('Data:', { entryId: entryId, amount: amount });


         const response = await makeAjaxCall(url, type, JSON.stringify(payload));
         console.log('Entry saved successfully:', response);
          // Ensure this runs only after the response is received
     }
     catch (error)
     {
     console.error('Error saving entry:', error);
     }
 }
function deleteEntry(entryId) {
    console.log('Delete button clicked for entry ID:', entryId);
    $.ajax({
        url: '/Visual_Grid/DeleteEntry'
        , type: 'POST'
        , contentType: 'application/json'
        , data: JSON.stringify({ entryId: entryId })
        , success: function (response) {
            console.log('Entry deleted successfully:', response);
            makeGrid();
            fnSetIncome();

        }
        , error: function (error) { console.error('Error deleting entry:', error); }
    });


}
function fnSetIncome() {
    var entryId = document.getElementById('hiddenID').innerHTML;
    var amount = $('#incomeInput').val();

    var entryId = document.getElementById('hiddenID').innerHTML;
    $.ajax(
        {
            url: '/Visual_Grid/Income_Amend'
            , type: 'POST'
            , contentType: 'application/json'
            , data: JSON.stringify({ entryId: entryId, amount: amount })
            , success: function (response) {
                console.log('Entry saved successfully:', response);
                fnSetVal(response);
                // document.getElementById('incomeInput').value = response.income;
                // document.getElementById('costInput').value = response.costs;
                // document.getElementById('netInput').value =  response.income - response.costs;
            }
            , error: function (error) { console.error('Error saving entry:', error); }


        });
}

document.addEventListener('DOMContentLoaded', function () {
    const incomeDropdown = document.getElementById('incomeDropdown');

    // 2. Define fetchData INSIDE this scope
    const fetchData = () => {
        const selectedValue = incomeDropdown.value;
        const url = `/Visual_Grid/Change?Selected=${encodeURIComponent(selectedValue)}`;

        fetch(url)
            .then(response => {
                if (!response.ok) throw new Error('Network error');
                return response.json();
            })
            .then(data => {
                if (data.Message === "No data found") window.location.href = '/Visual_Grid/Index';
                else updateTable(data);
            })
            .catch(console.error);
    };

    if (incomeDropdown) {
        fetchData(); // Initial load
        incomeDropdown.addEventListener('change', fetchData);
    }
});
document.addEventListener('click', async function (event) {
    if (event.target.matches('#addExtraData')) {
        try {
            const entryId = document.getElementById('hiddenID').textContent.trim() ||
                document.getElementById('incomeDropdown').value;

            await fnColateExtraData(entryId);
            console.log('Data processing completed successfully');

        } catch (error) {
            console.error('Failed to process data:', error);
            // Show user error notification
        }
    }
});
document.addEventListener('click', (event) => {
    const target = event.target;

    // Save Button Handler
    if (target.matches('.saveButton')) {
        const row = target.closest('tr');
        const entryId = target.dataset.entryId;
        const amount = parseFloat(row.querySelector('.amountInput').value);
        const id = parseInt(document.getElementById('hiddenID').textContent);

        console.log('Saving:', { entryId, amount, id });
        saveEntry(entryId, amount, id);
    }

    // Delete Button Handler
    if (target.matches('.deleteButton')) {
        const entryId = target.dataset.entryId;
        console.log('Deleting:', entryId);
        deleteEntry(entryId);
    }

    // Amend Income Handler
    if (target.matches('.amend_I')) {
        const entryId = document.getElementById('hiddenID').textContent;
        const amount = document.getElementById('incomeInput').value;

        console.log('Amending income:', { entryId, amount });
        saveIncome(entryId, amount);
    }
});

