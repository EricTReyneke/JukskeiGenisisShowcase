function LoginUser() {
    var userNameOrEmail = document.getElementById('userEmailUser').value;
    var password = document.getElementById('userPassword').value;

    $('.error-label').text('');

    if (!validateInputs(userNameOrEmail, password)) {
        return;
    }

    $.ajax({
        type: "POST",
        url: `/Login?handler=ValidateUserInfo`,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
            "Content-Type": "application/x-www-form-urlencoded"
        },
        data: {
            userNameOrEmail: userNameOrEmail,
            password: password,
        },
        success: function (response) {
            if (response.success === "true") {
                window.location.href = "/ViewTournaments";
            } else {
                $('#errorMessage').text(`${response.error}`);
            }
        },
        error: function (error) {
            $('#errorMessage').text(`${error}`);
        }
    });
}

function validateInputs(userNameOrEmail, password) {
    var isValid = true;

    if (!userNameOrEmail || userNameOrEmail.length < 2) {
        $('#userEmailUserError').text('Username must be at least 2 characters long or enter a valid email');
        isValid = false;
    }
    if (!password || password.length < 6) {
        $('#userPasswordError').text('Password must be at least 6 characters long.');
        isValid = false;
    }
    return isValid;
}

function HideErrorMessage() {
    document.getElementById('error-message').style.display = 'none';
}