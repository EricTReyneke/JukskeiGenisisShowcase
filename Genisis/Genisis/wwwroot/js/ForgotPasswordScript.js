function SendEmailToReset() {
    var userEmailValue = document.getElementById("userEmail").value;
    if (userEmailValue == "" || userEmailValue == undefined || userEmailValue == null) {
        $('#userEmailError').text('Please enter a valid email address.');
        return;
    }

    $('.error-label').text('');

    if (!validateEmail(userEmailValue)) {
        $('#userEmailError').text('Please enter a valid email address.');
        return;
    }

    $.ajax({
        type: "POST",
        url: `/ForgotPassword?handler=ResetPasswordEmail`,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
            "Content-Type": "application/x-www-form-urlencoded"
        },
        data: {
            userEmail: userEmailValue,
        },
        success: function (response) {
            if (response.success === "true") {

            } else {
                $('#errorMessage').text(`${response.error}`);
            }
        },
        error: function (error) {
            $('#errorMessage').text(`${error}`);
        }
    });
}

function validateEmail(email) {
    var re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}