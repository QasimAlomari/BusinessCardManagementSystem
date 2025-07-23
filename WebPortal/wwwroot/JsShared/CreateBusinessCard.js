function CreateBusinessCard() {
    var fields = [
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
        if (!validateField(value, field.name, field.required)) return;
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

    var success = function (response) {
        if (response && response.success) {
            ShowSweetFire('center', 'success', 'Data Saved!', false, 1500).then(() => {
                redirectToDashboard();
            });
        } else {
            ShowSweet("Save Failed!", "An error occurred", "error", "OK");
        }
    };

    var error = function () {
        ShowSweet("Server Error!", "Something went wrong", "error", "OK");
    };

    CallActions('/Common/CreateBusinessCard', data, success, error, true, "POST");
}
