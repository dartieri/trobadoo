/*
* jQuery File Upload Plugin JS Example 8.9.1
* https://github.com/blueimp/jQuery-File-Upload
*
* Copyright 2010, Sebastian Tschan
* https://blueimp.net
*
* Licensed under the MIT license:
* http://www.opensource.org/licenses/MIT
*/

/* global $, window */

$(function () {
    'use strict';

    // Initialize the jQuery File Upload widget:
//    $('#formValuate').fileupload({
//        // Uncomment the following to send cross-domain cookies:
//        //xhrFields: {withCredentials: true},
//        url: '/AjaxFileHandler'
//    });
    $('#fileupload').fileupload({
        dataType: 'json',
        url: '/AjaxFileHandler',
        maxFileSize: 1000000,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
        fileTypes: /^image\/(gif|jpeg|png)$/,
        done: function (e, data) {
            $.each(data.result, function (index, file) {
                if (file.size == 'Error') {
                    jAlert(file.name);
                } else {
                    $('#formEnvioMail #idFoto').val(file.idFoto);
                    $('#listaFotos').append(getHtmlFoto(file));
                }
            });
            $('#progressBar').hide();
        }
    });
    $('#fileupload').on('fileuploadprogress', function (e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        if (progress != 100) {
            $('#progressBar').show();
            $('#progressBar').html(progress + '%');
        }
    });
});
