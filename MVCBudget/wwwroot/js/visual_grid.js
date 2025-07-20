async function makeAjaxCall(url, type, data = null) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            type: type,
            contentType: 'application/json',
            data: data,
            success: function (response) {
                resolve(response);
            },
            error: function (error) {
                reject(error);
            }
        });
    });
}

function fnClearExtra() {
    document.getElementsByName('StringKeys').forEach(el => el.value = "");
    document.getElementsByName('DecimalValues').forEach(el => el.value = "");
}

async function fetchData() {
    const selectedValue = document.getElementById('incomeDropdown').value;
    const url = `/Visual_Grid/Change?Selected=${encodeURIComponent(selectedValue)}`;

    try {
        const response = await fetch(url);
        if (!response.ok) throw new Error('Network response was not ok');
        const data = await response.json();

        if (data.Message === "No data found") {
            window.location.href = '/Visual_Grid/Index';
        } else {
            await updateTable(data);
            await makePrevGrid();
        }
    } catch (error) {
        console.error("Error:", error);
    }
}

async function fnColateExtraData(entryID) {
    try {
        const containers = Array.from(document.getElementsByClassName('frmInputs'));
        const data = containers.map(container => {
            const tvElement = container.querySelector('.tv');
            const dvElement = container.querySelector('.dv');
            return {
                entryId: entryID,
                description: tvElement ? tvElement.value : '',
                amount: dvElement ? parseFloat(dvElement.value) || 0 : 0
            };
        });

        for (const item of data) {
            const response = await fetch('/Visual_Grid/ExtraData', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(item)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();
            fnSetVal(result);
        }

        await makeGrid(); // Ensure makeGrid is awaited if it's asynchronous
        fnClearExtra();
    } catch (error) {
        console.error('Error in fnColateExtraData:', error);
        throw error; // Re-throw the error to be caught by the event listener
    }
}

document.addEventListener('click', async function (event) {
    if (event.target.matches('#addExtraData')) {
        try {
            const entryId = document.getElementById('hiddenID').textContent.trim() || document.getElementById('incomeDropdown').value;
            await fnColateExtraData(entryId);
            console.log('Data processing completed successfully');
        } catch (error) {
            console.error('Failed to process data:', error);
            // Optionally show user error notification
        }
    }
});


async function makeGrid() {
    try {
        const entryId = document.getElementById('incomeDropdown').value;
        const url = `/Visual_Grid/Change?Selected=${encodeURIComponent(entryId)}`;

        console.log(`Fetching data from URL: ${url}`);

        const response = await fetch(url);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();

        console.log("Received data:", data);

        if (data.Message && data.Message === "No data found") {
            console.log("No data found, redirecting to index.");
            window.location.href = '/Visual_Grid/Index';
        } else if (data && Array.isArray(data) && data.length > 0) {
            console.log("Updating table with data:", data);
            await updateTable(data);
        } else {
            console.log("No valid data available for Main Grid. Clearing the table.");
            const table = document.getElementById('dataTable');
            table.innerHTML = '';
        }
    } catch (error) {
        console.error("Error in makeGrid:", error);
    }
}


async function updateTable(data) {
    const table = document.getElementById('dataTable');
    const incomeInput = document.getElementById('incomeInput');
    const costInput = document.getElementById('costInput');
    const netInput = document.getElementById('netInput');
    const hiddenID = document.getElementById('hiddenID');

    console.log("Data received in updateTable:", data);

    if (!data || !Array.isArray(data) || data.length === 0) {
        console.log("No valid data available for Main Grid. Clearing the table.");
        table.innerHTML = '';
        incomeInput.value = '';
        costInput.value = '';
        netInput.value = '';
        hiddenID.innerHTML = '';
        return;
    }

    // Validate data structure
    const isDataValid = data.every(item =>
        item &&
        typeof item.entry_id === 'number' &&
        item.description_time &&
        item.entry_name &&
        typeof item.amount === 'number' &&
        typeof item.income === 'number'
    );

    if (!isDataValid) {
        console.error("Data structure is invalid:", data);
        return;
    }

    const buildTableHeader = () => `
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
        </thead>`;

    const buildTableRow = (item) => `
        <tr>
            <td>${item.entry_id}</td>
            <td>${item.description_time}</td>
            <td>${item.entry_name}</td>
            <td><input type="text" class="amountInput" value="${item.amount}"></td>
            <td>${item.income}</td>
            <td><button type="button" class="btn btn-primary saveButton" data-entry-id="${item.entry_id}" data-amount="${item.amount}">Save</button></td>
            <td><button type="button" class="btn btn-primary deleteButton" data-entry-id="${item.entry_id}">Delete</button></td>
        </tr>`;

    let rows = buildTableHeader();
    let summedCosts = 0;
    let income = 0;
    let main_id = 0;

    data.forEach((item, index) => {
        if (index === 0) {
            income = item.income;
            main_id = item.id;
            hiddenID.innerHTML = main_id;
        }
        rows += buildTableRow(item);
        summedCosts += item.amount;
    });

    table.innerHTML = rows;
    incomeInput.value = income;
    costInput.value = summedCosts;
    netInput.value = income - summedCosts;
}

async function makePrevGrid() {
    try {
        const entryId = document.getElementById('incomeDropdown').value;
        const url = `/Visual_Grid/GetPossibles?id=${encodeURIComponent(entryId)}`;
        const response = await fetch(url);

        if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
        const data = await response.json();

        if (data.Message === "No data found") {
            window.location.href = '/Visual_Grid/Index';
        } else {
            updatePrevTable(data);
        }
    } catch (error) {
        console.error("Error in makePrevGrid:", error);
    }
}

function fnSetVal(response) {
    document.getElementById('incomeInput').value = response.income;
    document.getElementById('costInput').value = response.costs;
    document.getElementById('netInput').value = response.income - response.costs;
}

function updatePrevTable(data) {
    const table = document.getElementById('prevTable');
    if (!data || !Array.isArray(data) || data.length === 0) {
        table.innerHTML = '';
        console.log("No data available. Table has been cleared.");
        return;
    }

    const buildTableHeader = () => `<thead><tr><th scope="col">Possible Expense</th></tr></thead>`;
    const buildTableRow = (item) => `<tr><td>${item}</td></tr>`;

    table.innerHTML = data.reduce((rows, item) => rows + buildTableRow(item), buildTableHeader());
}

//async function updateTable(data) {
//    const table = document.getElementById('dataTable');
//    const incomeInput = document.getElementById('incomeInput');
//    const costInput = document.getElementById('costInput');
//    const netInput = document.getElementById('netInput');
//    const hiddenID = document.getElementById('hiddenID');

//    if (!data || !Array.isArray(data) || data.length === 0) {
//        table.innerHTML = '';
//        incomeInput.value = '';
//        costInput.value = '';
//        netInput.value = '';
//        hiddenID.innerHTML = '';
//        console.log("No data available for Main Grid. Table has been cleared.");
//        return;
//    }

//    const buildTableHeader = () => `
//        <thead>
//            <tr>
//                <th scope="col">Entry ID</th>
//                <th scope="col">Description Time</th>
//                <th scope="col">Entry Name</th>
//                <th scope="col">Amount</th>
//                <th scope="col">Income</th>
//                <th scope="col">Save</th>
//                <th scope="col">Delete</th>
//            </tr>
//        </thead>`;

//    const buildTableRow = (item) => `
//        <tr>
//            <td>${item.entry_id}</td>
//            <td>${item.description_time}</td>
//            <td>${item.entry_name}</td>
//            <td><input type="text" class="amountInput" value="${item.amount}"></td>
//            <td>${item.income}</td>
//            <td><button type="button" class="btn btn-primary saveButton" data-entry-id="${item.entry_id}" data-amount="${item.amount}">Save</button></td>
//            <td><button type="button" class="btn btn-primary deleteButton" data-entry-id="${item.entry_id}">Delete</button></td>
//        </tr>`;

//    const calculateTotalCosts = (data) => data.reduce((sum, item) => sum + item.amount, 0);

//    let rows = buildTableHeader();
//    let summedCosts = 0;
//    let income = 0;
//    let main_id = 0;

//    data.forEach((item, index) => {
//        if (item && typeof item.entry_id === 'number' && item.description_time && item.entry_name && typeof item.amount === 'number' && typeof item.income === 'number') {
//            if (index === 0) {
//                income = item.income;
//                main_id = item.id;
//                hiddenID.innerHTML = main_id;
//            }
//            rows += buildTableRow(item);
//            summedCosts += item.amount;
//        } else {
//            console.error('Invalid item:', item);
//        }
//    });

//    table.innerHTML = rows;
//    incomeInput.value = income;
//    costInput.value = summedCosts;
//    netInput.value = income - summedCosts;
//}

async function saveIncome(entryId, amount) {
    try {
        const response = await makeAjaxCall('/Visual_Grid/Income_Amend', 'POST', JSON.stringify({ entryId, amount }));
        await makeGrid();
        await makePrevGrid();
        await fnSetIncome();
        console.log('Entry saved successfully:', response);
    } catch (error) {
        console.error('Error saving entry:', error);
    }
}

async function saveEntry(entryId, amount, id) {
    try {
        const response = await makeAjaxCall('/Visual_Grid/Make', 'POST', JSON.stringify({ entryId, amount, id }));
        await makeGrid();
        await makePrevGrid();
        await fnSetIncome();



        console.log('Entry saved successfully:', response);
    } catch (error) {
        console.error('Error saving entry:', error);
    }
}

async function deleteEntry(entryId) {
    $.ajax({
        url: '/Visual_Grid/DeleteEntry',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ entryId: entryId }),
        success: async function (response) {
            console.log('Entry deleted successfully:', response);
            await makeGrid();
            await makePrevGrid();
            await fnSetIncome();
        },
        error: function (error) {
            console.error('Error deleting entry:', error);
        }
    });
}

