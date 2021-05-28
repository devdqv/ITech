function openModal(title, data) {
    $(".modal .modal-title").text(title);
    $(".modal .modal-body").html(data);
    $(".modal").modal('show');
}

function CloseModal() {
    $(".modal").modal('hide');
}



$('.datepicker').on('focusin', function () {

    $(this).datepicker({
        format: 'dd/mm/yyyy',
        language: 'vi',
        autoclose: true,
        todayHighlight: true,
        orientation: 'bottom'
    });
})

// Wait for the DOM to be ready
$(function () {
    // Initialize form validation on the registration form.
    // It has the name attribute "registration"
    jQuery.extend(jQuery.validator.messages, {
        required: "Không được bỏ trống trường này",
        remote: "Please fix this field.",
        email: "Please enter a valid email address.",
        url: "Please enter a valid URL.",
        date: "Please enter a valid date.",
        dateISO: "Please enter a valid date (ISO).",
        number: "Please enter a valid number.",
        digits: "Please enter only digits.",
        creditcard: "Please enter a valid credit card number.",
        equalTo: "Please enter the same value again.",
        accept: "Please enter a value with a valid extension.",
        maxlength: jQuery.validator.format("Please enter no more than {0} characters."),
        minlength: jQuery.validator.format("Please enter at least {0} characters."),
        rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
        range: jQuery.validator.format("Please enter a value between {0} and {1}."),
        max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
        min: jQuery.validator.format("Please enter a value greater than or equal to {0}.")
    });
   

});



function escapeHTML(str) {
    var div = document.createElement('div');
    var text = document.createTextNode(str);
    div.appendChild(text);
    return div.innerHTML;
}
function firstPageClick() {
    var rowPerPage = $("#sltRowPerPage").val();
    submitSearch(1, rowPerPage);
}
function lastPageClick() {
    var lastPage = parseInt($("#lblLastPage").val());
    var rowPerPage = $("#sltRowPerPage").val();

    submitSearch(lastPage, rowPerPage);
}
function nextPageClick() {
    var currentPage = parseInt($("#txtCurrentPage").val());
    var page = currentPage + 1;
    var rowPerPage = $("#sltRowPerPage").val();
    submitSearch(page, rowPerPage);
}
function prevPageClick() {
    var currentPage = parseInt($("#txtCurrentPage").val());
    var page = currentPage - 1;
    var rowPerPage = $("#sltRowPerPage").val();
    submitSearch(page, rowPerPage);
}
function onRowPerPageChange() {
    var currentPage = 1;
    var rowPerPage = $("#sltRowPerPage").val();
    submitSearch(currentPage, rowPerPage);
}

function matchCustom(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') {
        return data;
    }

    // Do not display the item if there is no 'text' property
    if (typeof data.text === 'undefined') {
        return null;
    }

    // `params.term` should be the term that is used for searching
    // `data.text` is the text that is displayed for the data object
    if (data.text.toLowerCase().indexOf(params.term.toLowerCase()) > -1) {
        var modifiedData = $.extend({}, data, true);
        modifiedData.text += '';

        // You can return modified objects from here
        // This includes matching the `children` how you want in nested data sets
        return modifiedData;
    }

    // Return `null` if the term should not be displayed
    return null;
}



var loadImage = function (event) {
    var output = document.getElementById('output');
    if ((event.target.files[0].size / 1024 / 1024) > 2) {
        createMessageBoxWarning("", "Ảnh không được vượt quá 2 MB");
        event.target.value = '';
        return false;
    }
    output.src = URL.createObjectURL(event.target.files[0]);
    output.onload = function () {
        URL.revokeObjectURL(output.src) // free memory
    }
}
var loadImage2 = function (event) {
    var output = event.target.parentElement.children[0].children[1];//document.getElementById('output');
    if ((event.target.files[0].size / 1024 / 1024) > 2) {
        createMessageBoxWarning("", "Ảnh không được vượt quá 2 MB");
        event.target.value = '';
        return false;
    }
    output.src = URL.createObjectURL(event.target.files[0]);

    var del = event.target.parentElement.children[0].children[0];
    del.removeAttribute("hidden");
    var del = event.target.parentElement.children[2];
    del.removeAttribute("value");

    output.onload = function () {
        URL.revokeObjectURL(output.src) // free memory
    }
}

