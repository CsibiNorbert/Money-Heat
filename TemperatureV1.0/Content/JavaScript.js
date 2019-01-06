
//Logged
function update() {
    $("#badge2").load(" #badge2");
    
    

};
function updateMain() {
    $(".WholePage").load(" .WholePage");

};

setInterval(updateMain, 2000);
setInterval(update, 1000);

//tabledanger
function x() {
    $("#alert").fadeOut("slow").delay(15500).hide(0);
};

function updateTable() {
    $("#table").load(" #table");
};

setInterval(x, 10000);

$(document).on('click', '.panel-heading span.clickable', function (e) {
    var $this = $(this);
    if (!$this.hasClass('panel-collapsed')) {
        $this.parents('.panel').find('.panel-body').slideUp();
        $this.addClass('panel-collapsed');
        $this.find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');

    } else {
        $this.parents('.panel').find('.panel-body').slideDown();
        $this.removeClass('panel-collapsed');
        $this.find('i').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');

    }
});

function closeModal() {
    $('.modal').modal('toggle');
};