async function fnSetIncome() {
    const entryId = document.getElementById('hiddenID').innerHTML;
    const amount = $('#incomeInput').val();
    $.ajax({
        url: '/Visual_Grid/Income_Amend',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ entryId: entryId, amount: amount }),
        success: function (response) {
            console.log('Entry saved successfully:', response);
            fnSetVal(response);
        },
        error: function (error) {
            console.error('Error saving entry:', error);
        }
    });
}

document.addEventListener('DOMContentLoaded', function () {
    const incomeDropdown = document.getElementById('incomeDropdown');
    if (incomeDropdown) {
        fetchData();
        incomeDropdown.addEventListener('change', fetchData);
    }

    document.addEventListener('click', async function (event) {
        //if (event.target.matches('#addExtraData')) {
        //    try {
        //        const entryId = document.getElementById('hiddenID').textContent.trim() || document.getElementById('incomeDropdown').value;
        //        await fnColateExtraData(entryId);
        //    } catch (error) {
        //        console.error('Failed to process data:', error);
        //    }
        //}

        const target = event.target;
        if (target.matches('.saveButton')) {
            const row = target.closest('tr');
            const entryId = target.dataset.entryId;
            const amount = parseFloat(row.querySelector('.amountInput').value);
            const id = parseInt(document.getElementById('hiddenID').textContent);
            await saveEntry(entryId, amount, id);
        }

        if (target.matches('.deleteButton')) {
            const entryId = target.dataset.entryId;
            await deleteEntry(entryId);
        }

        if (target.matches('.amend_I')) {
            const entryId = document.getElementById('incomeDropdown').value;
            const amount = document.getElementById('incomeInput').value;
            await saveIncome(entryId, amount);
        }
    });

    const table = document.getElementById('prevTable');
    const textBox = document.getElementById('EntryPoint');
    table.addEventListener('click', function (event) {
        let row = event.target;
        while (row && row !== table) {
            if (row.tagName === 'TR') {
                const cell = row.querySelector('td');
                if (cell) {
                    textBox.value = cell.textContent;
                }
                break;
            }
            row = row.parentNode;
        }
    });
});