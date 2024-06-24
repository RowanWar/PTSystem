
//let prev = null;

//document.body.addEventListener(
//    "mouseover",
//    (event) => {
//        if (event.target === document.body || (prev && prev === event.target)) {
//            return;
//        }
//        if (prev) {
//            prev.classList.remove("highlight");
//            prev = null;
//        }
//        if (event.target) {
//            prev = event.target;
//            prev.classList.add("highlight");
//            console.log('Worked')
//        }
//    },
//    false
//);

//.highlight {
//    background - color: yellow;
//}


//window.addEventListener('mouseover', function (e) {
//    /*updateMask(e.target);*/
//    e.target(highlightDom);
//});




let parentDiv = document.querySelector('.profileContainerDiv');
let grabEditableDivs = document.querySelectorAll('.editable')
function buttonClicked() {
    console.log('This worked');
    console.log(grabEditableDivs);

    grabEditableDivs.forEach((element) => console.log(element));
    //parentDiv.editable.add(".Editing")

}
