function ResetUserPassword() {
    var password = document.getElementById('newPassword').value;
    var confirmPassword = document.getElementById('confirmPassword').value;

    $('.error-label').text('');

    if (!validateInputs(password, confirmPassword)) {
        return;
    }

    $.ajax({
        type: "POST",
        url: `/ResetPassword?handler=UpdatePasswordDetails`,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
            "Content-Type": "application/x-www-form-urlencoded"
        },
        data: {
            newPassword: password,
        },
        success: function (response) {
            if (response.success === "true") {
                window.location.href = "/Login";
            } else {
                $('#errorMessage').text(`${response.error}`);
            }
        },
        error: function (error) {
            $('#errorMessage').text(`${error}`);
        }
    });
}

function validateInputs(password, confirmPassword) {
    var isValid = true;

    if (!password || password < 6) {
        $('#newPasswordError').text('Password must be at least 6 characters long.');
        isValid = false;
    }
    else {
        if (password != confirmPassword) {
            $('#confirmPasswordError').text('Both passwords must match.');
            isValid = false;
        }
    }

    return isValid;
}