var removeImg = function (event) {
    var output = event.target.parentElement.children[1];//document.getElementById('output');

    output.src = "/images/upload.png";//URL.createObjectURL(event.target.files[0]);

    output.onload = function () {
        URL.revokeObjectURL(output.src) // free memory
    }
    var hidden = event.target.parentElement.parentElement.children[2];
    hidden.removeAttribute("value");
    var file = event.target.parentElement.parentElement.children[1];
    file.type = "text";
    file.type = "file";
    var del = event.target;
    del.setAttribute("hidden", "");
    return false;
}
var removeImg2 = function (event) {
    var output = event.target.parentElement.children[1];//document.getElementById('output');
   
    output.src = "/images/noAvatar.png";//URL.createObjectURL(event.target.files[0]);

    output.onload = function () {
        URL.revokeObjectURL(output.src) // free memory
    }
    var hidden = event.target.parentElement.parentElement.children[2];
    hidden.removeAttribute("value");
    var file = event.target.parentElement.parentElement.children[1];
    file.type="text";
    file.type = "file";
    var del = event.target;
    del.setAttribute("hidden", "");
    return false;
}

function createCloseMessageBox(titlle, message, Error = true, href = "#") {
    if (Error == false) {
        Swal.fire({
            title: titlle,
            text: message,
            icon: "success", 
            timer: 2500,
            showConfirmButton: false 

        }).then(function () {
            location.href = location.href;

        });
    }
    else {
        Swal.fire({
            title: titlle,
            text: message,
            icon: "error"
        });
    }
}


function createMessageBoxUrl(titlle, message, Error = true, href = "#") {
    if (Error == false) {
        Swal.fire({
            title: titlle,
            text: message,
            icon: "success",
            timer: 2500,
            showConfirmButton: false,

        }).then(function () {
            window.location.href = href;

        });
    }
    else {
        Swal.fire({
            title: titlle,
            text: message,
            icon: "error"
        });
    }
}
function createMessageBox(titlle, message, Error = true) {
    if (Error == false) {
        Swal.fire({
            title: titlle,
            text: message,
            icon: "success",
            timer: 2500,
            showConfirmButton: false

        });
    }
    else {
        Swal.fire({
            title: titlle,
            text: message,
            icon: "error",
            timer: 2500

        });
    }


}


function createMessageBoxConfirm(titlle, message, callback) {
    Swal.fire({
        title: titlle,
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Xác Nhận!',
        cancelButtonText: 'Hủy!',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33'
    }).then((confirmed) => {
        callback(confirmed && confirmed.value == true);
    });
}
function createMessageBoxConfirm2(titlle, message, callback) {
    Swal.fire({
        title: titlle,
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Tiếp tục',
        cancelButtonText: 'Hủy!',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33'
    }).then((confirmed) => {
        callback(confirmed && confirmed.value == true);
    });
}

function createMessageBoxWarning(titlle, message) {
    Swal.fire({
        title: titlle,
        text: message,
        icon: "warning",
        showConfirmButton: false,
        timer: 2500
    });

}

$(document).on('focusin', 'input', function () {
    $(this).attr('autocomplete', 'off');
})
$(document).on('focusin', 'textarea', function () {
    $(this).attr('autocomplete', 'off');
})

var AppendArrFormData = function (arr, formdata) {
    $.map(arr, function (n, i) {
        formdata.append(n['name'], n['value']);
    });
}
function validate() {
    var counter = 0;
    $(".required").each(function () {
        if ($(this).val().trim() === "") {
            counter++;
            $(this).parents("div").children('.message').addClass('text-danger').text("Bạn không được bỏ trống trường này");
        }
    });
    if (counter > 0) {

        return false;
    }
    return true;
}

function IDasName() {
    $('input,textarea,select,hidden[id]').each(function () {
        $(this).attr('name', $(this).attr('id'));
    })
}

function SomeeAdsRemover() {
    $("center").each(function () {
        if ($(this).html() == '<a href="http://somee.com">Web hosting by Somee.com</a>') {
            $(this).next().remove();
            $(this).next().next().remove();
            $(this).next().next().next().remove();
            $(this).remove();

            return false;
        }
    });
}
$(document).ready(function () {
    $('input,textarea,select,hidden[id]').each(function () {
        $(this).attr('name', $(this).attr('id'));
    })
    SomeeAdsRemover();
    localStorage.setItem('token', "4427424b-9ccc-11eb-9035-ae038bcc764b"); //token giaon hang nhanh 0348840032

    $('.select2').select2({
        matcher: matchCustom,
        width: '100%'
    })
    $('.select2M').select2({
        matcher: matchCustom,
        width: '100%',
        dropdownParent: $("#myModal")
    })
    $('.select2-selection__rendered').attr('style', ' background: none;padding:0px;')


})
