//alert('hi');

document.addEventListener('DOMContentLoaded', function () {
    const incomeDropdown = document.getElementById('incomeDropdown');
    let dropdownChanged = false;

    // Function to handle data fetching
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

    if (incomeDropdown) {
        // Run on initial page load
        fetchData();

        // Also run when dropdown changes
        incomeDropdown.addEventListener('change', function () {
            dropdownChanged = true;
            fetchData();
        });
    }


});