let minute = 00;
let second = 00;
let count = 00;

document.addEventListener("DOMContentLoaded", function () {
    timer = true;
    stopWatch();
});

//stopBtn.addEventListener('click', function () {
//    timer = false;
//});


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
    console.log('Onclick worked');


}



// Stores a list of exercise IDs that the user has clicked on to be added to their workout
let selectedTd = [];
function activeRows(tableData) {
    const tdId = tableData.getAttribute("data-exercise-id");
    //const tdId = td.id;
    const tdIndex = selectedTd.indexOf(tdId);

    if (tdIndex === -1) {
        selectedTd.push(tdId);
        tableData.style.background = "aqua";
    }
    else {
        selectedTd.splice(tdIndex, 1);
        tableData.style.background = "white";
    }
    console.log('Clicked rows: ', selectedTd);
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