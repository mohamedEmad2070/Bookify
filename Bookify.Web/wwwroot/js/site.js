var table;
var datatable;
var UpdatedRow;
var exportedCols = [];
function showSuccessMessage(message = 'Saved successfully!') {
    Swal.fire({
        icon: 'success',
        title: 'Success',
        text: message,
        customClass: {
            confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary"
        }
    });
}

function showErrorMessage(message = 'Something went wrong!') {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: message,
        customClass: {
            confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary"
        }
    });
}
function onModalSuccess(item) {
    showSuccessMessage();
    $('#Modal').modal('hide');
    const $item = $(item); // Convert item HTML string to jQuery object
    if (UpdatedRow === undefined) {
        // Add new row using DataTables API
        datatable.row.add($item).draw();
    } else {
        // Update existing row using DataTables API
        datatable.row(UpdatedRow).data($item).draw();
        UpdatedRow = undefined;
    }
    KTMenu.init();
    KTMenu.initHandlers();
}
//datatabl
var headers = $('th');
$.each(headers, function (i) {
    if (!$(this).hasClass('js-no-export')) {
        exportedCols.push(i);

    }
}
// Class definition
var KTDatatables = function () {

    // Private functions
    var initDatatable = function () {


        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'order': [],
            'pageLength': 10,
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $('.js-datatables').data('document-titel');
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();
$(document).ready(function () {
    //Datatable
    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });
    //sweetalert
    var massage = $('#Message').text();
    if (massage != '') {
        showSuccessMessage(massage);
    }

    // handle form modal
    $('body').delegate('.js-render-modal', 'click', function () {
        var btn = $(this);
        var modal = $('#Modal');
        modal.find('#ModalLabel').text(btn.data('title'));
        if (btn.data('update') !== undefined) {
            UpdatedRow = btn.parents('tr');
            console.log(UpdatedRow);
        }
        $.get({
            url: btn.data('url'),

            success: function (form) {

                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
            },
            error: function () {
                showErrorMessage();
            },


        });

        modal.modal('show');



    });

});