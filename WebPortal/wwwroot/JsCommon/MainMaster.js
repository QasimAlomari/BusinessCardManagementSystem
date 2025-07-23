function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}
function ShowTost(heading, text, icon, position, afterHidden) {

    $.toast({
        heading: heading,// 'Headings',
        text: text,// 'You can use the `heading` property to specify the heading of the toast message.',
        icon: icon,// 'warning',
        position: position,//'top-right',
        textAlign: 'center',
        //bgColor: '#28a745',
        textColor: '#ffffff',
        hideAfter: 3000,
        afterHidden: function () {
            if (afterHidden != null) {
                afterHidden()

            }
        }

    })
}
function ShowSweet(title, text, icon, buttonText) {
    Swal.fire({
        title: title,
        text: text,
        icon: icon, // 'success', 'error', 'warning', 'info', 'question'
        confirmButtonText: buttonText
    });
}
function ShowSweetFire(position, icon, title, showConfirmButton, timer) {
    return Swal.fire({
        position: position,
        icon: icon,
        title: title,
        showConfirmButton: showConfirmButton,
        timer: timer
    });
}
function hasHtmlAndScript(input) {
    var scriptRegex = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi;
    var htmlRegex = /<[\/?][^>]+>/gi;
    return scriptRegex.test(input) || htmlRegex.test(input);
}
function hasSpecialCharacters(input) {
    var specialCharRegex = /[<>':";=]/;
    return specialCharRegex.test(input);
}
function redirectToDashboard() {
    var role = localStorage.getItem("userRole");

    if (role === "Admin") {
        window.location.href = "/AdminDashboard/Index";
    } else if (role === "User") {
        window.location.href = "/UserDashboard/Index";
    } else {
        window.location.href = "/Account/Login";
    }
}
function redirectToEditBusinessCard(param) {
    window.location.href = "/Common/Edit?businessCardId=" + param;
}
function isEmptyField(fieldValue, fieldName) {
    if (fieldValue.length === 0) {
        ShowSweet(`${fieldName} is Empty!`, `Please enter ${fieldName}`, "error", "OK");
        return true;
    }
    return false;
}
function containsHtmlOrScript(fieldValue, fieldName) {
    if (hasHtmlAndScript(fieldValue)) {
        ShowSweet(`${fieldName} contains invalid HTML or scripts!`, `Please remove HTML tags or scripts from ${fieldName}`, "error", "OK");
        return true;
    }
    return false;
}
function containsSpecialChars(fieldValue, fieldName) {
    if (hasSpecialCharacters(fieldValue)) {
        ShowSweet(`${fieldName} contains special characters!`, `Please remove special characters (< > ' : " ; =) from ${fieldName}`, "error", "OK");
        return true;
    }
    return false;
}
function validateField(fieldValue, fieldName, required = true) {
    if (required && isEmptyField(fieldValue, fieldName)) return false;
    if (containsHtmlOrScript(fieldValue, fieldName)) return false;
    if (containsSpecialChars(fieldValue, fieldName)) return false;
    return true;
}
function isValidPhone(phone) {
    var phoneRegex = /^[0-9+\-\s().]+$/;
    return phoneRegex.test(phone);
}
function isValidEmail(email) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}
function validateWebsite(url, fieldName, required = true) {
    if (required && (!url || url.trim() === "")) {
        ShowSweet(`${fieldName} is Empty!`, `Please enter ${fieldName}`, "error", "OK");
        return false;
    }

    if (url && !(url.startsWith("http://") || url.startsWith("https://"))) {
        ShowSweet(`${fieldName} is Invalid!`, `Website must start with http:// or https://`, "error", "OK");
        return false;
    }

    return true;
}
