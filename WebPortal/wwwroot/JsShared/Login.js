function Login() {

    var email = $("#txtEmail").val();
    var pass = $("#txtPassword").val();

    if (!email || !pass) {
        ShowTost("Error", "Email or Password is missing", "error", "top-right");
        return;
    }

    var data = {
        applicationUserEmail: email,
        password: pass
    };

    var success = function (response) {
        console.log("Login response:", response);

        if (response == null) {
            ShowTost("Error", "Server did not respond", "error", "top-right");
            return;
        }

        if (response.success) {
            var role = response.result?.applicationUserRole;
            var username = response.result?.applicationUserUsername;


            if (role && username) {
                localStorage.setItem("userRole", role);
                localStorage.setItem("username", username);

                ShowTost("Login Successful", "You have logged in successfully.", "success", "top-right", function () {
                    if (role === "User") {
                        window.location = "/UserDashboard/Index";
                    } else if (role === "Admin") {
                        window.location = "/AdminDashboard/Index";
                    } else {
                        window.location = "/Account/Login";
                    }
                });
            } else {
                console.error("User role missing in response.result");
                ShowTost("Login Failed", "User role is missing.", "error", "top-right");
            }
        } else {
            ShowTost("Login Failed", "Invalid username or password.", "error", "top-right");
        }
    };


    var error = function (response) {
        console.log("Error logging in: ", response);
    };

    var async = true;
    var methodType = "POST";
    var url = '/Account/Login';

    CallActions(url, data, success, error, async, methodType);
}