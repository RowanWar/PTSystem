let minute = 00;
let second = 00;
let count = 00;

// Static for now, this should be fetched later upon user login.
const UserId = 1;
let WorkoutId = null;

document.addEventListener("DOMContentLoaded", function () {
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
            console.log(activeWorkoutObj);

            console.log("Workout ID: " + WorkoutId);

            // If the first API lookup does not return a valid WorkoutId, the second API lookup which pulls the users active workout does not initiate.
            if (WorkoutId != null) {
                loadActiveWorkoutExercises(activeWorkoutObj);
            }
        })
        .catch(error => {
            console.log('Error occurred:', error);
        });
       
});

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


            let activeWorkoutContainer = document.querySelector("#activeWorkoutContainer");
            let generateTable = document.createElement("table");
            generateTable.setAttribute("class", "activeWorkoutTable");
            activeWorkoutContainer.appendChild(generateTable);


            // Iterate through each item in activeWorkoutObj
            activeWorkoutObj.forEach(workout => {
                // Create a new "header" row for each workout entry which displays the number of sets and name of exercise. This is used for collapsing/expanding an exercise and its related sets
                let newRowHead = generateTable.insertRow();
                newRowHead.className = "headerRow";
                newRowHead.setAttribute("headerworkoutexerciseid", workout["WorkoutExerciseId"]);
                newRowHead.addEventListener("click", collapseExerciseSets);

                //newRowHead.setAttribute("id", "headerRow");


                let newCellHeadSetsCount = newRowHead.insertCell();
                let newCellHeadExerciseName = newRowHead.insertCell();            

                let cellExerciseName = workout["ExerciseName"];
                let cellNoOfSetsValue = workout["SetsCount"];

                let textNodeExerciseName = document.createTextNode(cellExerciseName);
                let textNodeSets = document.createTextNode(cellNoOfSetsValue);
                
                newCellHeadSetsCount.appendChild(textNodeSets);
                newCellHeadExerciseName.appendChild(textNodeExerciseName);                
                
                // Iterate through the workout object exercises based on the amount of sets in the exercise
                for (let i = 0; i < workout.SetsCount; i++) {
                    console.log(workout["WeightPerSet"][i]);

                    let newRow = generateTable.insertRow();
                    // Appends the ID of the exercise (essentially a unique ID referencing this ID + set_id) so a workout can have two of same exercises with unique sets.
                    newRow.setAttribute("workoutexerciseid", workout["WorkoutExerciseId"]);

                    let weightPerSet = workout["WeightPerSet"][i];
                    let repsPerSet = workout["RepsPerSet"][i];
                    let setCategory = workout["SetCategoryArray"][i];


                    // Insert cells for each data point
                    let weightCell = newRow.insertCell();
                    let repsCell = newRow.insertCell();
                    let categoryCell = newRow.insertCell();

                    categoryCell.appendChild(document.createTextNode(setCategory));
                    weightCell.appendChild(document.createTextNode(weightPerSet));
                    repsCell.appendChild(document.createTextNode(repsPerSet));
                    repsCell.className = "RepsClass";

                }

            });
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

function collapseExerciseSets() {
    console.log("Clicked")
}