var UpdatedRow ;
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
	if (UpdatedRow === undefined) {
		$('tbody').append(item);
	}
	else {
		$(UpdatedRow).replaceWith(item);
		UpdatedRow = undefined;
	}
	KTMenu.init();
	KTMenu.initHandlers();


}

$(document).ready(function () {
	var massage = $('#Message').text();
	if (massage != '') {
		showSuccessMessage(massage);
	}

	// handle form modal
	$('body').delegate('.js-render-modal','click', function () {
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