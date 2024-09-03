let minute = 00;
let second = 00;
let count = 00;

// Static for now, this should be fetched later upon user login.
const UserId = 1;
let WorkoutId = null;

document.addEventListener("DOMContentLoaded", function () {

    // Load all the saved data from localstorage 
    //const localStorageData = localStorage.getItem("setComplete");
    //console.log("Local data: " + localStorageData);


    timer = true;
    stopWatch();

    fetch('/Workout/CheckForActiveWorkout?UserId=' + UserId)
        .then(response => response.json())
        .then(data => {
            //console.log(data);
            let activeWorkoutObj = JSON.parse(data);
            //console.log(activeWorkoutObj);
            /*loadActiveWorkoutExercises(activeWorkoutObj);*/

            // If the user has an active workout, updates WorkoutId to this value i.e. WorkoutId = 200.
            //try {
            //    if activeWorkoutObj.WorkoutId != null {
            //        WorkoutId = activeWorkoutObj[0].WorkoutId;
            //        console.log(WorkoutId);
            //    }

            //} catch (error) {
            //    console.error(error);
            //}

            WorkoutId = activeWorkoutObj[0].WorkoutId;
            //console.log(activeWorkoutObj);

            //console.log("Workout ID: " + WorkoutId);

            // If the first API lookup does not return a valid WorkoutId, the second API lookup which pulls the users active workout does not initiate.
            if (WorkoutId != null) {
                loadActiveWorkoutExercises(activeWorkoutObj);

            }
        })
        .catch(error => {
            console.log('Error occurred:', error);
        });


       
});


//window.addEventListener("load", (event) => {
//    // I need to do an if an item exists in setComplete
//    // Then parse it as json
//    // Then assign the values to the array
//    // Then for loop through each ID and apply the correct css class


//    // Checks if the set id tracking array is empty. If empty, skips writing the local storage data to it. Otherwise persistency between refreshes would not work.
//    console.log("Page is fully loaded");
    
//    let retrieveStorage = localStorage.getItem("setComplete");
//    let readJson = JSON.parse(retrieveStorage);
//    console.log(readJson);
//    let convertJsonToInt = readJson.map(item => Number(item));
//    console.log(convertJsonToInt);
//    setCompleteArray = convertJsonToInt;
//    console.log('Final array: ' + setCompleteArray);

    
//    setCompleteArray.forEach(dataSetId => {
//        console.log(dataSetId);
//        console.log(`Searching for setid='${dataSetId}'`);
//        //getRow = document.querySelector("#" + id);
//        let getRow = document.querySelector(`[data-setid='${dataSetId}']`);
//        console.log(getRow);
//        getRow.classList.toggle("setComplete");
//    });

//})

//if (WorkoutId != null) {
//    fetch('/Workout/CheckForActiveWorkout?UserId=' + UserId)
//        .then(response => response.json())
//        .then(data => {
//            console.log(data);
//            let obj = JSON.parse(data);

//            // If the user has an active workout, updates WorkoutId to this value i.e. WorkoutId = 200.
//            WorkoutId = obj[0].WorkoutId;

//        })
//        .catch(error => {
//            console.log('Error occurred:', error);
//        });
//}


function stopWatch() {
    if (timer) {
        count++;

        if (count == 100) {
            second++;
            count = 0;
        }

        if (second == 60) {
            minute++;
            second = 0;
        }

        // if (minute == 60) {
        //     hour++;
        //     minute = 0;
        //     second = 0;
        // }

        // let hrString = hour;
        let minString = minute;
        let secString = second;
        let countString = count;

        // if (hour < 10) {
        //     hrString = "0" + hrString;
        // }

        if (minute < 10) {
            minString = "0" + minString;
        }

        if (second < 10) {
            secString = "0" + secString;
        }

        // if (count < 10) {
        //     countString = "0" + countString;
        // }

        // document.getElementById('hr').innerHTML = hrString;
        document.getElementById('min').innerHTML = minString;
        document.getElementById('sec').innerHTML = secString;
        document.getElementById('count');
        setTimeout(stopWatch, 10);
    }
};



