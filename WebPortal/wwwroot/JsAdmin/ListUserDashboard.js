function GetApplicationUserList(requestData, callback) {
    var success = function (response) {
        if (response && response.success === true) {
            const model = response.result?.applicationUserModel || [];
            const pagination = response.result?.pagination;
            const deactiveCount = response.result?.countDeActiveAndDeleted?.deactiveCount ?? 0;
            const deletedCount = response.result?.countDeActiveAndDeleted?.deletedCount ?? 0;

            if (pagination?.totalCount !== undefined) {
                $('#totalUsersNumberActiveWithDelete').text(pagination.totalCount);
            }
            $('#totalDeActiveUser').text(deactiveCount);
            $('#totalDeletedUserJustDelete').text(deletedCount);

            callback({
                draw: requestData.draw,
                recordsTotal: pagination?.totalCount ?? model.length,
                recordsFiltered: pagination?.totalCount ?? model.length,
                data: model
            });
        } else {
            callback({
                draw: requestData.draw,
                recordsTotal: 0,
                recordsFiltered: 0,
                data: []
            });
        }
    };

    var error = function () {
        callback({
            draw: requestData.draw,
            recordsTotal: 0,
            recordsFiltered: 0,
            data: []
        });
    };

    var async = true;
    var Types = "POST";
    var Method = '/AdminDashboard/GetApplicationUserList';

    CallActions(Method, requestData, success, error, async, Types);
}
function HardDeleteApplicationUser(applicationUserId) {
    var data = {
        ApplicationUserId: applicationUserId,
    };

    Swal.fire({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this User!",
        icon: "error",
        showCancelButton: true,
        confirmButtonText: "Delete",
        cancelButtonText: "Cancel",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            var success = function (response) {
                if (response && response.success) {
                    ShowTost(
                        "Deleted",
                        "The user has been deleted successfully.",
                        "success",
                        "top-right"
                    );

                    $('#applicationUserTable').DataTable().ajax.reload(null, false);

                } else {
                    ShowTost(
                        "Error",
                        "Something went wrong while deleting.",
                        "error",
                        "top-right"
                    );
                }
            };

            var error = function (response) {
                ShowTost(
                    "Server Error",
                    "Failed to delete the user.",
                    "error",
                    "top-right"
                );
            };

            var async = true;
            var Types = "POST";
            var Method = '/AdminDashboard/HardDeleteUser';

            CallActions(Method, data, success, error, async, Types);
        } else {
            ShowSweet("Cancelled", "Your user is safe!", "info", "OK");
        }
    });
}
function SoftDeleteApplicationUser(applicationUserId) {
    var data = {
        ApplicationUserId: applicationUserId,
    };

    Swal.fire({
        title: "Are you sure?",
        text: "This user will be soft deleted.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete",
        cancelButtonText: "Cancel",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            var success = function (response) {
                if (response && response.success) {
                    ShowTost(
                        "Deleted",
                        "User has been soft deleted successfully.",
                        "success",
                        "top-right"
                    );

                    $('#applicationUserTable').DataTable().ajax.reload(null, false);

                } else {
                    ShowTost(
                        "Error",
                        "Something went wrong during soft delete.",
                        "error",
                        "top-right"
                    );
                }
            };

            var error = function () {
                ShowTost(
                    "Server Error",
                    "Soft delete operation failed.",
                    "error",
                    "top-right"
                );
            };

            var async = true;
            var Types = "POST";
            var Method = '/AdminDashboard/SoftDeleteUser';

            CallActions(Method, data, success, error, async, Types);
        } else {
            ShowSweet("Cancelled", "No changes were made.", "info", "OK");
        }
    });
}
function ActivateApplicationUser(applicationUserId, isActive) {
    const action = isActive ? 'deactivate' : 'activate';
    const actionText = isActive ? 'deactivated' : 'activated';

    const data = {
        ApplicationUserId: applicationUserId,
        IsActive: !isActive
    };

    Swal.fire({
        title: `Are you sure you want to ${action} this user?`,
        text: `This will ${action} the user.`,
        icon: "question",
        showCancelButton: true,
        confirmButtonText: `Yes, ${action.charAt(0).toUpperCase() + action.slice(1)}`,
        cancelButtonText: "Cancel",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            const success = function (response) {
                if (response && response.success) {
                    ShowTost(`${actionText.charAt(0).toUpperCase() + actionText.slice(1)}`, `User has been ${actionText} successfully.`, "success", "top-right");
                    $('#applicationUserTable').DataTable().ajax.reload(null, false);
                } else {
                    ShowTost("Error", "Something went wrong.", "error", "top-right");
                }
            };

            const error = function () {
                ShowTost("Server Error", "Failed to update user status.", "error", "top-right");
            };

            CallActions('/AdminDashboard/ActivateUser', data, success, error, true, 'POST');
        } else {
            ShowSweet("Cancelled", `${action.charAt(0).toUpperCase() + action.slice(1)} cancelled.`, "info", "OK");
        }
    });
}
function ResetPasswordApplicationUser() {
    var applicationUserId = $('#applicationUserId_changePassword').val();
    var oldPassword = $("#oldPassword").val().trim();
    var newPassword = $("#newPassword").val().trim();
    var confirmPassword = $("#confirmPassword").val().trim();

    function validateField(fieldValue, fieldName, required = true) {
        if (required && fieldValue.length === 0) {
            ShowSweet(`${fieldName} is Empty!`, `Please enter ${fieldName}`, "error", "OK");
            return false;
        }
        if (hasHtmlAndScript(fieldValue)) {
            ShowSweet(`${fieldName} contains invalid HTML or scripts!`, `Please remove HTML tags or scripts from ${fieldName}`, "error", "OK");
            return false;
        }
        if (hasSpecialCharacters(fieldValue)) {
            ShowSweet(`${fieldName} contains special characters!`, `Please remove special characters (< > ' : " ; =) from ${fieldName}`, "error", "OK");
            return false;
        }
        return true;
    }

    if (!validateField(oldPassword, "Old Password")) return;
    if (!validateField(newPassword, "New Password")) return;
    if (!validateField(confirmPassword, "Confirm New Password")) return;
    if (newPassword !== confirmPassword) {
        ShowSweet("Password Mismatch", "New password and confirm password do not match.", "error", "OK");
        return;
    }

    var data = {
        ApplicationUserId: applicationUserId,
        OldPasswordHash: oldPassword,
        NewPasswordHash: newPassword,
        ConfirmNewPasswordHash: confirmPassword
    };

    var success = function (response) {
        if (response && response.success) {
            ShowSweetFire('center', 'success', 'Password changed successfully!', false, 1500).then(() => {
                $('#changePasswordModal').fadeOut();
                $('#uploadForm')[0].reset();
                $('#applicationUserTable').DataTable().ajax.reload(null, false);
            });
        } else {
            ShowSweet("Save Failed!", response.message || "An error occurred", "error", "OK");
        }
    };

    var error = function () {
        ShowSweet("Server Error!", "Something went wrong", "error", "OK");
    };

    CallActions('/AdminDashboard/ResetPasswordUser', data, success, error, true, "POST");
}
function EditApplicationUserInfo() {
    const applicationUserId = $('#applicationUserId_editUser').val();
    const username = $('#applicationUserUsername').val().trim();
    const email = $('#applicationUserEmail').val().trim();

    if (!username || !email) {
        ShowSweet("Validation Failed", "Username and Email are required", "warning", "OK");
        return;
    }
    if (!isValidEmail(email)) {
        ShowSweet("Invalid Email", "Please enter a valid email address", "warning", "OK");
        return;
    }
    const payload = {
        ApplicationUserId: applicationUserId,
        ApplicationUserUsername: username,
        ApplicationUserEmail: email
    };

    const success = function (response) {
        if (response && response.success) {
            ShowSweetFire('center', 'success', 'Updated successfully!', false, 1500).then(() => {
                $('#editUserInfoModal').fadeOut();
                $('#editUserForm')[0].reset();
                $('#applicationUserTable').DataTable().ajax.reload(null, false);
            });
        } else {
            ShowSweet("Update Failed", response.message || "Unknown error", "error", "OK");
        }
    };

    const error = function () {
        ShowSweet("Server Error", "Something went wrong", "error", "OK");
    };

    CallActions('/AdminDashboard/UpdateUserInfo', payload, success, error, true, "POST");
}
