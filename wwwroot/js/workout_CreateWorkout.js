let minute = 00;
let second = 00;
let count = 00;

//Static UserId for now until proper config of logins 
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
            loadActiveWorkoutExercises(activeWorkoutObj);
            // If the user has an active workout, updates WorkoutId to this value i.e. WorkoutId = 200.
            //try {
            //    WorkoutId = obj[0].WorkoutId;
            //    console.log(WorkoutId);
            //} catch (error) {
            //    console.error(error);
            //}
            //if (WorkoutId != null) {
            //    fetch()
            //}
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

//registerNewWorkout();
//function registerNewWorkout() {
//    localStorage.setItem("workout", "Chest");
//};

//const activeWorkout = localStorage.getItem("workout");
//console.log(activeWorkout);






let modal = document.getElementById("myModal");
let btn = document.querySelector("#addExerciseBtn");
let span = document.querySelector(".close");


//1. Generate a workout in "workout" table with flag is_active = true.
//2. Populate "workout_exercise" with the exercises added based on workout_id.
//3. Figure out how to join sets/reps into the query to view those too.
//4. Probably add a new column "workoutCreatedAt" in "workout" to figure out how long elapsed the workout has been on page reload for the timer.
function submitExercises() {
    const UserId = 1; // Assuming you might use UserId later
    //let WorkoutId = 4;
    console.log('Onclick worked');
    //console.log(WorkoutId)
    //Converts the string array (required to be used by .push and .splice in activeRows()) to an integer so it can be parsed correctly by the backend, which only accepts <int> data type.
    let ExerciseIdsIntArray = selectedExerciseIds.map(item => Number(item));

    fetch('/Workout/InsertExercises', {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            ExerciseIds: ExerciseIdsIntArray, // This is now a stringified JSON array
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
    let modalContent = document.querySelector("#activeWorkoutContainer");

    let generateTable = document.createElement("table");
    generateTable.setAttribute("id", "activeWorkoutTable");
    modalContent.appendChild(generateTable);
    const columnNames = ["ExerciseName", "MuscleGroup"];

    // Pre-cache the length of the array prior to for loop to improve performance
    const arrayLength = activeWorkoutObj.length;
    // Iterates through each exercise and displays it within its own table row attribute
    for (let i = 0; i < arrayLength; i++) {
        console.log(activeWorkoutObj[i]);

        activeWorkoutObj.forEach()
        // Keep all the variables in one section for maintainability
        let exerciseName = activeWorkoutObj[i]["ExerciseName"];
        let muscleGroup = activeWorkoutObj[i]["MuscleGroup"];
        let sets = activeWorkoutObj[i]["Sets"];
        let reps = activeWorkoutObj[i]["Reps"];


        
        
        let newRow = generateTable.insertRow();
        
        let newCell = newRow.insertCell();
        let newCell2 = newRow.insertCell();
        //newCell.setAttribute("data-exercise-id", exerciseId);

        let addExerciseName = document.createTextNode(exerciseName);
        let addMuscleGroup = document.createTextNode(muscleGroup);
        let addSets = document.createTextNode(sets);
        let addReps = document.createTextNode(reps);


        newCell.appendChild(addExerciseName);
        newCell2.appendChild(addMuscleGroup);
        newCell.appendChild(addSets);
        newCell.appendChild(addReps);

    }
}


// Stores a list of exercise IDs that the user has clicked on to be added to their workout
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

                // This cannot be a lambda func as "this" context does not work.
                newCell.addEventListener("click", function () {
                    newCell.classList.toggle("highlightCell")
                    
                    console.log(this.getAttribute("data-exercise-id"));
                    activeRows(this);
                    //this.style.background = "aqua";

                });
            }

            let submitExerciseBtn = document.createElement("button");
            submitExerciseBtn.setAttribute("id", "submitExerciseBtn");
            // Populates text inside the button
            submitExerciseBtn.appendChild(document.createTextNode("Submit exercises"));
            submitExerciseBtn.onclick = submitExercises;
            //submitExerciseBtn.addEventListener("click", submitExercises);
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

}