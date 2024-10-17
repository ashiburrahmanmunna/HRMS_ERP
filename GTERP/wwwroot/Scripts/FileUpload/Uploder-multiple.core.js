var selectedFiles;
var DataURLFileReader = {
    read: function (file, callback) {
        var reader = new FileReader();
        var fileInfo = {
            name: file.name,
            type: file.type,
            fileContent: null,
            size: function () {
                var FileSize = 0;
                if (file.size > 1048576) {
                    FileSize = Math.round(file.size * 100 / 1048576) / 100 + " MB";
                }
                else if (file.size > 1024) {
                    FileSize = Math.round(file.size * 100 / 1024) / 100 + " KB";
                }
                else {
                    FileSize = file.size + " bytes";
                }
                return FileSize;
            }
        };
        if (!file.type.match('image.*')) {
            callback("file type not allowed", fileInfo);
            return;
        }
        reader.onload = function () {
            fileInfo.fileContent = reader.result;
            callback(null, fileInfo);
        };
        reader.onerror = function () {
            callback(reader.error, fileInfo);
        };
        reader.readAsDataURL(file);
    }
};

function multipleFiles_addMode() {
    $('.CreateLink').hide();
    Init_Multiple_Upload();
}


function Init_Multiple_Upload() {
    $("#UploadedFiles").change(function (evt) {
        MultiplefileSelected(evt);
    });
    $("#FormMultipleUpload button[id=Cancel_btn]").click(function () {
        Cancel_btn_handler()
    });
    $('#FormMultipleUpload button[id=Submit_btn]').click(function () {
        UploadMultipleFiles();
    });
    var dropZone = document.getElementById('drop_zone');
    dropZone.addEventListener('dragover', handleDragOver, false);
    dropZone.addEventListener('drop', MultiplefileSelected, false);
    dropZone.addEventListener('dragenter', dragenterHandler, false);
    dropZone.addEventListener('dragleave', dragleaveHandler, false);
    $.blockUI.defaults.overlayCSS = {
        backgroundColor: '#000',
        opacity: 0.6
    };
    $.blockUI.defaults.css = {
        padding: 10,
        margin: 5,
        width: '90%',
        //height: '80%',
        top: '10%',
        left: '5%',
        color: '#000',
        border: '3px solid #aaa',
        backgroundColor: '#fff',


    };
    $.blockUI({ message: $('#createView') });
}

function MultiplefileSelected(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    $('#drop_zone').removeClass('hover');
    selectedFiles = evt.target.files || evt.dataTransfer.files;
    if (selectedFiles) {
        $('#Files').empty();
        for (var i = 0; i < selectedFiles.length; i++) {
            DataURLFileReader.read(selectedFiles[i], function (err, fileInfo) {
                if (err != null) {
                    var RowInfo = '<div id="File_' + i + '" class="col"><div class="InfoContainer">' +
                                   '<div class="Error">' + err + '</div>' +
                                  '<div data-name="FileName" class="">' + fileInfo.name + '</div>' +
                                  '<div data-type="FileType" class="">' + fileInfo.type + '</div>' +
                                  '<div data-size="FileSize" class="">' + fileInfo.size() + '</div></div><hr/></div>';
                    $('#Files').append(RowInfo);
                }
                else {
                    var image = '<img src="' + fileInfo.fileContent + '" class="thumb" title="' + fileInfo.name + '" />';
                    var RowInfo = '<div id="File_' + i + '" class="col"><div class="InfoContainer">' +
                                  '<div data_img="Imagecontainer">' + image + '</div>' +
                                  '<div data-name="FileName" class="">' + fileInfo.name + '</div>' +
                                  '<div data-type="FileType" class="">' + fileInfo.type + '</div>' +
                                  '<div data-size="FileSize" class="">' + fileInfo.size() + '</div></div><hr/></div>';
                    $('#Files').append(RowInfo);
                }
            });
        }
    }
}

function UploadMultipleFiles() {
    var productid = $('#product').val();
    //alert(productid);
    //var attachment_sub = { "AttachmentMainId": "", "AttachmentSubId": "", "FileName": "", "FileType": "", "FileContent": "" };
    //var attachment_main = { "AttachmentMainID": "", "BasedOn": "", "BasedId": "", "AttachmentDate": "", "AttachmentDesc": "", "AttachmentSubs": [] };

    var AttachmentMain = { "AttachmentMainID": "", "BasedOn": "", "BasedId": "", "AttachmentDate": "", "AttachmentDesc": ""};


    AttachmentMain.AttachmentMainID = 0;
    AttachmentMain.BasedOn = "Product";
    AttachmentMain.BasedId = productid;
    //attachment_main.AttachmentDate = new Date();
    AttachmentMain.AttachmentDesc = "Product desc";


    console.log(AttachmentMain);

    // here we will create FormData manually to prevent sending mon image files 
    var dataString = new FormData();
    //var files = document.getElementById("UploadedFiles").files;
    for (var i = 0; i < selectedFiles.length; i++) {
        if (!selectedFiles[i].type.match('image.*')) {
            continue;
        }
        dataString.append("uploadedFiles", selectedFiles[i]);
    }

    //dataString.append('basedid', productid.toString());
    //dataString.append("AttachmentMain", attachment_main);
    //console.log(dataString);


    //console.log(dataString);
    //window.console && console.log('hey');

    $.ajax({
        url: '/Upload/UploadMultiple',  //Server script to process data
        type: 'POST',
        xhr: function () {  // Custom XMLHttpRequest
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // Check if upload property exists
                myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // For handling the progress of the upload
            }
            return myXhr;
        },
        //Ajax events

        success: successHandler,
        error: errorHandler,
        complete: completeHandler,
        // Form data
        data: (AttachmentMain,dataString) ,
        //data: { 'productid' : productid, 'uploadedFiles' :dataString },
        //Options to tell jQuery not to process data or worry about content-type.
        cache: false,
        contentType: false,
        processData: false
    });

    

}


// Drag and Drop Events
function handleDragOver(evt) {
    evt.preventDefault();
    evt.dataTransfer.effectAllowed = 'copy';
    evt.dataTransfer.dropEffect = 'copy';
}

function dragenterHandler() {
    //$('#drop_zone').removeClass('drop_zone');
    $('#drop_zone').addClass('hover');
}

function dragleaveHandler() {
    $('#drop_zone').removeClass('hover');
}