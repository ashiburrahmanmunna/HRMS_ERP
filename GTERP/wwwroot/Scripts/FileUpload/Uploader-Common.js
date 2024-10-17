function progressHandlingFunction(e) {
    if (e.lengthComputable) {
        var percentComplete = Math.round(e.loaded * 100 / e.total);
        $("#FileProgress").css("width", percentComplete + '%').attr('aria-valuenow', percentComplete);
        $('#FileProgress span').text(percentComplete + "%");
    }
    else {
        $('#FileProgress span').text('unable to compute');
    }
}

function completeHandler() {
    $('#createView').empty();
    $('.CreateLink').show();
    $.unblockUI();
}


function successHandler(data) {
    if (data.statusCode == 200) {
        $('#FilesList tr:last').after(data.NewRow);
        alert(data.status);
        window.location.reload();///fahad need to code off.

    }
    else {
        alert(data.status);

    }
}

function errorHandler(xhr, ajaxOptions, thrownError) {
    alert("There was an error attempting to upload the file. (" + thrownError + ")");
}

function OnDeleteAttachmentMainSuccess(data) {
   
    if (data != "") {
        $('.AttachmentMain_' + data).fadeOut('slow');
    }
    else {
        alert("Unable to Delete");
        console.log(data.message);
    }
}

function OnDeleteAttachmentSuccess(data) {


    if (data.ID && data.ID != "") {
        $('#AttachmentSub_' + data.ID).fadeOut('slow');
    }
    else {
        alert("Unable to Delete");
        console.log(data.message);
    }
}
function Cancel_btn_handler() {
    $('#createView').empty();
    $('.CreateLink').show();
    $.unblockUI();
}