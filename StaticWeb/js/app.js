function redirectToLogin() {
    window.location = "https://nssiauthpoc.azurewebsites.net/auth?returnurl=https://nssiauthpoc.azurewebsites.net";
}

function makeAjaxCallToInsecureEndpoint() {
    var url = "https://nssiauthpoc.azurewebsites.net/student_registration/contact.aspx"
    $.ajax({
        url: url,
        type: 'POST',
    }).done(function(data) {
        $('#response_body').html(data)
    });
}

// https://www.ifourtechnolab.com/blog/authentication-using-cookie-for-frontend-application-in-asp-net-core-web-api
function makeAjaxCallToSecureEndpoint() {
    var url = "https://nssiauthpoc.azurewebsites.net/student_registration/contact.aspx"
    $.ajax({
        url: url,
        type: 'POST',
        xhrFields: {
            withCredentials: true
        }
    }).done(function(data) {
        $('#response_body').html(data)
    });
}

