async function DisplayCategoryTables(categorySelectBox) {
    const categorySelectBoxValue = categorySelectBox.value;
    if (categorySelectBoxValue === "Select Category")
        return;

    try {
        var matchInScope = await RetrieveCategoryCurrentMatch(categorySelectBoxValue);

        DisplayNoneOnAllTables();

        const tableDiv = document.getElementById(`table-${categorySelectBoxValue}`);
        if (tableDiv) {
            tableDiv.style.display = "block";
            tableDiv.style.position = "relative";
        } else {
            console.warn(`Table for category '${categorySelectBoxValue}' not found.`);
        }

        DisplayNoneOnAllMatchOutOfScope(matchInScope, categorySelectBoxValue);
    } catch (error) {
        console.error("Failed to retrieve category current match:", error);
    }
}

function DisplayNoneOnAllTables() {
    const tableDivs = document.getElementsByClassName('tables-container');

    for (let i = 0; i < tableDivs.length; i++) {
        tableDivs[i].style.display = "none";
        tableDivs[i].style.display = "absolute";
    }
}

function DisplayNoneOnAllMatchOutOfScope(matchCounter, categoryName) {
    var tableRows = document.getElementsByClassName("matches-inscope");
    var tableRowsInScope = document.getElementsByClassName(`match-${matchCounter}-${categoryName}`);

    let displayCounter = 0;

    for (let i = 0; i < tableRows.length; i++) {
        tableRows[i].style.display = "none";
    }

    for (let i = 0; i < tableRowsInScope.length; i++) {
        tableRowsInScope[i].style.display = "table-row";
        displayCounter++;
    }

    if (displayCounter === 0) {
        DisplayAllScoresUpdatesDiv(categoryName);
    }
}

function RetrieveMatchScores(matchCounter, categoryName) {
    let matchScores = [];

    const team1ScoreSelects = document.querySelectorAll(`.select-team1-${matchCounter}-${categoryName}`);
    const team2ScoreSelects = document.querySelectorAll(`.select-team2-${matchCounter}-${categoryName}`);

    const team1NameElements = document.querySelectorAll(`.team1-${matchCounter}-${categoryName}`);
    const team2NameElements = document.querySelectorAll(`.team2-${matchCounter}-${categoryName}`);

    team1ScoreSelects.forEach((select, index) => {
        const team1Score = select.value;
        const team2Score = team2ScoreSelects[index].value;

        const team1Name = team1NameElements[index].textContent;
        const team2Name = team2NameElements[index].textContent;

        matchScores.push({
            MatchNumber: matchCounter,
            team1: {
                name: team1Name,
                score: team1Score
            },
            team2: {
                name: team2Name,
                score: team2Score
            }
        });
    });

    executeSequence(categoryName, matchScores, matchCounter);
}

function DisplaySuccessBox(matchInScope, categoryName) {
    var successText = document.getElementById('success-text');

    successText.textContent = `Scores were successfully updated for category ${categoryName} match ${matchInScope}!`;
    document.getElementById('success-message').style.display = "block";
}

function SendMatchDetailsToServer(categoryName, matchScores) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: `/AllocatePoints?handler=AllocatePointsForMatches&categoryName=${encodeURIComponent(categoryName)}`,
            type: 'POST',
            headers: {
                "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                "Content-Type": "application/json"
            },
            data: JSON.stringify(matchScores),
            success: function (response) {
                console.log('Success:', response);
                resolve(response);
            },
            error: function (xhr, status, error) {
                console.error('Error:', status, error);
                reject(error);
            }
        });
    });
}

async function executeSequence(categoryName, matchScores, matchCounter) {
    await SendMatchDetailsToServer(categoryName, matchScores);
    DisplaySuccessBox(matchCounter, categoryName);
    DisplayCategoryTables(document.getElementById("category-selectbox"));
}

function AddScoresToSecondTeam(teamOne, teamTwoSelectBoxId) {
    var teamTwoSelectBox = document.getElementById(teamTwoSelectBoxId);

    if (teamOne.value === "Enter Score") {
        teamTwoSelectBox.selectedIndex = 0;
        return;
    }

    var teamTwoScore = Math.abs(teamOne.value - 15);

    var scoreValueAsString = teamTwoScore.toString();

    var hasValue = Array.from(teamTwoSelectBox.options).some(option => option.value === scoreValueAsString);

    if (hasValue) {
        teamTwoSelectBox.value = scoreValueAsString;
    } else {
        console.warn("The calculated score value does not exist as an option in the select box.");
        teamTwoSelectBox.value = "";
    }
}

function RetrieveCategoryCurrentMatch(categoryName) {
    return new Promise((resolve, reject) => {
        $.ajax({
            type: "POST",
            url: `/AllocatePoints?handler=RetrieveCategoryCurrentMatch&categoryName=${encodeURIComponent(categoryName)}`,
            headers: {
                "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
                "Content-Type": "application/json"
            },
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    resolve(response.matchNumber);
                } else {
                    console.log("An unexpected response was received:", response.message);
                    reject(new Error("An unexpected response was received"));
                }
            },
            error: function (error) {
                console.error('Error sending data:', error);
                reject(error);
            }
        });
    });
}

function hideSuccessMessage() {
    document.getElementById('success-message').style.display = 'none';
}

function DisplayAllScoresUpdatesDiv(categoryName) {
    var allScoresUpdated = document.getElementById(`all-scores-updated-${categoryName}`);

    allScoresUpdated.style.display = "block";
}