let modal = document.getElementById("myModal");
let btn = document.querySelector("#addExerciseBtn");
let span = document.querySelector(".close");


//1. Generate a workout in "workout" table with flag is_active = true.
//2. Populate "workout_exercise" with the exercises added based on workout_id.
//3. Figure out how to join sets/reps into the query to view those too.
//4. Probably add a new column "workoutCreatedAt" in "workout" to figure out how long elapsed the workout has been on page reload for the timer.
function submitExercises() {
    
    console.log(WorkoutId);
    console.log(selectedExerciseIds);

    //Converts the GLOBAL string array (required to be used by .push and .splice in activeRows()) to an integer so it can be parsed correctly by the backend, which only accepts <int> data type.
    let ExerciseIdsIntArray = selectedExerciseIds.map(item => Number(item));
    console.log(ExerciseIdsIntArray);
    fetch('/Workout/InsertExercises', {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            ExerciseIds: ExerciseIdsIntArray, 
            WorkoutId: WorkoutId
        })
    })
        .then(response => response.json())
        .then(data => {
            console.log(data);
        })
        .catch(error => {
            console.log('Error occurred:', error);
        });
}


function loadActiveWorkoutExercises(activeWorkoutObj) {
    fetch('/Workout/ViewActiveWorkoutByUserId?UserId=' + UserId)
        .then(response => response.json())
        .then(data => {

            // Dictates the list of columns to be displayed on the page
            const columnNames = ["SetsCount", "WeightPerSet", "RepsPerSet", "SetCategoryArray"];
            let activeWorkoutObj = JSON.parse(data);
            //console.log(activeWorkoutObj);

            let activeWorkoutContainer = document.querySelector("#activeWorkoutContainer");
            let generateTable = document.createElement("table");
            generateTable.setAttribute("class", "activeWorkoutTable");
            activeWorkoutContainer.appendChild(generateTable);


            // Track the WorkoutExerciseIds we've already processed
            let processedWorkoutExerciseIds = new Set();

            // Iterate through each item in activeWorkoutObj
            activeWorkoutObj.forEach(workout => {
                //console.log(workout);

                // Only create a header row if we haven't processed this WorkoutExerciseId before
                if (!processedWorkoutExerciseIds.has(workout["WorkoutExerciseId"])) {
                    // Mark this WorkoutExerciseId as processed
                    processedWorkoutExerciseIds.add(workout["WorkoutExerciseId"]);

                    // Create a new "header" row for each unique WorkoutExerciseId
                    let newRowHead = generateTable.insertRow();
                    newRowHead.className = "headerRow";
                    newRowHead.setAttribute("headerworkoutexerciseid", workout["WorkoutExerciseId"]);
                    newRowHead.addEventListener("click", collapseExerciseSets);

                    let newCellHeadSetsCount = newRowHead.insertCell();
                    let newCellHeadExerciseName = newRowHead.insertCell();

                    let cellExerciseName = workout["ExerciseName"];
                    let cellNoOfSetsValue = workout["SetsCount"];

                    let textNodeExerciseName = document.createTextNode(cellExerciseName);
                    let textNodeSets = document.createTextNode(cellNoOfSetsValue);

                    newCellHeadSetsCount.appendChild(textNodeSets);
                    newCellHeadExerciseName.appendChild(textNodeExerciseName);
                }

                // Create a new row for each set of the workout
                let newRow = generateTable.insertRow();
                newRow.setAttribute("workoutexerciseid", workout["WorkoutExerciseId"]);
                newRow.setAttribute("data-setid", workout["SetId"])
                let weightPerSet = workout["Weight"];
                let repsPerSet = workout["Reps"];
                let setCategory = workout["SetCategory"];

                // Insert cells for each data point
                let weightCell = newRow.insertCell();
                let repsCell = newRow.insertCell();
                let categoryCell = newRow.insertCell();

                categoryCell.appendChild(document.createTextNode(setCategory));
                weightCell.appendChild(document.createTextNode(weightPerSet));
                weightCell.setAttribute("contenteditable", "true");

                repsCell.appendChild(document.createTextNode(repsPerSet));
                repsCell.setAttribute("contenteditable", "true");

                // Generate the button which marks a set as complete
                let setButton = document.createElement("button");
                setButton.setAttribute("class", "setButton");
                setButton.addEventListener("click", setButtonClicked);
                newRow.appendChild(setButton);

            });
        })
        // Loads from local storage the sets the user has marked as completed. This must be done here at the end of the promise chain, or else returns null elements in query selector.
        .then(loadCompletedSets => {
            loadLocalStorage();
        })
        .catch(error => {
            console.log('Error occurred:', error);
        });

}


