function SoftDeleteBusinessCard(businessCardId) {
    var data = {
        BusinessCardId: businessCardId,
    };

    Swal.fire({
        title: "Are you sure?",
        text: "This business card will be soft deleted.",
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
                        "Business card has been soft deleted successfully.",
                        "success",
                        "top-right"
                    );

                    var currentPage = parseInt(GetParameterValues("pageNumber")) || 1;
                    var pageSize = parseInt(GetParameterValues("pageSize")) || 3;
                    GetBusinessCardList(currentPage, pageSize);
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
            var Method = '/Common/SoftDeleteBusinessCard';

            CallActions(Method, data, success, error, async, Types);
        } else {
            ShowSweet("Cancelled", "No changes were made.", "info", "OK");
        }
    });
}

