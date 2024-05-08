function AddTeamToCategory() {
    var addPlayersCheckBox = document.getElementById("add-player-checkbox");
    var teamName = document.getElementById("team-name").value;
    var categoryName = document.getElementById('team-categories').value;
    var targetElement = document.getElementById(`category-${categoryName}`);

    //Player Names.
    var playerElements = document.getElementsByClassName('input-labels extension');
    var playerTextBoxes = "";
    var htmlContent = null;

    if (addPlayersCheckBox.checked) {
        for (var i = 0; i < playerElements.length; i++) {
            playerTextBoxes += `<div class="flex-update-elements">\n
                                    <input type="text" value="${playerElements[i].value}" class="input-labels update-text-boxes" id="${teamName}-player-${i}" oninput="TogglePlayerUpdateButtons('${teamName}', '${i}')" />\n
                                    <button type="button" class="push-team-button update-buttons" id="${teamName}-${i}" onclick="UpdatePlayerTeam('${teamName}', '${i}', '${categoryName}')">Update</button>\n
                                </div>\n
                                <input class="hidden-player-names" id="hidden-player-names-${teamName}-${i}" value="${playerElements[i].value}" />\n`;
        }

        htmlContent = `
        <div class="accordion-body">
            <button class="accordion-button" onclick="ToggleAccordion('${teamName}')">
                <div class="accordion-content">
                    <img />
                </div> 
                <div class="accordion-content">
                    <h5><span class="tour-heading">${teamName}</span></h5>
                </div>
            </button>
            <div class="panel" id="accordion-panel-${teamName}">
                ${playerTextBoxes}
            </div>
        </div>`;
    }
    else {
        htmlContent = `
        <div class="accordion-body">
            <button class="accordion-button" onclick="ToggleAccordion('${teamName}')">
                <div class="accordion-content">
                    <img />
                </div> 
                <div class="accordion-content">
                    <h5><span class="tour-heading">${teamName}</span></h5>
                </div>
            </button>
        </div>`;
    }
    if (targetElement) {
        targetElement.insertAdjacentHTML('afterend', htmlContent);
    }

    AddPlayersToList(playerElements, categoryName, teamName);
    ToggleInsertionInputs();
}

function ToggleWarningDiv() {
    var warningDiv = document.querySelector('.warning-popup-overlay');
    var warningDivIntractive = document.querySelector('.warning-popup');

    if (warningDivIntractive.style.display === 'none') {
        warningDiv.style.display = 'block';
        warningDiv.style.position = 'fixed';
        warningDiv.style.top = '0';
        warningDiv.style.left = '0';
        warningDiv.style.width = '100%';
        warningDiv.style.height = '100%';
        warningDiv.style.backgroundColor = 'rgba(0,0,0,0.5)';
        warningDiv.style.zIndex = '1000';
        warningDivIntractive.style.zIndex = '1001';
        warningDivIntractive.style.display = "block";
    } else {
        warningDiv.style.display = 'none';
        warningDiv.style.position = 'absolute';
        warningDivIntractive.style.display = "none";
    }
}

function TogglePlayerNameEntries() {
    var addPlayerNamesCheckBox = document.getElementById("add-player-checkbox");
    var playerNamesDiv = document.querySelector('.player-names');
    var teamNamesDiv = document.querySelector('.flex1');

    if (addPlayerNamesCheckBox.checked) {
        playerNamesDiv.style.position = 'relative';
        playerNamesDiv.style.display = 'block';

        teamNamesDiv.style.position = 'static';
        teamNamesDiv.style.left = '0';
        teamNamesDiv.style.transform = 'translateX(0%)';
        teamNamesDiv.style.transition = 'none';
    } else {
        playerNamesDiv.style.position = 'absolute';
        playerNamesDiv.style.display = 'none';

        teamNamesDiv.style.position = 'relative';
        teamNamesDiv.style.left = '0';
        teamNamesDiv.style.transform = 'translateX(0)';
        
        setTimeout(() => {
            teamNamesDiv.style.transition = 'all 0.7s ease';
            teamNamesDiv.style.transform = 'translateX(60%)';
        }, 0);
    }

    ClearValuesOnPlayerNameCheckBoxChange();
}

function ToggleCaptainCheckBoxes(checkboxNumber) {
    var captainCheckBoxes = document.getElementsByClassName("captain-checks");
    for (var i = 0; i < captainCheckBoxes.length; i++) {
        if (i === checkboxNumber) {
            continue;
        }

        captainCheckBoxes[i].checked = false;
    }
}

function ToggleInsertionInputs() {
    var teamName = document.getElementById("team-name");
    if (teamName) {
        teamName.value = "";
    }

    var categoryName = document.getElementById("team-categories");
    if (categoryName) {
        categoryName.selectedIndex = 0;
    }

    var playerNames = document.getElementsByClassName("input-labels extension");
    for (var i = 0; i < playerNames.length; i++) {
        playerNames[i].value = "";
    }
}

function TogglePlayerUpdateButtons(teamName, playerIndex) {
    var playerName = document.getElementById(`hidden-player-names-${teamName}-${playerIndex}`).value;
    var textBoxValue = document.getElementById(`${teamName}-player-${playerIndex}`).value;
    var updateButton = document.getElementById(`${teamName}-${playerIndex}`);
    if (textBoxValue === playerName) {
        updateButton.style.display = "none";
    } else {
        updateButton.style.display = "block";
        updateButton.style.maxHeight = "300px";
    }
}