// Stores a GLOBAL list of exercise IDs that the user has clicked on to be added to their workout
let selectedExerciseIds = [];
function activeRows(tableData) {
    const tdId = tableData.getAttribute("data-exercise-id");
    //const tdId = td.id;
    const tdIndex = selectedExerciseIds.indexOf(tdId);

    if (tdIndex === -1) {
        selectedExerciseIds.push(tdId);
        tableData.style.background = "aqua";
    }
    else {
        selectedExerciseIds.splice(tdIndex, 1);
        tableData.style.background = "white";
    }
    console.log('Clicked rows: ', selectedExerciseIds);
};

function addExerciseBtnClicked() {

    fetch('/Workout/ViewExerciseList')
        .then(response => response.json())
        .then(data => {
            // Causes the modal to pop-up upon SQL query returning succesfully
            modal.style.display = "block";

            // Tells JS to expect and parse as a Json obj.
            var jsonArr = JSON.parse(data);

            let modalContent = document.querySelector(".dynamic-content");

            let generateTable = document.createElement("table");
            generateTable.setAttribute("id", "DynamicExerciseTable");
            modalContent.appendChild(generateTable);

            // Iterates through each exercise and displays it within its own table row attribute
            for (let i = 0; i < jsonArr.length; i++) {
                //console.log(jsonArr[i]);

                let exerciseName = jsonArr[i]["ExerciseName"];
                let exerciseId = jsonArr[i]["ExerciseId"]

                let newRow = generateTable.insertRow();

                let newCell = newRow.insertCell();
                newCell.setAttribute("data-exercise-id", exerciseId);

                let addContent = document.createTextNode(exerciseName);
                newCell.appendChild(addContent);

                // This cannot be a lambda function, as it requires the use of "this" (i.e. referencing itself) to work.
                newCell.addEventListener("click", function () {
                    newCell.classList.toggle("highlightCell")
                    
                    console.log(this.getAttribute("data-exercise-id"));
                    activeRows(this);
                    //this.style.background = "aqua";

                });
            }

            // Dynamically generates the submit exercise button at the bottom of the modal
            let submitExerciseBtn = document.createElement("button");
            submitExerciseBtn.setAttribute("id", "submitExerciseBtn");

            // Populates text inside the button
            submitExerciseBtn.appendChild(document.createTextNode("Submit exercises"));
            submitExerciseBtn.onclick = submitExercises;

            // Adds the button to the page as the last child element of the modal pop-up
            modalContent.after(submitExerciseBtn);


        })
        .catch(error => {
            console.error('Error fetching data: ', error);
        });


};

// The close button for the modal 
span.onclick = function () {
    modal.style.display = "none";

    // Grabs the parent node inside of the modal-content
    let modalContent = document.querySelector(".dynamic-content");
    let submitExerciseBtn = document.querySelector("#submitExerciseBtn");

    while (modalContent.firstChild) {
        modalContent.removeChild(modalContent.firstChild);
        submitExerciseBtn.remove();
    }

};

