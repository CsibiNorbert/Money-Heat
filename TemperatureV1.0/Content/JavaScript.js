
//Logged
function update() {
    $("#badge2").load(" #badge2");

};

function updateMain() {
    $(".WholePage").load(" .WholePage");

};

setInterval(updateMain, 100);
setInterval(update, 1000);

//tabledanger
function x() {
    $("#alert").fadeOut("slow").delay(15500).hide(0);
};

function updateTable() {
    $("#table").load(" #table");
};

setInterval(x, 10000);