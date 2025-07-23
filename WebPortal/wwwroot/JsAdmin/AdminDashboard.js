function HardDeleteBusinessCard(businessCardId) {
    var data = {
        BusinessCardId: businessCardId,
    };

    Swal.fire({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this Card!",
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
                        "The business card has been deleted successfully.",
                        "success",
                        "top-right"
                    );

                    var currentPage = parseInt(GetParameterValues("pageNumber")) || 1;
                    var pageSize = parseInt(GetParameterValues("pageSize")) || 3;

                    GetBusinessCardList(currentPage, pageSize);
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
                    "Failed to delete the business card.",
                    "error",
                    "top-right"
                );
            };

            var async = true;
            var Types = "POST";
            var Method = '/AdminDashboard/HardDeleteBusinessCard';

            CallActions(Method, data, success, error, async, Types);
        } else {
            ShowSweet("Cancelled", "Your card is safe!", "info", "OK");
        }
    });
}
function ActivateBusinessCard(businessCardId) {
    var data = {
        BusinessCardId: businessCardId,
    };

    Swal.fire({
        title: "Are you sure?",
        text: "You are about to activate this business card.",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes, Activate",
        cancelButtonText: "Cancel",
        reverseButtons: true,
    }).then((result) => {
        if (result.isConfirmed) {
            var success = function (response) {
                if (response && response.success) {
                    ShowTost("Activated", "Business card has been activated successfully.", "success", "top-right");

                    var currentPage = parseInt(GetParameterValues("pageNumber")) || 1;
                    var pageSize = parseInt(GetParameterValues("pageSize")) || 3;
                    GetBusinessCardList(currentPage, pageSize);
                } else {
                    ShowTost("Error", "Something went wrong while activating.", "error", "top-right");
                }
            };

            var error = function () {
                ShowTost("Server Error", "Failed to activate the business card.", "error", "top-right");
            };

            var async = true;
            var Types = "POST";
            var Method = '/AdminDashboard/ActivateBusinessCard';

            CallActions(Method, data, success, error, async, Types);
        } else {
            ShowSweet("Cancelled", "Activation cancelled.", "info", "OK");
        }
    });
}
function ImportUsersFromExcel() {
    var fileInput = $("#fileInput")[0];
    var fileSizeLimit = 104857600; // 100 MB
    var importButton = $("#importButton");

    importButton.prop('disabled', true);

    if (fileInput.files.length === 0) {
        ShowSweet("Please select a file to upload.", "", "error", "OK");
        importButton.prop('disabled', false);
        return;
    }

    var file = fileInput.files[0];

    if (file.size > fileSizeLimit) {
        ShowSweet("The file is too large. Please select a smaller file.", "", "error", "OK");
        importButton.prop('disabled', false);
        return;
    }

    var fileData = new FormData();
    fileData.append('excelFile', file);

    $("#spinnerOverlay").show();

    var success = function (response) {
        $("#spinnerOverlay").hide();
        importButton.prop('disabled', false);
        if (response && response.success) {
            ShowSweet("File imported successfully", "", "success", "OK");
            var currentPage = parseInt(GetParameterValues("pageNumber")) || 1;
            var pageSize = parseInt(GetParameterValues("pageSize")) || 3;
            GetBusinessCardList(currentPage, pageSize);
            $("#uploadModal").hide();
        } else {
            ShowSweet("Import failed", response.message || "An unknown error occurred.", "error", "OK");
        }
    };

    var error = function (xhr, status, err) {
        $("#spinnerOverlay").hide();
        importButton.prop('disabled', false);
        console.error("Error uploading file:", err);
        ShowSweet("Error", "An error occurred. Please try again.", "error", "OK");
    };

    callServiceBackendMultiPart('/AdminDashboard/ImportExcel', fileData, success, error, true, "POST", false, false);
}




