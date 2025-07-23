function EditBusinessCard() {
    let fields = [
        { id: "cardName", name: "Card Name", required: true },
        { id: "cardTitle", name: "Card Title", required: true },
        { id: "cardPhone", name: "Card Phone", required: true },
        { id: "cardEmail", name: "Card Email", required: true },
        { id: "cardCompany", name: "Card Company", required: true },
        { id: "cardWebsite", name: "Card Website", required: true },
        { id: "cardAddress", name: "Card Address", required: true },
        { id: "cardNotes", name: "Card Notes", required: false }
    ];

    let data = {};

    for (let field of fields) {
        let value = $(`#${field.id}`).val().trim();

        if (field.required && isEmptyField(value, field.name)) return;

        // Add website validation here
        if (field.id === "cardWebsite" && !validateWebsite(value, field.name, field.required)) return;

        data[`Business${field.name.replace(/\s/g, '')}`] = value;
    }

    if (!isValidPhone(data.BusinessCardPhone)) {
        ShowSweet("Card Phone is invalid!", "Please enter a valid phone number", "error", "OK");
        return;
    }

    if (!isValidEmail(data.BusinessCardEmail)) {
        ShowSweet("Card Email is invalid!", "Please enter a valid email address", "error", "OK");
        return;
    }

    if (!globalBusinessCardId) {
        ShowSweet("Missing Business Card ID", "Business Card ID is required for editing.", "error", "OK");
        return;
    }

    data.BusinessCardId = globalBusinessCardId;

    let success = function (response) {
        if (response && response.success) {
            ShowSweetFire('center', 'success', 'Data Saved!', false, 1500).then(() => {
                redirectToDashboard();
            });
        } else {
            ShowSweet("Save Failed!", "An error occurred", "error", "OK");
        }
    };

    let error = function () {
        ShowSweet("Server Error!", "Something went wrong", "error", "OK");
    };

    CallActions('/Common/EditBusinessCard', data, success, error, true, "POST");
}
