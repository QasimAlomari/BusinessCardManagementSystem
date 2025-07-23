function CreateApplicationUser() {
    //debugger;
    var userName = $("#userName").val().trim();
    var userEmail = $("#userEmail").val().trim();
    var userPassword = $("#userPassword").val().trim();
    var userRole = $("#userRole").val().trim();

    function isValidPassword(password) {
        return password.length >= 6 && !/\s/.test(password);
    }

    if (!validateField(userName, "User Name")) return;
    if (!validateField(userEmail, "User Email")) return;
    if (!validateField(userPassword, "User Password")) return;
    if (!validateField(userRole, "User Role")) return;

    if (!isValidEmail(userEmail)) {
        ShowSweet("User Email is invalid!", "Please enter a valid email address", "error", "OK");
        return;
    }

    if (!isValidPassword(userPassword)) {
        ShowSweet("User Password is invalid!", "Password must be at least 6 characters with no spaces", "error", "OK");
        return;
    }

    var data = {
        UserName: userName,
        Email: userEmail,
        Password: userPassword,
        UserRole: userRole
    };

    var success = function (response) {
        if (response && response.success) {
            ShowSweetFire('center', 'success', 'Data Saved!', false, 1500).then(() => {
                window.location.href = "/AdminDashboard/ListUsers";
            });
        } else {
            ShowSweet("Save Failed!", response.message || "An error occurred", "error", "OK");
        }
    };

    var error = function () {
        ShowSweet("Server Error!", "Something went wrong", "error", "OK");
    };

    CallActions('/AdminDashboard/AdminRegisterUsers', data, success, error, true, "POST");
}