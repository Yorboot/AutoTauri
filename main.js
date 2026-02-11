//this is a js file for handling site wide functions not electron specific

let currentTab = 0; // Current tab is set to be the first tab (0)

document.addEventListener('DOMContentLoaded', function() {
    showTab(currentTab);
    addListenerToForm();

});

function showTab(n) {

    const c = document.getElementById("car-form");
    if(c === null) {
        console.error("Form with id 'car-form' not found.");
        return;
    }
    const x = c.getElementsByClassName("tab");
    if(x.length === 0) {
        console.error("No tabs found with class 'tab'.");
        return;
    }
    x[n].style.display = "block";

    if (n === 0) {
        document.getElementById("prevBtn").style.display = "none";
    } else {
        document.getElementById("prevBtn").style.display = "inline";
    }
    if (n === (x.length - 1)) {
        document.getElementById("nextBtn").style.display = "none";
        document.getElementById("submitBtn").style.display = "block";
    } else {
        document.getElementById("nextBtn").innerHTML = "Next";
    }

    fixStepIndicator(n)
}
function addListenerToForm(){
    document.getElementById('car-form').addEventListener('submit', async (e) => {
        e.preventDefault();
        let errorText = document.getElementById("error-box");
        const formData = new FormData(e.target);
        const data = Object.fromEntries(formData.entries());

        // Convert numeric fields
        data.userId = parseInt(data.userId);
        data.price = parseFloat(data.price);
        data.mileage = parseInt(data.mileage);
        data.seats = data.seats ? parseInt(data.seats) : null;
        data.doors = data.doors ? parseInt(data.doors) : null;
        data.productionYear = data.productionYear ? parseInt(data.productionYear) : null;
        data.weight = data.weight ? parseInt(data.weight) : null;

        const response = await fetch('http://localhost:8080/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            console.log('Car created successfully');
        } else {
            console.error('Car created failed with status ' + response.status);
            try{
                const error = await response.json();
                console.error('Failed to create car');
                errorText.style.display = "block";
                errorText.textContent = `Failed to create a car ${error.message || JSON.stringify(error)}`;
            }catch(parseError){
                console.error('Failed to parse error response', parseError);
                errorText.textContent = `Failed to create a car. Status: ${response.status}`;
            }
        }
        console.log("Form submitted with data:", data);
    });
}
function nextPrev(n) {

    const x = document.getElementsByClassName("tab");

    if (n === 1 && !validateForm()) return false;

    x[currentTab].style.display = "none";

    currentTab = currentTab + n;

    if (currentTab >= x.length) {

        document.getElementById("regForm").submit();
        return false;
    }

    showTab(currentTab);
}

function validateForm() {

    let x, y, i, valid = true;
    x = document.getElementsByClassName("tab");
    y = x[currentTab].getElementsByTagName("input");

    for (i = 0; i < y.length; i++) {

        if (y[i].value === "") {

            y[i].className += " invalid";

            valid = false;
        }
    }

    if (valid) {
        document.getElementsByClassName("step")[currentTab].className += " finish";
    }
    return valid;
}

function fixStepIndicator(n) {

    let i, x = document.getElementsByClassName("step");
    for (i = 0; i < x.length; i++) {
        x[i].className = x[i].className.replace(" active", "");
    }

    x[n].className += " active";
}