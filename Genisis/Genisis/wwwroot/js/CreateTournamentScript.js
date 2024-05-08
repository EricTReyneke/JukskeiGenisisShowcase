var addedCategories = [];

function AddCategoryFrontEnd() {
    var inputElement = document.getElementById('categories-input');
    var categoryName = inputElement.value.trim();

    if (categoryName !== "") {
        if (addedCategories.includes(categoryName.toLowerCase())) {
            alert("This category has already been added.");
            return;
        }

        addedCategories.push(categoryName.toLowerCase());

        var categoryDiv = document.createElement('div');
        categoryDiv.classList.add('category-item');
        categoryDiv.textContent = categoryName;

        var deleteBtn = document.createElement('button');
        deleteBtn.type = 'button';
        deleteBtn.classList.add('delete-btn');
        deleteBtn.textContent = 'Delete';

        deleteBtn.onclick = function () {
            var index = addedCategories.indexOf(categoryName.toLowerCase());
            if (index !== -1) {
                addedCategories.splice(index, 1);
            }
            categoryDiv.remove();

            RemoveCategory(categoryName);
        };

        categoryDiv.appendChild(deleteBtn);

        var categoriesListDiv = document.getElementById('categories-list');
        categoriesListDiv.appendChild(categoryDiv);

        inputElement.value = '';
    } else {
        alert("Please enter a category name.");
    }
}

function AddCategory() {
    var categoryNameValue = document.getElementById("categories-input").value;
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        type: "POST",
        url: "/NewTournament?handler=AddChatagoryToList",
        headers: {
            "RequestVerificationToken": token
        },
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { categoryName: categoryNameValue },
        success: function (response) {
            if (response.message === "success") {

            } else {
            }
        },
        error: function (error) {
            console.error('Error sending data:', error);
        }
    });
}

function RemoveCategory(categoryName) {
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        type: "POST",
        url: "/NewTournament?handler=RemoveCategoryFromList",
        headers: {
            "RequestVerificationToken": token
        },
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        data: { categoryName: categoryName },
        success: function (response) {
            if (response === "success") {
            } else {
            }
        },
        error: function (error) {
            console.error('Error sending data:', error);
        }
    });
}

function updateEndDate() {
    var startDate = new Date(document.querySelector('[name="StartDate"]').value);
    var duration = document.querySelector('[name="Duration"]').value;
    var endDate = new Date(startDate);

    switch (duration) {
        case "Day":
            endDate.setDate(startDate.getDate() + 1);
            break;
        case "League":
            endDate.setDate(startDate.getDate() + 21);
            break;
        case "Week":
            endDate.setDate(startDate.getDate() + 7);
            break;
    }

    document.querySelector('[name="EndDate"]').value = endDate.toISOString().split('T')[0];
}