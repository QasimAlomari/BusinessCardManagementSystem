function Logout() {
    Swal.fire({
        title: 'Are you sure?',
        text: "You will be signed out and redirected to login.",
        icon: 'warning',
        showCancelButton: true,
        showConfirmButton: true,
        confirmButtonText: 'Yes, sign out!',
        cancelButtonText: 'Cancel',
        customClass: {
            actions: 'my-actions',
            cancelButton: 'order-1 right-gap',
            confirmButton: 'order-2'
        },
        buttonsStyling: true
    }).then((result) => {
        if (result.isConfirmed) {
            localStorage.removeItem("username");
            localStorage.removeItem("userRole");
            location.href = '/Account/Login';
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            Swal.fire({
                title: "Cancelled",
                text: "Logout canceled.",
                icon: "info",
                showConfirmButton: false,
                timer: 1500,
                position: "center"
            });
        }
    });
}
