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
function registerNewWorkout() {
    localStorage.setItem("workout", "Chest");
};

const activeWorkout = localStorage.getItem("workout");
console.log(activeWorkout);






let modal = document.getElementById("myModal");
let btn = document.querySelector("#addExerciseBtn");
let span = document.querySelector(".close");

console.log('Before');
function addExerciseBtnClicked() {
    console.log('Fired');

    fetch('/Workout/ViewExerciseList')
        .then(response => response.json())
        .then(data => {
            console.log('Modal activated');

            // Causes the modal to pop-up upon SQL query returning succesfully
            modal.style.display = "block";

            // Tells JS to expect and parse as a Json obj.
            var jsonArr = JSON.parse(data);


            // Iterates through each returned image to unpack its unique ID and display it in the modal.
            for (let i = 0; i < jsonArr.length; i++) {
                console.log(i);
                //console.log(jsonArr[i]["ImageFilePath"]);

                //let fileId = jsonArr[i]["ImageFilePath"];

                //let generateImage = document.createElement("img");
                //// first slash in "/images/" dictates its an absolute path and not relative
                //generateImage.src = "/images/" + fileId;

                //document.querySelector('.images-div').appendChild(generateImage);
            }

        })
        .catch(error => {
            console.error('Error fetching data: ', error);
        });
};

addExerciseBtnClicked();