function ToggleAccordion(teamName) {
    var panel = document.getElementById(`accordion-panel-${teamName}`);
    if (panel.style.display === "block") {
        panel.style.display = "none";
    } else {
        panel.style.display = "block";
        panel.style.maxHeight = "300px";
    }
}

function DeletePlayerTeamsInTournament() {
    $.ajax({
        type: "POST",
        url: `/AddTeam?handler=DeletePlayerTeamsInTournament`,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
            "Content-Type": "application/json"
        },
        dataType: "json",
        success: function (response) {
            if (response.success) {
                ToggleWarningDiv();
                RemoveAccordions();
                ChangeStateOfCheckbox();
                TogglePlayerNameEntries();
            } else {
                console.log("An unexpected response was received:", response.message);
            } 
        },
        error: function (error) {
            console.error('Error sending data:', error);
        }
    });
}

function IsThereNoTeamsAdded(playerNameCheckbox) {
    $.ajax({
        type: "POST",
        url: `/AddTeam?handler=IsThereNoTeamsAdded`,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
            "Content-Type": "application/json"
        },
        dataType: "json",
        success: function (response) {
            if (response.success) {
                if (response.teams) {
                    ToggleWarningDiv();
                    playerNameCheckbox.checked = !playerNameCheckbox.checked;
                } else {
                    TogglePlayerNameEntries();
                }
            } else {
                console.log("An unexpected response was received:", response.message);
            }
        },
        error: function (error) {
            console.error('Error sending data:', error);
        }
    });
}

function AddPlayersToList(playerElements, categoryName, teamName) {
    var captainIndex = CaptainIndex();

    if (captainIndex === undefined) {
        captainIndex = 100;
    }

    var playerNames = [];
    for (var i = 0; i < playerElements.length; i++) {
        playerNames.push(playerElements[i].value);
    }

    $.ajax({
        type: "POST",
        url: `/AddTeam?handler=AddPlayersToList&playerNames=${encodeURIComponent(playerNames)}&categoryName=${encodeURIComponent(categoryName)}&teamName=${encodeURIComponent(teamName)}&isCaptain=${encodeURIComponent(captainIndex)}`,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val(),
            "Content-Type": "application/json"
        },
        dataType: "json",
        success: function (response) {
        },
        error: function (error) {

        }
    });
}

function UpdatePlayerTeam(teamName, playerIndex, categoryName) {
    var oldPlayerName = document.getElementById(`hidden-player-names-${teamName}-${playerIndex}`);
    const url = `/AddTeam?handler=UpdatePlayerName&oldPlayerName=${encodeURIComponent(oldPlayerName.value)}&categoryName=${encodeURIComponent(categoryName)}`;
    var token = $('input[name="__RequestVerificationToken"]').val();
    var newPlayerName = document.getElementById(`${teamName}-player-${playerIndex}`).value;
    var isCaptain = false;
    var isReserve = false;

    if (playerIndex === 3) {
        isCaptain = true;
    } else if (playerIndex === 4) {
        isReserve = true;
    }

    const data = {
        PlayerFullName: newPlayerName,
        TeamName: teamName,
        Captain: isCaptain,
        Reserve: isReserve
    };

    $.ajax({
        type: "POST",
        url: url,
        headers: {
            "RequestVerificationToken": token,
            "Content-Type": "application/json"
        },
        data: JSON.stringify(data),
        dataType: "json",
        success: function (response) {
            console.log("Update successful.");
        },
        error: function (error) {
            console.error('Error sending data:', error);
        }
    });

    oldPlayerName.value = newPlayerName;
    DeactivateUpdateButton(teamName, playerIndex);
}

function DeactivateUpdateButton(teamName, playerIndex) {
    var updateButton = document.getElementById(`${teamName}-${playerIndex}`);
    if (updateButton) {
        updateButton.style.display = "none";
    }
}

function CaptainIndex() {
    var captainCheckBoxes = document.getElementsByClassName("captain-checks");
    var captainIndex = null;
    for (var i = 0; i < captainCheckBoxes.length; i++) {
        if (captainCheckBoxes[i].checked) {
            captainIndex = i;
            return i;
        }
    }
}

function RemoveAccordions() {
    const divs = document.querySelectorAll('.accordion-body');

    divs.forEach(function (div) {
        div.parentNode.removeChild(div);
    });
}

function ChangeStateOfCheckbox() {
    var addPlayersCheckBox = document.getElementById("add-player-checkbox");

    if (addPlayersCheckBox.checked) {
        addPlayersCheckBox.checked = false;
    }
    else {
        addPlayersCheckBox.checked = true;
    }
}

function ClearValuesOnPlayerNameCheckBoxChange() {
    document.getElementById('team-name').value = "";

    var selectBox = document.getElementById('team-categories');
    selectBox.selectedIndex = 0;

    var playerNamesInputs = document.querySelectorAll('.player-names .input-labels.extension');
    playerNamesInputs.forEach(function (input) {
        input.value = "";
    });

    var captainCheckboxes = document.querySelectorAll('.player-names .captain-checks');
    captainCheckboxes.forEach(function (checkbox) {
        checkbox.checked = false;
    });
}