function collapseExerciseSets(event) {
    // The parent element of the row clicked
    let parentElement = this; 
    let workoutExerciseId = this.getAttribute("headerworkoutexerciseid");

    let matchingRows = document.querySelectorAll(`[workoutexerciseid='${workoutExerciseId}']`);
    console.log(matchingRows);

    // Toggle the display of each matching row
    // There is a bug that needs fixinghere. It takes two clicks to shrink a row header because it first has to apply display = table-row. 
    matchingRows.forEach(row => {
        if (row.style.display === "none" || row.style.display === "") {
            // sets the rows display back to a regular table row
            row.style.display = "table-row"; 
        } else {
            row.style.display = "none";
        };
    });
};






document.getElementById('start-button').addEventListener('click', startTimer);
document.getElementById('add-time-button').addEventListener('click', addTimeToTimer);

function startTimer() {
    let duration = 60 * 2; // Set the duration in seconds (e.g., 5 minutes)
    const countdownDisplay = document.getElementById('countdown-timer');
    const addTimeButton = document.querySelector("#add-time-button");



    let timerInterval = setInterval(() => {
        let minutes = Math.floor(duration / 60);
        let seconds = duration % 60;

        // Format minutes and seconds to be always two digits
        minutes = minutes < 10 ? '0' + minutes : minutes;
        seconds = seconds < 10 ? '0' + seconds : seconds;

        countdownDisplay.textContent = `${minutes}:${seconds}`;

        if (duration <= 0) {
            clearInterval(timerInterval);
            countdownDisplay.textContent = "Weight go brr!";
        }

        duration--;
    }, 1000);
};


function addTimeToTimer() {

}

let setCompleteArray = [];
function setButtonClicked(e) {
    /*console.log(localStorage.getItem("setComplete"));*/
    let parentElem = this.parentElement;
    let setId = parentElem.getAttribute("data-setid");

    console.log(setId);

    startTimer();

    //// Checks if the set id tracking array is empty. If empty, skips writing the local storage data to it. Otherwise persistency between refreshes would not work.
    //if (setCompleteArray.length > 0) {
    //    console.log("if ran");
    //    let retrieveStorage = localStorage.getItem("setComplete");
    //    let readJson = JSON.parse(retrieveStorage);
    //    //console.log(readJson);
    //    setCompleteArray = readJson;

    //    setCompleteArray.forEach(dataSetId => {
    //        //getRow = document.querySelector("#" + id);
    //        let getRow = document.querySelector(`[setid='${dataSetId}']`);
    //        getRow.classList.toggle("setComplete");
    //    })
    //};
    

    if (parentElem.classList.contains("setComplete")) {
        parentElem.classList.remove("setComplete");
        // Remove the set ID from the global array based on the items index
        setCompleteArray.splice(setCompleteArray.indexOf(setId), 1);

        let stringifieSetIdsArray = JSON.stringify(setCompleteArray);
        localStorage.setItem("setComplete", stringifieSetIdsArray);
    } else {
        parentElem.classList.toggle("setComplete");
        setCompleteArray.push(setId);

        let stringifieSetIdsArray = JSON.stringify(setCompleteArray);
        localStorage.setItem("setComplete", stringifieSetIdsArray);
    }

    function stringifyForLocalStorage() {

    }
    console.log(setCompleteArray)
    
}

function loadLocalStorage() {
    // I need to do an if an item exists in setComplete
    // Then parse it as json
    // Then assign the values to the array
    // Then for loop through each ID and apply the correct css class


    // Checks if the set id tracking array is empty. If empty, skips writing the local storage data to it. Otherwise persistency between refreshes would not work.
    console.log("Page is fully loaded");

    let retrieveStorage = localStorage.getItem("setComplete");
    let readJson = JSON.parse(retrieveStorage);
    console.log(readJson);
    let convertJsonToInt = readJson.map(item => Number(item));
    console.log(convertJsonToInt);
    setCompleteArray = convertJsonToInt;
    console.log('Final array: ' + setCompleteArray);


    setCompleteArray.forEach(dataSetId => {
        console.log(dataSetId);
        console.log(`Searching for setid='${dataSetId}'`);
        //getRow = document.querySelector("#" + id);
        let getRow = document.querySelector(`[data-setid='${dataSetId}']`);
        console.log(getRow);
        getRow.classList.toggle("setComplete");
    });
}