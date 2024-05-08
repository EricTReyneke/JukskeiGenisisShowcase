function RegisterNewUser() {
    var userInformation = {
        Id: "4407f958-5a0b-4cbe-b476-c3721ed8d66a",
        FullName: $('#fullName').val().trim(),
        UserName: $('#userName').val().trim(),
        Email: $('#email').val().trim(),
        Password: $('#password').val()
    };

    $('.error-label').text('');

    if (!validateInputs(userInformation)) {
        return;
    }

    $.ajax({
        type: "POST",
        url: `/Registration?handler=CreateNewUser`,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
            "Content-Type": "application/json"
        },
        data: JSON.stringify(userInformation),
        success: function (response) {
            if (response.success === "true") {
                window.location.href = "/Login";
            } else {
                if (response.error === "Email") {
                    $('#emailError').text('This email is already take. Please try another email address.');
                }
                else if (response.error === "UserName") {
                    $('#userNameError').text('This user name is already take. Please try another user name address.');
                }
                else {
                    $('#errorMessage').text(`${response.error}`);
                }                
            }
        },
        error: function (error) {
            console.error('Error sending data:', error);
        }
    });
}

function validateInputs(data) {
    var isValid = true;

    if (!data.FullName || data.FullName.length < 2) {
        $('#fullNameError').text('Full Name must be at least 2 characters long.');
        isValid = false;
    }
    if (!data.UserName || data.UserName.length < 2) {
        $('#userNameError').text('User Name must be at least 2 characters long.');
        isValid = false;
    }
    if (!data.Email || !validateEmail(data.Email)) {
        $('#emailError').text('Please enter a valid email address.');
        isValid = false;
    }
    if (!data.Password || data.Password.length < 6) {
        $('#passwordError').text('Password must be at least 6 characters long.');
        isValid = false;
    }
    else {
        var confirmPasswordValue = document.getElementById("confirmPassword").value;
        if (data.Password != confirmPasswordValue) {
            $('#confirmPasswordError').text('Both passwords must match.');
            isValid = false;
        }
    }
    return isValid;
}

function validateEmail(email) {
    var re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}